/*
 * Binary Image Exchange Format reference implementation.
 * 
 * Note that not everything here has been exhaustively tested.  There may be
 * bugs a-lurkin'.
 * 
 * This implementation is written to conform to version 1.0.3 of the BIEF spec.
 * 
 * Latest version:
 *  https://gist.github.com/2585085
 * 
 * Authors:
 *  Daniel Keep <daniel.keep@gmail.com>.
 *  
 * History:
 *  v1.1: Christened format "BIEF".  Changed to big-endian by default.
 *        ReadImage now detects non-BIEF files and reads them as big-endian
 *        raw files instead.
 *  v1.0: First release.
 */
/*
 * Copyright (c) 2012 Daniel Keep.
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a
 * copy of this software and associated documentation files (the "Software"),
 * to deal in the Software without restriction, including without limitation
 * the rights to use, copy, modify, merge, publish, distribute, sublicense,
 * and/or sell copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 * DEALINGS IN THE SOFTWARE.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

// NuGet Package: DotNetZip
using Ionic.Zlib;

namespace Dk.x10c
{
    /// <summary>
    /// This class has methods for reading and writing BIEF
    /// format files, as specified by <https://gist.github.com/2585085>.
    /// </summary>
    public static class BinaryImage
    {
        /// <summary>
        /// Reads a binary image.  Note that this method will work on both
        /// actual BIEF-formatted files and raw big-endian files.
        /// </summary>
        /// <param name="path">Path to the binary image file.</param>
        /// <param name="headers">If non-null, all headers will be
        /// added to this dictionary.  Header keys will be lower case.</param>
        /// <returns>The decoded image data.</returns>
        public static ushort[] ReadImage(string path, Dictionary<string, string> headers = null)
        {
            using (var fs = new FileStream(path, FileMode.Open))
                return ReadImage(fs, headers);
        }

        /// <summary>
        /// Reads a binary image.  Note that this method will work on both
        /// actual BIEF-formatted files and raw big-endian files.
        /// </summary>
        /// <param name="s">Stream to read from.  The stream must be seekable.</param>
        /// <param name="headers">If non-null, all headers will be
        /// added to this dictionary.  Header keys will be lower case.</param>
        /// <returns>The decoded image data.</returns>
        public static ushort[] ReadImage(Stream s, Dictionary<string, string> headers = null)
        {
            // Defaults as per spec
            var byteOrder = ByteOrder.BigEndian;
            var compress = Compression.None;
            var encoding = Encoding.None;
            var payLength = -1;

            // Mark current position
            var headerStart = s.Seek(0, SeekOrigin.Current);

            // Parse headers
            var tr = new StreamReader(s, System.Text.Encoding.UTF8);
            {
                string line = tr.ReadLine();
                {
                    var parts = line.Split(verSep, 2, StringSplitOptions.None);

                    if (parts.Length == 2 && parts[0] == "BIEF")
                    {
                        // Only version 0.1 supported.
                        if (parts[1] != "0.1")
                            throw new Exception("Unsupported version of BIEF: " + parts[1]);
                    }
                    else
                    {
                        // Ok, this means the file isn't BIEF.  Try to read
                        // as big-endian, uncompressed raw.
                        s.Seek(headerStart, SeekOrigin.Begin);
                        return ReadRawImage(s);
                    }
                }

                while ((line = tr.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (line.Length == 0)
                        // No more headers!
                        break;

                    var parts = line.Split(headerSep, 2, StringSplitOptions.None);

                    // We ignore lines not of the form "key: value".
                    if (parts.Length == 2)
                    {
                        var key = parts[0].Trim().ToLower();
                        var val = parts[1].Trim();

                        // Note that this includes standard headers, since users
                        // might be curious.
                        if (headers != null)
                            headers.Add(key, val);

                        switch (key)
                        {
                            case "byte-order":
                                switch (val.ToLower())
                                {
                                    case "big-endian":
                                        byteOrder = ByteOrder.BigEndian;
                                        break;

                                    case "little-endian":
                                        byteOrder = ByteOrder.LittleEndian;
                                        break;
                                }
                                break;

                            case "compression":
                                switch (val.ToLower())
                                {
                                    case "none":
                                        compress = Compression.None;
                                        break;

                                    case "zlib":
                                        compress = Compression.Zlib;
                                        break;
                                }
                                break;

                            case "encoding":
                                switch (val.ToLower())
                                {
                                    case "none":
                                        encoding = Encoding.None;
                                        break;

                                    case "base64":
                                        encoding = Encoding.Base64;
                                        break;
                                }
                                break;

                            case "payload-length":
                                if (!int.TryParse(val, out payLength))
                                    payLength = -1;
                                break;
                        }
                    }
                }
            }

            // Locate end of headers.
            /*
             * This is nasty because StreamReader doesn't deign to tell us
             * how many bytes its consumed and it buffers.  This means that
             * once we've read the headers, we don't *actually* know where in
             * the file we're supposed to be.
             * 
             * Outside of re-writing the header parsing code to avoid the use
             * of StreamReader entirely (*effort*), this is the next-best
             * solution I could think of.
             * 
             * All we do is scan the stream, looking for two consecutive new
             * lines.  gotEol is initialised to true so that if the first
             * thing we see is a newline (i.e. no headers at all), we take
             * that as the end of the headers.
             */
            s.Seek(headerStart, SeekOrigin.Begin);
            long dataStart = 0;
            {
                int b;
                bool gotEol = true,
                     gotCr = false;
                while ((b = s.ReadByte()) >= 0)
                {
                    var c = (char)b;
                    if (gotCr)
                    {
                        gotCr = false;
                        if (c == '\n')
                        {
                            if (gotEol)
                                break;
                            gotEol = true;
                        }
                        else
                            gotEol = false;
                    }
                    else if (b == '\n')
                    {
                        if (gotEol)
                            break;
                        gotEol = true;
                    }
                    else if (b == '\r')
                    {
                        gotCr = true;
                    }
                    else
                        gotEol = false;
                }

                dataStart = s.Seek(0, SeekOrigin.Current);
            }

            // Compute payload length if not specified.
            if (payLength < 0)
            {
                payLength = (int)(s.Seek(0, SeekOrigin.End) - dataStart);
                s.Seek(dataStart, SeekOrigin.Begin);
            }

            // Ensure we're at the start of the payload
            s.Seek(dataStart, SeekOrigin.Begin);

            // Read in the (potentially) encoded data
            var ebytes = new byte[payLength];
            s.Read(ebytes, 0, payLength);

            // Handle decoding to (potentially) compressed bytes.
            byte[] cbytes;
            switch (encoding)
            {
                case Encoding.None:
                    cbytes = ebytes;
                    break;

                case Encoding.Base64:
                    {
                        var str = System.Text.Encoding.UTF8.GetString(ebytes);
                        cbytes = Convert.FromBase64String(str);
                    }
                    break;

                default:
                    throw new NotImplementedException("unknown encoding");
            }

            // Handle decompressing to bytes.
            byte[] bytes;
            switch (compress)
            {
                case Compression.None:
                    bytes = cbytes;
                    break;

                case Compression.Zlib:
                    bytes = zlibDecompress(cbytes);
                    break;

                default:
                    throw new NotImplementedException("unknown compression");
            }

            // Ensure we have complete words.
            if ((bytes.Length & 1) == 1)
                throw new InvalidDataException("malformed binary image: last word is incomplete");

            // Convert to words.
            var words = new ushort[bytes.Length >> 1];
            {
                int lb, hb;
                switch (byteOrder)
                {
                    case ByteOrder.BigEndian:
                        hb = 0;
                        lb = 1;
                        break;

                    case ByteOrder.LittleEndian:
                        lb = 0;
                        hb = 1;
                        break;

                    default:
                        // What, seriously?!
                        throw new NotImplementedException("unknown byte order");
                }

                for (int i = 0, j = 0;
                     i < words.Length;
                     ++i, j += 2)
                {
                    words[i] = (ushort)(bytes[j + lb] | (bytes[j + hb] << 8));
                }
            }

            return words;
        }

        /// <summary>
        /// Writes data to a binary image.
        /// </summary>
        /// <param name="path">Path of the file to write to.</param>
        /// <param name="words">The data to write.</param>
        /// <param name="headers">Additional headers to include in the output.</param>
        /// <param name="compression">Compression scheme to use.</param>
        /// <param name="encoding">Byte encoding scheme to use.</param>
        /// <param name="byteOrder">On-disk byte ordering.</param>
        public static void WriteImage(string path,
            ushort[] words,
            Dictionary<string, string> headers = null,
            Compression compression = Compression.Zlib,
            Encoding encoding = Encoding.Base64,
            ByteOrder byteOrder = ByteOrder.BigEndian)
        {
            using (var fs = new FileStream(path, FileMode.Create))
                WriteImage(fs, words, headers, compression, encoding, byteOrder);
        }

        /// <summary>
        /// Writes data to a binary image.
        /// </summary>
        /// <param name="s">Stream to write to.</param>
        /// <param name="words">The data to write.</param>
        /// <param name="headers">Additional headers to include in the output.</param>
        /// <param name="compression">Compression scheme to use.</param>
        /// <param name="encoding">Byte encoding scheme to use.</param>
        /// <param name="byteOrder">On-disk byte ordering.</param>
        public static void WriteImage(
            Stream s,
            ushort[] words,
            Dictionary<string, string> headers = null,
            Compression compression = Compression.Zlib,
            Encoding encoding = Encoding.Base64,
            ByteOrder byteOrder = ByteOrder.BigEndian)
        {
            // What will eventually contain our encoded bytes.
            byte[] ebytes;

            // Write out headers.
            /* NOTE: we can't dispose this since it also disposes the
             * underlying stream.  How *thoughtful*.
             */
            var tw = new StreamWriter(s, System.Text.Encoding.ASCII);
            {
                tw.NewLine = "\r\n";

                tw.WriteLine("BIEF/0.1");

                // Write out extra headers.  If these include standard headers,
                // it won't matter because we'll supercede them.
                if (headers != null)
                    foreach (var kv in headers)
                        tw.WriteLine("{0}: {1}", kv.Key, kv.Value);

                int lb, hb;
                switch (byteOrder)
                {
                    case ByteOrder.BigEndian:
                        // Don't output an explicit header for this case.
                        //tw.WriteLine("Byte-Order: Big-Endian");
                        hb = 0;
                        lb = 1;
                        break;

                    case ByteOrder.LittleEndian:
                        tw.WriteLine("Byte-Order: Little-Endian");
                        lb = 0;
                        hb = 1;
                        break;

                    default:
                        // WAT.
                        throw new NotImplementedException("unknown byte order");
                }

                // Convert words into byte-ordered bytes.
                var bytes = new byte[2 * words.Length];
                for (int i = 0, j = 0;
                     i < words.Length;
                     ++i, j += 2)
                {
                    bytes[j + lb] = (byte)words[i];
                    bytes[j + hb] = (byte)(words[i] >> 8);
                }

                // Compress
                byte[] cbytes;
                switch (compression)
                {
                    case Compression.None:
                        // Default
                        //tw.WriteLine("Compression: None");
                        cbytes = bytes;
                        break;

                    case Compression.Zlib:
                        tw.WriteLine("Compression: Zlib");
                        cbytes = zlibCompress(bytes);
                        break;

                    default:
                        throw new NotImplementedException("unknown compression");
                }

                // Encode
                switch (encoding)
                {
                    case Encoding.None:
                        // Default.
                        //tw.WriteLine("Encoding: None");
                        ebytes = cbytes;
                        break;

                    case Encoding.Base64:
                        tw.WriteLine("Encoding: Base64");
                        {
                            var str = Convert.ToBase64String(cbytes, Base64FormattingOptions.InsertLineBreaks);
                            ebytes = System.Text.Encoding.ASCII.GetBytes(str);
                        }
                        break;

                    default:
                        throw new NotImplementedException("unknown encoding");
                }

                // Output payload-length header.
                tw.WriteLine("Payload-Length: {0}", ebytes.Length);

                // Terminate headers
                tw.WriteLine();
                tw.Flush();
            }

            // Write encoded bytes out; and we're done!
            s.Write(ebytes, 0, ebytes.Length);
        }

        /// <summary>
        /// Reads a raw, unencapsulated binary image, performing byte swapping
        /// as necessary.
        /// </summary>
        /// <param name="path">Path to the file to read from.</param>
        /// <param name="byteOrder">Byte order of the file.</param>
        /// <returns>The contents of the file.</returns>
        public static ushort[] ReadRawImage(
            string path,
            ByteOrder byteOrder = ByteOrder.BigEndian)
        {
            using (var fs = new FileStream(path, FileMode.Open))
                return ReadRawImage(fs, byteOrder);
        }

        /// <summary>
        /// Reads a raw, unencapsulated binary image, performing byte swapping
        /// as necessary.
        /// </summary>
        /// <param name="s">Stream to read from.</param>
        /// <param name="byteOrder">Byte order of the file.</param>
        /// <returns>The contents of the file.</returns>
        public static ushort[] ReadRawImage(
            Stream s,
            ByteOrder byteOrder = ByteOrder.BigEndian)
        {
            var start = s.Seek(0, SeekOrigin.Current);
            var end = s.Seek(0, SeekOrigin.End);
            s.Seek(start, SeekOrigin.Begin);

            var length = (int)(end - start);

            if ((length & 1) == 1)
                throw new InvalidDataException("malformed binary image: last word is incomplete");

            var bytes = new byte[length];
            s.Read(bytes, 0, length);
            var words = new ushort[length >> 1];

            int lb, hb;
            if (byteOrder == ByteOrder.LittleEndian)
            {
                lb = 0;
                hb = 1;
            }
            else
            {
                hb = 0;
                lb = 1;
            }

            for (int i = 0, j = 0;
                     i < words.Length;
                     ++i, j += 2)
            {
                words[i] = (ushort)(bytes[j + lb] | (bytes[j + hb] << 8));
            }

            return words;
        }

        /// <summary>
        /// Writes a raw, unencapsulated binary image, performing byte swapping
        /// as necessary.
        /// </summary>
        /// <param name="path">Path to the file to write to.</param>
        /// <param name="words">Data to write.</param>
        /// <param name="byteOrder">Byte order of the file.</param>
        public static void WriteRawImage(
            string path,
            ushort[] words,
            ByteOrder byteOrder = ByteOrder.BigEndian)
        {
            using (var fs = new FileStream(path, FileMode.Create))
                WriteRawImage(fs, words, byteOrder);
        }

        /// <summary>
        /// Writes a raw, unencapsulated binary image, performing byte swapping
        /// as necessary.
        /// </summary>
        /// <param name="s">Stream to write to.</param>
        /// <param name="words">Data to write.</param>
        /// <param name="byteOrder">Byte order of the file.</param>
        public static void WriteRawImage(
            Stream s,
            ushort[] words,
            ByteOrder byteOrder = ByteOrder.BigEndian)
        {
            var bytes = new byte[words.Length << 1];

            int lb, hb;
            if (byteOrder == ByteOrder.LittleEndian)
            {
                lb = 0;
                hb = 1;
            }
            else
            {
                hb = 0;
                lb = 1;
            }

            for (int i = 0, j = 0;
                 i < words.Length;
                 ++i, j += 2)
            {
                bytes[j + lb] = (byte)words[i];
                bytes[j + hb] = (byte)(words[i] >> 8);
            }

            s.Write(bytes, 0, bytes.Length);
        }

        private static byte[] zlibCompress(byte[] bytes)
        {
            return ZlibStream.CompressBuffer(bytes);
        }

        private static byte[] zlibDecompress(byte[] cbytes)
        {
            return ZlibStream.UncompressBuffer(cbytes);
        }

        static BinaryImage()
        {
            verSep = new char[] { '/' };
            headerSep = new char[] { ':' };
        }

        private static readonly char[] verSep;
        private static readonly char[] headerSep;
    }

    public enum ByteOrder
    {
        BigEndian,
        LittleEndian,
    }

    public enum Encoding
    {
        None,
        Base64,
    }

    public enum Compression
    {
        None,
        Zlib,
    }
}
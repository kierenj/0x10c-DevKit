using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Devkit.Interfaces;
using Devkit.Interfaces.Build;

namespace Devkit.BinaryResourcePlugin
{
    public class BinaryFileDataInstruction : Instruction
    {
        private readonly string _filename;

        public BinaryFileDataInstruction(string filename, IEnumerable<SourceReference> sourceRefs)
            : base(sourceRefs)
        {
            this._filename = filename;
        }

        public override MemoryRangeType RangeType
        {
            get { return MemoryRangeType.Data; }
        }

        public override int GetSize(CompileToolContext context)
        {
            return (int)(new FileInfo(this._filename).Length + 1) / 2;
        }

        public override ushort[] GetWords(CompileToolContext context)
        {
            var wordArr = ReadBytes().ToArray();
            if (wordArr.Length != GetSize(context)) throw new ApplicationException("Word count mismatch");
            return wordArr;
        }

        public override IEnumerable<int> GetRelocatableWordIndices(CompileToolContext context)
        {
            return new int[] { };
        }

        private IEnumerable<ushort> ReadBytes()
        {
            var bytes = File.ReadAllBytes(this._filename);
            ushort currentWord = 0;
            bool firstByte = true;
            foreach (var fileByte in bytes)
            {
                if (firstByte)
                {
                    currentWord = fileByte;
                }
                else
                {
                    currentWord = (ushort)(currentWord + (fileByte << 8));
                    yield return currentWord;
                }
                firstByte = !firstByte;
            }

            if (!firstByte)
                yield return currentWord;
        }
    }
}

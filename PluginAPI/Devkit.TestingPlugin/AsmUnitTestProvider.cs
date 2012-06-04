using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Devkit.Interfaces;
using Devkit.Interfaces.Build;

namespace Devkit.TestingPlugin
{
    public class AsmUnitTestProvider : IFileTypeProvider
    {
        private readonly IWorkspace _workspace;
        private readonly IFileTypeProvider _asmProvider;

        public string FileTypeName
        {
            get { return "10c assembly unit test"; }
        }

        public IEnumerable<string> DefaultFileExtensions
        {
            get { yield return ".10ctest"; }
        }

        public bool CanCreateNew
        {
            get { return true; }
        }

        public bool IsTextual
        {
            get { return true; }
        }

        public string GetDefaultFileContent(IOpenFile openFile)
        {
            return this._asmProvider.GetDefaultFileContent(openFile) + @"

#segment code
; ========================================================================
; user unit test code function
; ========================================================================
:unit_test

    ; enter unit test code here. you can jump to .pass or .fail
    ; to signal success or failure
    
    ; unit tests will also fail after 1,000,000 cycles by default
    
    	
.pass:
    set a, UNIT_TEST_PASS
    hwi [debugger_device]
    
.fail:
	set a, UNIT_TEST_FAIL
	hwi [debugger_device]

.hang:
    set pc, .hang

; ========================================================================
; unit test support code (debugger device interface)
; ========================================================================
#define NO_DEVICE 0xffff
#define TRIGGER_BREAKPOINT 0
#define OUTPUT_DIAG_WORD 1
#define OUTPUT_DIAG_WORD_ZSTRING 2
#define OUTPUT_DIAG_WORD_PSTRING 3
#define UNIT_TEST_PASS 4
#define UNIT_TEST_FAIL 5
#define SET_PROCESSOR_SPEED 6
#define GET_PROCESSOR_SPEED 7
#define RESET_CYCLE_COUNTER 8
#define GET_CYCLE_COUNTER 9

#segment data
:debugger_device
    dat 0

#segment boot
    :detect_debugger
        hwn z
    .loop
        sub z, 1
        hwq z
        ife b,0xdeb9
            ife a,0x1111
                set pc, .found
        ifn z, 0
            set pc, .loop
    .notfound
        set z, NO_DEVICE
    .found
        set [debugger_device], z
        set pc, unit_test
";
        }

        public IEditorControlStrategy EditorControlStrategy
        {
            get { return this._asmProvider.EditorControlStrategy; }
        }

        public AsmUnitTestProvider(IWorkspace workspace)
        {
            this._workspace = workspace;
            this._asmProvider = workspace.BuildManager.GetFileBuildProvider(".10c");
        }

        public ISourceFileScope CreateFileScope(IFile file, IProjectScope projectScope, CompileToolContext context)
        {
            return this._asmProvider.CreateFileScope(file, projectScope, context);
        }
    }
}

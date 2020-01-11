using System;
using System.Collections.Generic;
using System.Text;

namespace Rabbit.Classes.BrainFuckEmulator
{
    public class StaticsAndDefaults
    {
        #region PublicConst
        public const char OperatorBegin = '[';
        public const char OperatorEnd = ']';
        public const char OperatorInc = '+';
        public const char OperatorDec = '-';
        public const char OperatorLeft = '<';
        public const char OperatorRight = '>';
        public const char OperatorOutput = '.';
        public const char OperatorInput = ',';

        public const int OperatorBeginCode = 101;
        public const int OperatorEndCode = 102;
        public const int OperatorIncCode = 103;
        public const int OperatorDecCode = 104;
        public const int OperatorLeftCode = 105;
        public const int OperatorRightCode = 106;
        public const int OperatorOutputCode = 107;
        public const int OperatorInputCode = 108;

        public const int OperatorInputInit = 0900;
        public const int OperatorLeftInit = 1010;
        public const int OperatorRightInit = 1030;
        public const int OperatorEndInit = 1040;
        public const int OperatorOutputInit = 1050;
        public const int OperatorDecInit = 1060;
        public const int OperatorBeginInit = 1070;
        public const int OperatorIncInit = 1080;

        public const int MaxNestLoop = 2;
        public const int MaxLoopRun = 500;
        public const bool DefaultVerbose = true;
        public const bool DefaultShowOutput = true;
        #endregion
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchRename
{
    public class NewCaseArgs : StringArgs
    {
        public char Mode { get; set; }
    }

    public class NewCaseOperation : StringOperation
    {
        public override string Name => "New Case";

        public override string Description {
            get {
                var arg = Args as NewCaseArgs;

                string detail=null;
                switch(arg.Mode)
                {
                    case 'a':
                        detail = "lower"; break;
                    case 'b':
                        detail = "upper"; break;
                    case 'c':
                        detail = "upper first letter"; break;
                }

                return ($"Change the case format of the string to {detail}");
            }
        }

        public NewCaseOperation()
        {
            Args = new NewCaseArgs()
            {
                Mode = 'a',
            };
        }

        static string UpperFirstLetter(string input)
        {
            StringBuilder result = new StringBuilder(input);

            if (result[0] >= 'a' && result[0] <= 'z')
            {
                result[0] = (char)('A' + (input[0] - 'a'));
            }

            for (int i = 1; i < input.Length; i++)
            {
                if (input[i] >= 'a' && input[i] <= 'z' && input[i - 1] == ' ')
                {
                    result[i] = (char)('A' + (input[i] - 'a')); //change le
                }
            }
            return result.ToString();
        }

        public override void OpenDialog()
        {
            throw new NotImplementedException();
        }

        public override string OperateString(string origin)
        {
            string result = null;
            var arg = Args as NewCaseArgs;

            switch (arg.Mode)
            {
                case 'a':

                    result = origin.ToLower();
                    break;

                case 'b':

                    result = origin.ToUpper();
                    break;

                case 'c':

                    result = UpperFirstLetter(origin);
                    break;

            }
            return result;
        }


    }
}

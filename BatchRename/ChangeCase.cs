using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchRename
{
    public class CaseArg : OptArgs
    {
        public string Case { get; set; }
    }

    public class ChangeCaseStringOperation : StringOperations
    {
        public override string Name => "Change Case";

        public override string Description {
            get {
                var arg = Arguments as CaseArg;
                return ($"Change the case format of the string to {arg.Case}");
            }
        }

        public ChangeCaseStringOperation()
        {
            Arguments = new CaseArg()
            {
                Case = "Lower",
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

        public override string Operate(string input)
        {
            string result = input;
            var arg = Arguments as CaseArg;

            if (arg.Case == "Lower")
            {
                result = input.ToLower();
            }
            if (arg.Case == "Upper")
            {
                result = input.ToUpper();
            }
            if (arg.Case == "Upper First Letter")
            {
                result = UpperFirstLetter(input);
            }
            return result;
        }


    }
}

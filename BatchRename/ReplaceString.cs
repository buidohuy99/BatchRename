using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchRename
{
    // CLASS REPLACE STRING
    public class ReplaceArgs : OptArgs
    {
        public string From { get; set; }
        public string To { get; set; }
    }

    public class ReplaceStringOperation : StringOperations
    {
        public override string Name => "Replace";

        public override string Description
        {
            get
            {
                string from = (Arguments as ReplaceArgs).From;
                string to = (Arguments as ReplaceArgs).To;
                return ($"Replace substring {from} with {to}");
            }
        }

        public ReplaceStringOperation()
        {
            Arguments = new ReplaceArgs()
            {
                From = "From",
                To = "To"
            };
        }

        public override string Operate(string input)
        {
            string result;
            var args = Arguments as ReplaceArgs;
            result = input.Replace(args.From, args.To);
            return result;
        }

        public override void OpenDialog()
        {
            var screen = new ReplaceStringDialog(Arguments);
            screen.OptArgsChange += ChangeReplaceArgs;
            if (screen.ShowDialog() == true)
            {
            }
        }

        void ChangeReplaceArgs(string from, string to)
        {
            (Arguments as ReplaceArgs).From = from;
            (Arguments as ReplaceArgs).To = to;
        }
    }
}

using BatchRename.UtilsClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BatchRename
{
    public abstract class StringArgs
    {
        
    }

    public abstract class StringOperation : INotifyPropertyChanged
    {
        public StringOperation()
        {
            OpenDialogCommand = new DelegateCommand(temp => this.OpenDialog(),null);
        }

        public StringArgs Args { get; set; }

        public abstract string Name { get;}

        public abstract string Description { get;}
        
        public event PropertyChangedEventHandler PropertyChanged;

        public abstract string OperateString(string origin);

        public abstract void OpenDialog();

        public DelegateCommand OpenDialogCommand { get; private set; }
    }


    ////CLASS NEW CASE

    //public class CaseArg : OptArgs
    //{
    //    public string Case { get; set; }
    //}

    //public class NewCaseStringOperation : StringOperations
    //{
    //    static string UpperFirstLetter(string input)
    //    {
    //        StringBuilder result = new StringBuilder(input);

    //        if (result[0] >= 'a' && result[0] <= 'z')
    //        {
    //            result[0] = (char)('A' + (input[0] - 'a'));
    //        }

    //        for (int i = 1; i < input.Length; i++)
    //        {
    //            if (input[i] >= 'a' && input[i] <= 'z' && input[i - 1] == ' ')
    //            {
    //                result[i] = (char)('A' + (input[i] - 'a')); //change le
    //            }
    //        }
    //        return result.ToString();
    //    }

    //    public override string Operate(string input)
    //    {
    //        string result = input;
    //        var arg = Arguments as CaseArg;

    //        if (arg.Case == "Lower")
    //        {
    //            result = input.ToLower();
    //        }
    //        if (arg.Case == "Upper")
    //        {
    //            result = input.ToUpper();
    //        }
    //        if (arg.Case == "Upper First Letter")
    //        {
    //            result = UpperFirstLetter(input);
    //        }
    //        return result;
    //    }


    //}


    ////FULL NAME NORMALIZATION CLASS

    //public class NormalizationStringOperation : StringOperations
    //{
    //    public override string Operate(string input)
    //    {
    //        string result = String.Join(" ", input.Split(new char[] { ' ' },
    //            StringSplitOptions.RemoveEmptyEntries));
    //        return result;
    //    }
    //}

    ////GUID CLASS
    //public class UniqueNameStringOperation : StringOperations
    //{
    //    public override string Operate(string input)
    //    {
    //        Guid guid = Guid.NewGuid();
    //        string result = guid.ToString();
    //        return result;
    //    }
    //}
}

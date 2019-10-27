using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchRename
{

    public class MoveArgs : StringArgs
    {
        public int Mode { get; set; }
    }
    public class MoveOperation : StringOperation
    {
        public override string Name => "Move";
 
        public override string Description
        {
            get
            {
                string result = null;
                switch ((Args as MoveArgs).Mode)
                {
                    case 1:
                        result = "Move ISBN characters to after the name";
                        break;
                    case 2:
                        result = "Move ISBN chacracters to after the name";
                        break;
                }
                return result;
            }
        }

        public MoveOperation()
        {
            Args = new MoveArgs()
            {
                Mode = 1
            };
        }


        public override string OperateString(string origin)
        {
            string result = null;

            switch((Args as MoveArgs).Mode)
            {
                case 1:
                    result = preISBN(origin);
                    break;
                case 2:
                    result = postISBN(origin);
                    break;
            }

            return result;
        }

        /// <summary>
        /// Move ISBN characters to before the name
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        private static string preISBN(string origin)
        {
            string ISBN = origin.Substring(origin.Length - 13, 13);
            string result = ISBN + origin.Substring(0, origin.Length - 13);
            return result;
        }

        /// <summary>
        /// Move ISBN characters to after the name
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        private static string postISBN(string origin)
        {
            string ISBN = origin.Substring(0, 13);
            string result = origin.Substring(14) + ISBN;
            return result;
        }

        public override void OpenDialog()
        {
            throw new NotImplementedException();
        }
    }
}

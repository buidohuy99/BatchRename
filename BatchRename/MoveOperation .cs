using System;

namespace BatchRename
{

    public class MoveArgs : StringArgs
    {
        public int Mode { get; set; }
        public int Number { get; set; }
    }
    public class MoveOperation : StringOperation
    {
        public override string Name => "Move";
 
        public override string Description
        {
            get
            {
                string result = null;
                var arg = Args as MoveArgs;
                switch (arg.Mode)
                {
                    case 1:
                        result = $"Move {arg.Number} characters to front of the string";
                        break;
                    case 2:
                        result = $"Move {arg.Number} chacracters to back of the string";
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
                    try
                    {
                        result = BringToFront(origin, (Args as MoveArgs).Number);
                        break;
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                    }
                    
                case 2:
                    try
                    {
                        result = BringToBack(origin, (Args as MoveArgs).Number);
                        break;
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.Message);
                    }
            }

            return result;
        }

        /// <summary>
        /// Move ISBN characters to before the name
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        private static string BringToFront(string origin, int num)
        {
            if (num > origin.Length)
            {
                throw new Exception("Substring wanted to move is longer than the string itself");
            }

            string substring = origin.Substring(origin.Length - num, num);
            string result = substring + origin.Substring(0, origin.Length - num);
            return result;
        }

        /// <summary>
        /// Move ISBN characters to after the name
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        private static string BringToBack(string origin, int num)
        {
            if (num > origin.Length)
            {
                throw new Exception("Substring wanted to move is longer than the string itself");
            }

            string substring = origin.Substring(0, num);
            string result = origin.Substring(num + 1) + substring;
            return result;
        }

        public override void OpenDialog()
        {
            throw new NotImplementedException();
        }

        public override StringOperation Clone()
        {
            throw new NotImplementedException();
        }
    }
}

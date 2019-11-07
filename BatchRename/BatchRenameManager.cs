using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BatchRename.UtilsClass;

namespace BatchRename
{
    class BatchRenameError
    {
        public int NameErrorIndex { get; set; }
        public string LastNameValue { get; set; }
        public string Message { get; set; }
    }

    class BatchRenameManager
    {
        private List<FileInfo> FileList;
        private List<string> NewFileNames;
        private List<BatchRenameError> errors;
        private int NameCount;

        /// <summary>
        /// create manager to manage String Batch Renaming
        /// </summary>
        /// <param name="StringNames">names wanted to change</param>
        /// <param name="Operations">String operation wanted to perform on input names</param>
        public BatchRenameManager()
        {

            errors = new List<BatchRenameError>();
            FileList = new List<FileInfo>();
            NewFileNames = new List<string>();
        }

        public List<string> GetErrorList()
        {
            List<string> result = new List<string>();
            for (int i = 0; i < NameCount; i++) //fill list with default vaule
            {
                result.Add("None");
            }
            for (int i = 0; i < errors.Count; i++)
            {
                int ErrorIndex = errors[i].NameErrorIndex;
                string Message = errors[i].Message;
                result[ErrorIndex] = Message;
            }
            return result;
        }

        public List<FileObj> BatchRename(List<FileObj> fileList, List<StringOperation> operations)
        { 
            List<FileObj> result = new List<FileObj>(fileList);
            if (NewFileNames.Count != 0) // clear list to save new changed names
            {
                NewFileNames.Clear();
            }

            for (int i = 0; i < fileList.Count; i++)
            {
                string path = fileList[i].Path + "\\" + fileList[i].Name;
                FileInfo fileInfo = new FileInfo(path);
                FileList.Add(fileInfo);
                NewFileNames.Add(fileInfo.Name);
            }

            

            for (int i = 0; i < operations.Count; i++)
            {

                for (int j = 0; j < result.Count; j++)
                {
                    try
                    {
                        NewFileNames[j] = operations[i].OperateString(NewFileNamess[i]); // perform operation
                    }
                    catch (Exception e) //if operation has failed
                    {
                        BatchRenameError error = new BatchRenameError()
                        {
                            NameErrorIndex = j, // save the position of the string which caused the error
                            LastNameValue = NewFileNames[j], //save the last values of the string before error
                            Message = e.Message, //the error message
                        };
                        errors.Add(error);
                        NewFileNames.RemoveAt(j); //remove the string from the Batch reanaming list so that it can't be changed by later Operations
                        j--;
                    }
                }
            }
            //add the faulty names back to the list
            for (int i = 0; i < errors.Count; i++)
            {
                string ErrorString = errors[i].LastNameValue;
                int index = errors[i].NameErrorIndex;
                NewFileNames.Insert(index, ErrorString);
            }

            //attach file name with its file extension and error messages that goes along with it if there's one
            List<string> ErrorMessages = GetErrorList();

            for (int i = 0; i <fileList.Count; i++)
            {
                NewFileNames[i] += FileList[i].Extension;
                result[i].NewName = NewFileNames[i];
                result[i].Error = ErrorMessages[i];
            }

            
            return result;

        }

        public void HandleDuplicate()
        {

        }


        public void CommitChange()
        {
            
            for (int i = 0; i < FileList.Count; i++)
            {
                string newPath = FileList[i].Directory + "\\" + NewFileNames[i];
            }


        }

        //public List<string> StartBatching(string[] names, List<StringOperation> operations)
        //{
        //    NameCount = names.Length;
        //    List<string> result = new List<string>(names);

        //    for (int i = 0; i < operations.Count; i++)
        //    {

        //        for (int j = 0; j < result.Count; j++)
        //        {
        //            try
        //            {
        //                result[j] = operations[i].OperateString(result[j]); // perform operation
        //            }
        //            catch (Exception e) //if operation has failed
        //            {
        //                BatchRenameError error = new BatchRenameError()
        //                {
        //                    NameErrorIndex = j, // save the position of the string which caused the error
        //                    LastNameValue = names[j], //save the last values of the string before error
        //                    Message = e.Message, //the error message
        //                };
        //                errors.Add(error);
        //                result.RemoveAt(j); //remove the string from the Batch reanaming list so that it can't be changed by later Operations
        //                j--;
        //            }
        //        }
        //    }
        //    //add the faulty names back to the list
        //    for (int i = 0; i < errors.Count; i++)
        //    {
        //        string ErrorString = errors[i].LastNameValue;
        //        int index = errors[i].NameErrorIndex;
        //        result.Insert(index, ErrorString);
        //    }
        //    return result;
        //}

        
    }


}

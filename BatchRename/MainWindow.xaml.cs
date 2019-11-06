using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;

using BatchRename.UtilsClass;

namespace BatchRename
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BindingList<FileObj> filesList;
        private BindingList<FolderObj> foldersList;
        private List<StringOperationPrototype> addMethodPrototypes;
        public BindingList<StringOperation> operationsList;

        public BindingList<StringOperation> FileOperationsList;
        public BindingList<StringOperation> FolderOperationsList;

        private BackgroundWorker fetchFilesWorker;
        private BackgroundWorker excludeFilesWorker;
        private BackgroundWorker fetchFoldersWorker;
        private BackgroundWorker excludeFoldersWorker;

        public MainWindow()
        {
            InitializeComponent();
            filesList = new BindingList<FileObj>();
            foldersList = new BindingList<FolderObj>();

            
            FileOperationsList = new BindingList<StringOperation>();
            FolderOperationsList = new BindingList<StringOperation>();
            operationsList = FileOperationsList;//new BindingList<StringOperation>();

            //Populate prototypes
            addMethodPrototypes = new List<StringOperationPrototype>
            {
                new StringOperationPrototype(new ReplaceOperation(), this),
                new StringOperationPrototype(new NewCaseStringOperation(), this),
                new StringOperationPrototype(new MoveOperation(), this),
                new StringOperationPrototype(new FullnameNormalizeOperation(), this),
                new StringOperationPrototype(new UniqueNameOperation(), this)
            };

            //Bind
            RenameFilesList.ItemsSource = filesList;
            RenameFoldersList.ItemsSource = foldersList;
            AddMethodButton.ContextMenu.ItemsSource = addMethodPrototypes;
            OperationsList.ItemsSource = operationsList;

            //Create fetch files worker to invoke on click
            fetchFilesWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            fetchFilesWorker.DoWork += FetchFiles_DoWork;
            fetchFilesWorker.ProgressChanged += ProgressChanged;
            fetchFilesWorker.RunWorkerCompleted += RunWorkerCompleted;

            //Create exclude files worker to invoke on click
            excludeFilesWorker = new BackgroundWorker {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true,
            };
            excludeFilesWorker.DoWork += ExcludeFiles_DoWork;
            excludeFilesWorker.ProgressChanged += ProgressChanged;
            excludeFilesWorker.RunWorkerCompleted += RunWorkerCompleted;

            //Create fetch folders worker to invoke on click
            fetchFoldersWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            fetchFoldersWorker.DoWork += FetchFolders_DoWork;
            fetchFoldersWorker.ProgressChanged += ProgressChanged;
            fetchFoldersWorker.RunWorkerCompleted += RunWorkerCompleted;

            //Create exclude folders worker to invoke on click
            excludeFoldersWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true,
            };
            excludeFoldersWorker.DoWork += ExcludeFolders_DoWork;
            excludeFoldersWorker.ProgressChanged += ProgressChanged;
            excludeFoldersWorker.RunWorkerCompleted += RunWorkerCompleted;

        }

        //------------------------------Background Workers---------------------------------

        private void DisableLoadingViews()
        {
            AddFileButton.IsEnabled = false;
            ExcludeFileButton.IsEnabled = false;
            AddFolderButton.IsEnabled = false;
            ExcludeFolderButton.IsEnabled = false;
            StartButton.IsEnabled = false;
        }

        private void EnableLoadingViews()
        {
            AddFileButton.IsEnabled = true;
            ExcludeFileButton.IsEnabled = true;
            AddFolderButton.IsEnabled = true;
            ExcludeFolderButton.IsEnabled = true;
            StartButton.IsEnabled = true;
        }

        private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            LoadingBar.Value = 100;
            Mouse.OverrideCursor = null;
            LoadingOutput.Text = "Action completed";

            EnableLoadingViews();
        }

        private void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            LoadingBar.Value = e.ProgressPercentage;
            if(e.UserState != null)
                LoadingOutput.Text = (string)e.UserState;
        }

        private void FetchFiles_DoWork(object sender, DoWorkEventArgs e)
        {
            string path = (string)e.Argument + "\\";
            var children = Directory.GetFiles(path);
            StringBuilder output = new StringBuilder();

            for (int child = 0; child < children.Length; child++)
            {
                bool isDuplicated = false;
                string childName = children[child].Remove(0, path.Length);

                //Check duplicates
                for (int i = 0; i < filesList.Count; i++)
                {
                    if (filesList[i].Name.Equals(childName) && filesList[i].Path.Equals(path))
                    {
                        isDuplicated = true;
                        break;
                    }
                }

                output.Clear();
                string result = "Skip duplicate ";
                if (!isDuplicated) { 
                    result = "Add ";
                    Dispatcher.Invoke(() =>
                    {
                        filesList.Add(new FileObj() { Name = childName, Path = path });
                    });
                }
                output.Append(result);
                output.Append(path);
                output.Append(childName);
                
                fetchFilesWorker.ReportProgress((child * 100/children.Length), output.ToString());
            }
        }

        private void ExcludeFiles_DoWork(object sender, DoWorkEventArgs e)
        {
            var items = ((IList<object>)e.Argument).Cast<FileObj>().ToList();

            int amount = items.Count;

            StringBuilder output = new StringBuilder();

            for (int item = 0; item < amount; item++)
            {
                Dispatcher.Invoke(()=> {
                    filesList.Remove(items[item]);
                });
                output.Clear();
                output.Append("Remove ");
                output.Append(items[item].Path + items[item].Name);
                excludeFilesWorker.ReportProgress((item * 100 / amount), output.ToString());
            }
        }

        private void FetchFolders_DoWork(object sender, DoWorkEventArgs e)
        {
            string path = (string)e.Argument + "\\";
            var children = Directory.GetDirectories(path);
            StringBuilder output = new StringBuilder();

            for (int child = 0; child < children.Length; child++)
            {
                bool isDuplicated = false;
                string childName = children[child].Remove(0, path.Length);

                //Check duplicates
                for (int i = 0; i < foldersList.Count; i++)
                {
                    if (foldersList[i].Name.Equals(childName) && foldersList[i].Path.Equals(path))
                    {
                        isDuplicated = true;
                        break;
                    }
                }

                output.Clear();
                string result = "Skip duplicate ";
                if (!isDuplicated)
                {
                    result = "Add ";
                    Dispatcher.Invoke(() =>
                    {
                        foldersList.Add(new FolderObj() { Name = childName, Path = path });
                    });
                }
                output.Append(result);
                output.Append(path);
                output.Append(childName);

                fetchFoldersWorker.ReportProgress((child * 100 / children.Length), output.ToString());
            }
        }

        private void ExcludeFolders_DoWork(object sender, DoWorkEventArgs e)
        {
            var items = ((IList<object>)e.Argument).Cast<FolderObj>().ToList();

            int amount = items.Count;

            StringBuilder output = new StringBuilder();

            for (int item = 0; item < amount; item++)
            {
                Dispatcher.Invoke(() => {
                    foldersList.Remove(items[item]);
                });
                output.Clear();
                output.Append("Remove ");
                output.Append(items[item].Path + items[item].Name);
                excludeFoldersWorker.ReportProgress((item * 100 / amount), output.ToString());
            }
        }

        //-------------------------Button Clicks Implements---------------------------------

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            LoadingBar.Value = 0;
            LoadingOutput.Text = "";
            filesList.Clear();
            foldersList.Clear();
            Mouse.OverrideCursor = null;
        }

        private void OptionButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddMethodButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement addButton)
            {
                addButton.ContextMenu.PlacementTarget = addButton;
                addButton.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
                addButton.ContextMenu.MinWidth = addButton.ActualWidth;
                addButton.ContextMenu.MinHeight = 30;
                addButton.ContextMenu.Margin = new Thickness(0,5,0,0);
                addButton.ContextMenu.IsOpen = true;
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (fetchFilesWorker.IsBusy || fetchFoldersWorker.IsBusy 
                || excludeFilesWorker.IsBusy || excludeFoldersWorker.IsBusy) return;
            var dialog = new FolderBrowserDialog();

            if(dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

                DisableLoadingViews();
                LoadingBar.Value = 0;
                fetchFilesWorker.RunWorkerAsync(dialog.SelectedPath);
            }

        }

        private void ExcludeFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (fetchFilesWorker.IsBusy || fetchFoldersWorker.IsBusy
                || excludeFilesWorker.IsBusy || excludeFoldersWorker.IsBusy 
                || filesList.Count <= 0) return;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

            DisableLoadingViews();
            LoadingBar.Value = 0;
            excludeFilesWorker.RunWorkerAsync(RenameFilesList.SelectedItems);
        }

        private void AddFolderButton_Click(object sender, RoutedEventArgs e)
        {
            if (fetchFilesWorker.IsBusy || fetchFoldersWorker.IsBusy
                || excludeFilesWorker.IsBusy || excludeFoldersWorker.IsBusy) return;
            var dialog = new FolderBrowserDialog();

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

                DisableLoadingViews();
                LoadingBar.Value = 0;
                fetchFoldersWorker.RunWorkerAsync(dialog.SelectedPath);
            }
        }

        private void ExcludeFolderButton_Click(object sender, RoutedEventArgs e)
        {
            if (fetchFilesWorker.IsBusy || fetchFoldersWorker.IsBusy
                || excludeFilesWorker.IsBusy || excludeFoldersWorker.IsBusy
                || foldersList.Count <= 0) return;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

            DisableLoadingViews();
            LoadingBar.Value = 0;
            excludeFoldersWorker.RunWorkerAsync(RenameFoldersList.SelectedItems);
        }

        private void OperationsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var item = OperationsList.SelectedItem as StringOperation;
            try
            {
                item.OpenDialog();
            }
            catch
            {
                var screen = new NoEditAvailableDialog();
                screen.ShowDialog();
            }
            
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            StringOperation local = ((sender as System.Windows.Controls.Button).Tag as StringOperation);
            operationsList.Remove(local);
        }

        private void RenameTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((string)(RenameTabControl.SelectedItem as TabItem).Header == "Rename Files")
            {
                operationsList = FileOperationsList;
                OperationsList.ItemsSource = operationsList;
                
                //FolderOperationsList = new List<StringOperation>(operationsList);
                //operationsList = new BindingList<StringOperation>(FileOperationsList);
                //OperationsList.ItemsSource = operationsList;


                //operationsList.Clear();

                //for (int i = 0; i < FileOperationsList.Count; i++)
                //{
                //    operationsList.Add(FileOperationsList[i]);
                //}
                
            }
            if ((string)(RenameTabControl.SelectedItem as TabItem).Header == "Rename Folders")
            {
                operationsList = FolderOperationsList;
                OperationsList.ItemsSource = operationsList;

                //FileOperationsList = new List<StringOperation>(operationsList);
                //operationsList = new BindingList<StringOperation>(FolderOperationsList);
                //OperationsList.ItemsSource = operationsList;

                //operationsList.Clear();
                //for (int i = 0; i < FolderOperationsList.Count; i++)
                //{
                //    operationsList.Add(FolderOperationsList[i]);
                //}
            }
        }


        private void PreviewButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

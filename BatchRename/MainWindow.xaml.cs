using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
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
        private BindingList<StringOperation> addMethodPrototypes;

        private BackgroundWorker fetchFilesWorker;
        private BackgroundWorker excludeFilesWorker;

        public MainWindow()
        {
            InitializeComponent();
            filesList = new BindingList<FileObj>();
            foldersList = new BindingList<FolderObj>();
            addMethodPrototypes = new BindingList<StringOperation>();

            //Populate prototypes
            addMethodPrototypes.Add(new ReplaceOperation());

            //Bind
            RenameFilesList.ItemsSource = filesList;
            RenameFoldersList.ItemsSource = foldersList;
            AddMethodButton.ContextMenu.ItemsSource = addMethodPrototypes;

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


        }

        //------------------------------Background Workers---------------------------------

        private void DisableLoadingViews()
        {
            AddFileButton.IsEnabled = false;
            ExcludeFileButton.IsEnabled = false;
            AddFolderButton.IsEnabled = false;
            ExcludeFolderButton.IsEnabled = false;
        }

        private void EnableLoadingViews()
        {
            AddFileButton.IsEnabled = true;
            ExcludeFileButton.IsEnabled = true;
            AddFolderButton.IsEnabled = true;
            ExcludeFolderButton.IsEnabled = true;
        }

        private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            LoadingBar.Value = 100;
            Mouse.OverrideCursor = null;

            EnableLoadingViews();
        }

        private void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            LoadingBar.Value = e.ProgressPercentage;
        }

        private void FetchFiles_DoWork(object sender, DoWorkEventArgs e)
        {
            string path = (string)e.Argument + "\\";
            var children = Directory.GetFiles(path);

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

                if (!isDuplicated)
                Dispatcher.Invoke(() => {
                    filesList.Add(new FileObj() { Name = childName, Path = path });
                });
                
                fetchFilesWorker.ReportProgress((child * 100/children.Length));
            }
        }

        private void ExcludeFiles_DoWork(object sender, DoWorkEventArgs e)
        {
            var items = ((IList<object>)e.Argument).Cast<FileObj>().ToList();

            int amount = items.Count;

            for (int item = 0; item < amount; item++)
            {
                Dispatcher.Invoke(()=> {
                    filesList.Remove(items[item]);
                });
                excludeFilesWorker.ReportProgress((item * 100 / amount));
            }
        }

        //-------------------------Button Clicks Implements---------------------------------

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
            LoadingBar.Value = 0;
            filesList.Clear();
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
                addButton.ContextMenu.Width = addButton.ActualWidth;
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
            if (fetchFilesWorker.IsBusy || excludeFilesWorker.IsBusy) return;
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
            if (fetchFilesWorker.IsBusy || excludeFilesWorker.IsBusy || filesList.Count <= 0) return;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

            DisableLoadingViews();
            LoadingBar.Value = 0;
            excludeFilesWorker.RunWorkerAsync(RenameFilesList.SelectedItems);
        }

        private void AddFolderButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExcludeFolderButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OperationsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

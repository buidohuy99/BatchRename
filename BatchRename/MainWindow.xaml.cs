using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace BatchRename
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        class File
        {
            public string Name { get; set; }
            public string NewName { get; set; }
            public string Path { get; set; }
            public string Error { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();
            filesList = new BindingList<File>();

            RenameFilesList.ItemsSource = filesList;
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OptionButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddMethodButton_Click(object sender, RoutedEventArgs e)
        {

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

        BindingList<File> filesList;

        private void AddFileButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();

            if(dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path = dialog.SelectedPath + "\\";
                var children = Directory.GetFiles(path);

                foreach (string child in children)
                {
                    filesList.Add(new File() { Name = child.Remove(0, path.Length), Path = path });
                }
            }

        }

        private void ExcludeFileButton_Click(object sender, RoutedEventArgs e)
        {
            var items = RenameFilesList.SelectedItems;

            for(int item = 0; item < items.Count; item++)
            {
                File currentFile = items[item] as File;
                string name = currentFile.Name;
                string path = currentFile.Path;
                for(int i = 0; i < filesList.Count; i++)
                {
                    if(filesList[i] == currentFile)
                    {
                        filesList.RemoveAt(i);
                        Console.WriteLine(i);
                        break;
                    }
                }
            }
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

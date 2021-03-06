﻿using System.IO;
using System.Windows.Forms;

using SystemTrayApp.Classes;
using static System.DateTime;

namespace SystemTrayApp.Forms
{
    public partial class ViewerForm : Form
    {
        public ViewerForm()
        {
            InitializeComponent();

            /*
             * Setup desired listeners
             */
            WatchOperations.Instance.FileSystemWatcher.Created += FileSystemWatcherCreated;
            WatchOperations.Instance.FileSystemWatcher.Renamed += FileSystemWatcherRenamed;
        }
        /// <summary>
        /// Monitor file rename operations. Since the FileSystemWatcher is in another
        /// thread Invoke is required to prevent cross thread violations between threads.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileSystemWatcherRenamed(object sender, RenamedEventArgs e)
        {
            
            if (ResultsListView.InvokeRequired)
            {
                Invoke((MethodInvoker)(() =>
                    ResultsListView.Items.Add(new ListViewItem(new string[]
                    {
                        "Renamed", $"{e.OldName} to {e.Name}",
                        Now.ToString("yyyy/MM/dd HH:mm:ss")
                    }))));

                ResizeSetFocus();

            }
        }
        private void FileSystemWatcherCreated(object sender, FileSystemEventArgs e)
        {
            if (ResultsListView.InvokeRequired)
            {
                Invoke((MethodInvoker)(() =>
                    ResultsListView.Items.Add(new ListViewItem(new string[]
                    {
                        "Created", $"{e.Name}",
                        Now.ToString("yyyy/MM/dd HH:mm:ss")
                    }))));

                
            }

            ResizeSetFocus();

        }

        private void ResizeSetFocus()
        {
            if (ResultsListView.Items.Count <= 0) return;

            Invoke((MethodInvoker)(() => ResultsListView.AutoResizeColumns(
                    ColumnHeaderAutoResizeStyle.HeaderSize)
                ));

            Invoke((MethodInvoker)(() => ResultsListView.EndUpdate()));

            Invoke((MethodInvoker)(() => ResultsListView.FocusedItem = 
                ResultsListView.Items[ResultsListView.Items.Count -1]));

            Invoke((MethodInvoker)(() => 
                ResultsListView.Items[ResultsListView.Items.Count - 1].Selected = true));


            Invoke((MethodInvoker)(() =>
                ActiveControl = ResultsListView));
        }

    }
}

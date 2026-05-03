using SharpShell.Attributes;
using SharpShell.SharpContextMenu;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace shellext
{
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.AllFilesAndFolders)]
    public class ShellExtension : SharpContextMenu
    {
        private readonly string _appExePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "VirusTotalUploaderNewgene.exe");
        private const string _pipeName = "VirusTotalUploaderNewgenePipe";
        private const int _pipeTimeout = 500;
        private const string _menuItemText = "VirusTotal Uploader Newgene";

        protected override bool CanShowMenu()
        {
            // Depending on the files in 'SelectedItemPaths' you might not show the menu
            return SelectedItemPaths.Any(path => File.Exists(path) || Directory.Exists(path));
        }

        protected override ContextMenuStrip CreateMenu()
        {
            var menu = new ContextMenuStrip();
            var itemCountLines = new ToolStripMenuItem
            {
                Text = _menuItemText,
                Image = Properties.Resources.icon
            };
            //  Handler for the click event
            itemCountLines.Click += MenuItemOnClick;
            menu.Items.Add(itemCountLines);
            return menu;
        }

        private void MenuItemOnClick(object sender, EventArgs args)
        {
            try
            {
                CheckAndRunApp(_appExePath);
                var filtered = SelectedItemPaths.Where(path => File.Exists(path) || Directory.Exists(path)).ToList();
                SendBatchToApp(filtered);
            }
            catch (Exception ex)
            {
                SystemSounds.Hand.Play();
                MessageBox.Show($"{ex.Source}\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void SendBatchToApp(List<string> filePaths)
        {
            using (var client = new NamedPipeClientStream(".", _pipeName, PipeDirection.Out))
            {
                try
                {
                    client.Connect(_pipeTimeout);
                    using (var writer = new StreamWriter(client))
                    {
                        writer.WriteLine(filePaths.Count);
                        foreach (string path in filePaths)
                        {
                            writer.WriteLine(path);
                        }
                        writer.Flush();
                    }
                }
                catch (TimeoutException)
                {
                    SystemSounds.Hand.Play();
                    MessageBox.Show("Can't connect to VirusTotal Uploader Newgene.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        // Same function in: shellext, scanner
        private void CheckAndRunApp(string appPath)
        {
            bool processFound = false;
            string processName = Path.GetFileNameWithoutExtension(appPath);

            Process[] processes = Process.GetProcessesByName(processName);
            foreach (Process proc in processes)
            {
                if (!proc.HasExited)
                {
                    proc.WaitForInputIdle();
                    processFound = true;
                }
            }

            if (!processFound)
            {
                using (Process proc = new Process())
                {
                    proc.StartInfo = new ProcessStartInfo
                    {
                        FileName = appPath,
                        WorkingDirectory = Path.GetDirectoryName(appPath),
                        UseShellExecute = true
                    };
                    proc.Start();
                    proc.WaitForInputIdle();
                }
            }
        }
    }
}

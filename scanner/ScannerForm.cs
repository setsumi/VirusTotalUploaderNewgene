using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DarkUI.Forms;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Media;
using System.Security.Principal;
using System.IO.Pipes;
using System.Drawing;

namespace scanner
{
    public partial class ScannerForm : DarkForm
    {
        private static readonly string _exeFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static readonly string _uploaderExePath = Path.Combine(_exeFolder, "VirusTotalUploaderNewgene.exe");
        private static readonly string _windowsDllNamesFile = "SystemDlls.txt";
        private static readonly string _windowsFolder = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
        private CancellationTokenSource _cts;
        private HashSet<string> _windowsDllNames = null;
        private HashSet<string> _appExeFolders = null;
        private List<FileEntry> _appFiles = new List<FileEntry>();

        public ScannerForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            buttonScanWindows.Tag = 0;
            buttonCheckApp.Tag = 0;
            if (IsAdministrator()) this.Text += " (Administrator)";

            var (header, files) = LoadWindowsDlls(Path.Combine(_exeFolder, _windowsDllNamesFile));
            _windowsDllNames = files;
            UpdateWindowsText(header);
            textBoxApp.Text = GetLastFolder();
            checkFilter.Checked = Properties.Settings.Default.BesideChecked;
        }

        private async void buttonScanDll_Click(object sender, EventArgs e)
        {
            if ((int)buttonScanWindows.Tag == 0)
            {
                // Scan Windows
                buttonScanWindows.Tag = 1;
                buttonScanWindows.Text = "Abort";
                buttonCheckApp.Enabled = false;
                _cts = new CancellationTokenSource();

                try
                {
                    UpdateWindowsText("- Scanning...");
                    _windowsDllNames = await Task.Run(() => ScanWindows(_windowsFolder, _cts.Token), _cts.Token);
                    UpdateWindowsText($"({_windowsDllNames.Count}) {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                    SaveWindowsDlls(Path.Combine(_exeFolder, _windowsDllNamesFile));
                }
                catch (OperationCanceledException)
                {
                    // Cancelled
                    _windowsDllNames = null;
                    UpdateWindowsText("- Scan was cancelled by user.");
                }
                finally
                {
                    // Finished
                    _cts.Dispose();
                    buttonScanWindows.Tag = 0;
                    buttonScanWindows.Text = "Scan";
                    buttonCheckApp.Enabled = true;
                }
            }
            else
            {
                // Cancel
                _cts?.Cancel();
            }
        }

        public HashSet<string> ScanWindows(string rootPath, CancellationToken token)
        {
            HashSet<string> uniqueDllNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            Stack<string> stack = new Stack<string>();
            stack.Push(rootPath);

            while (stack.Count > 0)
            {
                token.ThrowIfCancellationRequested();

                string currentDir = stack.Pop();
                try
                {
                    foreach (string file in Directory.EnumerateFiles(currentDir, "*.dll"))
                    {
                        uniqueDllNames.Add(Path.GetFileName(file));
                    }

                    foreach (string dir in Directory.EnumerateDirectories(currentDir))
                    {
                        stack.Push(dir);
                    }
                }
                catch (UnauthorizedAccessException) { continue; }
                catch (DirectoryNotFoundException) { continue; }
            }
            return uniqueDllNames;
        }

        private async void buttonCheckApp_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxApp.Text)) return;
            if (!Directory.Exists(textBoxApp.Text))
            {
                SystemSounds.Exclamation.Play();
                DarkMessageBox.ShowWarning("Invalid program folder.", this.Text);
                return;
            }

            if ((int)buttonCheckApp.Tag == 0)
            {
                // Scan App
                if (_windowsDllNames == null || _windowsDllNames.Count == 0)
                {
                    SystemSounds.Exclamation.Play();
                    DarkMessageBox.ShowWarning("Scan system DLLs first.", this.Text);
                    return;
                }

                // Save last folder
                SaveLastFolder(textBoxApp.Text);

                buttonCheckApp.Tag = 1;
                buttonCheckApp.Text = "Abort";
                buttonScanWindows.Enabled = false;
                labelCheckResult.Visible = false;

                _cts = new CancellationTokenSource();

                try
                {
                    var dlls = await Task.Run(() => ScanApp(textBoxApp.Text, _cts.Token), _cts.Token);

                    // find dlls with a name the same as system's
                    List<string> matchingPaths = dlls.Where(path => _windowsDllNames.Contains(Path.GetFileName(path))).ToList();
                    // generate grid data
                    _appFiles = matchingPaths.Select(p => new FileEntry
                    {
                        Path = p,
                        Status = _appExeFolders.Contains(Path.GetDirectoryName(p)) ? "Beside" : ""
                    }).ToList();
                    // Update grid
                    checkFilter_CheckedChanged(null, null);
                    // Report result
                    if (_appFiles.Count == 0)
                    {
                        labelCheckResult.ForeColor = Color.Lime;
                        labelCheckResult.BackColor = Color.FromArgb(60, 63, 65);
                        labelCheckResult.Text = "No system DLLs in program folder.";
                        toolTip1.SetToolTip(labelCheckResult, "All clean");
                    }
                    else
                    {
                        if (_appFiles.Count(f => f.Status == "Beside") > 0)
                        {
                            labelCheckResult.ForeColor = Color.Yellow;
                            labelCheckResult.BackColor = Color.OrangeRed;
                            labelCheckResult.Text = "DLL sideloading candidates found.";
                            toolTip1.SetToolTip(labelCheckResult, "System DLL beside EXE, inspect manually");
                        }
                        else
                        {
                            labelCheckResult.ForeColor = Color.Yellow;
                            labelCheckResult.BackColor = Color.FromArgb(60, 63, 65);
                            labelCheckResult.Text = "Found system DLLs in program folder.";
                            toolTip1.SetToolTip(labelCheckResult, "Redundant or portable for some reason");
                        }
                    }
                    labelCheckResult.Visible = true;
                }
                catch (OperationCanceledException)
                {
                    // Cancelled
                    // do nothing special
                }
                finally
                {
                    // Finished
                    _cts.Dispose();
                    buttonCheckApp.Tag = 0;
                    buttonCheckApp.Text = "Check";
                    buttonScanWindows.Enabled = true;
                }
            }
            else
            {
                // Cancel
                _cts?.Cancel();
            }
        }

        private List<string> ScanApp(string rootPath, CancellationToken token)
        {
            _appExeFolders = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            List<string> dllFiles = new List<string>();
            Stack<string> stack = new Stack<string>();
            stack.Push(rootPath);

            while (stack.Count > 0)
            {
                token.ThrowIfCancellationRequested();

                string currentDir = stack.Pop();
                try
                {
                    foreach (string file in Directory.EnumerateFiles(currentDir, "*.dll"))
                    {
                        dllFiles.Add(file);
                    }

                    if (Directory.EnumerateFiles(currentDir, "*.exe").Count() > 0)
                    {
                        _appExeFolders.Add(currentDir);
                    }

                    foreach (string dir in Directory.EnumerateDirectories(currentDir))
                    {
                        stack.Push(dir);
                    }
                }
                catch (UnauthorizedAccessException) { continue; }
                catch (DirectoryNotFoundException) { continue; }
            }
            return dllFiles;
        }

        private void UpdateGrid(string filterStatus)
        {
            var filtered = filterStatus == "All"
                ? _appFiles
                : _appFiles.Where(f => f.Status == filterStatus).ToList();

            dataGridView1.DataSource = new BindingSource { DataSource = filtered };
            dataGridView1.Columns["Status"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns["Path"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void checkFilter_CheckedChanged(object sender, EventArgs e)
        {
            UpdateGrid(checkFilter.Checked ? "Beside" : "All");
            Properties.Settings.Default.BesideChecked = checkFilter.Checked;
            Properties.Settings.Default.Save();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            buttonOpenFolder_Click(null, null);
        }

        private void buttonOpenFolder_Click(object sender, EventArgs e)
        {
            string selectedPath = dataGridView1.CurrentRow?.Cells["Path"].Value.ToString();
            if (!string.IsNullOrEmpty(selectedPath))
            {
                var psi = new ProcessStartInfo
                {
                    FileName = Path.GetDirectoryName(selectedPath),
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0)
                {
                    string file = files[0];
                    if (Directory.Exists(file))
                        e.Effect = DragDropEffects.Copy;
                }
            }
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 0)
            {
                string file = files[0];
                if (Directory.Exists(file))
                {
                    OpenAppFolder(file);
                }
            }
        }

        private void buttonOpenApp_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select the program folder";
                fbd.ShowNewFolderButton = false;
                // Set starting folder
                fbd.SelectedPath = GetLastFolder();
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    OpenAppFolder(fbd.SelectedPath);
                }
            }
        }

        private string GetLastFolder()
        {
            var lastFolder = Properties.Settings.Default.LastFolder;
            if (string.IsNullOrEmpty(lastFolder) || !Directory.Exists(lastFolder))
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            }
            else
            {
                return lastFolder;
            }
        }

        private void SaveLastFolder(string path)
        {
            Properties.Settings.Default.LastFolder = path;
            Properties.Settings.Default.Save();
        }

        private void OpenAppFolder(string path)
        {
            textBoxApp.Text = path;
        }

        private void UpdateWindowsText(string notes)
        {
            textBoxWindows.Text = $"{_windowsFolder} {notes}";
        }

        private void SaveWindowsDlls(string filePath)
        {
            string header = $"({_windowsDllNames.Count}) {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
            var output = new List<string> { header };
            output.AddRange(_windowsDllNames);
            File.WriteAllLines(filePath, output);
        }

        private (string header, HashSet<string> fileNames) LoadWindowsDlls(string filePath)
        {
            if (!File.Exists(filePath)) return ("", new HashSet<string>(StringComparer.OrdinalIgnoreCase));
            string[] allLines = File.ReadAllLines(filePath);
            if (allLines.Length == 0) return ("", new HashSet<string>(StringComparer.OrdinalIgnoreCase));
            string header = allLines[0];
            List<string> fileNames = allLines.Skip(1).ToList();
            return (header, new HashSet<string>(fileNames, StringComparer.OrdinalIgnoreCase));
        }

        private bool IsAdministrator()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        private void SendBatchToApp(List<string> filePaths)
        {
            using (var client = new NamedPipeClientStream(".", "VirusTotalUploaderNewgenePipe", PipeDirection.Out))
            {
                try
                {
                    client.Connect(1000);
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
                    DarkMessageBox.ShowError("VirusTotal Uploader Newgene is not running.", this.Text);
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

        private void buttonUpload_Click(object sender, EventArgs e)
        {
            var files = dataGridView1.SelectedRows
                .Cast<DataGridViewRow>()
                .OrderBy(row => row.Index) // sort by position in the grid
                .Select(row =>
                {
                    return row.Cells["Path"].Value?.ToString() ?? "";
                })
                .Where(name => !string.IsNullOrEmpty(name))
                .ToList();
            if (files.Count > 0)
            {
                try
                {
                    CheckAndRunApp(_uploaderExePath);
                    SendBatchToApp(files);
                }
                catch (Exception ex)
                {
                    SystemSounds.Hand.Play();
                    DarkMessageBox.ShowError($"{ex.Source}\n{ex.Message}", this.Text);
                }
            }
        }
    }

    public class FileEntry
    {
        public string Path { get; set; }
        public string Status { get; set; }
    }
}

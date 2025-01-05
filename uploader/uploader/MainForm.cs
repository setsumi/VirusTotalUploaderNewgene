using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DarkUI.Forms;

namespace uploader
{
    public partial class MainForm : DarkForm
    {
        private SettingsForm _settingsForm = new SettingsForm();
        private const int _maxArgs = 20;
        private const int _maxFiles = 200;
        private int _uploaderCount = 0;
        private bool _addingFiles = false;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Set working directory to exe location because of language files
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            //LocalizationHelper.Export();

            LocalizationHelper.Update();

            labelMessage.Text = LocalizationHelper.Base.MainForm_DragFile;
            moreLabel.Text = LocalizationHelper.Base.MainForm_More;
            labelClear.Text = LocalizationHelper.Base.MainForm_Clear;
        }

        private void moreLabel_Click(object sender, EventArgs e)
        {
            if (_settingsForm.IsDisposed)
            {
                _settingsForm = new SettingsForm();
            }
            _settingsForm.Show(this);
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            var settings = Settings.LoadSettings();

            var arguments = (string[])e.Data.GetData(DataFormats.FileDrop);
            //var files = FileUtil.GetFiles(arguments);
            showMultipleUploadForms(arguments, settings);
        }

        private void showMultipleUploadForms(string[] files, Settings settings)
        {
            _addingFiles = true;
            labelClear.Enabled = false;

            int i = 0;
            foreach (var file in files)
            {
                i++;
                labelMessage.Text = $"Adding object {i} / {files.Count()} ... (Click to abort)";
                labelMessage.Update();

                AddUpload(settings, file);
            }
            panelUploads_Resize(null, null);
            labelMessage.Text = $"{_uploaderCount} file(s) in total.{(!_addingFiles ? " (Last operation was aborted)" : "")}";

            _addingFiles = false;
            labelClear.Enabled = true;
        }

        private void AddUpload(Settings settings, string path)
        {
            Application.DoEvents();
            if (!_addingFiles) return;
            if (_uploaderCount >= _maxFiles)
            {
                _addingFiles = false;
                var messageBox = new DarkMessageBox($"Reached upper limit of {_maxFiles} files.\nOperation aborted.", this.Text, DarkMessageBoxIcon.Warning, DarkDialogButton.Ok);
                messageBox.ShowDialog();
                return;
            }

            bool isFolder = Directory.Exists(path);
            if (isFolder)
            {
                var filesToUpload = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).ToList();
                foreach (var file in filesToUpload)
                {
                    AddUpload(settings, file);
                }
            }
            else
            {
                // file or don't exist
                var upload = new UploadForm(this, settings, path, _uploaderCount + 1);
                upload.Location = new Point(5, _uploaderCount * (upload.Height + 5));
                panelUploads.Controls.Add(upload);
                upload.Show();
                _uploaderCount++;
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            var settings = Settings.LoadSettings();
            var args = Environment.GetCommandLineArgs();
            var numArgs = args.Length - 1; // don't count exe itself

            if (numArgs >= 1 && numArgs <= _maxArgs)
            {
                var files = args.ToList().GetRange(1, args.Length - 1).ToArray();
                showMultipleUploadForms(files, settings);
            }
            else if (numArgs > _maxArgs)
            {
                var messageBox = new DarkMessageBox($"Number of arguments {numArgs} exceeds maximum.\n" +
                    $"The app can handle max {_maxArgs} arguments at once.", this.Text, DarkMessageBoxIcon.Error, DarkDialogButton.Ok);
                messageBox.ShowDialog();
                this.Close();
            }
        }

        private void panelUploads_Resize(object sender, EventArgs e)
        {
            foreach (Form upload in panelUploads.Controls)
            {
                upload.Width = panelUploads.Width - 35;
            }
        }

        public void labelClear_Click(object sender, EventArgs e)
        {
            foreach (Form upload in panelUploads.Controls)
            {
                upload.Close();
            }
            panelUploads.Controls.Clear();
            _uploaderCount = 0;
            labelMessage.Text = LocalizationHelper.Base.MainForm_DragFile;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // on exit abort any threads
            labelClear_Click(null, null);
        }

        private void labelMessage_Click(object sender, EventArgs e)
        {
            // abort the adding files operation
            _addingFiles = false;
        }
    }
}

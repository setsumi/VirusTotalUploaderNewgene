using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using DarkUI.Controls;
using DarkUI.Forms;

namespace uploader
{
    public partial class MainForm : DarkForm
    {
        private readonly object _lock = new object();
        internal RateLimiter rateLimiter { get { lock (_lock) { return _rateLimiter; } } }

        private SettingsForm _settingsForm = new SettingsForm();
        private const int _maxArgs = 20;
        private const int _maxFiles = 200;
        private int _uploaderCount = 0;
        private bool _addingFiles = false;
        private readonly RateLimiter _rateLimiter;
        private Settings _settings;
        private int _vertScroll = 0;
        private readonly List<UploadForm> _uploadList = new List<UploadForm>();
        private readonly List<Label> _selectorList = new List<Label>();
        private readonly DarkContextMenu _doAllMenu = new DarkContextMenu();
        private FormWindowState _previousWindowState;
        private bool _formInit = true;

        public MainForm()
        {
            InitializeComponent();

            _settings = Settings.LoadSettings();
            _rateLimiter = new RateLimiter(_settings.CallsPerMinute);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Set working directory to exe location because of language files
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            //LocalizationHelper.Export();

            LocalizationHelper.Update();

            labelMessage.Text = LocalizationHelper.Base.MainForm_DragFile;
            moreLabel.Text = LocalizationHelper.Base.MainForm_More;
            queueLabel.BackColor = Color.FromArgb(255, 255, 0);
            queueLabel.ForeColor = Color.FromArgb(0, 0, 0);
            queueLabel.Text = "";
            panelUploads.VerticalScroll.SmallChange = 26;

            _selectorList.Add(selAll);
            _selectorList.Add(selIdle);
            _selectorList.Add(selDetected);
            _selectorList.Add(selUndetected);
            _selectorList.Add(selChecking);
            _selectorList.Add(selUploading);
            _selectorList.Add(selUploaded);
            _selectorList.Add(selError);
            _selectorList.Add(selAborted);

            selDetected.BackColor = Color.FromArgb(60, 63, 65);
            selDetected.ForeColor = Color.FromArgb(255, 100, 0);
            selUndetected.BackColor = Color.FromArgb(60, 63, 65);
            selUndetected.ForeColor = Color.FromArgb(0, 255, 0);
            selChecking.BackColor = Color.FromArgb(60, 63, 65);
            selChecking.ForeColor = Color.FromArgb(255, 255, 0);
            selUploading.BackColor = Color.FromArgb(255, 255, 0);
            selUploading.ForeColor = Color.FromArgb(0, 0, 0);
            selUploaded.BackColor = Color.FromArgb(0, 255, 100);
            selUploaded.ForeColor = Color.FromArgb(0, 0, 0);
            selError.BackColor = Color.FromArgb(255, 0, 0);
            selError.ForeColor = Color.FromArgb(255, 255, 0);
            selAborted.BackColor = Color.FromArgb(255, 0, 0);
            selAborted.ForeColor = Color.FromArgb(255, 255, 255);

            SelectorSelect(selAll);
            SelectorUpdateStats();

            _previousWindowState = WindowState;

            if (Properties.Settings.Default.winWidth != -1)
                Width = Properties.Settings.Default.winWidth;
            if (Properties.Settings.Default.winHeight != -1)
                Height = Properties.Settings.Default.winHeight;
            if (Properties.Settings.Default.winLeft != -1)
                Left = Properties.Settings.Default.winLeft;
            if (Properties.Settings.Default.winTop != -1)
                Top = Properties.Settings.Default.winTop;

            UpdateSounds();

            _formInit = false;
        }

        private void moreLabel_Click(object sender, EventArgs e)
        {
            contextMenu1.Show(moreLabel, 0, moreLabel.Height);
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            ResetScroll();
            var arguments = (string[])e.Data.GetData(DataFormats.FileDrop);
            showMultipleUploadForms(arguments, _settings);
        }

        private void showMultipleUploadForms(string[] files, Settings settings)
        {
            _addingFiles = true;
            clearToolStripMenuItem.Enabled = false;
            labelMessage.ForeColor = Color.FromArgb(255, 255, 0);

            int i = 0;
            foreach (var file in files)
            {
                i++;
                labelMessage.Text = $"Adding object {i} / {files.Count()} ... (Click to abort)";
                labelMessage.Update();

                AddUpload(settings, file);
            }
            ResizeUploads();
            ulong totalSize = 0;
            foreach (UploadForm upload in _uploadList)
            {
                totalSize += (ulong)upload.FileSize;
            }
            labelMessage.Text = $"{_uploadList.Count} files in total, {Utils.BytesToHumanReadable(totalSize)} to upload.{(!_addingFiles ? " (Last operation was aborted)" : "")}";

            labelMessage.ForeColor = Color.FromArgb(220, 220, 220);
            clearToolStripMenuItem.Enabled = true;
            _addingFiles = false;
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
                // file or does't exist
                var upload = new UploadForm(this, settings, path, _uploadList.Count + 1);
                if (upload.FileIsTooSmall() || UploadIsDuplicate(upload))
                {
                    upload.Close();
                    upload.Dispose();
                }
                else
                {
                    _uploadList.Add(upload);
                    ResizeUploadSingle(upload);
                    upload.Location = new Point(5, _uploaderCount * (upload.Height + 5));
                    panelUploads.Controls.Add(upload);
                    upload.Show();
                    _uploaderCount++;
                }
            }
        }

        private bool UploadIsDuplicate(UploadForm newOne)
        {
            foreach (UploadForm upload in panelUploads.Controls)
            {
                if (upload.FullPath == newOne.FullPath)
                    return true;
            }

            if (string.IsNullOrEmpty(newOne.SHA256))
                return false;

            foreach (UploadForm upload in panelUploads.Controls)
            {
                if (upload.SHA256 == newOne.SHA256)
                    return true;
            }
            return false;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            var args = Environment.GetCommandLineArgs();
            var numArgs = args.Length - 1; // don't count exe itself

            if (numArgs >= 1 && numArgs <= _maxArgs)
            {
                var files = args.ToList().GetRange(1, args.Length - 1).ToArray();
                showMultipleUploadForms(files, _settings);
            }
            else if (numArgs > _maxArgs)
            {
                var messageBox = new DarkMessageBox($"Number of arguments {numArgs} exceeds maximum.\n" +
                    $"The app can handle max {_maxArgs} arguments at once.", this.Text, DarkMessageBoxIcon.Error, DarkDialogButton.Ok);
                messageBox.ShowDialog();
                this.Close();
            }

            tmrRateLimiter.Start();
        }

        public void Clear()
        {
            clearToolStripMenuItem.Enabled = false;
            tmrRateLimiter.Stop();
            queueLabel.Text = "Clearing, please wait...";
            Application.DoEvents();

            foreach (var upload in _uploadList)
            {
                upload.FormAbort();
            }
            _rateLimiter.Clear();
            panelUploads.Controls.Clear();

            foreach (var form in _uploadList)
            {
                form.Close();
                form.Dispose();
            }
            _uploadList.Clear();
            _uploaderCount = 0;

            labelMessage.Text = LocalizationHelper.Base.MainForm_DragFile;
            queueLabel.Text = "";
            tmrRateLimiter.Start();
            clearToolStripMenuItem.Enabled = true;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // on exit abort any upload threads
            Clear();
            // save window placement
            Properties.Settings.Default.Save();
        }

        private void labelMessage_Click(object sender, EventArgs e)
        {
            // abort the adding files operation
            _addingFiles = false;
        }

        private void tmrRateLimiter_Tick(object sender, EventArgs e)
        {
            tmrRateLimiter.Stop();

            SaveScroll();
            _rateLimiter.TimeTick();
            _rateLimiter.GetQueueLength(out int active, out int activeTotal, out int pending);
            string text = "";
            if (active != 0 || pending != 0)
            {
                text = $"Requests: {active}/{activeTotal} {pending}";
            }
            if (queueLabel.Text != text)
            {
                queueLabel.Text = text;
                RestoreScroll();
            }

            SelectorUpdateStats();

            tmrRateLimiter.Start();
        }

        private void SaveScroll()
        {
            _vertScroll = panelUploads.VerticalScroll.Value;
        }

        private void RestoreScroll()
        {
            panelUploads.VerticalScroll.Value = _vertScroll;
            panelUploads.AutoScrollPosition = new Point(0, _vertScroll);
        }

        private void ResetScroll()
        {
            panelUploads.VerticalScroll.Value = 0;
            panelUploads.AutoScrollPosition = new Point(0, 0);
            Application.DoEvents();
        }

        private void MainForm_Deactivate(object sender, EventArgs e)
        {
            SaveScroll();
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            RestoreScroll();
        }

        private void selAll_Click(object sender, EventArgs e)
        {
            SelectorSelect(sender as Label);
        }

        private void SelectorSelect(Label sel)
        {
            foreach (var item in _selectorList)
            {
                item.Font = new Font(item.Font, FontStyle.Regular);
            }
            sel.Font = new Font(sel.Font, FontStyle.Underline);
            doAllLabel.BackColor = sel.BackColor;
            doAllLabel.ForeColor = sel.ForeColor;

            SelectorUpdateUploads();
        }

        private void SelectorUpdateUploads()
        {
            Label sel = selAll;
            foreach (var selector in _selectorList)
            {
                if (selector.Font.Underline)
                {
                    sel = selector;
                    break;
                }
            }

            var uploads = new List<UploadForm>();
            foreach (var upload in _uploadList)
            {
                if (sel == selAll) { uploads.Add(upload); }
                else if (sel == selIdle) { if (upload.State == UploadForm.StatusMessageStyle.Normal) uploads.Add(upload); }
                else if (sel == selDetected) { if (upload.State == UploadForm.StatusMessageStyle.Red) uploads.Add(upload); }
                else if (sel == selUndetected) { if (upload.State == UploadForm.StatusMessageStyle.Green) uploads.Add(upload); }
                else if (sel == selChecking) { if (upload.State == UploadForm.StatusMessageStyle.ShortWait) uploads.Add(upload); }
                else if (sel == selUploading) { if (upload.State == UploadForm.StatusMessageStyle.Progress) uploads.Add(upload); }
                else if (sel == selUploaded) { if (upload.State == UploadForm.StatusMessageStyle.Success) uploads.Add(upload); }
                else if (sel == selError) { if (upload.State == UploadForm.StatusMessageStyle.Error) uploads.Add(upload); }
                else if (sel == selAborted) { if (upload.State == UploadForm.StatusMessageStyle.Abort) uploads.Add(upload); }
            }

            panelUploads.Controls.Clear();
            ResetScroll();
            _uploaderCount = 0;

            foreach (var upload in uploads)
            {
                upload.Location = new Point(5, _uploaderCount * (upload.Height + 5));
                panelUploads.Controls.Add(upload);
                _uploaderCount++;
            }
        }

        private void SelectorUpdateStats()
        {
            int All = 0, Idle = 0, Detected = 0, Undetected = 0, Checking = 0, Uploading = 0, Uploaded = 0, Error = 0, Aborted = 0;
            foreach (var upload in _uploadList)
            {
                All++;
                var state = upload.State;
                switch (state)
                {
                    case UploadForm.StatusMessageStyle.Normal:
                        Idle++;
                        break;
                    case UploadForm.StatusMessageStyle.Red:
                        Detected++;
                        break;
                    case UploadForm.StatusMessageStyle.Green:
                        Undetected++;
                        break;
                    case UploadForm.StatusMessageStyle.ShortWait:
                        Checking++;
                        break;
                    case UploadForm.StatusMessageStyle.Progress:
                        Uploading++;
                        break;
                    case UploadForm.StatusMessageStyle.Success:
                        Uploaded++;
                        break;
                    case UploadForm.StatusMessageStyle.Error:
                        Error++;
                        break;
                    case UploadForm.StatusMessageStyle.Abort:
                        Aborted++;
                        break;
                }
            }

            bool updated = false;
            string caption = $"All: {All}";
            if (selAll.Text != caption) { updated = true; selAll.Text = caption; }
            caption = $"Idle: {Idle}";
            if (selIdle.Text != caption) { updated = true; selIdle.Text = caption; }
            caption = $"Detected: {Detected}";
            if (selDetected.Text != caption) { updated = true; selDetected.Text = caption; }
            caption = $"Clean: {Undetected}";
            if (selUndetected.Text != caption) { updated = true; selUndetected.Text = caption; }
            caption = $"Checking: {Checking}";
            if (selChecking.Text != caption) { updated = true; selChecking.Text = caption; }
            caption = $"Uploading: {Uploading}";
            if (selUploading.Text != caption) { updated = true; selUploading.Text = caption; }
            caption = $"Uploaded: {Uploaded}";
            if (selUploaded.Text != caption) { updated = true; selUploaded.Text = caption; }
            caption = $"Error: {Error}";
            if (selError.Text != caption) { updated = true; selError.Text = caption; }
            caption = $"Aborted: {Aborted}";
            if (selAborted.Text != caption) { updated = true; selAborted.Text = caption; }

            if (updated)
            {
                for (int i = 0; i < _selectorList.Count; i++)
                {
                    if (i > 0)
                    {
                        var prev = _selectorList[i - 1];
                        _selectorList[i].Left = prev.Left + prev.Width + 5;
                    }
                }
                doAllLabel.Left = selAborted.Left + selAborted.Width + 10;
                queueLabel.Left = doAllLabel.Left + doAllLabel.Width + 10;
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_settingsForm.IsDisposed)
            {
                _settingsForm = new SettingsForm();
            }
            _settingsForm.Show(this);
        }

        private void doAllLabel_Click(object sender, EventArgs e)
        {
            var operations = new HashSet<string>();
            foreach (UploadForm upload in panelUploads.Controls)
            {
                var o = upload.Operation;
                if (!string.IsNullOrEmpty(o))
                    operations.Add(o);
            }

            _doAllMenu.Items.Clear();
            foreach (string operation in operations)
            {
                var item = _doAllMenu.Items.Add(operation);
                item.Click += doAllItemClick;
            }

            _doAllMenu.Show(doAllLabel, 0, doAllLabel.Height);
        }

        private void doAllItemClick(object sender, EventArgs e)
        {
            DoAll((sender as ToolStripItem).Text);
        }

        private void DoAll(string action)
        {
            foreach (UploadForm upload in panelUploads.Controls)
            {
                if (upload.Operation == action)
                {
                    upload.uploadButton_Click(null, null);
                }
            }
        }

        private void ResizeUploadSingle(Form upload)
        {
            upload.Width = panelUploads.Width - 35;
        }

        private void ResizeUploads()
        {
            panelUploads.SuspendLayout();
            foreach (Form upload in panelUploads.Controls)
            {
                ResizeUploadSingle(upload);
            }
            panelUploads.ResumeLayout();
        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            ResizeUploads();
            panelUploads.AutoScroll = true;
        }

        private void MainForm_ResizeBegin(object sender, EventArgs e)
        {
            panelUploads.AutoScroll = false;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (_formInit) return;

            if (WindowState != _previousWindowState)
            {
                if (WindowState == FormWindowState.Maximized)
                {
                    // Maximized
                    ResizeUploads();
                }
                else if (WindowState == FormWindowState.Normal && _previousWindowState == FormWindowState.Maximized)
                {
                    //Restored from maximized
                    ResizeUploads();
                }
                _previousWindowState = WindowState;
            }
            else if (WindowState == FormWindowState.Normal)
            {
                // Resizing
                Properties.Settings.Default.winWidth = Width;
                Properties.Settings.Default.winHeight = Height;
            }
        }

        private void MainForm_Move(object sender, EventArgs e)
        {
            if (_formInit) return;

            if (WindowState == FormWindowState.Normal)
            {
                // Moving
                Properties.Settings.Default.winLeft = Left;
                Properties.Settings.Default.winTop = Top;
            }
        }

        private void labelSounds_Click(object sender, EventArgs e)
        {
            _settings.Sounds = !_settings.Sounds;
            UpdateSounds();
        }

        private void UpdateSounds()
        {
            labelSounds.Font = new Font(labelSounds.Font, _settings.Sounds ? FontStyle.Regular : FontStyle.Strikeout);
        }
    }
}

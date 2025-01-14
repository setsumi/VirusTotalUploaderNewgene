using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Media;
using System.Threading;
using System.Windows.Forms;
using DarkUI.Forms;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace uploader
{
    public partial class UploadForm : DarkForm
    {
        private readonly object _formLock = new object();
        private readonly object _threadLock = new object();

        private enum StatusMessageStyle { Normal, Red, Green, Error, Success, Progress }
        private enum FormStatus { Check, Upload }

        private readonly string _path;
        private readonly MainForm _mainForm;
        private readonly Settings _settings;
        private Thread _uploadThread;
        private RestClient _client;
        private readonly int _fileCounter;
        private CancellationTokenSource _cancelTokenSource = null;
        private bool _formAborted = false;
        private bool _fileExists = false;
        private bool _fileTooLarge = false;
        private bool _fileTooSmall = false;
        // thread
        private bool _largeFile = false;
        private FormStatus _status = FormStatus.Check;
        private string _SHA256 = "";

        private bool LargeFile { get { lock (_formLock) { return _largeFile; } } set { lock (_formLock) { _largeFile = value; } } }
        private FormStatus Status { get { lock (_formLock) { return _status; } } set { lock (_formLock) { _status = value; } } }
        public string SHA256 { get { lock (_formLock) { return _SHA256; } } set { lock (_formLock) { _SHA256 = value; } } }

        public UploadForm(MainForm mainForm, Settings settings, string path, int counter)
        {
            _mainForm = mainForm;
            _settings = settings;
            _path = path;
            _fileCounter = counter;

            // for the embedding of this form into main form's panel
            TopLevel = false;
            FormBorderStyle = FormBorderStyle.None;

            InitializeComponent();
            // my load
            FormInit();
        }

        private void ChangeStatus(string text, StatusMessageStyle status)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(() => ChangeStatus(text, status)));
                return;
            }

            statusLabel.Text = text;
            switch (status)
            {
                case StatusMessageStyle.Normal:
                    statusLabel.BackColor = Color.FromArgb(60, 63, 65);
                    statusLabel.ForeColor = Color.FromArgb(220, 220, 220);
                    break;
                case StatusMessageStyle.Red:
                    statusLabel.BackColor = Color.FromArgb(60, 63, 65);
                    statusLabel.ForeColor = Color.FromArgb(255, 100, 0);
                    break;
                case StatusMessageStyle.Green:
                    statusLabel.BackColor = Color.FromArgb(60, 63, 65);
                    statusLabel.ForeColor = Color.FromArgb(0, 255, 0);
                    break;
                case StatusMessageStyle.Error:
                    statusLabel.BackColor = Color.FromArgb(255, 0, 0);
                    statusLabel.ForeColor = Color.FromArgb(255, 255, 0);
                    break;
                case StatusMessageStyle.Success:
                    statusLabel.BackColor = Color.FromArgb(0, 255, 100);
                    statusLabel.ForeColor = Color.FromArgb(0, 0, 0);
                    break;
                case StatusMessageStyle.Progress:
                    statusLabel.BackColor = Color.FromArgb(255, 255, 0);
                    statusLabel.ForeColor = Color.FromArgb(0, 0, 0);
                    break;
            }
        }

        private void Finish(FormStatus nextStatus)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(() => Finish(nextStatus)));
                return;
            }

            switch (nextStatus)
            {
                case FormStatus.Check:
                    uploadButton.Text = LocalizationHelper.Base.UploadForm_Check;
                    break;
                case FormStatus.Upload:
                    uploadButton.Text = LocalizationHelper.Base.UploadForm_Upload;
                    break;
            }
            uploadButton.Enabled = true;
            Status = nextStatus;
        }

        private void CloseWindow()
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(() => CloseWindow()));
                return;
            }

            this.Close();
        }

        private void DisplayError(string error)
        {
            //var messageBox = new DarkMessageBox(error, LocalizationHelper.Base.UploadForm_Error, DarkMessageBoxIcon.Error, DarkDialogButton.Ok);
            //messageBox.ShowDialog();
            if (InvokeRequired)
            {
                this.Invoke(new Action(() => DisplayError(error)));
                return;
            }

            ChangeStatus(error, StatusMessageStyle.Error);
            SystemSounds.Hand.Play();
        }

        private void DisplaySuccess(string success)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(() => DisplaySuccess(success)));
                return;
            }

            ChangeStatus(success, StatusMessageStyle.Success);
            SystemSounds.Beep.Play();
        }

        // Thread function
        private void Upload()
        {
            // catch abort thread exception
            try
            {
                if (string.IsNullOrEmpty(_settings.ApiKey))
                {
                    DisplayError(LocalizationHelper.Base.UploadForm_NoApiKey);
                    return;
                }

                if (_settings.ApiKey.Length != 64)
                {
                    DisplayError(LocalizationHelper.Base.UploadForm_InvalidLength);
                    return;
                }

                ChangeStatus(LocalizationHelper.Base.Message_Init, StatusMessageStyle.Normal);
                _client = new RestClient("https://www.virustotal.com");

                bool result = false;
                result = UploadFile(_path);

                if (result)
                    Finish(Status == FormStatus.Check ? FormStatus.Upload : FormStatus.Check);
                else
                    Finish(Status);
            }
            catch (Exception ex)
            {
                // Don't update the form when disposing of it
                if (!_formAborted)
                {
                    string type, msg;
                    if (ex is ThreadAbortException || ex is System.Threading.Tasks.TaskCanceledException)
                    {
                        type = "";
                        msg = "Operation aborted.";
                    }
                    else
                    {
                        type = ex.GetType() + ": ";
                        msg = ex.Message.Replace("\r", "").Replace("\n", " ");
                    }
                    DisplayError(type + msg);
                    Finish(Status);
                }
            }
        }

        private bool UploadFile(string fullPath)
        {
            if (!File.Exists(fullPath))
            {
                DisplayError($"File does not exist.");
                return false;
            }

            dynamic json = null;
            SetLink($"https://www.virustotal.com/gui/file/{SHA256}");

            // Check file
            if (Status == FormStatus.Check)
            {
                try
                {
                    ChangeStatus(LocalizationHelper.Base.Message_Check, StatusMessageStyle.Normal);

                    ApiRateWait();
                    var reportRequest = new RestRequest($"api/v3/files/{SHA256}", Method.Get);
                    reportRequest.AddHeader("x-apikey", _settings.ApiKey);

                    var reportResponse = _client.Execute(reportRequest);
                    var reportContent = reportResponse.Content;
                    //debug
                    //File.WriteAllText("response-check.json", reportContent);
                    json = JsonConvert.DeserializeObject(reportContent);

                    // check for API error
                    var errorCode = json.error.code.ToString();
                    var errorMsg = json.error.message.ToString();

                    // file isn't uploaded
                    if (errorCode == "NotFoundError")
                    {
                        // Upload the file immediately
                        Status = FormStatus.Upload;
                    }
                    else
                    {
                        // other API error, break
                        DisplayError(errorMsg);
                        return false;
                    }
                }
                catch (RuntimeBinderException) // no error in API json meaning file is already uploaded
                {
                    // get file info
                    try
                    {
                        //var reportLink = reportJson.data.links.self.ToString();
                        //reportLink = reportLink.Replace("api/v3/files", "gui/file");
                        var lastAnalysisResults = json.data.attributes.last_analysis_results as JObject;
                        string lastAnalysisDate = json.data.attributes.last_analysis_date.ToString();
                        string typeTag = "---";
                        try { typeTag = $"{json.data.attributes.type_tag.ToString()}"; } catch { }

                        // Count number of detects
                        int numberTotal = 0, numberDetected = 0;
                        foreach (var property in lastAnalysisResults.Properties())
                        {
                            var category = property.Value["category"].ToString();
                            if (category.Contains("unsupported") || category.Contains("timeout") || category.Contains("failure"))
                                continue;
                            numberTotal++;
                            var result = property.Value["result"];
                            if (result != null && result.Type != JTokenType.Null)
                            {
                                numberDetected++;
                            }
                        }

                        // Format last analysis date
                        long unixTimestamp = long.Parse(lastAnalysisDate);
                        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp);
                        DateTime localDateTime = dateTimeOffset.LocalDateTime;
                        string formattedDate = localDateTime.ToString("yyyy-MM-dd HH:mm:ss");

                        var stat = StatusMessageStyle.Normal;
                        if (numberTotal > 0)
                        {
                            stat = numberDetected > 0 ? StatusMessageStyle.Red : StatusMessageStyle.Green;
                        }

                        ChangeStatus($"[{typeTag}] {numberDetected}/{numberTotal} detected ({formattedDate}), click for details.", stat);
                    }
                    catch (Exception ex)
                    {
                        DisplayError($"Unrecognized responce: {ex.GetType()}: {ex.Message}");
                        return false;
                    }
                }
            }

            // Upload file
            if (Status == FormStatus.Upload)
            {
                ChangeStatus(LocalizationHelper.Base.Message_Upload, StatusMessageStyle.Progress);

                //string uploadUri = "https://www.virustotal.com/vtapi/v2/file/scan";
                string uploadUri = "api/v3/files";

                // get special uri for big files
                if (LargeFile)
                {
                    ApiRateWait();
                    var uploadUrlRequest = new RestRequest($"api/v3/files/upload_url", Method.Get);
                    uploadUrlRequest.AddHeader("x-apikey", _settings.ApiKey);

                    var uploadUrlResponse = _client.Execute(uploadUrlRequest);
                    var uploadUrlContent = uploadUrlResponse.Content;

                    json = JsonConvert.DeserializeObject(uploadUrlContent);
                    uploadUri = json.data.ToString();
                }

                ApiRateWait();
                var scanRequest = new RestRequest(uploadUri, Method.Post);
                scanRequest.AddHeader("x-apikey", _settings.ApiKey);
                scanRequest.AddFile("file", fullPath);
                scanRequest.Timeout = TimeSpan.FromHours(6); // super timeout

                _cancelTokenSource = new CancellationTokenSource();
                var task = _client.ExecuteAsync(scanRequest, _cancelTokenSource.Token);
                // this will wait for completion or cancel/error
                var scanResponse = task.Result;
                _cancelTokenSource = null;
                if (scanResponse.ErrorException != null)
                    throw scanResponse.ErrorException;

                //var scanResponse = _client.Execute(scanRequest);
                var scanContent = scanResponse.Content;
                //debug
                //File.WriteAllText("response-upload.json", scanContent);
                json = JsonConvert.DeserializeObject(scanContent);

                var scanId = json.data.id.ToString();
                var scanLink = $"https://www.virustotal.com/gui/file/{SHA256}/detection/{scanId}";
                SetLink(scanLink);
                DisplaySuccess(LocalizationHelper.Base.Message_Uploaded);
            }

            return true;
        }

        private void ApiRateWait()
        {
            lock (_threadLock)
            {
                var waiter = _mainForm.rateLimiter.GetWaiter();
                waiter.WaitOne();
            }
        }

        private bool StopUploadThread()
        {
            if (_uploadThread != null && _uploadThread.IsAlive)
            {
                if (_cancelTokenSource != null)
                {
                    _cancelTokenSource.Cancel();
                    _cancelTokenSource = null;
                }
                else
                {
                    _uploadThread.Abort();
                }
                return true;
            }
            return false;
        }

        private void StartStopUploadThread()
        {
            // stop thread
            if (StopUploadThread())
            {
                //uploadButton.Text = LocalizationHelper.Base.UploadForm_Upload;
                uploadButton.Enabled = false;
                return;
            }
            // start thread
            uploadButton.Text = LocalizationHelper.Base.UploadForm_Cancel;

            _uploadThread = new Thread(Upload);
            _uploadThread.Start();
        }

        private void FormInit()
        {
            fileTextbox.Text = Path.GetFileName(_path);
            folderTextbox.Text = Path.GetDirectoryName(_path);
            uploadButton.Text = LocalizationHelper.Base.UploadForm_Check;

            _fileExists = File.Exists(_path);
            if (_fileExists)
            {
                // check file size
                double fileSizeInBytes = new FileInfo(_path).Length;
                double fileSizeInKB = fileSizeInBytes / 1024;
                double fileSizeInMB = fileSizeInBytes / (1024 * 1024);

                string formattedFileSize;
                if (fileSizeInKB < 0.01)
                    formattedFileSize = fileSizeInBytes.ToString("0") + " B";
                else if (fileSizeInMB < 0.01)
                    formattedFileSize = fileSizeInKB.ToString("0.00") + " KB";
                else
                    formattedFileSize = fileSizeInMB.ToString("0.00") + " MB";
                string suffix = "";

                _fileTooSmall = fileSizeInBytes < _settings.MinimumFileSize;
                _fileTooLarge = fileSizeInMB > 650;
                LargeFile = fileSizeInMB > 32;

                if (_fileTooLarge)
                    suffix = "   exceeds 650MB limit";
                else if (LargeFile)
                    suffix = "   large file >32MB";

                if (!_fileTooSmall && !_fileTooLarge)
                {
                    SHA256 = Utils.GetSHA256(_path).ToLower();
                    sha2Textbox.Text = SHA256;
                    statusLabel.Text = LocalizationHelper.Base.Message_Idle;
                    SetLink($"https://www.virustotal.com/gui/file/{SHA256}");
                }
                else
                {
                    statusLabel.Text = "";
                    uploadButton.Enabled = false;
                }

                fileGroup.Text = $"#{_fileCounter}   {formattedFileSize}{suffix}";
            }
            else
            {
                statusLabel.Text = "";
                fileGroup.Text = $"#{_fileCounter}   File does not exist.";
                uploadButton.Enabled = false;
            }
        }

        private void UploadForm_Load(object sender, EventArgs e)
        {
            if (_settings.DirectUpload && _fileExists && !_fileTooSmall && !_fileTooLarge)
            {
                StartStopUploadThread();
            }
        }

        public bool FileIsTooSmall()
        {
            return _fileTooSmall;
        }

        private void SetLink(string link)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(() => SetLink(link)));
                return;
            }

            toolTip1.SetToolTip(statusLabel, link);
        }

        private void uploadButton_Click(object sender, EventArgs e)
        {
            StartStopUploadThread();
        }

        public void FormAbort()
        {
            _formAborted = true;
            StopUploadThread();
        }

        private void statusLabel_Click(object sender, EventArgs e)
        {
            var url = toolTip1.GetToolTip(statusLabel);
            if (url.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                Process.Start(url);
            }
        }
    }
}
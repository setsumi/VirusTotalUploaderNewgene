﻿using System;
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

        public enum StatusMessageStyle { Normal, Red, Green, Error, Success, ShortWait, Progress, Abort }
        private enum FormStatus { Check, Upload }

        private readonly string _path;
        public string FullPath { get { return _path; } }
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
        private Semaphore _waiter = null, _quickWaiter = null;

        private StatusMessageStyle _state = StatusMessageStyle.Normal;
        public StatusMessageStyle State { get { return _state; } }
        public string Operation { get { return uploadButton.Enabled ? uploadButton.Text : ""; } }
        private long _fileSize = 0;
        public long FileSize { get { return _fileSize; } }

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
            _path = Path.GetFullPath(path);
            _fileCounter = counter;

            // for the embedding of this form into main form's panel
            TopLevel = false;
            FormBorderStyle = FormBorderStyle.None;

            InitializeComponent();
            // my load
            FormInit();
        }

        private void ChangeStatus(string text, StatusMessageStyle mode)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(() => ChangeStatus(text, mode)));
                return;
            }

            _state = mode;
            statusLabel.Text = text;
            switch (mode)
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
                case StatusMessageStyle.ShortWait:
                    statusLabel.BackColor = Color.FromArgb(60, 63, 65);
                    statusLabel.ForeColor = Color.FromArgb(255, 255, 0);
                    break;
                case StatusMessageStyle.Progress:
                    statusLabel.BackColor = Color.FromArgb(255, 255, 0);
                    statusLabel.ForeColor = Color.FromArgb(0, 0, 0);
                    break;
                case StatusMessageStyle.Abort:
                    statusLabel.BackColor = Color.FromArgb(255, 0, 0);
                    statusLabel.ForeColor = Color.FromArgb(255, 255, 255);
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

        private void DisplayError(string error, StatusMessageStyle mode = StatusMessageStyle.Error)
        {
            //var messageBox = new DarkMessageBox(error, LocalizationHelper.Base.UploadForm_Error, DarkMessageBoxIcon.Error, DarkDialogButton.Ok);
            //messageBox.ShowDialog();
            if (InvokeRequired)
            {
                this.Invoke(new Action(() => DisplayError(error, mode)));
                return;
            }

            ChangeStatus(error, mode);
            if (_settings.Sounds) SystemSounds.Hand.Play();
        }

        private void DisplaySuccess(string success)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(() => DisplaySuccess(success)));
                return;
            }

            ChangeStatus(success, StatusMessageStyle.Success);
            if (_settings.Sounds) SystemSounds.Beep.Play();
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

                ChangeStatus(LocalizationHelper.Base.Message_Init, StatusMessageStyle.ShortWait);
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
                    var mode = StatusMessageStyle.Error;
                    if (ex is ThreadAbortException || ex is System.Threading.Tasks.TaskCanceledException)
                    {
                        type = "";
                        msg = "Operation aborted.";
                        mode = StatusMessageStyle.Abort;
                    }
                    else
                    {
                        type = ex.GetType() + ": ";
                        msg = ex.Message.OneLine();
                    }
                    DisplayError(type + msg, mode);
                    Finish(Status);
                }
            }
            finally
            {
                ApiRateRelease();
                QuickApiRateRelease();
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

            // Check file
            if (Status == FormStatus.Check)
            {
                ChangeStatus(LocalizationHelper.Base.Message_Check, StatusMessageStyle.ShortWait);

                if (!_settings.QuickCheck)
                {
                    goto labelCheck;
                }
                (bool found, bool error) = QuickCheck();
                if (error)
                {
                    goto labelCheck;
                }
                else if (found)
                {
                    return true;
                }
                else
                {
                    Status = FormStatus.Upload;
                    goto labelUpload;
                }

            labelCheck:
                try
                {
                    ApiRateWait();
                    var reportRequest = new RestRequest($"api/v3/files/{SHA256}", Method.Get);
                    reportRequest.AddHeader("x-apikey", _settings.ApiKey);
                    reportRequest.Timeout = TimeSpan.FromMinutes(1); // normal timeout

                    var reportResponse = _client.Execute(reportRequest);
                    var reportContent = reportResponse.Content;
                    ApiRateRelease();
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
                        var lastAnalysisResults = json.data.attributes.last_analysis_results as JObject;
                        string lastAnalysisDate = null;
                        try { lastAnalysisDate = json.data.attributes.last_analysis_date.ToString(); }
                        catch (RuntimeBinderException)
                        {
                            throw new MyException("Analysis is probably not finished yet. Wait and retry, or click for details.");
                        }
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
                        else
                        {
                            stat = StatusMessageStyle.Error;
                        }

                        SetLink($"https://www.virustotal.com/gui/file/{SHA256}");
                        ChangeStatus($"[{typeTag}] {numberDetected}/{numberTotal} detected ({formattedDate}), click for details.", stat);
                    }
                    catch (Exception ex)
                    {
                        if (ex is MyException)
                            DisplayError(ex.Message.OneLine());
                        else
                            DisplayError($"Unrecognized responce: {ex.GetType()}: {ex.Message.OneLine()}");
                        return false;
                    }
                }
            }

        labelUpload:
            // Upload file
            if (Status == FormStatus.Upload)
            {
                ChangeStatus("Upload wait...", StatusMessageStyle.Progress);

                //string uploadUri = "https://www.virustotal.com/vtapi/v2/file/scan";
                string uploadUri = "api/v3/files";

                // get special uri for big files
                if (LargeFile)
                {
                    ApiRateWait();
                    var uploadUrlRequest = new RestRequest($"api/v3/files/upload_url", Method.Get);
                    uploadUrlRequest.AddHeader("x-apikey", _settings.ApiKey);
                    uploadUrlRequest.Timeout = TimeSpan.FromMinutes(1); // normal timeout

                    var uploadUrlResponse = _client.Execute(uploadUrlRequest);
                    var uploadUrlContent = uploadUrlResponse.Content;
                    ApiRateRelease();

                    json = JsonConvert.DeserializeObject(uploadUrlContent);
                    uploadUri = json.data.ToString();
                }

                ApiRateWait(true);
                ChangeStatus($"Uploading {Utils.BytesToHumanReadable((ulong)_fileSize)}...", StatusMessageStyle.Progress);

                var scanRequest = new RestRequest(uploadUri, Method.Post);
                scanRequest.AddHeader("x-apikey", _settings.ApiKey);
                scanRequest.AddFile("file", fullPath);
                scanRequest.Timeout = TimeSpan.FromHours(6); // super upload timeout

                _cancelTokenSource = new CancellationTokenSource();
                var task = _client.ExecuteAsync(scanRequest, _cancelTokenSource.Token);
                // this will wait for completion or cancel/error
                var scanResponse = task.Result;
                ApiRateRelease();
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

        private (bool Found, bool Error) QuickCheck()
        {
            bool found = false;
            bool error = false;

            RestClientOptions opt = new RestClientOptions
            {
                BaseUrl = new Uri("https://www.virustotal.com"),
                UserAgent = "VirusTotal"
            };
            using (var client = new RestClient(opt))
            {
                try
                {
                    QuickApiRateWait();
                    var url = "4h0w8MnA2hHsFQ15A2uH+KkCLy2rEy0c5BOS1t0KCzJKgHQQ/ZZr28gaLiWSLu4S5tcOqz8ZpONqlAELRrit394MbXf13qcI+qY3p17gd6fATRhCNJeVMAZdXnrZcbdsx4NSY4/mmHH+OpL+Rwc/bOR5QMSri5tysWiSiIqKhPfD7a4ATuSs211W+IFfS60GK6kiqLeKpxWIyU/j10RmfNBTI1yL0bex8SgUoFwJV65ErB2dESZEs1opcQ34TKsKK0GoK0kMbPGLJD7XCwsaDUPaNxxXG3ss".Decode();
                    var reportRequest = new RestRequest(url, Method.Post);
                    reportRequest.Timeout = TimeSpan.FromSeconds(15); // quick timeout
                    var jsonBody = $"[{{\"hash\": \"{SHA256}\", \"image_path\": \"{Path.GetFileName(_path)}\"}}]";
                    reportRequest.AddJsonBody(jsonBody, false, ContentType.Json);

                    var reportResponse = client.Execute(reportRequest);
                    var reportContent = reportResponse.Content;
                    QuickApiRateRelease();

                    dynamic json = JsonConvert.DeserializeObject(reportContent);
                    found = json.data[0].found;
                    int total = 0;
                    int positives = 0;
                    string permalink = "";
                    if (found)
                    {
                        total = json.data[0].total;
                        positives = json.data[0].positives;
                        permalink = json.data[0].permalink;

                        var stat = StatusMessageStyle.Normal;
                        if (total > 0)
                        {
                            stat = positives > 0 ? StatusMessageStyle.Red : StatusMessageStyle.Green;
                        }
                        else
                        {
                            throw new MyException("Analysis is probably not finished yet. Wait and retry, or click for details.");
                        }
                        SetLink(permalink);
                        ChangeStatus($"{positives}/{total} detected, click for details.", stat);
                    }
                }
                catch //(Exception ex)
                {
                    error = true;
                    //debug
                    //DisplayError(ex.Message.OneLine());
                }
            }
            return (Found: found, Error: error);
        }

        private void ApiRateWait(bool upload = false)
        {
            lock (_threadLock)
            {
                _waiter = _mainForm.rateLimiter.GetWaiter(upload);
                _waiter.WaitOne();
            }
        }

        private void ApiRateRelease()
        {
            lock (_threadLock)
            {
                if (_waiter != null)
                {
                    _mainForm.rateLimiter.ReleaseWaiter(_waiter);
                }
            }
        }

        private void QuickApiRateWait()
        {
            lock (_threadLock)
            {
                _quickWaiter = _mainForm.quickRateLimiter.GetWaiter();
                _quickWaiter.WaitOne();
            }
        }

        private void QuickApiRateRelease()
        {
            lock (_threadLock)
            {
                if (_quickWaiter != null)
                {
                    _mainForm.quickRateLimiter.ReleaseWaiter(_quickWaiter);
                }
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
                _fileSize = new FileInfo(_path).Length;
                double fileSizeInBytes = _fileSize;
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
                    _fileSize = 0;
                    statusLabel.Text = "";
                    uploadButton.Enabled = false;
                }

                fileGroup.Text = $"#{_fileCounter}   {formattedFileSize}{suffix}";
            }
            else
            {
                _fileSize = 0;
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

        public void uploadButton_Click(object sender, EventArgs e)
        {
            if (uploadButton.Enabled)
            {
                StartStopUploadThread();
            }
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
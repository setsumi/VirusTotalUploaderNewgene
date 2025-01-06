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
        private readonly object obj = new object();

        private enum StatusMessageStyle { Normal, Red, Green, Error, Success, Progress }
        private enum FormStatus { Check, Upload }

        private readonly string _path;
        private readonly MainForm _mainForm;
        private readonly Settings _settings;
        private Thread _uploadThread;
        private RestClient _client;
        private readonly int _fileCounter;

        private bool _largeFile = false;
        private FormStatus _status = FormStatus.Check;
        private string _SHA256 = "";

        private bool LargeFile { get { lock (obj) { return _largeFile; } } set { lock (obj) { _largeFile = value; } } }
        private FormStatus Status { get { lock (obj) { return _status; } } set { lock (obj) { _status = value; } } }
        private string SHA256 { get { lock (obj) { return _SHA256; } } set { lock (obj) { _SHA256 = value; } } }

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
                string message = ex.Message.Replace("\r", "").Replace("\n", " ");
                DisplayError($"{ex.GetType()}: {message}");
                Finish(Status);
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

                    var reportRequest = new RestRequest($"api/v3/files/{SHA256}", Method.Get);
                    reportRequest.AddHeader("x-apikey", _settings.ApiKey);

                    var reportResponse = _client.Execute(reportRequest);
                    var reportContent = reportResponse.Content;
#if (DEBUG)
                    File.WriteAllText("response-check.json", reportContent);
#endif
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
                    var uploadUrlRequest = new RestRequest($"api/v3/files/upload_url", Method.Get);
                    uploadUrlRequest.AddHeader("x-apikey", _settings.ApiKey);

                    var uploadUrlResponse = _client.Execute(uploadUrlRequest);
                    var uploadUrlContent = uploadUrlResponse.Content;

                    json = JsonConvert.DeserializeObject(uploadUrlContent);
                    uploadUri = json.data.ToString();
                }

                /*byte[] fileData = File.ReadAllBytes(fullPath);

                // Generate post objects
                Dictionary<string, object> postParameters = new Dictionary<string, object>();
                postParameters.Add("apikey", _settings.ApiKey);
                postParameters.Add("file", new Uploader.FileParameter(fileData, Path.GetFileName(fullPath), "multipart/form-data"));

                // Create request and receive response
                HttpWebResponse webResponse = Uploader.MultipartFormDataPost(uploadUri, "VirusTotalUploaderNewgene", postParameters);

                // Process response
                StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
                string fullResponse = responseReader.ReadToEnd();
                webResponse.Close();

                json = JsonConvert.DeserializeObject(fullResponse);
                var scanId = json.scan_id.ToString();
                var sha256 = json.sha256.ToString();
                var scanLink = $"https://www.virustotal.com/gui/file/{sha256}/detection/{scanId}";

                if (sha256.ToLower() != SHA256.ToLower())
                {
                    MessageBox.Show($"Checksum mismatch.\n{Path.GetFileName(fullPath)}\nOriginal:\n{SHA256.ToLower()}\nUploaded:\n{sha256.ToLower()}");
                }*/

                //=== APIv3 ====================
                var scanRequest = new RestRequest(uploadUri, Method.Post);
                scanRequest.AddHeader("x-apikey", _settings.ApiKey);
                scanRequest.AddFile("file", fullPath);
                scanRequest.Timeout = TimeSpan.FromHours(6); // super timeout

                var scanResponse = _client.Execute(scanRequest);
                var scanContent = scanResponse.Content;
#if (DEBUG)
                File.WriteAllText("response-upload.json", scanContent);
#endif
                json = JsonConvert.DeserializeObject(scanContent);

                var scanId = json.data.id.ToString();
                var scanLink = $"https://www.virustotal.com/gui/file/{SHA256}/detection/{scanId}";
                SetLink(scanLink);
                DisplaySuccess(LocalizationHelper.Base.Message_Uploaded);
            }

            return true;
        }

        private bool StopUploadThread()
        {
            if (_uploadThread != null && _uploadThread.IsAlive)
            {
                _uploadThread.Abort();
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

        private void UploadForm_Load(object sender, EventArgs e)
        {
            bool tooLarge = true;
            bool exists = File.Exists(_path);
            fileTextbox.Text = Path.GetFileName(_path);
            folderTextbox.Text = Path.GetDirectoryName(_path);
            uploadButton.Text = LocalizationHelper.Base.UploadForm_Check;

            if (exists)
            {
                // check file size
                double fileSizeInBytes = new FileInfo(_path).Length;
                double fileSizeInMB = fileSizeInBytes / (1024 * 1024);
                string formattedFileSize = fileSizeInMB.ToString("0.00");
                string suffix = "";
                tooLarge = fileSizeInMB > 650;
                LargeFile = fileSizeInMB > 32;
                if (tooLarge)
                    suffix = "   exceeds 650MB limit";
                else if (LargeFile)
                    suffix = "   large file >32MB";

                if (!tooLarge)
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

                fileGroup.Text = $"#{_fileCounter}   {formattedFileSize} MB{suffix}";
            }
            else
            {
                statusLabel.Text = "";
                fileGroup.Text = $"#{_fileCounter}   File does not exist.";
                uploadButton.Enabled = false;
            }

            if (exists && !tooLarge && _settings.DirectUpload)
            {
                StartStopUploadThread();
            }
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

        private void UploadForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopUploadThread();
        }

        private void statusLabel_Click(object sender, EventArgs e)
        {
            fileTextbox.Focus(); // otherwise it will scroll to last focused control
            var url = toolTip1.GetToolTip(statusLabel);
            if (url.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            {
                Process.Start(url);
            }
        }
    }
}
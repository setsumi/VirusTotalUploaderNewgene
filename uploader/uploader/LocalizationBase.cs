namespace uploader
{
    class LocalizationBase
    {
        public string MainForm_DragFile = "Drag and drop files or folders";
        public string MainForm_More = "Menu";

        public string SettingsForm_Title = "Settings";
        public string SettingsForm_General = "General settings";
        public string SettingsForm_Key = "API key";
        public string SettingsForm_Get = "Get API key";
        public string SettingsForm_Language = "Language";
        public string SettingsForm_Save = "Save";
        public string SettingsForm_Open = "Explore settings file";
        public string SettingsForm_DirectUpload = "Upload file(s) immediately";

        public string UploadForm_Check = "Check";
        public string UploadForm_Upload = "Upload";
        public string UploadForm_Cancel = "Abort";
        public string UploadForm_NoApiKey = "You have not entered an API key. Please go to settings and add one.";
        public string UploadForm_InvalidLength = "Invalid API key length. The key must contain 64 characters.";

        public string Message_Idle = "Idle, click for details.";
        public string Message_Init = "Initializing...";
        public string Message_Check = "Checking...";
        public string Message_Upload = "Uploading...";
        public string Message_Uploaded = "Uploaded, click for details.";
        public string Message_NoSettings = "No settings file exists.";
    }
}

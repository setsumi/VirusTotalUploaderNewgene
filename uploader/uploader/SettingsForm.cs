using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using DarkUI.Forms;

namespace uploader
{
    public partial class SettingsForm : DarkForm
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            var settings = Settings.LoadSettings();

            apiTextbox.Text = settings.ApiKey;
            directCheckbox.Checked = settings.DirectUpload;
            callsPerMinuteUpDown.Value = settings.CallsPerMinute;
            minSizeUpDown.Value = settings.MinimumFileSize;
            soundCheckbox.Checked = settings.Sounds;

            var languages = LocalizationHelper.GetLanguages();
            languageCombo.Items.Clear();
            foreach (var language in languages)
            {
                languageCombo.Items.Add(language);
            }

            if (string.IsNullOrEmpty(settings.Language))
            {
                var defaultLanguage = languageCombo.Items.Add("Default (Build-in English)");
                languageCombo.SelectedIndex = defaultLanguage;
            }
            else
            {
                var index = languageCombo.Items.IndexOf(settings.Language);
                languageCombo.SelectedIndex = index;
            }

            generalGroupBox.Text = "";
            apiLabel.Text = LocalizationHelper.Base.SettingsForm_Key;
            getApiButton.Text = LocalizationHelper.Base.SettingsForm_Get;
            languageLabel.Text = LocalizationHelper.Base.SettingsForm_Language;
            saveButton.Text = LocalizationHelper.Base.SettingsForm_Save;
            openButton.Text = LocalizationHelper.Base.SettingsForm_Open;
            this.Text = LocalizationHelper.Base.SettingsForm_Title;
            directCheckbox.Text = LocalizationHelper.Base.SettingsForm_DirectUpload;
            statusLabel.Text = "";

            //LocalizationHelper.Export();
        }

        private void darkButton1_Click(object sender, EventArgs e)
        {
            var file = Settings.GetSettingsFilename();
            if (!File.Exists(file))
            {
                statusLabel.Text = LocalizationHelper.Base.Message_NoSettings;
                return;
            }

            var args = $"/e, /select, \"{Settings.GetSettingsFilename()}\"";

            var info = new ProcessStartInfo { FileName = "explorer", Arguments = args };
            Process.Start(info);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            apiTextbox.Text = apiTextbox.Text.Trim();

            var settings = new Settings
            {
                ApiKey = apiTextbox.Text,
                Language = languageCombo.Text,
                DirectUpload = directCheckbox.Checked,
                CallsPerMinute = (int)callsPerMinuteUpDown.Value,
                MinimumFileSize = (int)minSizeUpDown.Value,
                Sounds = soundCheckbox.Checked
            };

            Settings.SaveSettings(settings);

            // Needs full restart to initialize main form strings again
            (Owner as MainForm).Clear();
            Application.Restart();
            Environment.Exit(0);
        }

        private void getApiButton_Click(object sender, EventArgs e)
        {
            Process.Start("https://docs.virustotal.com/reference/getting-started");
        }

        private void apiLimitsLabel_Click(object sender, EventArgs e)
        {
            Process.Start("https://docs.virustotal.com/reference/public-vs-premium-api");
        }
    }
}

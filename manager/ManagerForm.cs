using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace manager
{
    public partial class ManagerForm : Form
    {
        private readonly string _regasmPath = Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), "RegAsm.exe");
        private readonly string _dllPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "shellext.dll");

        public ManagerForm()
        {
            InitializeComponent();
        }

        private void ManagerForm_Load(object sender, EventArgs e)
        {
            textCmdInstall.Text = $"\"{_regasmPath}\" /codebase \"{_dllPath}\"";
            textCmdUninstall.Text = $"\"{_regasmPath}\" /u \"{_dllPath}\"";
            textOutput.Clear();
        }

        private void ManagerForm_Shown(object sender, EventArgs e)
        {
            textOutput.Focus();
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            RunRegasm($"/codebase \"{_dllPath}\"", "Install");
        }

        private void buttonUnregister_Click(object sender, EventArgs e)
        {
            RunRegasm($"/u \"{_dllPath}\"", "Uninstall");
        }

        private async void RunRegasm(string command, string prompt)
        {
            buttonRegister.Enabled = false;
            buttonUnregister.Enabled = false;
            textOutput.Clear();
            try
            {
                var psi = new ProcessStartInfo()
                {
                    FileName = _regasmPath,
                    Arguments = command,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = new Process { StartInfo = psi })
                {
                    process.OutputDataReceived += DataReceived;
                    process.ErrorDataReceived += DataReceived;

                    process.Start();
                    process.BeginOutputReadLine(); // Start asynchronous reading
                    process.BeginErrorReadLine();
                    // Wait for exit without blocking UI
                    await Task.Run(() => process.WaitForExit());
                    if (process.ExitCode == 0)
                    {
                        MessageBox.Show($"{prompt} Success.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"{prompt}\nOperation failed.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Source}\n{ex.Message}", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            buttonRegister.Enabled = true;
            buttonUnregister.Enabled = true;
        }

        private void DataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                this.BeginInvoke(new Action(() =>
                {
                    textOutput.AppendText(e.Data + Environment.NewLine);
                    textOutput.ScrollToCaret();
                }));
            }
        }

        private void textCmdInstall_TextChanged(object sender, EventArgs e)
        {
            var control = (sender as TextBox);
            toolTip1.SetToolTip(control, control.Text);
        }
    } // ManagerForm
}

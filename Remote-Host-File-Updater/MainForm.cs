using System;
using System.IO;
using System.Windows.Forms;

namespace Remote_Host_File_Updater
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            string[] hostnames = computerTBx.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (var hostname in hostnames)
            {
                var modified = AddHostEntry(hostname);
                if (modified)
                {
                    MessageBox.Show("Success!");
                }
                MessageBox.Show("Failed!");
            }
        }

        private bool AddHostEntry(string hostname)
        {
            var hostfile = @"\\" + hostname + @"\C$\Windows\System32\drivers\etc\hosts";

            if (File.Exists(hostfile))
            {
                //var hostsInfo = new FileInfo(hostfile);
                try
                {
                    using (
                        StreamWriter w =
                            File.AppendText(hostfile))
                    {
                        w.WriteLine(IpAddressTBx.Text + "\t" + TargetHostTBx.Text);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }
            }
            MessageBox.Show(hostfile + " not found!");
            return false;
        }
    }
}

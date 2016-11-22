using System;
using System.IO;
using System.Reflection;
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

        private void removeBtn_Click(object sender, EventArgs e)
        {
            string[] hostnames = computerTBx.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (var hostname in hostnames)
            {
                var removed = RemoveHostEntry(hostname);
                if (removed)
                {
                    MessageBox.Show("Success!");
                }
                MessageBox.Show("Failed!");
            }
        }

        private bool RemoveHostEntry(string hostname)
        {
            var hostfile = @"\\" + hostname + @"\C$\Windows\System32\drivers\etc\hosts";
            var output = @"\\" + hostname + @"\C$\Windows\System32\drivers\etc\temp.txt";

            try
            {
                string line;
                using (StreamReader reader = new StreamReader(hostfile))
                using (var sw = new StreamWriter(output))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (!(line.Contains(IpAddressTBx.Text) && line.Contains(TargetHostTBx.Text)))
                        {
                            sw.WriteLine(line);
                        }
                    }
                }
                File.Delete(hostfile);
                File.Move(output, hostfile);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void resetBtn_Click(object sender, EventArgs e)
        {
            string[] hostnames = computerTBx.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (var hostname in hostnames)
            {
                var reset = ResetHosts(hostname);
                if (reset)
                {
                    MessageBox.Show("Success!");
                }
                MessageBox.Show("Failed!");
            }
        }

        private bool ResetHosts(string hostname)
        {
            var hostfile = @"\\" + hostname + @"\C$\Windows\System32\drivers\etc\hosts";
            var blankhosts = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"hosts");
            File.Delete(hostfile);
            File.Copy(blankhosts, hostfile);
            return true;
        }
    }
}

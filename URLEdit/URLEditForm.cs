using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace URLEdit
{
    public partial class URLEditForm : Form
    {
        private List<Parameter> parameters = new List<Parameter>();
        private BindingList<Parameter> paramterBList = new BindingList<Parameter>();
        private string StartOfURL = string.Empty;


        public URLEditForm()
        {
            InitializeComponent();
            parameters.Add(new Parameter("", ""));
            var bindlingList = new BindingList<Parameter>(parameters);
            var source = new BindingSource(bindlingList,null);
            dgvParameters.DataSource = source;
        }

        private void InURLTextChanged(object sender, EventArgs e)
        {
            string querystring = string.Empty;
            string currurl = tbInURL.Text;

            // Check to make sure some query string variables
            // exist and if not add some and redirect.
            int iqs = currurl.IndexOf('?');
            if (iqs == -1)
            {
                tbOutURL.Text = currurl;
                return; // do nothing
            }
            else if (iqs >= 0)
            {
                querystring = (iqs < currurl.Length - 1) ? currurl.Substring(iqs + 1) : String.Empty;
                StartOfURL = currurl.Substring(0, iqs);
                tbBaseUrl.Text = StartOfURL;
            }

            var parameterList = querystring.Split('&');

            parameters.Clear();
            foreach (var element in parameterList)
            {
                string[] items = element.Split('=');
                if (items.Count() > 1)
                {
                    parameters.Add(new Parameter(items[0], items[1]));
                }
            }
            var bindlingList = new BindingList<Parameter>(parameters);
            var source = new BindingSource(bindlingList, null);
            dgvParameters.DataSource = source;
            tbOutURL.Text = CreateRevisedURL();
        }

        private string CreateRevisedURL()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var s in parameters)
            {
                sb.Append(s.Name + "=" + s.Value + "&");
            }
            string updatedUrl = StartOfURL + '?' + sb.ToString();
            updatedUrl = updatedUrl.Remove(updatedUrl.Length - 1);
            return updatedUrl;
        }

        private void tbBaseUrl_TextChanged(object sender, EventArgs e)
        {
            StartOfURL = tbBaseUrl.Text;
            tbOutURL.Text = CreateRevisedURL();
        }

        private void dgvParameters_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            tbOutURL.Text = CreateRevisedURL();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Clipboard.SetText(tbOutURL.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string url = tbOutURL.Text;
            url = url.Replace("&", "^&");
            url = url.Replace("\"","%22");

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start(new ProcessStartInfo("cmd", $"/q /c start {url}"));
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url);
            }
            else
            {
                Console.WriteLine("Found no browser to start\n");
            }
        }
    }
}

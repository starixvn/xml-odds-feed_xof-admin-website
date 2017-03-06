using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Couchbase;
using Fanex.Data.Configuration;
using Fanex.OddsFeed.Common.DTO;
using Fanex.OddsFeed.WindowsServiceLibrary.Services;
using Newtonsoft.Json;

namespace CouchbaseApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnVersionFullChain_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cbVersionFullchain.Text))
            {
                MessageBox.Show("Please select Version/Fullchain key!");
                return;
            }

            //using (var cache = new CouchbaseClient())
            //{
            //    var data =  cache.Get(cbVersionFullchain.Text);
            //    textBox1.Text = data.ToString();
            //}
        }

        private void btnMatchOutright_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cbMatchOutright.Text))
            {
                MessageBox.Show("Please select Match/Outright key!");
                return;
            }

            var version = string.IsNullOrEmpty(cbNumberVersion.Text) ? 0 : Convert.ToInt32(cbNumberVersion.Text);
            var key = $"{cbMatchOutright.Text}{version}";

            //using (var cache = new CouchbaseClient())
            //{
            //    HandleData(cache.Get(key), cbMatchOutright.Text);
            //}
        }

        private void HandleData(object data, string cbKey)
        {
            if (cbKey.Equals("apolloevents", StringComparison.OrdinalIgnoreCase))
            {
                var outrights = data as IEnumerable<Outright>;
                ParseToForm(outrights);
            }
            else
            {
                var matches = data as IEnumerable<Match>;
                ParseToForm(matches);
            }
        }

        private void ParseToForm<T>(T data)
        {
            if (data != null)
            {
                textBox1.Text = JsonConvert.SerializeObject(data);
            }
            else
            {
                textBox1.Text = string.Empty;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var oddsFeedService = new WindowsOddsFeedService();
            oddsFeedService.Update(1);
        }
    }
}

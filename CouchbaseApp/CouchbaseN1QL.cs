using System;
using System.Windows.Forms;

namespace CouchbaseApp
{
    public partial class CouchbaseN1QL : Form
    {
        private readonly N1QLV1 n1qlV1;

        public CouchbaseN1QL()
        {
            InitializeComponent();

            n1qlV1 = new N1QLV1();
        }


        private void store_Click(object sender, EventArgs e)
        {
            var startTime = DateTime.Now;
            n1qlV1.StoreData();
            var endTime = DateTime.Now;

            var period = endTime - startTime;
            MessageBox.Show("OK in " + period);
        }
    }
}

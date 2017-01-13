using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fanex.Data.Configuration;

namespace CouchbaseApp
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            DbSettingProviderManager.Current.Start();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args == null || args[0] == "1")
            {
                using (var form1 = new Form1())
                {
                    Application.Run(form1);
                }
            }else
            {
                using (var logF = new LogFormcs())
                {
                    Application.Run(logF);
                }
            }
        }
    }
}

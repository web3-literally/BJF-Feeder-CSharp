using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Feeder_CSharp
{
    public partial class MainWnd : Form
    {
        public string m_strIP = "";
        public string m_region = "";
        public string m_strUserName = "";

        public MainWnd()
        {
            InitializeComponent();
        }

        private void m_BtnStart_Click(object sender, EventArgs e)
        {
            m_strUserName = m_TxtUserName.Text;

            if (m_ComboFeeder.Text == "UK")
            {
                m_region = "LD";
                m_strIP = "185.95.16.126";
            }
            else if (m_ComboFeeder.Text == "US")
            {
                m_region = "NY";
                m_strIP = "185.95.19.32";
            }

            if (string.IsNullOrEmpty(m_strIP) || string.IsNullOrEmpty(m_strUserName))
            {
                Console.WriteLine("*** Please input feeder and username correctly. ***");
                return;
            }

            Feeder.Instance().m_strIP = m_strIP;
            Feeder.Instance().m_region = m_region;
            Feeder.Instance().m_strUserName = m_strUserName;
            Feeder.Instance().ScrapStart();
        }

        
        private void MainWnd_FormClosing(object sender, FormClosingEventArgs e)
        {
            Feeder.Instance().ScrapFinish();
        }
    }
}

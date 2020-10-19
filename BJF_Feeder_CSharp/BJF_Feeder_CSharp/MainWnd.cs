using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace BJF_Feeder_CSharp
{
    public partial class MainWnd : Form
    {
        public string m_strIP = "";
        public string m_strUserName = "";

        public MainWnd()
        {
            InitializeComponent();
        }

        private void m_BtnStart_Click(object sender, EventArgs e)
        {
            m_strUserName = m_TxtUserName.Text;

            if (m_ComboFeeder.Text == "UK")
                m_strIP = "185.95.16.126";
            else if (m_ComboFeeder.Text == "US")
                m_strIP = "185.95.19.32";

            if (string.IsNullOrEmpty(m_strIP) || string.IsNullOrEmpty(m_strUserName))
            {
                Console.WriteLine("*** Please input feeder and username correctly. ***");
                return;
            }

            BJF_Feeder.Instance().m_strIP = m_strIP;
            BJF_Feeder.Instance().m_strUserName = m_strUserName;
            BJF_Feeder.Instance().ScrapStart();

            m_BtnStart.Enabled = false;
        }

        
        private void MainWnd_FormClosing(object sender, FormClosingEventArgs e)
        {
            BJF_Feeder.Instance().ScrapFinish();
        }
    }
}

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
        [DllImport("feeder_connector.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern int ConnectToFeeder(String strIP, int nPort, String strUserName);
        [DllImport("feeder_connector.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern void DisconnectFromFeeder();
        [DllImport("feeder_connector.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern int ReceiveData(StringBuilder strVal1, double[] listVal1, double[] listVal2);

        public string m_strIP = "";
        public int m_nPort = 9236;
        public string m_strUserName = "";
        public bool m_bIsConnect = false;

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

            m_Timer.Enabled = true;
        }

        private void m_Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (ConnectToFeeder(m_strIP, m_nPort, m_strUserName) == 1)
                {
                    if (m_bIsConnect == false)
                    {
                        Console.WriteLine("--- Server Connected! ---");
                        m_bIsConnect = true;
                    }
                }
                else
                {
                    if (m_bIsConnect == true)
                    {
                        Console.WriteLine("--- Server Disconnected! ---");
                        m_bIsConnect = false;
                    }
                }

                if (!m_bIsConnect) return;

                double[] listVal1 = new double[200];
                double[] listVal2 = new double[200];
                StringBuilder strVal1 = new StringBuilder(100);

                int nRet = ReceiveData(strVal1, listVal1, listVal2);

                if (nRet > 0)
                {
                    string[] SymbolList = strVal1.ToString().Replace("/", "").Split(';');
                    for (int i = 0; i < nRet; i++)
                    {
                        string strSymbol = SymbolList[i];
                        double dBid = listVal1[i];
                        double dAsk = listVal2[i];

                        Console.WriteLine(string.Format("{0}, {1}, {2}", strSymbol, dBid, dAsk));
                        Logger.Instance().DumpQuote(strSymbol, dBid.ToString(), dAsk.ToString());
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void MainWnd_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_Timer.Enabled = false;
            DisconnectFromFeeder();
        }
    }
}

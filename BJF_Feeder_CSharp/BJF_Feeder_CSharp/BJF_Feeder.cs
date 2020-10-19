using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace BJF_Feeder_CSharp
{
    class BJF_Feeder
    {
        [DllImport("feeder_connector.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern int ConnectToFeeder(String strIP, int nPort, String strUserName);
        [DllImport("feeder_connector.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern void DisconnectFromFeeder();
        [DllImport("feeder_connector.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern int ReceiveData(StringBuilder strVal1, double[] listVal1, double[] listVal2);

        private static BJF_Feeder m_sFeeder = null;
        private Thread m_threadScrap = null;

        public string m_strIP = "";
        public int m_nPort = 9236;
        public string m_strUserName = "";
        public bool m_bIsConnect = false;
        public bool m_bThreadStart = false;

        public static BJF_Feeder Instance()
        {
            if (m_sFeeder == null)
                m_sFeeder = new BJF_Feeder();
            return m_sFeeder;
        }
        public void ScrapStart()
        {
            Thread m_threadScrap = new Thread(new ThreadStart(FuncThreadScrap));
            m_bThreadStart = true;
            m_threadScrap.Start();
        }
        public void ScrapFinish()
        {
            m_bThreadStart = false;
            Thread.Sleep(1000);

            if (m_threadScrap != null && m_threadScrap.IsAlive)
            {
                m_threadScrap.Abort();
            }

            DisconnectFromFeeder();
        }
        public void FuncThreadScrap()
        {
            while (m_bThreadStart)
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
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                Thread.Sleep(1);
            }
            DisconnectFromFeeder();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Feeder_CSharp
{
    class Feeder
    {
        [DllImport("connector.dll", CharSet = CharSet.Unicode)]
        public static extern int ConnectToFeeder(String strIP, int nPort, String strUserName);
        [DllImport("connector.dll", CharSet = CharSet.Unicode)]
        public static extern void DisconnectFromFeeder();
        [DllImport("connector.dll", CharSet = CharSet.Unicode)]
        public static extern int ReceiveData(StringBuilder strVal1, double[] listVal1, double[] listVal2);


        [DllImport("MemMap.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr SetMemString(string tag, string msg);
        [DllImport("MemMap.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr GetMemString(string tag);

        private static Feeder m_sFeeder = null;
        private Thread m_threadScrap = null;

        public static double[] listVal1 = new double[1000];
        public static double[] listVal2 = new double[1000];
        public static StringBuilder strVal1 = new StringBuilder(10000);

        public string m_strIP = "";
        public string m_region = "";
        public int m_nPort = 9236;
        public string m_strUserName = "";
        public bool m_bIsConnect = false;
        public bool m_bThreadStart = false;

        public static Feeder Instance()
        {
            if (m_sFeeder == null)
                m_sFeeder = new Feeder();
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
                    if (m_bIsConnect == false && ConnectToFeeder(m_strIP, m_nPort, m_strUserName) == 1)
                    {
                        Console.WriteLine("--- Server Connected! ---");
                        m_bIsConnect = true;
                    }

                    if (!m_bIsConnect) return;

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

                            if (strSymbol == "USDJPY" || strSymbol == "GBPJPY" || strSymbol == "GBPUSD")
                            {
                                if (m_region == "NY")
                                {
                                    SetMemString(strSymbol + "_Bid_NY", dBid.ToString());
                                    SetMemString(strSymbol + "_Ask_NY", dAsk.ToString());
                                }
                                else if (m_region == "LD")
                                {
                                    SetMemString(strSymbol + "_Bid_LD", dBid.ToString());
                                    SetMemString(strSymbol + "_Ask_LD", dAsk.ToString());
                                }

                            }
                        }
                    }
                    else if (nRet < 0)
                    {
                        Console.WriteLine("--- Server Disconnected! ---");
                        m_bIsConnect = false;
                        Thread.Sleep(1000);
                    }
                }
                catch (AccessViolationException ex) // Memory Violation Exception
                {
                    Console.WriteLine(ex);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                //Thread.Sleep(1);
            }
            DisconnectFromFeeder();
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BJF_Feeder_CSharp
{
    class Logger
    {
        private static Logger m_Logger = null;
        private string m_strAppDirPath = "";
        private bool m_bSaveing = false;
        private Dictionary<string, string> m_dicLog = new Dictionary<string, string>();

        public static Logger Instance()
        {
            if (m_Logger == null)
            {
                m_Logger = new Logger();
            }
            return m_Logger;
        }
        public Logger()
        {
            m_strAppDirPath = Path.GetDirectoryName(Application.ExecutablePath);
            if (!Directory.Exists(m_strAppDirPath + "/Logs"))
                Directory.CreateDirectory(m_strAppDirPath + "/Logs");
        }

        private void Save(string filename, string outStr)
        {
            try
            {
                if (!m_bSaveing)
                {
                    m_bSaveing = true;
                    if (!m_dicLog.ContainsKey(filename))
                    {
                        m_dicLog.Add(filename, outStr);
                    }
                    else
                    {
                        string curLog = m_dicLog[filename];
                        curLog += outStr;
                        m_dicLog[filename] = curLog;

                        if (curLog.Length >= 10000)
                        {
                            using (var fs = File.Open(filename, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                            {
                                byte[] toBytes = Encoding.ASCII.GetBytes(curLog);
                                fs.Write(toBytes, 0, toBytes.Length);
                            }
                            m_dicLog[filename] = "";
                        }
                        else
                        {
                            m_dicLog[filename] = curLog;
                        }

                    }
                    m_bSaveing = false;
                }
            }
            catch
            {
                m_bSaveing = false;
            }
        }


        public void DumpQuote(string symbol, string Bid, string Ask)
        {
            string filename = m_strAppDirPath + "/Logs/" + symbol + ".txt";
            string outStr = DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss:fffffff") + "," + symbol + ",Bid," + Bid + ",Ask," + Ask + "\r\n";
            Save(filename, outStr);
        }

    }
}

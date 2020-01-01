using CryptoShark.Utility.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoShark.Utility
{
    public class FileLogger
    {
        public const string DEFAULT_LOG_FILENAME_PREFIX = "CryptoSharkLog_";
        public const string DEFAULT_LOG_FILEPATH = "CryptoSharkLogs";

        public event EventHandler<FileLoggedEventArgs> OnFileLogged;

        private static string fileName;

        #region Singleton Members
        private static FileLogger instance;
        public static FileLogger Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FileLogger();
                    fileName = Path.Combine(DEFAULT_LOG_FILEPATH, DEFAULT_LOG_FILENAME_PREFIX + DateTime.Now.ToString("dd.MM.yyyy_HH.mm") + ".txt");

                    if (!Directory.Exists(DEFAULT_LOG_FILEPATH))
                        Directory.CreateDirectory(DEFAULT_LOG_FILEPATH);
                }
                return instance;
            }
        }
        #endregion

        public void WriteLog(string text, bool withTimeStamp = true, bool invokeEvent = true)
        {
            if (!File.Exists(fileName))
            {
                var stream = File.CreateText(fileName);
                stream.Close();
            }

            if (withTimeStamp)
                text += " [" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "]";

            if (invokeEvent)
                OnFileLogged?.Invoke(this, new FileLoggedEventArgs()
                {
                    Text = text
                });

            File.AppendAllText(fileName, text + Environment.NewLine);
        }
    }
}

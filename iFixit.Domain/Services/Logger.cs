using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace iFixit.UI.Services
{
    public static class Logger
    {
        private static int _max = 500;

        /// <summary> 
        /// max number of line logged by the system 
        /// </summary> 
        public static int MaxSize
        {
            get { return _max; }
            set { _max = value; }
        }

        private static bool _enabled = true;
        /// <summary> 
        /// enable/disable store logging 
        /// </summary> 
        public static bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        private static List<string> buffer { get; set; }

        public static void WriteLine(Exception e)
        {
            WriteLine("EXCEPTION {0} {1} STACK TRACE {2}", e.Message, e.InnerException != null ? " HAS INNER EXCEPTION" : "", e.StackTrace);
            if (e.InnerException != null)
            {
                WriteLine(e.InnerException);
            }
        }

        public static void WriteLine(string format, params object[] args)
        {
            string s = string.Format(format, args);
            WriteLine(s);
        }

        public static void WriteLine(string line)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(DateTime.Now.ToString("yyyyMMddhhmss"));
            sb.Append("TID");
           // sb.Append(Thread.CurrentThread.ManagedThreadId);
            sb.Append(" ");
            sb.Append(line);

            if (Enabled)
            {
                if (buffer == null)
                {
                    buffer = new System.Collections.Generic.List<string>();
                }

                buffer.Add(sb.ToString());

                while (buffer.Count() > MaxSize)
                {
                    buffer.RemoveAt(0);
                }
            }

            System.Diagnostics.Debug.WriteLine(sb);
        }

        public static void Load(StreamReader stream)
        {
            buffer = new List<string>();

            while (!stream.EndOfStream)
            {
                buffer.Add(stream.ReadLine());
            }
        }

     
        public static void Save(StreamWriter stream)
        {
            foreach (string s in buffer)
            {
                stream.WriteLine(s);
            }
        }

        public static string GetStoredLog()
        {
            StringBuilder sb = new StringBuilder();

            if (buffer != null)
            {
                foreach (string s in buffer)
                {
                    sb.AppendLine(s);
                }
            }

            return sb.ToString();
        }
    }
}

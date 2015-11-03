using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace iFixit.UI.Services
{
    public static class Logger
    {
        /// <summary> 
        /// max number of line logged by the system 
        /// </summary> 
        public static int MaxSize { get; set; } = 500;

        /// <summary> 
        /// enable/disable store logging 
        /// </summary> 
        public static bool Enabled { get; set; } = true;

        private static List<string> Buffer { get; set; }

        public static void WriteLine(Exception e)
        {
            while (true)
            {
                WriteLine("EXCEPTION {0} {1} STACK TRACE {2}", e.Message, e.InnerException != null ? " HAS INNER EXCEPTION" : "", e.StackTrace);
                if (e.InnerException != null)
                {
                    e = e.InnerException;
                    continue;
                }
                break;
            }
        }

        public static void WriteLine(string format, params object[] args)
        {
            var s = string.Format(format, args);
            WriteLine(s);
        }

        public static void WriteLine(string line)
        {
            var sb = new StringBuilder();
            sb.Append(DateTime.Now.ToString("yyyyMMddhhmss"));
            sb.Append("TID");
           // sb.Append(Thread.CurrentThread.ManagedThreadId);
            sb.Append(" ");
            sb.Append(line);

            if (Enabled)
            {
                if (Buffer == null)
                {
                    Buffer = new List<string>();
                }

                Buffer.Add(sb.ToString());

                while (Buffer.Count() > MaxSize)
                {
                    Buffer.RemoveAt(0);
                }
            }

            System.Diagnostics.Debug.WriteLine(sb);
        }

        public static void Load(StreamReader stream)
        {
            Buffer = new List<string>();

            while (!stream.EndOfStream)
            {
                Buffer.Add(stream.ReadLine());
            }
        }

     
        public static void Save(StreamWriter stream)
        {
            foreach (var s in Buffer)
            {
                stream.WriteLine(s);
            }
        }

        public static string GetStoredLog()
        {
            var sb = new StringBuilder();

            if (Buffer == null) return sb.ToString();
            foreach (var s in Buffer)
            {
                sb.AppendLine(s);
            }

            return sb.ToString();
        }
    }
}

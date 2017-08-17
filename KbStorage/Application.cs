using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KbStorage
{
    internal class Application
    {
        private static volatile Application current;
        private static object syncRoot = new Object();

        public Dictionary<string,string> Properties { get; set; }

        private Application()
        {
            Properties = new Dictionary<string, string>();

            Properties.Add("DataStorage", string.Empty);            
        }

        public static Application Current
        {
            get
            {
                if (current == null)
                {
                    lock (syncRoot)
                    {
                        if (current == null)
                            current = new Application();
                    }
                }

                return current;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KbParser.Dto;
using System.Threading.Tasks;

namespace KbParser
{
    public sealed class CoreParser
    {
        private static volatile CoreParser instance;
        private static object syncRoot = new Object();

        private CoreParser() { }

        public static CoreParser Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new CoreParser();
                    }
                }

                return instance;
            }
        }

        public async Task<LcKbsDto> MapStringData(string data)
        {
            throw new NotImplementedException();
        }
    }
}

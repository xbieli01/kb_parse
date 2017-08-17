using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KbParser.Dto
{
    public class LcKbsDto
    {
        public LcKbsDto()
        {
            Days = new Dictionary<DateTime, LcDayDto>();
        }

        public Dictionary<DateTime, LcDayDto> Days { get; set; }
    }
}

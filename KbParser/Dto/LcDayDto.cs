using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KbParser.Dto
{
    public class LcDayDto
    {
        public EnumerableDto<LcDayCelebrationDto> Celebrations { get; set; }
        public DateTime Date { get; set; }
        public string LiturgicalYear { get; set; }
        public string HolyObligation { get; set; }
        public int Week { get; set; }
        public string Weekday { get; set; }
        public string NameDay { get; set; }
    }
}

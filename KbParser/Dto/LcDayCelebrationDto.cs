using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KbParser.Dto
{
    public class LcDayCelebrationDto
    {
        public string Memo { get; set; }
        public EnumerableDto<LcDayCelebrationHeadDto> Heads { get; set; }
        public EnumerableDto<LcDayCelebrationGroupDto> Groups { get; set; }
        public string Directory { get; set; }
    }
}

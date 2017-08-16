namespace KbParser.Dto
{
    public class LcDayCelebrationGroupDto
    {
        public string GroupTitle { get; set; }
        public EnumerableDto<LcDayCelebrationGroupReadingDto> Readings { get; set; }
        public EnumerableDto<LcDayCelebrationGroupPsalmDto> Psalms { get; set; }
        public EnumerableDto<LcDayCelebrationGroupGospelDto> Gospels { get; set; }
    }
}
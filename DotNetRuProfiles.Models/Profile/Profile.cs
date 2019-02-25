using System.Collections.Generic;

namespace DotNetRuProfiles.Models.Profile
{
    public class Profile
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string WorkplaceName { get; set; }
        public string WorkplaceUrl { get; set; }
        
        public IEnumerable<ProfileLectureLink> Meetups { get; set; } = new List<ProfileLectureLink>();
    }
}
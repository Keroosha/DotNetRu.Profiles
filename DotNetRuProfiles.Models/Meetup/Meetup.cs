using System.Collections.Generic;
using DotNetRuProfiles.Models.Profile;

namespace DotNetRuProfiles.Models.Meetup
{
    public class Meetup
    {
        public string Name {get; set;}
        public string Description {get; set;}
        public IEnumerable<ProfileLectureLink> LectureLinks {get; set;} = new List<ProfileLectureLink>();
        public string Address {get; set;}
    }
}
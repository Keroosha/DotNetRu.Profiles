using System.Collections.Generic;

namespace DotNetRuProfiles.Models.Place
{
    public class Place
    {
        public string Name { get; set; }
        public string ImageLink { get; set; }
        public string PlaceWebLink { get; set; }
        public string Description { get; set; }
        public IEnumerable<Meetup.Meetup> Meetups { get; set; } = new List<Meetup.Meetup>();
    }
}
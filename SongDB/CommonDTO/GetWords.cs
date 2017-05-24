using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CommonDTO
{
    [DataContract]
    public class GetWordsRequest
    {
        [DataMember]
        public Guid? SongId { get; set; }
    }

    [DataContract]
    public class GetWordsResponse
    {
        [DataMember]
        public List<Tuple<Guid, string>> Words { get; set; }
    }

    [DataContract]
    public class LocationsRequest
    {
        [DataMember]
        public Guid? SongId { get; set; }
    }

    [DataContract]
    public class LocationsResponse
    {
        [DataMember]
        public List<LocationDTO> Locations { get; set; }
    }

    [DataContract]
    public class LocationDTO
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string Word { get; set; }
        [DataMember]
        public string Song { get; set; }
        [DataMember]
        public int NumberInSong { get; set; }
        [DataMember]
        public int VerseNumber { get; set; }
        [DataMember]
        public int LineInVerse { get; set; }
    }
}
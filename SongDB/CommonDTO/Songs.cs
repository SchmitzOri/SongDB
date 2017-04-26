using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CommonDTO
{
    [DataContract]
    public class SongsRequest
    {
    }

    [DataContract]
    public class SongsResponse
    {
        [DataMember]
        public List<SongDTO> Songs { get; set; }
    }

    [DataContract]
    public class WordSongsRequest
    {
        [DataMember]
        public Guid WordId { get; set; }
    }

    [DataContract]
    public class WordSongsResponse
    {
        [DataMember]
        public List<SongDTO> WordSongs { get; set; }
    }

    [DataContract]
    public class SongLyricsRequest
    {
        [DataMember]
        public Guid SongId { get; set; }
    }

    [DataContract]
    public class SongLyricsResponse
    {
        [DataMember]
        public string SongLyrics { get; set; }
    }

    [DataContract]
    public class SongDTO
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public Guid ArtistId { get; set; }
    }
}

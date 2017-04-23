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

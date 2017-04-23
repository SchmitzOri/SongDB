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
}

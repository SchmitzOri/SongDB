using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CommonDTO
{
    [DataContract]
    public class PhraseAddRequest
    {
        [DataMember]
        public List<Guid> Words { get; set; }
    }

    [DataContract]
    public class PhraseAddResponse
    {
        [DataMember]
        public Guid Id { get; set; }
    }


    [DataContract]
    public class PhraseDeleteRequest
    {
        [DataMember]
        public Guid Id { get; set; }
    }

    [DataContract]
    public class PhraseDeleteResponse
    {
        [DataMember]
        public bool Success { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CommonDTO
{
    [DataContract]
    public class GroupAddRequest
    {
        [DataMember]
        public string Name { get; set; }
    }

    [DataContract]
    public class GroupAddResponse
    {
        [DataMember]
        public Guid Id { get; set; }
    }

    [DataContract]
    public class GroupUpdateRequest
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public List<Guid> Words { get; set; }
    }

    [DataContract]
    public class GroupUpdateResponse
    {
        [DataMember]
        public bool Success { get; set; }
    }

    [DataContract]
    public class GroupDeleteRequest
    {
        [DataMember]
        public Guid Id { get; set; }
    }

    [DataContract]
    public class GroupDeleteResponse
    {
        [DataMember]
        public bool Success { get; set; }
    }
}

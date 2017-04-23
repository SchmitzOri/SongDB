using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CommonDTO
{
    [DataContract]
    public class RelationAddRequest
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public Guid RelationType { get; set; }
        [DataMember]
        public Guid Word1 { get; set; }
        [DataMember]
        public Guid Word2 { get; set; }
    }

    [DataContract]
    public class RelationAddResponse
    {
        [DataMember]
        public Guid Id { get; set; }
    }


    [DataContract]
    public class RelationDeleteRequest
    {
        [DataMember]
        public Guid Id { get; set; }
    }

    [DataContract]
    public class RelationDeleteResponse
    {
        [DataMember]
        public bool Success { get; set; }
    }
}

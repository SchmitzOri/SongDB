using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CommonDTO
{
    [DataContract]
    public class RelationAllRequest
    {
        [DataMember]
        public Guid? TypeId { get; set; }
    }

    [DataContract]
    public class RelationAllResponse
    {
        [DataMember]
        public List<RelationDTO> Relations { get; set; }
    }

    [DataContract]
    public class RelationTypeUseCountRequest
    {
        [DataMember]
        public Guid TypeId { get; set; }
    }

    [DataContract]
    public class RelationTypeUseCountResponse
    {
        [DataMember]
        public int Count { get; set; }
    }
    [DataContract]
    public class RelationTypesRequest
    {
    }

    [DataContract]
    public class RelationTypesResponse
    {
        [DataMember]
        public List<RelationTypeDTO> Types { get; set; }
    }

    [DataContract]
    public class RelationDTO
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public RelationTypeDTO RelationType { get; set; }
        [DataMember]
        public Tuple<Guid, string> Word1 { get; set; }
        [DataMember]
        public Tuple<Guid, string> Word2 { get; set; }
    }

    [DataContract]
    public class RelationTypeDTO
    {
        [DataMember]
        public string TypeName { get; set; }
        [DataMember]
        public Guid Id { get; set; }
    }

    [DataContract]
    public class RelationAddTypeRequest
    {
        [DataMember]
        public string Name { get; set; }
    }

    [DataContract]
    public class RelationAddTypeResponse
    {
        [DataMember]
        public Guid TypeId { get; set; }
    }

    [DataContract]
    public class RelationAddRequest
    {
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

    [DataContract]
    public class RelationTypeDeleteRequest
    {
        [DataMember]
        public Guid TypeId { get; set; }
    }

    [DataContract]
    public class RelationTypeDeleteResponse
    {
        [DataMember]
        public bool Success { get; set; }
    }
}

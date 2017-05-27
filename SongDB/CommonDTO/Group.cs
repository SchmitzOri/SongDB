using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CommonDTO
{
    [DataContract]
    public class Group
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public Guid Id { get; set; }
    }


    [DataContract]
    public class GroupAddRequest
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<string> Words { get; set; }
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
        public List<string> Words { get; set; }
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

    [DataContract]
    public class GroupWordsRequest
    {
        [DataMember]
        public Guid Id { get; set; }
    }

    [DataContract]
    public class GroupWordsResponse
    {
        [DataMember]
        public List<Tuple<Guid, string>> Words { get; set; }
    }

    [DataContract]
    public class GroupAllRequest
    {
    }

    [DataContract]
    public class GroupAllResponse
    {
        [DataMember]
        public List<Group> Groups { get; set; }
    }
}

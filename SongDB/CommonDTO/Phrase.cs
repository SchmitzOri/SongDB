using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CommonDTO
{
    [DataContract]
    public class PhraseDTO
    {
        [DataMember]
        public Guid PhraseId { get; set; }
        [DataMember]
        public string Phrase { get; set; }
    }

    [DataContract]
    public class PhraseLocation
    {
        [DataMember]
        public Guid SongId { get; set; }
        [DataMember]
        public string SongName { get; set; }
        [DataMember]
        public int WordNumberInFile { get; set; }
    }

    [DataContract]
    public class PhraseLocationRequest
    {
        [DataMember]
        public string Phrase { get; set; }
    }

    [DataContract]
    public class PhraseLocationResponse
    {
        [DataMember]
        public List<PhraseLocation> Locations { get; set; }
    }

    [DataContract]
    public class PhraseAllRequest
    {
    }

    [DataContract]
    public class PhraseAllResponse
    {
        [DataMember]
        public List<PhraseDTO> Phrases { get; set; }
    }

    [DataContract]
    public class PhraseAddRequest
    {
        [DataMember]
        public List<string> Words { get; set; }
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

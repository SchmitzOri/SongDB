using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CommonDTO
{
    [DataContract]
    public class GetStatsRequest
    {
    }

    [DataContract]
    public class GetStatsResponse
    {
        [DataMember]
        public decimal CharsPerWord { get; set; }
        [DataMember]
        public decimal WordsInRow { get; set; }
        [DataMember]
        public decimal RowsInVerse{ get; set; }
        [DataMember]
        public decimal VersesInSongs { get; set; }
    }
}

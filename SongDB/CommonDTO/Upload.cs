using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CommonDTO
{
    [DataContract]
    public class UploadSongResponse
    {
        [DataMember]
        public Guid SongId { get; set; }
    }
}

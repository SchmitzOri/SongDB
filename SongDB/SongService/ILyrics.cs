using CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SongService
{
    [ServiceContract]
    interface ILyrics
    {
        [OperationContract]
        string Hello(int x);

        [OperationContract]
        GetWordsResponse GetWords(GetWordsRequest request);

    }
}

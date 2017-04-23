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
        GetWordsResponse GetWords(GetWordsRequest request);

        [OperationContract]
        GetStatsResponse GetStats(GetStatsRequest request);

        [OperationContract]
        GroupAddResponse GroupAdd(GroupAddRequest request);

        [OperationContract]
        GroupUpdateResponse GroupUpdate(GroupUpdateRequest request);

        [OperationContract]
        GroupDeleteResponse GroupDelete(GroupDeleteRequest request);

        [OperationContract]
        RelationAddResponse RelationAdd(RelationAddRequest request);

        [OperationContract]
        RelationDeleteResponse RelationDelete(RelationDeleteRequest request);

        [OperationContract]
        PhraseAddResponse PhraseAdd(PhraseAddRequest request);

        [OperationContract]
        PhraseDeleteResponse PhraseDelete(PhraseDeleteRequest request);
    }
}

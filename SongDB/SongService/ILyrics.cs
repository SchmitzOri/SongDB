using CommonDTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace SongService
{
    [ServiceContract]
    interface ILyrics
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "UploadSong")]
        UploadSongResponse UploadSong(Stream file);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetWords")]
        GetWordsResponse GetWords(GetWordsRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "Locations")]
        LocationsResponse Locations(LocationsRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "WordSongs")]
        WordSongsResponse WordSongs(WordSongsRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "SongLyrics")]
        SongLyricsResponse SongLyrics(SongLyricsRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "Songs")]
        SongsResponse Songs(SongsRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetStats")]
        GetStatsResponse GetStats(GetStatsRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GroupAdd")]
        GroupAddResponse GroupAdd(GroupAddRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GroupUpdate")]
        GroupUpdateResponse GroupUpdate(GroupUpdateRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GroupDelete")]
        GroupDeleteResponse GroupDelete(GroupDeleteRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "RelationAdd")]
        RelationAddResponse RelationAdd(RelationAddRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "RelationDelete")]
        RelationDeleteResponse RelationDelete(RelationDeleteRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "PhraseAdd")]
        PhraseAddResponse PhraseAdd(PhraseAddRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "PhraseDelete")]
        PhraseDeleteResponse PhraseDelete(PhraseDeleteRequest request);

        [OperationContract]
        [WebGet(UriTemplate = "ExportXML")]
        Stream ExportXML();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/ImportXML")]
        bool ImportXML(Stream stream);
    }
}

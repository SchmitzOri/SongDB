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
        [WebInvoke(Method = "POST", UriTemplate = "UploadMultipleSongs")]
        List<UploadSongResponse> UploadMultipleSongs(Stream stream);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GetWords")]
        GetWordsResponse GetWords(GetWordsRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "Locations")]
        LocationsResponse Locations(LocationsRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "WordByLocation")]
        LocationsResponse WordByLocation(WordLocationRequest request);

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

        #region Groups
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
        [WebInvoke(Method = "POST", UriTemplate = "GroupWords")]
        GroupWordsResponse GroupWords(GroupWordsRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "GroupAll")]
        GroupAllResponse GroupAll(GroupAllRequest request);
        #endregion

        #region Relations
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "RelationAdd")]
        RelationAddResponse RelationAdd(RelationAddRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "RelationDelete")]
        RelationDeleteResponse RelationDelete(RelationDeleteRequest request);
        #endregion

        #region Phrases
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "PhraseAdd")]
        PhraseAddResponse PhraseAdd(PhraseAddRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "PhraseDelete")]
        PhraseDeleteResponse PhraseDelete(PhraseDeleteRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "PhraseAll")]
        PhraseAllResponse PhraseAll(PhraseAllRequest request);
        #endregion

        #region ExportImport
        [OperationContract]
        [WebGet(UriTemplate = "ExportXML")]
        Stream ExportXML();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/ImportXML")]
        bool ImportXML(Stream stream); 
        #endregion
    }
}
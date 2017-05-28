using CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LyricsConcord
{
    public partial class Phrases : System.Web.UI.Page
    {
        protected PhraseAllResponse phrases;

        protected void Page_Load(object sender, EventArgs e)
        {
            phrases = ServiceAccessor.MakeRequest<PhraseAllRequest, PhraseAllResponse>(new PhraseAllRequest(), "PhraseAll");
        }

        [WebMethod]
        public static object AddPhrase(string phrase)
        {
            return ServiceAccessor.MakeRequest<PhraseAddRequest, PhraseAddResponse>(new PhraseAddRequest() { Words = phrase.Split(' ').ToList() }, "PhraseAdd");
        }

        [WebMethod]
        public static object DeletePhrase(Guid id)
        {
            return ServiceAccessor.MakeRequest<PhraseDeleteRequest, PhraseDeleteResponse>(new PhraseDeleteRequest() { Id = id }, "PhraseDelete");
        }

        [WebMethod]
        public static object GetPhraseLocs(string phrase)
        {
            return ServiceAccessor.MakeRequest<PhraseLocationRequest, PhraseLocationResponse>(new PhraseLocationRequest() { Phrase = phrase }, "PhraseLocations").Locations;
        }

        [WebMethod]
        public static object GetSongLyrics(Guid songId)
        {
            return ServiceAccessor.MakeRequest<SongLyricsRequest, SongLyricsResponse>(new SongLyricsRequest() { SongId = songId }, "SongLyrics").SongLyrics;
        }
    }
}
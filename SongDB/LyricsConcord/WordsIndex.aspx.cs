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
    public partial class WordsIndex : System.Web.UI.Page
    {
        protected SongsResponse songs;

        protected void Page_Load(object sender, EventArgs e)
        {
            songs = ServiceAccessor.MakeRequest<SongsRequest, SongsResponse>(new SongsRequest(), "Songs");
        }

        [WebMethod]
        public static object GetLocations(Guid? songId)
        {
            return ServiceAccessor.MakeRequest<LocationsRequest, 
                LocationsResponse>(new LocationsRequest() { SongId = songId }, "Locations").Locations;
        }

        [WebMethod]
        public static object GetWordByLocation(Guid songId, int numInSong, int verseNum, int lineInVerse)
        {
            return ServiceAccessor.MakeRequest<WordLocationRequest,LocationsResponse>(
                new WordLocationRequest()
                {
                    SongId = songId,
                    NumInSong = numInSong,
                    VerseNum = verseNum,
                    LineInVerse = lineInVerse
                }, 
                "WordByLocation").Locations;
        }
    }
}
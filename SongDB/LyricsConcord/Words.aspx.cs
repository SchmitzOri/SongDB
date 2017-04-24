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
    public partial class Words : System.Web.UI.Page
    {
        protected SongsResponse songs;

        protected void Page_Load(object sender, EventArgs e)
        {
            songs = ServiceAccessor.MakeRequest<SongsRequest, SongsResponse>(new SongsRequest(), "Songs");
        }

        [WebMethod]
        public static object GetSongWords(Guid? songId)
        {
            return ServiceAccessor.MakeRequest<GetWordsRequest, GetWordsResponse>(new GetWordsRequest() { SongId = songId }, "GetWords").Words;
        }
    }
}
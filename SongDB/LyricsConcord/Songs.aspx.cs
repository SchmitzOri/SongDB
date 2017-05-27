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
    public partial class Songs : System.Web.UI.Page
    {
        protected System.Web.UI.HtmlControls.HtmlInputFile File1;
        protected System.Web.UI.HtmlControls.HtmlInputButton Submit1;
        protected SongsResponse songs;

        protected void Page_Load(object sender, EventArgs e)
        {
            songs = ServiceAccessor.MakeRequest<SongsRequest, SongsResponse>(new SongsRequest() { PartSongName="", PartArtistName=""}, "Songs");
        }

        // TODO: Ajax doesn't work. Check Why
        [WebMethod]
        public static object UploadFile()
        {
            //HttpContext.Current.Request.Files;
            byte[] songData = null;
            HttpPostedFile songFile = HttpContext.Current.Request.Files[0];

            using (var binaryReader = new System.IO.BinaryReader(songFile.InputStream))
            {
                songData = binaryReader.ReadBytes(songFile.ContentLength);
            }

            return ServiceAccessor.UploadSong(songData);
        }

        protected void Submit1_ServerClick(object sender, System.EventArgs e)
        {
            if ((File1.PostedFile != null) && (File1.PostedFile.ContentLength > 0))
            {
                try
                {
                    byte[] songData = null;
                    using (var binaryReader = new System.IO.BinaryReader(File1.PostedFile.InputStream))
                    {
                        songData = binaryReader.ReadBytes(File1.PostedFile.ContentLength);
                    }

                    if (System.IO.Path.GetExtension(File1.PostedFile.FileName) == "txt")
                    {
                        ServiceAccessor.UploadSong(songData);
                    }
                    else
                    {

                    }
                    
                    successMsg.Attributes.Remove("hidden");
                    //Response.Write(".");
                }
                catch (Exception ex)
                {
                    Response.Write("Error: " + ex.Message);
                }
            }
            else
            {
                Response.Write("Please select a file to upload.");
            }

        }

        [WebMethod]
        public static object GetSongs(string partSongName, string partAritstName)
        {
            return ServiceAccessor.MakeRequest<SongsRequest, SongsResponse>(new SongsRequest() { PartSongName=partSongName, PartArtistName=partAritstName }, "Songs").Songs;
        }
    }
}
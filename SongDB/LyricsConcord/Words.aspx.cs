﻿using CommonDTO;
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
        protected GroupAllResponse groups;

        protected void Page_Load(object sender, EventArgs e)
        {
            songs = ServiceAccessor.MakeRequest<SongsRequest, SongsResponse>(new SongsRequest(), "Songs");
            groups = ServiceAccessor.MakeRequest<GroupAllRequest, GroupAllResponse>(new GroupAllRequest(), "GroupAll");
        }

        [WebMethod]
        public static object GetSongWords(Guid? songId)
        {
            return ServiceAccessor.MakeRequest<GetWordsRequest, GetWordsResponse>(new GetWordsRequest() { SongId = songId }, "GetWords").Words;
        }

        [WebMethod]
        public static object GetWordSongs(Guid wordId)
        {
            return ServiceAccessor.MakeRequest<WordSongsRequest, WordSongsResponse>(new WordSongsRequest() { WordId = wordId }, "WordSongs").WordSongs;
        }

        [WebMethod]
        public static object GetSongLyrics(Guid songId)
        {
            return ServiceAccessor.MakeRequest<SongLyricsRequest, SongLyricsResponse>(new SongLyricsRequest() { SongId = songId}, "SongLyrics").SongLyrics;
        }

        [WebMethod]
        public static object GetGroupWords(Guid groupId)
        {
            return ServiceAccessor.MakeRequest<GroupWordsRequest, GroupWordsResponse>(new GroupWordsRequest() { Id = groupId }, "GroupWords").Words;
        }
    }
}
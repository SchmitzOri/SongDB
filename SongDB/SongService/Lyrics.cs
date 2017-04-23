using CommonDTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SongService
{

    public class Lyrics : ILyrics
    {
        public string Hello(int x)
        {
            return x++.ToString();
        }

        public GetWordsResponse GetWords(GetWordsRequest request)
        {
            try
            {
                return DB.GetWords(request.SongId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public GetStatsResponse GetStats(GetStatsRequest request)
        {
            try
            {
                return new GetStatsResponse();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Guid UploadSong(Stream file)
        {
            string Artist = null;
            string Name = null;
            string Lyrics = null;

            using (StreamReader sr = new StreamReader(file))
            {
                Artist = sr.ReadLine();
                Name = sr.ReadLine();
                Lyrics = sr.ReadToEnd().Trim();
            }

            return DB.AddSongLyrics(Artist, Name, Lyrics);
        }

        public Guid UploadMultipleSongs(Stream file)
        {
            //string Artist = null;
            //string Name = null;
            //string Lyrics = null;

            //using (StreamReader sr = new StreamReader(file))
            //{
            //    Artist = sr.ReadLine();
            //    Name = sr.ReadLine();
            //    Lyrics = sr.ReadToEnd().Trim();
            //}

            //return DB.AddSongLyrics(Artist, Name, Lyrics);
            //TODO: UploadMultipleSongs
            return Guid.Empty;
        }
    }
}


using CommonDTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace SongService
{

    public class Lyrics : ILyrics
    {

        public UploadSongResponse UploadSong(Stream file)
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

            return new UploadSongResponse() { SongId = DB.AddSongLyrics(Artist, Name, Lyrics) };
        }

        public bool UploadMultipleSongs(Stream file)
        {
            var gzip = new GZipStream(file, CompressionMode.Decompress);
            //new ZipArchive
            //TODO: UploadMultipleSongs
            return true;
        }

        public SongsResponse Songs(SongsRequest request)
        {
            try
            {
                return DB.Songs(request.PartSongName, request.PartArtistName);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public GetWordsResponse GetWords(GetWordsRequest request)
        {
            try
            {
                return DB.GetWords(request.SongId);
            }
            catch (Exception ex)
            {
                var x = new GetWordsResponse() { Words = new List<Tuple<Guid, string>>() };
                x.Words.Add(new Tuple<Guid, string>(Guid.Empty, ex.ToString()));
                return x;
            }
        }

        public WordSongsResponse WordSongs(WordSongsRequest request)
        {
            try
            {
                return DB.GetWordSongs(request.WordId);
            }
            catch (Exception ex)
            {
                var x = new WordSongsResponse() { WordSongs = new List<SongDTO>() };
                x.WordSongs.Add(new SongDTO() { Id = Guid.Empty, Name = ex.ToString(), ArtistId = Guid.Empty });
                return x;
            }
        }

        public SongLyricsResponse SongLyrics(SongLyricsRequest request)
        {
            try
            {
                return DB.SongLyrics(request.SongId);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public GetStatsResponse GetStats(GetStatsRequest request)
        {
            try
            {
                return DB.GetStats();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public GroupAddResponse GroupAdd(GroupAddRequest request)
        {
            try
            {
                return new GroupAddResponse()
                {
                    Id = DB.GroupAdd(request.Name),
                };
            }
            catch (Exception)
            {
                return new GroupAddResponse()
                {
                    Id = Guid.Empty,
                };
            }
        }

        public GroupUpdateResponse GroupUpdate(GroupUpdateRequest request)
        {
            try
            {
                return new GroupUpdateResponse()
                {
                    Success = DB.GroupUpdate(request.Id, request.Name, request.Words),
                };
            }
            catch (Exception)
            {
                return new GroupUpdateResponse()
                {
                    Success = false,
                };
            }
        }

        public GroupDeleteResponse GroupDelete(GroupDeleteRequest request)
        {
            try
            {
                return new GroupDeleteResponse()
                {
                    Success = DB.RelationDelete(request.Id),
                };
            }
            catch (Exception)
            {
                return new GroupDeleteResponse()
                {
                    Success = false,
                };
            }
        }

        public RelationAddResponse RelationAdd(RelationAddRequest request)
        {
            try
            {
                return new RelationAddResponse()
                {
                    Id = DB.RelationAdd(request.Name, request.RelationType, request.Word1, request.Word2),
                };
            }
            catch (Exception)
            {
                return new RelationAddResponse()
                {
                    Id = Guid.Empty,
                };
            }
        }

        public RelationDeleteResponse RelationDelete(RelationDeleteRequest request)
        {
            try
            {
                return new RelationDeleteResponse()
                {
                    Success = DB.RelationDelete(request.Id),
                };
            }
            catch (Exception)
            {
                return new RelationDeleteResponse()
                {
                    Success = false,
                };
            }
        }

        public PhraseAddResponse PhraseAdd(PhraseAddRequest request)
        {
            try
            {
                return new PhraseAddResponse()
                {
                    Id = DB.PhraseAdd(request.Words),
                };
            }
            catch (Exception)
            {
                return new PhraseAddResponse()
                {
                    Id = Guid.Empty,
                };
            }
        }

        public PhraseDeleteResponse PhraseDelete(PhraseDeleteRequest request)
        {
            try
            {
                return new PhraseDeleteResponse()
                {
                    Success = DB.PhraseDelete(request.Id),
                };
            }
            catch (Exception)
            {
                return new PhraseDeleteResponse()
                {
                    Success = false,
                };
            }
        }

        public Stream ExportXML()
        {
            // Exporting all tables to xml files
            DB.ExportTables(ConfigurationManager.AppSettings["EXPORT_FOLDER"]);

            // Zipping all files
            string zipFilePath = ConfigurationManager.AppSettings["ZIP_FOLDER"] + "DB.zip";
            if (File.Exists(zipFilePath))
            {
                File.Delete(zipFilePath);
            }

            ZipFile.CreateFromDirectory(ConfigurationManager.AppSettings["EXPORT_FOLDER"], zipFilePath, CompressionLevel.Fastest, false);

            // Returning zip stream
            String headerInfo = "attachment; filename=DB.zip";
            WebOperationContext.Current.OutgoingResponse.Headers["Content-Disposition"] = headerInfo;
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/octet-stream";

            return File.OpenRead(zipFilePath);
        }

        public bool ImportXML(Stream stream)
        {
            try
            {
                // Save stream to file
                string zipFilePath = ConfigurationManager.AppSettings["ZIP_FOLDER"] + "Import.zip";
                if (File.Exists(zipFilePath))
                {
                    File.Delete(zipFilePath);
                }

                using (FileStream writer = new FileStream(zipFilePath, FileMode.Create))
                {
                    stream.CopyTo(writer);
                }

                // Clean export folder
                System.IO.DirectoryInfo di = new DirectoryInfo(ConfigurationManager.AppSettings["EXPORT_FOLDER"]);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }

                // Unzip to folder
                ZipFile.ExtractToDirectory(zipFilePath, ConfigurationManager.AppSettings["EXPORT_FOLDER"]);

                DB.ImportTables(ConfigurationManager.AppSettings["EXPORT_FOLDER"]);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}


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

        public List<UploadSongResponse> UploadMultipleSongs(Stream stream)
        {
            try
            {
                // Save stream to file
                string zipFilePath = ConfigurationManager.AppSettings["ZIP_FOLDER"] + "Songs.zip";
                if (File.Exists(zipFilePath))
                {
                    File.Delete(zipFilePath);
                }

                using (FileStream writer = new FileStream(zipFilePath, FileMode.Create))
                {
                    stream.CopyTo(writer);
                }

                // Clean export folder
                foreach (FileInfo file in new DirectoryInfo(ConfigurationManager.AppSettings["EXPORT_FOLDER"]).GetFiles())
                {
                    file.Delete();
                }

                // Unzip to folder
                ZipFile.ExtractToDirectory(zipFilePath, ConfigurationManager.AppSettings["EXPORT_FOLDER"]);

                // Import all songs in folder
                List<UploadSongResponse> ret = new List<UploadSongResponse>();

                foreach (FileInfo file in new DirectoryInfo(ConfigurationManager.AppSettings["EXPORT_FOLDER"]).GetFiles())
                {
                    ret.Add(UploadSong(file.OpenRead()));
                }

                return ret;
            }
            catch (Exception)
            {
                return null;
            }
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

        public LocationsResponse Locations(LocationsRequest request)
        {
            try
            {
                return DB.Locations(request.SongId);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public LocationsResponse WordByLocation(WordLocationRequest request)
        {
            try
            {
                return DB.WordByLocation(request.SongId, request.NumInSong, request.VerseNum, request.LineInVerse);
            }
            catch (Exception ex)
            {
                return null;
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
                    Id = DB.GroupAdd(request.Name, request.Words),
                };
            }
            catch (Exception ex)
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
            catch (Exception ex)
            {
                return new GroupUpdateResponse()
                {
                    Success = false,
                };
            }
        }

        public GroupWordsResponse GroupWords(GroupWordsRequest request)
        {
            try
            {
                return new GroupWordsResponse()
                {
                    Words = DB.GroupGetWords(request.Id),
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public GroupAllResponse GroupAll(GroupAllRequest request)
        {
            try
            {
                return new GroupAllResponse()
                {
                    Groups = DB.GroupGetAll(),
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public GroupDeleteResponse GroupDelete(GroupDeleteRequest request)
        {
            try
            {
                return new GroupDeleteResponse()
                {
                    Success = DB.GroupDelete(request.Id),
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

        public RelationTypesResponse RelationTypes(RelationTypesRequest request)
        {
            try
            {
                return new RelationTypesResponse()
                {
                    Types = DB.RelationTypes(),
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public RelationAddTypeResponse RelationAddType(RelationAddTypeRequest request)
        {
            try
            {
                return new RelationAddTypeResponse()
                {
                    TypeId = DB.RelationAddType(request.Name),
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public RelationAllResponse RelationAll(RelationAllRequest request)
        {
            try
            {
                return new RelationAllResponse()
                {
                    Relations = DB.RelationGetAll(request.TypeId),
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public RelationTypeUseCountResponse RelationTypeUseCount(RelationTypeUseCountRequest request)
        {
            try
            {
                return new RelationTypeUseCountResponse()
                {
                    Count = DB.RelationTypeCount(request.TypeId),
                };
            }
            catch (Exception)
            {
                return new RelationTypeUseCountResponse()
                {
                    Count = 999,
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

        public PhraseAllResponse PhraseAll(PhraseAllRequest request)
        {
            try
            {
                return new PhraseAllResponse()
                {
                    Phrases = DB.PhraseGetAll(),
                };
            }
            catch (Exception ex)
            {
                return null;
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

        public PhraseLocationResponse PhraseLocations(PhraseLocationRequest request)
        {
            try
            {
                return new PhraseLocationResponse()
                {
                    Locations = DB.PhraseLocations(request.Phrase),
                };
            }
            catch (Exception ex)
            {
                return null;
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
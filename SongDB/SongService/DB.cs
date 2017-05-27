using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonDTO;
using System.Data;

namespace SongService
{
    public static class DB
    {
        #region Connection
        private static SqlConnection GetConnection()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CONN_STRING"].ConnectionString);
            conn.Open();
            return conn;
        }
        #endregion

        internal static GetWordsResponse GetWords(Guid? songId)
        {
            using (SqlConnection conn = GetConnection())
            {
                GetWordsResponse ret = new GetWordsResponse()
                {
                    Words = new List<Tuple<Guid, string>>(),
                };
                if (!songId.HasValue)
                {
                    using (SqlCommand comm = new SqlCommand("SELECT word_id, word FROM word", conn))
                    using (SqlDataReader dr = comm.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            ret.Words.Add(new Tuple<Guid, string>(Guid.Parse(dr["word_id"].ToString()), dr["word"].ToString()));
                        }
                    }
                }
                else
                {
                    using (SqlCommand comm = new SqlCommand("SELECT DISTINCT w.word_id, word " +
                                                            "FROM word w " +
                                                            "JOIN location l ON w.word_id = l.word_id " +
                                                            "WHERE song_id = @song_id", conn))
                    {
                        comm.Parameters.AddWithValue("@song_id", songId.Value);

                        using (SqlDataReader dr = comm.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                ret.Words.Add(new Tuple<Guid, string>(Guid.Parse(dr["word_id"].ToString()), dr["word"].ToString()));
                            }
                        }
                    }
                }

                return ret;
            }
        }

        internal static LocationsResponse Locations(Guid? songId)
        {
            using (SqlConnection conn = GetConnection())
            {
                LocationsResponse ret = new LocationsResponse()
                {
                    Locations = new List<LocationDTO>()
                };

                string command = "SELECT location_id, s.song_name, w.word, word_number_in_file, " +
                                    "verse_number, row_in_verse " +
                                 "FROM location l " +
                                 "JOIN song s ON s.song_id = l.song_id " +
                                 "JOIN word w ON w.word_id = l.word_id";

                if (songId.HasValue)
                {
                    command += " WHERE l.song_id = @song_id";
                }

                using (SqlCommand comm = new SqlCommand(command, conn))
                {
                    if (songId.HasValue)
                    {
                        comm.Parameters.AddWithValue("@song_id", songId);
                    }

                    using (SqlDataReader dr = comm.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            ret.Locations.Add(new LocationDTO()
                            {
                                Id = Guid.Parse(dr["location_id"].ToString()),
                                Word = dr["word"].ToString(),
                                Song = dr["song_name"].ToString(),
                                NumberInSong = int.Parse(dr["word_number_in_file"].ToString()),
                                VerseNumber = int.Parse(dr["verse_number"].ToString()),
                                LineInVerse = int.Parse(dr["row_in_verse"].ToString())
                            });
                        }

                        return ret;
                    }
                }
            }
        }

        internal static LocationsResponse WordByLocation(Guid songId, int numInSong,
            int verseNum, int lineInVerse)
        {
            using (SqlConnection conn = GetConnection())
            {
                LocationsResponse ret = new LocationsResponse()
                {
                    Locations = new List<LocationDTO>()
                };

                string command = "SELECT location_id, s.song_name, w.word, word_number_in_file, " +
                                    "verse_number, row_in_verse " +
                                 "FROM location l " +
                                 "JOIN song s ON s.song_id = l.song_id " +
                                 "JOIN word w ON w.word_id = l.word_id " +
                                 "WHERE l.song_id = @song_id AND " +
                                 "(word_number_in_file = @num_in_file OR " +
                                 "verse_number = @verse_num AND row_in_verse = @verse_line)";

                using (SqlCommand comm = new SqlCommand(command, conn))
                {
                    comm.Parameters.AddWithValue("@song_id", songId);
                    comm.Parameters.AddWithValue("@num_in_file", numInSong);
                    comm.Parameters.AddWithValue("@verse_num", verseNum);
                    comm.Parameters.AddWithValue("@verse_line", lineInVerse);

                    using (SqlDataReader dr = comm.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            ret.Locations.Add(new LocationDTO()
                            {
                                Id = Guid.Parse(dr["location_id"].ToString()),
                                Word = dr["word"].ToString(),
                                Song = dr["song_name"].ToString(),
                                NumberInSong = int.Parse(dr["word_number_in_file"].ToString()),
                                VerseNumber = int.Parse(dr["verse_number"].ToString()),
                                LineInVerse = int.Parse(dr["row_in_verse"].ToString())
                            });
                        }

                        return ret;
                    }
                }
            }
        }

        internal static WordSongsResponse GetWordSongs(Guid wordId)
        {
            using (SqlConnection conn = GetConnection())
            {
                WordSongsResponse ret = new WordSongsResponse()
                {
                    WordSongs = new List<SongDTO>()
                };

                using (SqlCommand comm = new SqlCommand("SELECT DISTINCT s.song_id, song_name, artist_id " +
                                                        "FROM song s " +
                                                        "JOIN location l ON s.song_id = l.song_id " +
                                                        "WHERE l.word_id = @word_id", conn))
                {
                    comm.Parameters.AddWithValue("@word_id", wordId);

                    using (SqlDataReader dr = comm.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            ret.WordSongs.Add(new SongDTO()
                            {
                                Id = Guid.Parse(dr["song_id"].ToString()),
                                Name = dr["song_name"].ToString(),
                                ArtistId = Guid.Parse(dr["artist_id"].ToString())
                            });
                        }
                    }
                }

                return ret;
            }
        }

        internal static SongLyricsResponse SongLyrics(Guid songId)
        {
            SongLyricsResponse ret = new SongLyricsResponse();

            ret.SongLyrics = File.ReadAllText(ConfigurationManager.AppSettings["LYRICS_FOLDER"] + songId.ToString() + ".txt");

            return ret;
        }

        internal static SongsResponse Songs(string partSongName, string partArtistName)
        {
            using (SqlConnection conn = GetConnection())
            {
                SongsResponse ret = new SongsResponse()
                {
                    Songs = new List<SongDTO>(),
                };

                using (SqlCommand comm = new SqlCommand("SELECT song_id, song_name, a.artist_id, a.artist_name " +
                                                        "FROM song s " +
                                                        "JOIN artist a ON a.artist_id = s.artist_id " +
                                                        "WHERE s.song_name LIKE @part_name AND " +
                                                        "a.artist_name LIKE @part_artist", conn))
                {
                    comm.Parameters.AddWithValue("@part_name", "%" + partSongName + "%");
                    comm.Parameters.AddWithValue("@part_artist", "%" + partArtistName + "%");

                    using (SqlDataReader dr = comm.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            ret.Songs.Add(new SongDTO()
                            {
                                ArtistId = Guid.Parse(dr["artist_id"].ToString()),
                                Id = Guid.Parse(dr["song_id"].ToString()),
                                Name = dr["song_name"].ToString(),
                                ArtistName = dr["artist_name"].ToString()
                            });
                        }

                        return ret;
                    }
                }
            }
        }

        internal static Guid GroupAdd(string name)
        {
            using (SqlConnection conn = GetConnection())
            using (SqlCommand comm = new SqlCommand("INSERT INTO group (group_id, group_name) VALUES (@group_id, @group_name)", conn))
            {
                Guid groupId = Guid.NewGuid();
                comm.Parameters.AddWithValue("@group_id", groupId);
                comm.Parameters.AddWithValue("@group_name", name);
                comm.ExecuteNonQuery();

                return groupId;
            }
        }

        internal static bool GroupUpdate(Guid id, string name, List<Guid> words)
        {
            using (SqlConnection conn = GetConnection())
            using (SqlTransaction trans = conn.BeginTransaction())
            {
                using (SqlCommand comm = new SqlCommand("DELETE group_words WHERE group_id = @group_id", conn))
                {
                    comm.Parameters.AddWithValue("@group_id", id);
                    comm.ExecuteNonQuery();
                }

                foreach (var wordId in words)
                {
                    using (SqlCommand comm = new SqlCommand("INSERT INTO group_words (group_id, word_id) VALUES (@group_id, @word_id)", conn))
                    {
                        comm.Parameters.AddWithValue("@group_id", id);
                        comm.Parameters.AddWithValue("@word_id", wordId);
                        comm.ExecuteNonQuery();
                    }
                }

                using (SqlCommand comm = new SqlCommand("UPDATE group SET group_name = @group_name WHERE group_id = @group_id", conn))
                {
                    comm.Parameters.AddWithValue("@group_id", id);
                    comm.Parameters.AddWithValue("@group_name", name);
                    comm.ExecuteNonQuery();
                }

                trans.Commit();
                return true;
            }
        }

        internal static bool GroupDelete(Guid id)
        {
            using (SqlConnection conn = GetConnection())
            using (SqlTransaction trans = conn.BeginTransaction())
            {
                using (SqlCommand comm = new SqlCommand("DELETE group_words WHERE group_id = @group_id", conn))
                {
                    comm.Parameters.AddWithValue("@group_id", id);
                    comm.ExecuteNonQuery();
                }

                using (SqlCommand comm = new SqlCommand("DELETE group WHERE group_id = @group_id", conn))
                {
                    comm.Parameters.AddWithValue("@group_id", id);
                    comm.ExecuteNonQuery();
                }

                trans.Commit();
                return true;
            }
        }

        internal static Guid PhraseAdd(List<Guid> words)
        {
            using (SqlConnection conn = GetConnection())
            using (SqlTransaction trans = conn.BeginTransaction())
            {
                Guid phraseId = Guid.NewGuid();

                using (SqlCommand comm = new SqlCommand("INSERT INTO phrase (phrase_id) VALUES (@phrase_id)", conn))
                {
                    comm.Parameters.AddWithValue("@phrase_id", phraseId);
                    comm.ExecuteNonQuery();
                }

                for (int i = 1; i <= words.Count; i++)
                {
                    using (SqlCommand comm = new SqlCommand("INSERT INTO phrase_words (phrase_id, word_id, order_index) VALUES (@phrase_id, @word_id, @order_index)", conn))
                    {
                        comm.Parameters.AddWithValue("@phrase_id", phraseId);
                        comm.Parameters.AddWithValue("@word_id", words[i]);
                        comm.Parameters.AddWithValue("@order_index", i);

                        comm.ExecuteNonQuery();
                    }
                }

                trans.Commit();
                return phraseId;
            }

        }

        internal static bool PhraseDelete(Guid id)
        {
            using (SqlConnection conn = GetConnection())
            using (SqlTransaction trans = conn.BeginTransaction())
            {
                using (SqlCommand comm = new SqlCommand("DELETE phrase_words WHERE phrase_id = @phrase_id", conn))
                {
                    comm.Parameters.AddWithValue("@phrase_id", id);
                    comm.ExecuteNonQuery();
                }

                using (SqlCommand comm = new SqlCommand("DELETE phrase WHERE phrase_id = @phrase_id", conn))
                {
                    comm.Parameters.AddWithValue("@phrase_id", id);
                    comm.ExecuteNonQuery();
                }

                trans.Commit();
                return true;
            }
        }

        internal static Guid RelationAdd(string name, Guid relationType, Guid word1, Guid word2)
        {
            using (SqlConnection conn = GetConnection())
            using (SqlCommand comm = new SqlCommand("INSERT INTO relation (relation_id, relation_type_id, relation_name, word1, word2) VALUES (@relation_id, @relation_type_id, @relation_name, @word1, @word2)", conn))
            {
                Guid relationId = Guid.NewGuid();

                comm.Parameters.AddWithValue("@relation_id", relationId);
                comm.Parameters.AddWithValue("@relation_type_id", relationType);
                comm.Parameters.AddWithValue("@relation_name", name);
                comm.Parameters.AddWithValue("@word1", word1);
                comm.Parameters.AddWithValue("@word2", word2);
                comm.ExecuteNonQuery();

                return relationId;
            }
        }

        internal static bool RelationDelete(Guid id)
        {
            using (SqlConnection conn = GetConnection())
            using (SqlCommand comm = new SqlCommand("DELETE relation WHERE relation_id = @relation_id", conn))
            {
                comm.Parameters.AddWithValue("@relation_id", id);
                comm.ExecuteNonQuery();

                return true;
            }
        }

        internal static GetStatsResponse GetStats()
        {
            using (SqlConnection conn = GetConnection())
            using (SqlCommand comm = new SqlCommand("SELECT CASE num_of_rows WHEN 0 THEN 0 ELSE (num_of_characters / num_of_words) END chars_per_word, " +
                                                    "CASE num_of_rows WHEN 0 THEN 0 ELSE(num_of_words / num_of_rows) END words_in_row, " +
                                                    "CASE num_of_verses WHEN 0 THEN 0 ELSE(num_of_rows / num_of_verses) END rows_in_verse, " +
                                                    "CASE num_of_songs WHEN 0 THEN 0 ELSE(num_of_verses / num_of_songs) END verses_in_songs " +
                                                    "FROM stats", conn))
            using (SqlDataReader dr = comm.ExecuteReader())
            {
                if (dr.Read())
                {
                    return new GetStatsResponse()
                    {
                        CharsPerWord = Convert.ToDecimal(dr["chars_per_word"]),
                        RowsInVerse = Convert.ToDecimal(dr["rows_in_verse"]),
                        VersesInSongs = Convert.ToDecimal(dr["verses_in_songs"]),
                        WordsInRow = Convert.ToDecimal(dr["words_in_row"]),
                    };
                }
                return null;
            }
        }

        internal static Guid AddSongLyrics(string Artist, string Song, string Lyrics)
        {
            Guid songId = Guid.NewGuid();
            using (SqlConnection conn = GetConnection())
            using (SqlTransaction trans = conn.BeginTransaction())
            {
                Guid artistId;
                #region Artist
                object artistIdObj = null;
                using (SqlCommand comm = new SqlCommand("SELECT TOP 1 artist_id FROM artist WHERE artist_name = @artist", conn, trans))
                {
                    comm.Parameters.AddWithValue("@artist", Artist);

                    artistIdObj = comm.ExecuteScalar();
                }

                if (artistIdObj == null)
                {
                    using (SqlCommand comm = new SqlCommand("INSERT INTO artist (artist_id, artist_name) VALUES (@artist_id, @artist_name) ", conn, trans))
                    {
                        artistId = Guid.NewGuid();
                        comm.Parameters.AddWithValue("@artist_name", Artist);
                        comm.Parameters.AddWithValue("@artist_id", artistId);

                        comm.ExecuteNonQuery();
                    }
                }
                else
                {
                    artistId = Guid.Parse(artistIdObj.ToString());
                }
                #endregion

                #region Song

                using (SqlCommand comm = new SqlCommand("INSERT INTO song (song_id, song_name, artist_id) VALUES (@song_id, @song_name, @artist_id) ", conn, trans))
                {
                    comm.Parameters.AddWithValue("@song_id", songId);
                    comm.Parameters.AddWithValue("@song_name", Song);
                    comm.Parameters.AddWithValue("@artist_id", artistId);
                    comm.ExecuteNonQuery();
                }

                #endregion

                #region Lyrics

                // Remove special characters
                StringBuilder sb = new StringBuilder();
                foreach (char c in Lyrics)
                {
                    if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == ' ' || c == '\n')
                    {
                        sb.Append(c);
                    }
                }

                // Split words
                var rows = sb.ToString().Split('\n');
                int verseNum = 1;
                int rowNumInVerse = 1;
                int rowCount = 1;
                int wordNum = 1;
                int numberOfCharacters = 0;
                foreach (string row in rows)
                {
                    // New verse
                    if (string.IsNullOrWhiteSpace(row))
                    {
                        verseNum++;
                        rowNumInVerse = 1;
                    }
                    else
                    {
                        foreach (var word in row.Split(' '))
                        {
                            // Insert word
                            object wordIdObj = null;
                            Guid wordId;
                            using (SqlCommand comm = new SqlCommand("SELECT TOP 1 word_id FROM word WHERE word = @word", conn, trans))
                            {
                                comm.Parameters.AddWithValue("@word", word);

                                wordIdObj = comm.ExecuteScalar();
                            }

                            if (wordIdObj == null)
                            {
                                using (SqlCommand comm = new SqlCommand("INSERT INTO word (word_id, word) VALUES (@word_id, @word) ", conn, trans))
                                {
                                    wordId = Guid.NewGuid();
                                    comm.Parameters.AddWithValue("@word", word);
                                    comm.Parameters.AddWithValue("@word_id", wordId);

                                    comm.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                wordId = Guid.Parse(wordIdObj.ToString());
                            }

                            using (SqlCommand comm = new SqlCommand("INSERT INTO location (location_id, song_id, word_id, word_number_in_file, verse_number, row_in_verse) " +
                                                                                  "VALUES (NEWID(), @song_id, @word_id, @word_number_in_file, @verse_number, @row_in_verse) ", conn, trans))
                            {
                                comm.Parameters.AddWithValue("@song_id", songId);
                                comm.Parameters.AddWithValue("@word_id", wordId);
                                comm.Parameters.AddWithValue("@word_number_in_file", wordNum);
                                comm.Parameters.AddWithValue("@verse_number", verseNum);
                                comm.Parameters.AddWithValue("@row_in_verse", rowNumInVerse);

                                comm.ExecuteNonQuery();
                            }

                            numberOfCharacters += word.Length;
                            wordNum++;
                        }
                        rowNumInVerse++;
                        rowCount++;
                    }
                }

                #endregion

                #region Statistics
                int count = 0;
                using (SqlCommand comm = new SqlCommand("SELECT COUNT(*) FROM stats", conn, trans))
                {
                    count = Convert.ToInt32(comm.ExecuteScalar());
                }

                if (count == 0)
                {
                    using (SqlCommand comm = new SqlCommand("INSERT INTO stats ([num_of_characters], [num_of_words], [num_of_rows], [num_of_verses], [num_of_songs]) VALUES (0, 0, 0, 0, 0)", conn, trans))
                    {
                        comm.ExecuteNonQuery();
                    }
                }

                using (SqlCommand comm = new SqlCommand("UPDATE stats SET num_of_characters = num_of_characters + @num_of_characters, num_of_words = num_of_words + @num_of_words, num_of_rows = num_of_rows + @num_of_rows, num_of_verses = num_of_verses + @num_of_verses, num_of_songs = num_of_songs + 1", conn, trans))
                {
                    comm.Parameters.AddWithValue("@num_of_characters", numberOfCharacters);
                    comm.Parameters.AddWithValue("@num_of_words", wordNum);
                    comm.Parameters.AddWithValue("@num_of_rows", rowCount);
                    comm.Parameters.AddWithValue("@num_of_verses", verseNum);

                    comm.ExecuteNonQuery();
                }

                #endregion

                #region Save File In Server

                File.WriteAllText(ConfigurationManager.AppSettings["LYRICS_FOLDER"] + songId.ToString() + ".txt", Lyrics);

                #endregion

                trans.Commit();

                return songId;
            }
        }

        internal static bool ExportTables(string folder)
        {
            using (SqlConnection conn = GetConnection())
            {
                List<string> tables = new List<string>();
                using (SqlCommand comm = new SqlCommand("SELECT name FROM sys.Tables", conn))
                using (SqlDataReader dr = comm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        tables.Add(dr["name"].ToString());
                    }
                }

                foreach (string table in tables)
                {
                    using (SqlCommand comm = new SqlCommand("SELECT * FROM [" + table + "]", conn))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(comm);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        ds.Tables[0].WriteXml(folder + table + ".xml");
                        ds.Tables[0].WriteXmlSchema(folder + table + ".xsd");
                    }
                }

                return true;
            }
        }

        internal static bool ImportTables(string folder)
        {
            using (SqlConnection conn = GetConnection())
            {
                // Get tables in order of dependency
                List<string> tables = new List<string>();
                using (SqlCommand comm = new SqlCommand("with cte (lvl,object_id,name) " +
                                                                "as " +
                                                                "(" +
                                                                    "select      1 " +
                                                                               ", object_id " +
                                                                               ", name " +
                                                                    "from sys.tables " +
                                                                    "where type_desc = 'USER_TABLE' " +
                                                                            "and is_ms_shipped = 0 " +
                                                                    "union all " +
                                                                    "select      cte.lvl + 1 " +
                                                                               ", t.object_id " +
                                                                               ", t.name " +
                                                                    "from cte " +
                                                                    "join sys.tables  as t " +
                                                                                "on exists " +
                                                                                            "(" +
                                                                                                "select null " +
                                                                                                "from sys.foreign_keys    as fk " +
                                                                                                "where fk.parent_object_id = t.object_id " +
                                                                                                        "and fk.referenced_object_id = cte.object_id " +
                                                                                            ") " +
                                                                                        "and t.object_id<> cte.object_id " +
                                                                                       "and cte.lvl < 30 " +
                                                                    "where t.type_desc = 'USER_TABLE' " +
                                                                            "and t.is_ms_shipped = 0 " +
                                                                ") " +
                                                    "select name " +
                                                               ", max (lvl)   as dependency_level " +
                                                    "from cte " +
                                                    "group by name " +
                                                    "order by    dependency_level, name", conn))
                using (SqlDataReader dr = comm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        tables.Add(dr["name"].ToString());
                    }
                }

                // Reverse table depdendency order
                tables.Reverse();

                // Delete old data
                foreach (string table in tables)
                {
                    using (SqlCommand comm = new SqlCommand("DELETE [" + table + "]", conn))
                    {
                        comm.ExecuteNonQuery();
                    }
                }

                // Reverse table depdendency order
                tables.Reverse();

                // Import data
                foreach (string table in tables)
                {
                    DataSet reportData = new DataSet();
                    reportData.ReadXmlSchema(folder + table + ".xsd");
                    reportData.ReadXml(folder + table + ".xml");

                    using (SqlBulkCopy sbc = new SqlBulkCopy(conn))
                    {
                        sbc.DestinationTableName = "[" + table + "]";
                        sbc.WriteToServer(reportData.Tables[0]);
                    }
                }

                return true;
            }
        }
    }
}


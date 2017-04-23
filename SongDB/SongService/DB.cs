using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonDTO;

namespace SongService
{
    public static class DB
    {
        private static SqlConnection GetConnection()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["CONN_STRING"].ConnectionString);
            conn.Open();
            return conn;
        }

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
                    using (SqlCommand comm = new SqlCommand("SELECT word_id, word " +
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
    }
}


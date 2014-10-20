using System;
using System.Configuration;
using System.Linq;
using EchoNest.Playlist;
using EchoNest.Song;
using NUnit.Framework;

namespace EchoNest.Tests
{
    using System.Diagnostics.CodeAnalysis;

    [TestFixture]
    [SuppressMessage("StyleCopPlus.StyleCopPlusRules", "SP0100:AdvancedNamingRules", Justification = "OK.")]
    public class PlaylistTests
    {
        [TestCase("Jimi Hendrix")]
        [TestCase("Amy Winehouse")]
        [TestCase("Miles Davis")]
        [TestCase("Alison Krauss")]
        [TestCase("Led Zeppelin")]
        [Test]
        public void GetBasicPlaylist_WhereArtistName_HasSongsByArtist(string artistName)
        {
            BasicArgument basicArgument = new BasicArgument
            {
                Results = 10,
                Dmca = true
            };

            TermList artistTerms = new TermList { artistName };
            basicArgument.Artist.AddRange(artistTerms);

            using (EchoNestSession session = new EchoNestSession(ConfigurationManager.AppSettings.Get("echoNestApiKey")))
            {
                PlaylistResponse searchResponse = session.Query<Basic>().Execute(basicArgument);

                Assert.IsNotNull(searchResponse);

                Console.WriteLine("Songs for : {0}", artistName);
                foreach (SongBucketItem song in searchResponse.Songs)
                {
                    Console.WriteLine("\t{0} ({1})", song.Title, song.ArtistName);
                }

                Console.WriteLine();
            }
        }

        [TestCase("Apocalypse Now Phyc Rock", "60s,guitar,psychadelic,rock,sountrack^0.5", "eeirie^0.5,dark^0.5,disturbing^0.5,groovy^0.5,melancholia^0.5,ominous^0.5")]
        [TestCase("Apocalypse Now", "60s,psychadelic,rock^0.5,sountrack^0.5", "eeirie,dark,disturbing,groovy,melancholia,ominous^0.5")]
        [Test]
        public void GetStaticPlaylist_WhereMoodAndStyle_HasVarietyOfArtists(string title, string styles, string moods)
        {
            TermList styleTerms = new TermList();
            foreach (string s in styles.Split(','))
            {
                styleTerms.Add(s);
            }

            TermList moodTerms = new TermList();
            foreach (string s in moods.Split(','))
            {
                moodTerms.Add(s);
            }

            StaticArgument staticArgument = new StaticArgument
            {
                Results = 25,
                Adventurousness = 0.4,
                Type = "artist-description",
                Variety = 0.4 /* variety of artists */
            };

            staticArgument.Styles.AddRange(styleTerms);

            staticArgument.Moods.AddRange(moodTerms);

            using (EchoNestSession session = new EchoNestSession(ConfigurationManager.AppSettings.Get("echoNestApiKey")))
            {
                PlaylistResponse searchResponse = session.Query<Static>().Execute(staticArgument);

                Assert.IsNotNull(searchResponse);
                Assert.IsNotNull(searchResponse.Songs);
                Assert.IsTrue(searchResponse.Songs.Any());

                Console.WriteLine("Songs for : {0}", title);
                foreach (SongBucketItem song in searchResponse.Songs)
                {
                    Console.WriteLine("\t{0} ({1})", song.Title, song.ArtistName);
                }

                Console.WriteLine();
            }
        }

#if false
        [TestCase("Apocalypse Now Phyc Rock", "60s,guitar,psychadelic,rock,sountrack^0.5", "eeirie^0.5,dark^0.5,disturbing^0.5,groovy^0.5,melancholia^0.5,ominous^0.5")]
        [TestCase("Apocalypse Now", "60s,psychadelic,rock^0.5,sountrack^0.5", "eeirie,dark,disturbing,groovy,melancholia,ominous^0.5")]
        [Test]
        public void GetDynamicPlaylist_WhereMoodAndStyle_CanSteerDynamicPlaylistByMood(string title, string styles, string moods)
        {
            TermList styleTerms = new TermList();
            foreach (string s in styles.Split(','))
            {
                styleTerms.Add(s);
            }

            TermList moodTerms = new TermList();
            foreach (string s in moods.Split(','))
            {
                moodTerms.Add(s);
            }

            DynamicArgument dynamicArgument = new DynamicArgument
            {
                Adventurousness = 0.4,
                Type = "artist-description",
                Variety = 0.4 /* variety of artists */
            };

            dynamicArgument.Styles.AddRange(styleTerms);

            dynamicArgument.Moods.AddRange(moodTerms);

            using (EchoNestSession session = new EchoNestSession(ConfigurationManager.AppSettings.Get("echoNestApiKey")))
            {
                DynamicPlaylistResponse searchResponse = session.Query<Dynamic>().Execute(dynamicArgument);

                Assert.IsNotNull(searchResponse);
                Assert.IsNotNull(searchResponse.Songs);
                Assert.IsTrue(searchResponse.Songs.Any());

                string sessionId = searchResponse.SessionId;

                Console.WriteLine("Dynamic Playlist Session Id: {0}", sessionId);
                Console.WriteLine();
                Console.WriteLine("Songs for : {0}", title);
                foreach (SongBucketItem song in searchResponse.Songs)
                {
                    Console.WriteLine("\t{0} ({1})", song.Title, song.ArtistName);
                }

                Console.WriteLine();
                Console.WriteLine("Steering Playlist by mood = -happy");
                Console.WriteLine();

                dynamicArgument = new DynamicArgument { SteerMood = "-happy", SessionId = sessionId };
                searchResponse = session.Query<Dynamic>().Execute(dynamicArgument);
                Console.WriteLine("Next song in dynamic playlist for : {0}", title);
                foreach (SongBucketItem song in searchResponse.Songs)
                {
                    Console.WriteLine("\t{0} ({1})", song.Title, song.ArtistName);
                }

                Console.WriteLine();
                Console.WriteLine("Steering Playlist by tempo = +10% (tempo^1.1)");
                Console.WriteLine();
                dynamicArgument = new DynamicArgument { Steer = "tempo^1.1", SessionId = sessionId };
                searchResponse = session.Query<Dynamic>().Execute(dynamicArgument);
                Console.WriteLine("Next song in dynamic playlist for : {0}", title);
                foreach (SongBucketItem song in searchResponse.Songs)
                {
                    Console.WriteLine("\t{0} ({1})", song.Title, song.ArtistName);
                }

                Console.WriteLine();
            }
        }
#endif
    }
}

using System;
using System.Configuration;
using System.Linq;

using EchoNest.Song;
using NUnit.Framework;
using Search = EchoNest.Song.Search;
using SearchArgument = EchoNest.Song.SearchArgument;
using SearchResponse = EchoNest.Song.SearchResponse;

namespace EchoNest.Tests
{
    using System.Diagnostics.CodeAnalysis;

    [TestFixture]
    [SuppressMessage("StyleCopPlus.StyleCopPlusRules", "SP0100:AdvancedNamingRules", Justification = "OK.")]
    public class SongTests
    {
        [Test]
        public void GetSongs_WhereDescription_Christmas_IsNotNull()
        {
            const string description = "christmas";

            using (EchoNestSession session = new EchoNestSession(ConfigurationManager.AppSettings.Get("echoNestApiKey")))
            {
                SearchArgument searchArgument = new SearchArgument { Results = 10, Bucket = SongBucket.ArtistHotttness, Sort = "artist_familiarity-desc" };

                searchArgument.Description.AddRange(new TermList { description });

                SearchResponse searchResponse = session.Query<Search>().Execute(searchArgument);

                Assert.IsNotNull(searchResponse);

                foreach (SongBucketItem song in searchResponse.Songs)
                {
                    Console.WriteLine("\t{0} ({1})", song.Title, song.ArtistName);
                }

                Console.WriteLine();
            }
        }

        [TestCase("Apocalypse Now Phyc Rock", "60s,guitar,psychadelic,rock,sountrack^0.5", "eeirie^0.5,dark^0.5,disturbing^0.5,groovy^0.5,melancholia^0.5,ominous^0.5", "Hendrix")]
        [TestCase("Apocalypse Now", "60s,psychadelic,rock^0.5,sountrack^0.5", "eeirie,dark,disturbing,groovy,melancholia,ominous^0.5", "Floyd")]
        public void GetSongs_ForApocalypseNow_ExpectedArtist(string title, string styles, string moods, string expect)
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

            SearchArgument searchArgument = new SearchArgument
            {
                Mode = "0", /* minor */
                Sort = "artist_familiarity-desc",
                Results = 10
            };

            searchArgument.Styles.AddRange(styleTerms);

            searchArgument.Moods.AddRange(moodTerms);

            using (EchoNestSession session = new EchoNestSession(ConfigurationManager.AppSettings.Get("echoNestApiKey")))
            {
                SearchResponse searchResponse = session.Query<Search>().Execute(searchArgument);

                Assert.IsNotNull(searchResponse);
                Assert.IsNotNull(searchResponse.Songs);

                var matches = (from s in searchResponse.Songs
                               where s.ArtistName.ToLower().Contains(expect)
                               select s).ToList();

                Assert.IsNotNull(matches, "Failed to find songs where artist name contains: {0}", expect);

                Console.WriteLine("Tracks for '{0}'", title);
                foreach (SongBucketItem song in searchResponse.Songs)
                {
                    Console.WriteLine("\t{0} ({1})", song.Title, song.ArtistName);
                }

                Console.WriteLine();
                Console.WriteLine();
            }
        }
    }
}

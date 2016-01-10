using System;
using EchoNest.Song;

namespace EchoNest.Shell
{
    public class SongShell
    {
        public static void Display(EchoNestSession session)
        {
            ConsoleKeyInfo keyInfo;

            using (ConsoleEx.BeginColorBlock(ConsoleColor.Cyan))
            {
                Console.WriteLine("=== Song API ===");
                Console.WriteLine("1: Search");
                Console.WriteLine("2: Profile");
                Console.WriteLine("=================");
            }

            ConsoleEx.Write("Enter api >> ", ConsoleColor.Green);
            keyInfo = Console.ReadKey();
            Console.Write(Environment.NewLine);

            switch (keyInfo.Key)
            {
                case ConsoleKey.D1:
                    Search(session);
                    break;
                case ConsoleKey.D2:
                    Profile(session);
                    break;
            }
        }

        private static void Search(EchoNestSession session)
        {
            ConsoleEx.WriteLine("=== Song Search ===", ConsoleColor.Cyan);
            ConsoleEx.Write("Query >> ", ConsoleColor.Green);
            string query = Console.ReadLine();

            ConsoleEx.WriteLine("Searching..", ConsoleColor.Yellow);

            var result = session.Query<Search>().Execute(new SearchArgument
                                                             {
                                                                 Title = query,
                                                                 Bucket = SongBucket.ArtistHotttness,
                                                                 Sort = "artist_hotttnesss-desc"
                                                             });

            if (result.Status.Code == ResponseCode.Success)
            {
                foreach (var item in result.Songs)
                {
                    ConsoleEx.Write("Artist: ", ConsoleColor.White);
                    ConsoleEx.WriteLine(item.ArtistName, ConsoleColor.DarkYellow);
                    ConsoleEx.Write("Title: ", ConsoleColor.White);
                    ConsoleEx.WriteLine(item.Title, ConsoleColor.DarkYellow);
                    ConsoleEx.Write("Hotttness: ", ConsoleColor.White);
                    ConsoleEx.WriteLine(item.ArtistHotttnesss.ToString(), ConsoleColor.DarkYellow);
                }
            }
            else
            {
                ConsoleEx.WriteLine(result.Status.Message, ConsoleColor.Red);
            }
        }

        private static void Profile(EchoNestSession session)
        {
            ConsoleEx.WriteLine("=== Song Profile ===", ConsoleColor.Cyan);

            // ConsoleEx.Write("Query >> ", ConsoleColor.Green);
            // string query = Console.ReadLine();
            ConsoleEx.WriteLine("Loading..", ConsoleColor.Yellow);

            var result = session.Query<Profile>().Execute(new ProfileArgument
            {
                Id = "SOWMQIK144BDD38DD3",
                Bucket =
                SongBucket.ArtistHotttness
                | SongBucket.ArtistHotttnesssRank
                | SongBucket.SongHotttness
                | SongBucket.SongHotttnesssRank
                | SongBucket.ArtistDiscovery
                | SongBucket.ArtistDiscoveryRank
                | SongBucket.SongCurrency
                | SongBucket.SongCurrencyRank
                | SongBucket.SongDiscovery
                | SongBucket.SongDiscoveryRank
                | SongBucket.SongType
                | SongBucket.ArtistFamiliarity
                | SongBucket.ArtistFamiliarityRank
                | SongBucket.AudioSummary
                | SongBucket.ArtistLocation
                | SongBucket.Tracks
                | SongBucket.IdSpotifyWw
                | SongBucket.IdDeezer
                | SongBucket.IdMusicBrainz
            });

            if (result.Status.Code == ResponseCode.Success)
            {
                foreach (var item in result.Songs)
                {
                    ConsoleEx.Write("Artist: ", ConsoleColor.White);
                    ConsoleEx.WriteLine(item.ArtistName, ConsoleColor.DarkYellow);
                    ConsoleEx.Write("Title: ", ConsoleColor.White);
                    ConsoleEx.WriteLine(item.Title, ConsoleColor.DarkYellow);
                    ConsoleEx.Write("Hotttness: ", ConsoleColor.White);
                    ConsoleEx.WriteLine(item.ArtistHotttnesss.ToString(), ConsoleColor.DarkYellow);
                }
            }
            else
            {
                ConsoleEx.WriteLine(result.Status.Message, ConsoleColor.Red);
            }
        }
    }
}
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

            foreach (var item in result.Songs)
            {
                ConsoleEx.Write("Artist: ", ConsoleColor.White);
                ConsoleEx.WriteLine(item.ArtistName, ConsoleColor.DarkYellow);
                ConsoleEx.Write("Hotttness: ", ConsoleColor.White);
                ConsoleEx.WriteLine(item.ArtistHotttnesss.ToString(), ConsoleColor.DarkYellow);
            }
        }
    }
}
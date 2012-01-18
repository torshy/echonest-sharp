using System;
using EchoNest.Playlist;

namespace EchoNest.Shell
{
    public class PlaylistShell
    {
        public static void Display(EchoNestSession session)
        {
            ConsoleKeyInfo keyInfo;

            using (ConsoleEx.BeginColorBlock(ConsoleColor.Cyan))
            {
                Console.WriteLine("=== Playlist API ===");
                Console.WriteLine("1: Basic");
                Console.WriteLine("2: Static");
                Console.WriteLine("3: Dynamic");
                Console.WriteLine("=================");
            }

            ConsoleEx.Write("Enter api >> ", ConsoleColor.Green);
            keyInfo = Console.ReadKey();
            Console.Write(Environment.NewLine);

            switch (keyInfo.Key)
            {
                case ConsoleKey.D1:
                    Basic(session);
                    break;
                case ConsoleKey.D2:
                    Basic(session);
                    break;
                case ConsoleKey.D3:
                    Basic(session);
                    break;
            }
        }

        private static void Basic(EchoNestSession session)
        {
            String query = String.Empty;

            ConsoleEx.WriteLine("=== Basic Playlist Generation ===", ConsoleColor.Cyan);
            ConsoleEx.WriteLine("(Enter a comma separated list of artist names for generating a baisc 'artist-radio' type playlist)", ConsoleColor.Cyan);
            
            ConsoleEx.Write("Query >> ", ConsoleColor.Green);
            query = Console.ReadLine();

            if (String.IsNullOrEmpty(query))
            {
                ConsoleEx.WriteLine("Query was empty. Press any key to start over.", ConsoleColor.Cyan);
                Console.ReadLine();
                Basic(session);
            }

            ConsoleEx.WriteLine("Generating..", ConsoleColor.Yellow);

            BasicArgument basicArgument = new BasicArgument();

            string[] terms = query.Split(',');

            TermList artistTerms = new TermList();
            foreach (string term in terms)
            {
                artistTerms.Add(term.Trim());
            }
            
            basicArgument.Artist.AddRange(artistTerms);
            
            var result = session.Query<Basic>().Execute(basicArgument);
            
            if (result.Status.Code == ResponseCode.Success)
            {
                foreach (var item in result.Songs)
                {
                    ConsoleEx.Write("Artist: ", ConsoleColor.White);
                    ConsoleEx.WriteLine(item.ArtistName, ConsoleColor.DarkYellow);
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
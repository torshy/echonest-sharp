using System;
using EchoNest.Artist;

namespace EchoNest.Shell
{
    public class ArtistShell
    {
        public static void Display(EchoNestSession session)
        {
            ConsoleKeyInfo keyInfo;

            using (ConsoleEx.BeginColorBlock(ConsoleColor.Cyan))
            {
                Console.WriteLine("=== Artist API ===");
                Console.WriteLine("1: Biographies");
                Console.WriteLine("2: Blogs");
                Console.WriteLine("3: Familiarity");
                Console.WriteLine("4: Hotttnesss");
                Console.WriteLine("5: Images");
                Console.WriteLine("6: List terms");
                Console.WriteLine("7: News");
                Console.WriteLine("8: Reviews");
                Console.WriteLine("9: Similar");
                Console.WriteLine("0: Suggest");
                Console.WriteLine("=================");
            }

            ConsoleEx.Write("Enter api >> ", ConsoleColor.Green);
            keyInfo = Console.ReadKey();
            Console.Write(Environment.NewLine);

            switch (keyInfo.Key)
            {
                case ConsoleKey.D1:
                    Biographies(session);
                    break;
                case ConsoleKey.D2:
                    Blogs(session);
                    break;
                case ConsoleKey.D3:
                    Familiarity(session);
                    break;
                case ConsoleKey.D4:
                    Hotttnesss(session);
                    break;
                case ConsoleKey.D5:
                    Images(session);
                    break;
                case ConsoleKey.D6:
                    ListTerms(session);
                    break;
                case ConsoleKey.D7:
                    News(session);
                    break;
                case ConsoleKey.D8:
                    Reviews(session);
                    break;
                case ConsoleKey.D9:
                    Similar(session);
                    break;
                case ConsoleKey.D0:
                    Suggest(session);
                    break;
            }
        }

        private static void Biographies(EchoNestSession session)
        {
            ConsoleEx.WriteLine("=== Biographies ===", ConsoleColor.Cyan);
            ConsoleEx.Write("Query >> ", ConsoleColor.Green);
            string query = Console.ReadLine();

            ConsoleEx.WriteLine("Searching..", ConsoleColor.Yellow);

            var result = session.Query<Biography>().Execute(query, numberOfResults: 1);

            if (result.Status.Code == ResponseCode.Success)
            {
                foreach (var biography in result.Biographies)
                {
                    ConsoleEx.WriteLine("Site:", ConsoleColor.White);
                    ConsoleEx.WriteLine(biography.Site, ConsoleColor.DarkYellow);
                    ConsoleEx.WriteLine("Biography text:", ConsoleColor.White);
                    ConsoleEx.WriteLine(biography.Text, ConsoleColor.DarkYellow);
                }
            }
            else
            {
                ConsoleEx.WriteLine(result.Status.Message, ConsoleColor.Red);
            }
        }

        private static void Blogs(EchoNestSession session)
        {
            ConsoleEx.WriteLine("=== Blogs ===", ConsoleColor.Cyan);
            ConsoleEx.Write("Query >> ", ConsoleColor.Green);
            string query = Console.ReadLine();

            ConsoleEx.WriteLine("Searching..", ConsoleColor.Yellow);

            var result = session.Query<Blog>().Execute(query, numberOfResults: 1);

            if (result.Status.Code == ResponseCode.Success)
            {
                foreach (var biography in result.Blogs)
                {
                    ConsoleEx.WriteLine("Name:", ConsoleColor.White);
                    ConsoleEx.WriteLine(biography.Name, ConsoleColor.DarkYellow);
                    ConsoleEx.WriteLine("Summary:", ConsoleColor.White);
                    ConsoleEx.WriteLine(biography.Summary, ConsoleColor.DarkYellow);
                    ConsoleEx.WriteLine("Url:", ConsoleColor.White);
                    ConsoleEx.WriteLine(biography.Url, ConsoleColor.DarkYellow);
                }
            }
            else
            {
                ConsoleEx.WriteLine(result.Status.Message, ConsoleColor.Red);
            }
        }

        private static void Familiarity(EchoNestSession session)
        {
            ConsoleEx.WriteLine("=== Familiarity ===", ConsoleColor.Cyan);
            ConsoleEx.Write("Query >> ", ConsoleColor.Green);
            string query = Console.ReadLine();

            ConsoleEx.WriteLine("Searching..", ConsoleColor.Yellow);

            var result = session.Query<Familiarity>().Execute(query);

            if (result.Status.Code == ResponseCode.Success)
            {
                ConsoleEx.WriteLine("Name:", ConsoleColor.White);
                ConsoleEx.WriteLine(result.Artist.Name, ConsoleColor.DarkYellow);
                ConsoleEx.WriteLine("Familiarity:", ConsoleColor.White);
                ConsoleEx.WriteLine(result.Artist.Familiarity.ToString(), ConsoleColor.DarkYellow);
            }
            else
            {
                ConsoleEx.WriteLine(result.Status.Message, ConsoleColor.Red);
            }
        }

        private static void Hotttnesss(EchoNestSession session)
        {
            ConsoleEx.WriteLine("=== Hotttnesss ===", ConsoleColor.Cyan);
            ConsoleEx.Write("Query >> ", ConsoleColor.Green);
            string query = Console.ReadLine();

            ConsoleEx.WriteLine("Searching..", ConsoleColor.Yellow);

            var result = session.Query<Hotttnesss>().Execute(query);

            if (result.Status.Code == ResponseCode.Success)
            {
                ConsoleEx.WriteLine("Name:", ConsoleColor.White);
                ConsoleEx.WriteLine(result.Artist.Name, ConsoleColor.DarkYellow);
                ConsoleEx.WriteLine("Familiarity:", ConsoleColor.White);
                ConsoleEx.WriteLine(result.Artist.Hotttnesss.ToString(), ConsoleColor.DarkYellow);
            }
            else
            {
                ConsoleEx.WriteLine(result.Status.Message, ConsoleColor.Red);
            }
        }

        private static void Images(EchoNestSession session)
        {
            ConsoleEx.WriteLine("=== Images ===", ConsoleColor.Cyan);
            ConsoleEx.Write("Query >> ", ConsoleColor.Green);
            string query = Console.ReadLine();

            ConsoleEx.WriteLine("Searching..", ConsoleColor.Yellow);

            var result = session.Query<Image>().Execute(query, numberOfResults: 5);

            if (result.Status.Code == ResponseCode.Success)
            {
                foreach (var item in result.Images)
                {
                    ConsoleEx.WriteLine("Url:", ConsoleColor.White);
                    ConsoleEx.WriteLine(item.Url, ConsoleColor.DarkYellow);
                }
            }
            else
            {
                ConsoleEx.WriteLine(result.Status.Message, ConsoleColor.Red);
            }
        }

        private static void ListTerms(EchoNestSession session)
        {
            ConsoleEx.WriteLine("=== List Terms ===", ConsoleColor.Cyan);

            var result = session.Query<ListTerms>().Execute(ListTermsType.Style);

            if (result.Status.Code == ResponseCode.Success)
            {
                Console.WriteLine("Term type: " + result.TypeAsString);
                foreach (var item in result.Terms)
                {
                    ConsoleEx.Write("Name: ", ConsoleColor.White);
                    ConsoleEx.WriteLine(item.Name, ConsoleColor.DarkYellow);
                }
            }
            else
            {
                ConsoleEx.WriteLine(result.Status.Message, ConsoleColor.Red);
            }

            ConsoleEx.WriteLine("-------------");

            result = session.Query<ListTerms>().Execute(ListTermsType.Mood);

            if (result.Status.Code == ResponseCode.Success)
            {
                Console.WriteLine("Term type: " + result.TypeAsString);
                foreach (var item in result.Terms)
                {
                    ConsoleEx.Write("Name: ", ConsoleColor.White);
                    ConsoleEx.WriteLine(item.Name, ConsoleColor.DarkYellow);
                }
            }
            else
            {
                ConsoleEx.WriteLine(result.Status.Message, ConsoleColor.Red);
            }
        }

        private static void News(EchoNestSession session)
        {
            ConsoleEx.WriteLine("=== News ===", ConsoleColor.Cyan);
            ConsoleEx.Write("Query >> ", ConsoleColor.Green);
            string query = Console.ReadLine();

            ConsoleEx.WriteLine("Searching..", ConsoleColor.Yellow);

            var result = session.Query<News>().Execute(query, numberOfResults: 5);

            if (result.Status.Code == ResponseCode.Success)
            {
                foreach (var item in result.News)
                {
                    ConsoleEx.WriteLine("Name:", ConsoleColor.White);
                    ConsoleEx.WriteLine(item.Name, ConsoleColor.DarkYellow);
                    ConsoleEx.WriteLine("Url:", ConsoleColor.White);
                    ConsoleEx.WriteLine(item.Url, ConsoleColor.DarkYellow);
                }
            }
            else
            {
                ConsoleEx.WriteLine(result.Status.Message, ConsoleColor.Red);
            }
        }

        private static void Reviews(EchoNestSession session)
        {
            ConsoleEx.WriteLine("=== Reviews ===", ConsoleColor.Cyan);
            ConsoleEx.Write("Query >> ", ConsoleColor.Green);
            string query = Console.ReadLine();

            ConsoleEx.WriteLine("Searching..", ConsoleColor.Yellow);

            var result = session.Query<Reviews>().Execute(query, numberOfResults: 5);

            if (result.Status.Code == ResponseCode.Success)
            {
                foreach (var item in result.Reviews)
                {
                    ConsoleEx.WriteLine("Name:", ConsoleColor.White);
                    ConsoleEx.WriteLine(item.Name, ConsoleColor.DarkYellow);
                    ConsoleEx.WriteLine("Url:", ConsoleColor.White);
                    ConsoleEx.WriteLine(item.Url, ConsoleColor.DarkYellow);
                }
            }
            else
            {
                ConsoleEx.WriteLine(result.Status.Message, ConsoleColor.Red);
            }
        }

        private static void Similar(EchoNestSession session)
        {
            ConsoleEx.WriteLine("=== Similar ===", ConsoleColor.Cyan);
            ConsoleEx.Write("Query >> ", ConsoleColor.Green);
            string query = Console.ReadLine();

            ConsoleEx.WriteLine("Searching..", ConsoleColor.Yellow);

            var result = session.Query<SimilarArtists>().Execute(new SimilarArtistsArgument { Name = query, Results = 5 });

            if (result.Status.Code == ResponseCode.Success)
            {
                foreach (var item in result.Artists)
                {
                    ConsoleEx.WriteLine(item.Name, ConsoleColor.DarkYellow);
                }
            }
            else
            {
                ConsoleEx.WriteLine(result.Status.Message, ConsoleColor.Red);
            }
        }

        private static void Suggest(EchoNestSession session)
        {
            ConsoleEx.WriteLine("=== Suggest ===", ConsoleColor.Cyan);
            ConsoleEx.Write("Query >> ", ConsoleColor.Green);
            string query = Console.ReadLine();

            ConsoleEx.WriteLine("Searching..", ConsoleColor.Yellow);

            var result = session.Query<SuggestArtist>().Execute(query, 5);

            if (result.Status.Code == ResponseCode.Success)
            {
                foreach (var item in result.Artists)
                {
                    ConsoleEx.WriteLine(item.Name, ConsoleColor.DarkYellow);
                }
            }
            else
            {
                ConsoleEx.WriteLine(result.Status.Message, ConsoleColor.Red);
            }
        }
    }
}

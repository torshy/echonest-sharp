using System;

namespace EchoNest.Shell
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please provide the Echo Nest API key");
                Console.ReadLine();
                return;
            }

            string apiKey = args[0];

            using (var session = new EchoNestSession(apiKey))
            {
                ConsoleKeyInfo keyInfo;

                do
                {
                    using (ConsoleEx.BeginColorBlock(ConsoleColor.Cyan))
                    {
                        Console.WriteLine("=== Main menu ===");
                        Console.WriteLine("1: Artist API");
                        Console.WriteLine("2: Song API");
                        Console.WriteLine("3: Playlist API");
                        Console.WriteLine("=================");
                    }

                    ConsoleEx.Write("Enter menu >> ", ConsoleColor.Green);
                    keyInfo = Console.ReadKey();
                    Console.Write(Environment.NewLine);

                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.D1:
                            ArtistShell.Display(session);
                            break;
                        case ConsoleKey.D2:
                            SongShell.Display(session);
                            break;
                        case ConsoleKey.D3:
                            PlaylistShell.Display(session);
                            break;
                    }

                    if (keyInfo.Key != ConsoleKey.Escape)
                    {
                        ConsoleEx.Write("Press any key to continue (except Esc which will exit)", ConsoleColor.Yellow);
                        keyInfo = Console.ReadKey();
                        Console.WriteLine();
                    }

                } while (keyInfo.Key != ConsoleKey.Escape);
            }

            Console.ReadLine();
        }
    }
}

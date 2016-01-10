using System;
using EchoNest.Track;

namespace EchoNest.Shell
{
    public class TrackShell
    {
        public static void Display(EchoNestSession session)
        {
            ConsoleKeyInfo keyInfo;

            using (ConsoleEx.BeginColorBlock(ConsoleColor.Cyan))
            {
                Console.WriteLine("=== Track API ===");
                Console.WriteLine("1: Profile");
                Console.WriteLine("=================");
            }

            ConsoleEx.Write("Enter api >> ", ConsoleColor.Green);
            keyInfo = Console.ReadKey();
            Console.Write(Environment.NewLine);

            switch (keyInfo.Key)
            {
                case ConsoleKey.D1:
                    Profile(session);
                    break;
            }
        }

        private static void Profile(EchoNestSession session)
        {
            ConsoleEx.WriteLine("=== Song Search ===", ConsoleColor.Cyan);
            ConsoleEx.Write("Query >> ", ConsoleColor.Green);
            string query = Console.ReadLine();

            ConsoleEx.WriteLine("Searching..", ConsoleColor.Yellow);

            string id = "spotify:track:58HONnHW8ZvMDb4In757x4";

            // id = "TRNATAV144D0B601A7";
            var result = session.Query<Profile>().Execute(new ProfileArgument
                                                             {
                                                                 Id = id,
                                                                 Bucket = TrackBucket.AudioSummary
                                                             });

            if (result.Status.Code == ResponseCode.Success)
            {
                var item = result.Track;
                ConsoleEx.Write("Artist: ", ConsoleColor.White);
                ConsoleEx.WriteLine(item.Artist, ConsoleColor.DarkYellow);
                ConsoleEx.Write("Title: ", ConsoleColor.White);
                ConsoleEx.WriteLine(item.Title, ConsoleColor.DarkYellow);                
            }
            else
            {
                ConsoleEx.WriteLine(result.Status.Message, ConsoleColor.Red);
            }
        }
    }
}
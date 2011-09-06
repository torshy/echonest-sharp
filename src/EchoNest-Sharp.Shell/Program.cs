using System;
using EchoNest.Artist;

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
            
            using(var session = new EchoNestSession(apiKey))
            {
                var result = session.Query<Suggest>().Execute("NO", 50);

                foreach (var artist in result.Artists)
                {
                    Console.WriteLine(artist.Name);
                }
            }

            Console.ReadLine();
        }
    }
}

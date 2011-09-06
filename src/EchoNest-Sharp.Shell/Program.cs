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
                var result = session.Query<Suggest>().Execute("no", 50);

                if (result.Status.Code == ResponseCode.Success)
                {
                    foreach (var item in result.Artists)
                    {
                        Console.WriteLine(item.Name + " [" + item.ID + "]");
                    }
                }
                else
                {
                    Console.WriteLine(result.Status.Message);
                }
            }

            Console.ReadLine();
        }
    }
}

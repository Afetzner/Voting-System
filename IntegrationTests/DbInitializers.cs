using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegrationTests.Interactive;

namespace IntegrationTests
{
    public class DbInitializers
    {
        public static Func<bool> DbInitMenu()
        {
            Console.WriteLine("Select a group to test");
            Console.WriteLine(
                " (0) Reset Db tables\n" +
                " (1) Load Test data\n" +
                " (2) Exit\n");

            while (true)
            {
                if (!Console.KeyAvailable)
                    continue;

                var key = Console.ReadKey();
                Console.WriteLine();

                return key.KeyChar switch
                {
                    '0' => ResetDb,
                    '1' => LoadTestData,
                    '2' => Menu.Exit,
                    _ => Menu.Exit
                };
            }
        }

        public static bool ResetDb()
        {
            throw new NotImplementedException();
        }

        public static bool LoadTestData()
        {
            throw new NotImplementedException();
        }
    }
}

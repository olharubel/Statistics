using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistics
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                SiteInfo info = new SiteInfo();
                info.ReadFromFile(@"C:\Users\Ростик\source\repos\Statistics\Info.txt");

                Console.WriteLine(info.ToString());
                Console.WriteLine();

                Dictionary<string, int> visits = info.GetVisitsNumber();
                Console.WriteLine(info.VisitsToString(visits));

                Console.WriteLine();

                Dictionary<string, string> popularDay = info.GetMostPopularDay();
                Console.WriteLine(info.PopularDayToString(popularDay));

                Console.WriteLine();

                var popularHour = info.GetMostPopularTimeForIP();
                Console.WriteLine(info.PopularHourToString(popularHour));

                var totalPopularHour = info.GetTotalPopularTime();
                Console.WriteLine(info.TotalPopularHourToString(totalPopularHour));

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }


            Console.ReadLine();
        }
    }
}

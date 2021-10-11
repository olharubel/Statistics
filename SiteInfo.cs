using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Statistics
{
   
    class SiteInfo
    {
        private Dictionary<string, List<Date>> info;

        public SiteInfo(Dictionary<string, List<Date>> info){
            this.info = info;
        }

        public SiteInfo()
        {
            this.info = new Dictionary<string, List<Date>>();
        }

        private void AddInfo(string ip, TimeSpan time, string dayName)
        {
            if (!info.ContainsKey(ip))
            {
                info.Add(ip, new List<Date>());
            }
            info[ip].Add(new Date(time, dayName));
        }

        public void InitializeInfo(string ip, TimeSpan time, string dayName)
        {
            AddInfo(ip, time, dayName);
        }

        public override string ToString()
        {
            string str = "";
            foreach(var site in info)
            {
                str += site.Key + " ";
                foreach(var date in site.Value)
                {
                    str += date.ToString() + "\n";
                }
            }
            return str;
        }

        public void ReadFromFile(string path)
        {
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string data;
                    bool isEmpty = true;
                    while (!sr.EndOfStream)
                    {
                        data = sr.ReadLine();
                        if(data.Length == 0)
                        {
                            break;
                        }
                        string[] siteInfo = data.Split();
                        string[] date = siteInfo[1].Split(':');
                        int hour = Convert.ToInt32(date[0]);
                        int minutes = Convert.ToInt32(date[1]);
                        int seconds = Convert.ToInt32(date[2]);
                        TimeSpan timeSpan = new TimeSpan(hour, minutes, seconds);
                        AddInfo(siteInfo[0], timeSpan, siteInfo[2]);
                        isEmpty = false;
                    }
                    if (isEmpty)
                    {
                        throw new Exception("File is empty!");
                    }
                    sr.Close();
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }

        }

        public Dictionary<string, int> GetVisitsNumber()
        {
            if(info.Count == 0)
            {
                throw new Exception("Any information about sites is not found. " +
                    "Impossible to determine the number of visits.");
            }
            Dictionary<string, int> visits = new Dictionary<string, int>();
            foreach(var site in info)
            {
                visits.Add(site.Key, site.Value.Count);
            }
            return visits;
        }

        public string VisitsToString(Dictionary<string, int> visits)
        {
            string str = "";
            foreach(var site in visits)
            {
                str += $"{site.Key} {site.Value}\n";
            }
            return str;
        }

        public Dictionary<string, string> GetMostPopularDay()
        {
            if (info.Count == 0)
            {
                throw new Exception("Any information about sites is not found. " +
                    "Impossible to determine the most popular day per week.");
            }
            Dictionary<string, string> popularDay = new Dictionary<string, string>();

            int count = 1;
            foreach (var site in info)
            {
                Dictionary<string, int> temp = new Dictionary<string, int>();
                for (int i = 0; i < site.Value.Count; ++i)
                {

                    string day = site.Value[i].DayName;
                    if (!temp.ContainsKey(day))
                    {
                        temp.Add(day, count);
                    }
                    else
                    {
                        temp[day] += count;
                    }
                }

                int max = temp.Values.Max();
                string val = temp.FirstOrDefault(x => x.Value == max).Key;
                popularDay.Add(site.Key, val);
            }
            return popularDay;
        }

        public string PopularDayToString(Dictionary<string, string> visits)
        {
            string str = "";
            foreach (var site in visits)
            {
                str += $"{site.Key} {site.Value}\n";
            }
            return str;
        }

        private (TimeSpan, TimeSpan) GetMostPopularTime(KeyValuePair<string, List<Date>> site)
        {
                Dictionary<int, int> temp = new Dictionary<int, int>();
                for (int i = 0; i < site.Value.Count; ++i)
                {
                    int count = 1;
                    int hour = site.Value[i].Time.Hours;
                    if (!temp.ContainsKey(hour))
                    {
                        temp.Add(hour, count);
                    }
                    else
                    {
                        temp[hour] += count;
                    }
                }

                int max = temp.Values.Max();
                int hourMax = temp.FirstOrDefault(x => x.Value == max).Key;
               
                return (new TimeSpan(hourMax, 0, 0), new TimeSpan(hourMax + 1, 0, 0));
        }

        public Dictionary<string, (TimeSpan, TimeSpan)> GetMostPopularTimeForIP()
        { 
            Dictionary<string, (TimeSpan, TimeSpan)> time = new Dictionary<string, (TimeSpan, TimeSpan)>();

            foreach (var site in info)
            {
                var timeSpan = GetMostPopularTime(site);
                time.Add(site.Key, timeSpan);
            }
            return time;
        }


        public string PopularHourToString(Dictionary<string, (TimeSpan, TimeSpan)> hours)
        {
            string str = "";
            foreach (var site in hours)
            {
                str += $"{site.Key} {site.Value.Item1:hh\\:mm\\:ss} - {site.Value.Item2:hh\\:mm\\:ss}\n";
            }
            return str;
        }

        public (TimeSpan, TimeSpan) GetTotalPopularTime()
        {
            List<(TimeSpan, TimeSpan)> timeSpans = new List<(TimeSpan, TimeSpan)>();
            foreach (var site in info)
            {
                timeSpans.Add(GetMostPopularTime(site));
                
            }

            var maxSpan = timeSpans.Max();

            return maxSpan;
        }

        public string TotalPopularHourToString((TimeSpan, TimeSpan) hours)
        {
            string str = "";
                str += $"Total popular time: {hours.Item1:hh\\:mm\\:ss} - {hours.Item2:hh\\:mm\\:ss}\n";
            return str;
        }

    }
}


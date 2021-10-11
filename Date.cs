using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistics
{
    struct Date
    {
        private TimeSpan time;
        private string dayName;

        public Date(TimeSpan time, string dayName)
        {
            this.time = time;
            this.dayName = dayName;
        }

        public override string ToString()
        {
            return $"{time:hh\\:mm\\:ss} {dayName}";
        }

        public TimeSpan Time
        {
            get { return time; }
        }

        public string DayName
        {
            get { return dayName; }
        }
    }
}

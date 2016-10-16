using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.CollaborationWindow.TableModule
{
    public class DataAttribute
    {
        public String Name { get; set; }
        public ATTRIBUTETYPE Type { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }

        private Dictionary<String, int> valuesCount;

        internal DataAttribute()
        {
            this.valuesCount = new Dictionary<string, int>();
            Min = double.MaxValue;
            Max = double.MinValue;
        }

        internal String GetInfo()
        {
            String result = "Type: " + Type; 
            if (this.Type == ATTRIBUTETYPE.Categorical)
            {
                result += "\nMost Common Items:\n" + MostCommonKeys(3);
            }
            else
            {
                result += "\nMin: " + Min + "\nMax: " + Max;
            }
            return result;
            
        }

        internal void AddCategorical(string str) {
            if (valuesCount.Keys.Contains(str))
            {
                valuesCount[str]++;
            }
            else {
                valuesCount.Add(str, 0);
            }
        }
        internal int GetCountIndex(string value) {
            return valuesCount[value];
        }
        /// <summary>
        /// Retrieves the top N most common values in the attribute.
        /// </summary>
        /// <param name="n">the integer denoting how many most common values to return</param>
        /// <returns>string representation of the top N most common key values</returns>
        internal String MostCommonKeys(int n)
        {
            var list = valuesCount.OrderByDescending(x => x.Value);
            String result = "";
            if (n > list.Count())
            {
                n = list.Count();
            }
            int counter = 0;
            foreach (KeyValuePair<string, int> pair in valuesCount)
            {
                result += pair.Key + " " + pair.Value+"\n";
                counter++;
                if (counter == n) {
                    break;
                }
            }
            return result.Trim();
        }
    }
}

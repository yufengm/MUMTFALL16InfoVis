using CoLocatedCardSystem.CollaborationWindow.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.CollaborationWindow.DocumentModule
{
    class ImageVector
    {
        string id;
        Dictionary<string, double> list = new Dictionary<string, double>();

        public string Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public Dictionary<string, double> List
        {
            get
            {
                return list;
            }

            set
            {
                list = value;
            }
        }

        internal void AddImage(string key, double weight)
        {
            if (!list.ContainsKey(key)) {
                list.Add(key, weight);
            }
        }

        internal void SetImage(string line)
        {
            string[] values = CSVParser.Parse(line);
            id = values[0];
            for (int i = 1; i < 6; i++) {
                double def= 0;
                Double.TryParse(values[i + 5], out def);
                list.Add(values[i], def);
            }
        }
    }
}

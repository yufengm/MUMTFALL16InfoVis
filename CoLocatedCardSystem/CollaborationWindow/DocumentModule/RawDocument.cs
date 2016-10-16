using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.CollaborationWindow.DocumentModule
{
    class RawDocument
    {
        string id;
        string title;
        string docTime;
        string[] content;
        List<Token> date = new List<Token>();
        List<Token> location = new List<Token>();
        List<Token> money = new List<Token>();
        List<Token> organization = new List<Token>();
        List<Token> person = new List<Token>();
        List<Token> time = new List<Token>();

        public string ToJson() {
            return JsonConvert.SerializeObject(this);
        }
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

        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                title = value;
            }
        }


        public string[] Content
        {
            get
            {
                return content;
            }

            set
            {
                content = value;
            }
        }
        

        public string DocTime
        {
            get
            {
                return docTime;
            }

            set
            {
                docTime = value;
            }
        }

        internal List<Token> Date
        {
            get
            {
                return date;
            }

            set
            {
                date = value;
            }
        }

        internal List<Token> Location
        {
            get
            {
                return location;
            }

            set
            {
                location = value;
            }
        }

        internal List<Token> Money
        {
            get
            {
                return money;
            }

            set
            {
                money = value;
            }
        }

        internal List<Token> Organization
        {
            get
            {
                return organization;
            }

            set
            {
                organization = value;
            }
        }

        internal List<Token> Person
        {
            get
            {
                return person;
            }

            set
            {
                person = value;
            }
        }

        internal List<Token> Time
        {
            get
            {
                return time;
            }

            set
            {
                time = value;
            }
        }

        internal bool IsContainContent(string content)
        {
            ProcessedDocument tempPD = new ProcessedDocument();
            tempPD.InitTokens(content);
            foreach (Token tki in tempPD.List) {
                foreach (Token tkj in person) {
                    if (tkj.StemmedWord.Contains(tki.StemmedWord)) {
                        return true;
                    }
                }
                foreach (Token tkj in date)
                {
                    if (tkj.StemmedWord.Contains(tki.StemmedWord))
                    {
                        return true;
                    }
                }
                foreach (Token tkj in money)
                {
                    if (tkj.StemmedWord.Contains(tki.StemmedWord))
                    {
                        return true;
                    }
                }
                foreach (Token tkj in location)
                {
                    if (tkj.StemmedWord.Contains(tki.StemmedWord))
                    {
                        return true;
                    }
                }
                foreach (Token tkj in time)
                {
                    if (tkj.StemmedWord.Contains(tki.StemmedWord))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}

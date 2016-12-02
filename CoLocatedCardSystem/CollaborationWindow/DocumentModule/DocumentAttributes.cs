using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.CollaborationWindow.DocumentModule
{
    class DocumentAttributes
    {
        string id;
        string name = "";
        string[] reviewTime;
        string[] rating;
        string[] jpg;
        double[][] vectors;

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

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public string[] ReviewTime
        {
            get
            {
                return reviewTime;
            }

            set
            {
                reviewTime = value;
            }
        }

        public string[] Rating
        {
            get
            {
                return rating;
            }

            set
            {
                rating = value;
            }
        }

        public string[] Jpg
        {
            get
            {
                return jpg;
            }

            set
            {
                jpg = value;
            }
        }

        public double[][] Vectors
        {
            get
            {
                return vectors;
            }

            set
            {
                vectors = value;
            }
        }
    }
}

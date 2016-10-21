﻿using Newtonsoft.Json;
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
        string name = "";
        string[] reviewTime;
        string[] rating;
        string[] jpg;
        string[][] serializedProcessedDocument;

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

        public string[][] SerializedProcessedDocument
        {
            get
            {
                return serializedProcessedDocument;
            }

            set
            {
                serializedProcessedDocument = value;
            }
        }
    }
}

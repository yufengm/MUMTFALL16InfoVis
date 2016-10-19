using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocProcess
{
    class Token
    {
        string originalWord = "";
        string stemmedWord = "";
        string wordTypeJson = "";
        WordType wordType = WordType.DEFAULT;

        public string OriginalWord
        {
            get
            {
                return originalWord;
            }

            set
            {
                originalWord = value;
            }
        }

        public string StemmedWord
        {
            get
            {
                return stemmedWord;
            }

            set
            {
                stemmedWord = value;
            }
        }

        public string WordTypeJson
        {
            get
            {
                return wordTypeJson;
            }

            set
            {
                wordTypeJson = value;
            }
        }

        internal WordType WordType
        {
            get
            {
                return wordType;
            }

            set
            {
                wordType = value;
            }
        }

        /// <summary>
        /// Assign value to the stemmed word and the type
        /// </summary>
        public void Process()
        {
            PunctuationMarker.Mark(this);
            Stemmer.Stem(this); // convert to root form
            StopwordMarker.Mark(this);
            IrregularMarker.Mark(this);
        }

        internal String ToJson()
        {
            wordTypeJson = wordType.ToString();
            return JsonConvert.SerializeObject(this);
        }
    }
}

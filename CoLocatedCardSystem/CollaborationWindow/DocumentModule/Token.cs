using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.CollaborationWindow.DocumentModule
{
    class Token
    {
        string originalWord = "";
        string stemmedWord = "";
        string wordTypeJson = "";
        WordType wordType=WordType.DEFAULT;

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

        internal String ToJson() {
            wordTypeJson = wordType.ToString();
            return JsonConvert.SerializeObject(this);
        }
        internal bool EqualContent(Token token)
        {
            if (token.StemmedWord.Contains(this.StemmedWord))
            {
                return true;
            }
            else
            {
                if (token.OriginalWord.ToLower().Equals(this.OriginalWord.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Assign value to the stemmed word and the type
        /// </summary>
        public void Process() {
            PunctuationMarker.Mark(this);
            Stemmer.Stem(this); // convert to root form
            StopwordMarker.Mark(this);
            IrregularMarker.Mark(this);
        }
        internal void AssignTypeFromJson() {
            switch (this.wordTypeJson)
            {
                case "PUNCTUATION":
                    this.wordType = WordType.PUNCTUATION;
                    break;
                case "STOPWORD":
                    this.wordType = WordType.STOPWORD;
                    break;
                case "IRREGULAR":
                    this.wordType = WordType.IRREGULAR;
                    break;
                case "REGULAR":
                    this.wordType = WordType.REGULAR;
                    break;
                case "LINEBREAK":
                    this.wordType = WordType.LINEBREAK;
                    break;
                case "DATE":
                    this.wordType = WordType.DATE;
                    break;
                case "LOCACTION":
                    this.wordType = WordType.LOCACTION;
                    break;
                case "MONEY":
                    this.wordType = WordType.MONEY;
                    break;
                case "ORGANIZATION":
                    this.wordType = WordType.ORGANIZATION;
                    break;
                case "PERSON":
                    this.wordType = WordType.PERSON;
                    break;
                case "TIME":
                    this.wordType = WordType.TIME;
                    break;
                default:
                    this.wordType = WordType.DEFAULT;
                    break;
            }
        }
    }
}

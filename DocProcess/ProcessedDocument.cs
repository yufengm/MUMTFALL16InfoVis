using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
namespace DocProcess
{
    class ProcessedDocument
    {
        Token[][] list;
        Regex regex = new Regex("([\\s{}():;., \"“”])");
        string docID = "";
        string name = "";
        string[] time;
        string[] rating;
        string[] jpg;
        string[][] serializedProcessedDocument;
        

        public string DocID
        {
            get
            {
                return docID;
            }

            set
            {
                docID = value;
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

        public string[] Time
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

        public ProcessedDocument(string docID)
        {
            this.docID = docID;           
        }

        public ProcessedDocument(string docID, string name, string[] time, string[] rating, string[] jpg, string[][] serializedProcessedDocument) : this(docID)
        {
            this.name = name;
            this.time = time;
            this.rating = rating;
            this.jpg = jpg;
            this.serializedProcessedDocument = serializedProcessedDocument;
        }

        public void LoadDoc(string[][] items) {

            try
            {
                this.name = items[0][0];
                time = items.Select(a => a[1].Replace("\"", "")).ToArray() as string[];
                rating = items.Select(a => a[3].Replace("\"", "")).ToArray() as string[];
                jpg = items.Select(a => a[9].Replace("\"", "")).ToArray() as string[];
                string[] texts = items.Select(a => a[2]).ToArray() as String[];
                List<Token[]> tlist = new List<Token[]>();
                foreach (String text in texts)
                {
                    List<Token> tokenList = new List<Token>();
                    ProcessRegular(text, tokenList);
                    tlist.Add(tokenList.ToArray());
                }
                list = tlist.ToArray();

            }
            catch (Exception ex) {

            }
        }
        internal string ToJson()
        {
            List<string[]> tempList = new List<string[]>();
            foreach (Token[] ts in list) {
                tempList.Add(ts.Select(t => t.ToJson()).ToArray());
            }
            Console.WriteLine(JsonConvert.SerializeObject(this));
            serializedProcessedDocument = tempList.ToArray();
            return JsonConvert.SerializeObject(this);
        }
        private void ProcessRegular(String text, List<Token> tokenList)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(text);
            string content = Regex.Replace(htmlDocument.DocumentNode.InnerText, @"[^\u0000-\u007F]+", " ");
            string stack = "";
            List<Token> tempList = new List<Token>();
            //Partition the content with the regular expression.
            for (int i = 0; i < content.Length; i++)
            {
                if (regex.IsMatch("" + content[i]))//Add a spliter
                {
                    if (stack.Length > 0)
                    {
                        Token token = new Token();
                        token.OriginalWord = stack.Trim();
                        tempList.Add(token);
                        stack = "";
                    }
                    Token tk = new Token();
                    tk.OriginalWord = "" + content[i];
                    tempList.Add(tk);
                }
                else//Add the letter to the word stack
                {
                    stack += content[i];
                }
            }
            if (stack.Length > 0)
            {
                Token token = new Token();
                token.OriginalWord = stack;
                tempList.Add(token);
            }
            foreach (Token tk in tempList)
            {
                tk.Process();
                tokenList.Add(tk);
            }
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.CollaborationWindow.DocumentModule
{
    public class Document
    {
        string docID;
        DocumentAttributes documentAttributes;
        ProcessedDocument[] processedDocuments;
        int[] defaultTopicIndex;//the index of the topic
        private class JDocument
        {
            public string DocID = "";
            public string name = "";
            public string[] time;
            public string[] rating;
            public string[] jpg;
            private string[] topics;
            public string[][] serializedProcessedDocument;

            internal string Name
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

            internal string[] Time
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

            internal string[][] SerializedProcessedDocument
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

            public string[] Topics
            {
                get
                {
                    return topics;
                }

                set
                {
                    topics = value;
                }
            }
        }
        internal string DocID
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

        internal ProcessedDocument[] ProcessedDocument
        {
            get
            {
                return processedDocuments;
            }

            set
            {
                processedDocuments = value;
            }
        }

        internal DocumentAttributes DocumentAttributes
        {
            get
            {
                return documentAttributes;
            }

            set
            {
                documentAttributes = value;
            }
        }

        /// <summary>
        /// Get json object from string
        /// </summary>
        /// <param name="line"></param>
        internal void Deserialize(string line)
        {
            JDocument tempDoc = JsonConvert.DeserializeObject<JDocument>(line);
            this.docID = tempDoc.DocID;
            documentAttributes = new DocumentAttributes();
            documentAttributes.Id = tempDoc.DocID;
            documentAttributes.Name = tempDoc.Name;
            documentAttributes.ReviewTime = tempDoc.Time;
            documentAttributes.Jpg = tempDoc.jpg;
            documentAttributes.Rating = tempDoc.rating;
            documentAttributes.Vectors = new double[tempDoc.Topics.Length][];
            for (int i = 0; i < tempDoc.Topics.Length; i++) {
                string[] strs = tempDoc.Topics[i].Split(',');
                List<double> vs = new List<double>();
                foreach (string s in strs) {
                    vs.Add(Double.Parse(s));
                }
                documentAttributes.Vectors[i] = vs.ToArray();
            }
            defaultTopicIndex = new int[documentAttributes.Vectors.Length];
            for (int i = 0; i < documentAttributes.Vectors.Length; i++) {
                double max = documentAttributes.Vectors[i].Max();
                defaultTopicIndex[i] = Array.IndexOf(documentAttributes.Vectors[i], max);
            }
            processedDocuments = new ProcessedDocument[tempDoc.SerializedProcessedDocument.Length];
            for (int i = 0; i < tempDoc.SerializedProcessedDocument.Length; i++)
            {
                Token[] tList = new Token[tempDoc.SerializedProcessedDocument[i].Length];
                for (int j = 0; j < tempDoc.SerializedProcessedDocument[i].Length; j++)
                {
                    Token t = JsonConvert.DeserializeObject<Token>(tempDoc.SerializedProcessedDocument[i][j]);
                    t.AssignTypeFromJson();
                    tList[j] = t;
                }
                processedDocuments[i] = new ProcessedDocument();
                processedDocuments[i].List = tList;
            }

        }

        /// <summary>
        /// Get the title of the article
        /// </summary>
        /// <returns></returns>
        internal string GetName()
        {
            return documentAttributes.Name;
        }

        /// <summary>
        /// Check if the document content a word
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        internal bool HasWord(ProcessedDocument tempPD)
        {
            if (tempPD.List.Count() == 0) {
                return false;
            }
            foreach (ProcessedDocument pd in processedDocuments)
            {
                foreach (Token tk in tempPD.List)
                {
                    if (tk.WordType == WordType.REGULAR && pd.IsContainToken(tk) != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Get all token objects contained tokens in tempPD
        /// </summary>
        /// <param name="tempPD"></param>
        /// <returns></returns>
        internal IEnumerable<Token> GetToken(ProcessedDocument tempPD){
            List<Token> result = new List<Token>();
            if (tempPD.List.Count() == 0)
            {
                return result;
            }
            foreach (ProcessedDocument pd in processedDocuments)
            {
                foreach (Token tk in tempPD.List)
                {
                    if (tk.WordType == WordType.REGULAR)
                    {
                        Token sameToken = pd.IsContainToken(tk);
                        result.Add(sameToken);
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// Get the index of the topic
        /// </summary>
        /// <returns></returns>
        internal int GetDefaultTopicIndex() {
            return defaultTopicIndex[0];
        }

        internal Token FindToken(string word) {
            for (int index = 0; index < ProcessedDocument.Length; index++)
            {
                Token[] tokens = ProcessedDocument[index].List;
                foreach (Token token in tokens)
                {
                    if (token.WordType == WordType.REGULAR && token.StemmedWord.Equals(word))
                    {
                        return token;
                    }
                }
            }
            return null;
        }
    }
}
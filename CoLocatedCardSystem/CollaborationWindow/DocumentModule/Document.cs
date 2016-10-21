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
        RawDocument rawDocument;
        ProcessedDocument[] processedDocuments;
        private class JDocument
        {
            public string DocID = "";
            public string name = "";
            public string[] time;
            public string[] rating;
            public string[] jpg;
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

        internal RawDocument RawDocument
        {
            get
            {
                return rawDocument;
            }

            set
            {
                rawDocument = value;
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
            rawDocument = new RawDocument();
            rawDocument.Id = tempDoc.DocID;
            rawDocument.Name = tempDoc.Name;
            rawDocument.ReviewTime = tempDoc.Time;
            rawDocument.Jpg = tempDoc.jpg;
            rawDocument.SerializedProcessedDocument = tempDoc.SerializedProcessedDocument;
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
            return rawDocument.Name;
        }

        /// <summary>
        /// Check if the document content a word
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        internal bool HasWord(string content)
        {
            if (content.Trim().Length == 0) {
                return false;
            }
            ProcessedDocument tempPD = new ProcessedDocument();
            tempPD.InitTokens(content);
            foreach (ProcessedDocument pd in processedDocuments) {
                foreach (Token tk in tempPD.List) {
                    if (pd.IsContainToken(tk)!=null) {
                        return true;
                    }
                }
            }
            return false;
        }

        internal IEnumerable<Token> GetToken(string content){
            List<Token> result = new List<Token>();
            if (content.Trim().Length == 0)
            {
                return result;
            }
            ProcessedDocument tempPD = new ProcessedDocument();
            tempPD.InitTokens(content);
            foreach (ProcessedDocument pd in processedDocuments)
            {
                foreach (Token tk in tempPD.List)
                {
                    Token sameToken = pd.IsContainToken(tk);
                    result.Add(sameToken);
                }
            }
            return result;
        }
    }
}
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
        ProcessedDocument processedDocument;
        private class JDocument
        {
            public string Title = "";
            public string Time = "";
            public string DocID = "";
            public string[] serializedProcessedDocument;
        }
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

        internal ProcessedDocument ProcessedDocument
        {
            get
            {
                return processedDocument;
            }
        }
        /// <summary>
        /// Convert the processed document to json array.
        /// </summary>
        /// <returns></returns>
        internal string ToJson()
        {
            JDocument jdoc = new JDocument();
            jdoc.DocID = docID;
            //jdoc.serializedRawDocument = serializedRawDocument;
            //jdoc.serializedProcessedDocument = serializedProcessedDocument;
            return JsonConvert.SerializeObject(jdoc);
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
            rawDocument.Title = tempDoc.Title;
            rawDocument.DocTime = tempDoc.Time;
            rawDocument.Content = tempDoc.serializedProcessedDocument;
            processedDocument = new ProcessedDocument();
            Token[] tList = new Token[tempDoc.serializedProcessedDocument.Length];
            for (int i = 0; i < tempDoc.serializedProcessedDocument.Length; i++)
            {
                Token t = JsonConvert.DeserializeObject<Token>(tempDoc.serializedProcessedDocument[i]);
                t.AssignTypeFromJson();
                switch (t.WordType) {
                    case WordType.DATE:
                        rawDocument.Date.Add(t);
                        break;
                    case WordType.LOCACTION:
                        rawDocument.Location.Add(t);
                        break;
                    case WordType.MONEY:
                        rawDocument.Money.Add(t);
                        break;
                    case WordType.ORGANIZATION:
                        rawDocument.Organization.Add(t);
                        break;
                    case WordType.PERSON:
                        rawDocument.Person.Add(t);
                        break;
                }
                tList[i] = t;
            }
            this.processedDocument.List = tList;
        }
        /// <summary>
        /// Get the title of the article
        /// </summary>
        /// <returns></returns>
        internal string GetTitle()
        {
            return rawDocument.Title;
        }
        /// <summary>
        /// Get the time when the article is published
        /// </summary>
        /// <returns></returns>
        internal string GetRawTime()
        {
            return rawDocument.DocTime;
        }
        /// <summary>
        /// Check if the article mention a person
        /// </summary>
        /// <param name="people"></param>
        /// <returns></returns>
        internal bool HasPeople(string people)
        {
            return false;
        }


        /// <summary>
        /// Check if the document content a word
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        internal bool HasWord(string content)
        {
            bool isContainContent = rawDocument.IsContainContent(content);       
            return isContainContent;
        }

        /// <summary>
        /// Check if the article mentions a location
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        internal bool HasLocation(string location)
        {
            return false;
        }
        /// <summary>
        /// Check if the article mentions an organization
        /// </summary>
        /// <param name="organization"></param>
        /// <returns></returns>
        internal bool HasOrganization(string organization)
        {
            return false;
        }
    }
}
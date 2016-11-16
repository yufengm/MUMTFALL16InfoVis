using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Windows.Storage;

namespace CoLocatedCardSystem.CollaborationWindow.DocumentModule
{
    class DocumentController
    {
        DocumentList list=new DocumentList();
        CentralControllers controllers;
        string[][] topicList = new string[][] {
            new string[] { "one" , "get" , "stay" ,"day", "night","desk" , "go", "front","us","park" },
            new string[] { "place" , "bed" , "stay" , "clean" , "area" , "one" , "great" , "breakfast" , "like" , "nice" },
            new string[] { "breakfast" , "stay" , "free" , "clean" , "like" , "park" , "good" , "well" , "disney" , "day" },
            new string[] { "stay" ,"nice","clean" ,"area","night" ,"place","pool" ,"day" ,"one" ,"get"},
            new string[] { "look" ,"stay" ,"time" , "see", "like" , "around" , "bed" , "us", "clean", "one" }
        };
        Token[][] topicTokenList;

        internal Token[][] TopicTokenList
        {
            get
            {
                return topicTokenList;
            }

            set
            {
                topicTokenList = value;
            }
        }

        public DocumentController(CentralControllers ctrls) {
            this.controllers = ctrls;
        }
        /// <summary>
        /// Initialize documents from jsonFile
        /// </summary>
        /// <param name="jsonFilePath"></param>
        public async Task Init(String jsonFilePath)
        {
            StorageFolder assetsFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            StorageFile file = await assetsFolder.GetFileAsync(jsonFilePath);
            using (var inputStream = await file.OpenReadAsync())
            using (var classicStream = inputStream.AsStreamForRead())
            using (var streamReader = new StreamReader(classicStream))
            {
                while (streamReader.Peek() >= 0)
                {
                    string line = streamReader.ReadLine();
                    Document doc = new Document();
                    doc.Deserialize(line);
                    list.AddDocument(doc);
                }
            }
            topicTokenList = new Token[topicList.Length][];
            for (int i = 0; i < topicList.Length; i++) {
                topicTokenList[i] = new Token[topicList[i].Length];
                for (int j = 0; j < topicList[i].Length; j++) {
                    topicTokenList[i][j] = new Token();
                    topicTokenList[i][j].OriginalWord = topicList[i][j];
                    topicTokenList[i][j].Process();
                }
            }
        }

        /// <summary>
        /// Deinit the document module
        /// </summary>
        internal void Deinit() {
            list.Clear();
        }
        /// <summary>
        /// Get the document by ID
        /// </summary>
        /// <param name="docID"></param>
        /// <returns></returns>
        internal Document GetDocument(string docID) {
            return list.GetDocument(docID);
        }
        /// <summary>
        /// Get all documents
        /// </summary>
        /// <returns></returns>
        internal Document[] GetDocument() {
            return list.GetDocument();
        }      
    }
}

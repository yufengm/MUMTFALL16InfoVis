using CoLocatedCardSystem.CollaborationWindow.InteractionModule;
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
        DocumentList list = new DocumentList();
        CentralControllers controllers;
        ImageList imageList = new ImageList();

        internal ImageList ImageList
        {
            get
            {
                return imageList;
            }

            set
            {
                imageList = value;
            }
        }

        public DocumentController(CentralControllers ctrls)
        {
            this.controllers = ctrls;
        }
        /// <summary>
        /// Initialize documents from jsonFile
        /// </summary>
        /// <param name="jsonFilePath"></param>
        public async Task Init(string jsonFilePath, string imageFilePath)
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

            assetsFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            file = await assetsFolder.GetFileAsync(imageFilePath);
            using (var inputStream = await file.OpenReadAsync())
            using (var classicStream = inputStream.AsStreamForRead())
            using (var streamReader = new StreamReader(classicStream))
            {
                while (streamReader.Peek() >= 0)
                {
                    string line = streamReader.ReadLine();
                    ImageVector vector = new ImageVector();
                    vector.SetImage(line);
                    imageList.AddImage(vector);
                }
            }
        }

        /// <summary>
        /// Deinit the document module
        /// </summary>
        internal void Deinit()
        {
            list.Clear();
        }

        internal string[] GetDocumentIDs()
        {
            return list.GetDocIDs();
        }

        /// <summary>
        /// Get the document by ID
        /// </summary>
        /// <param name="docID"></param>
        /// <returns></returns>
        internal Document GetDocument(string docID)
        {
            return list.GetDocument(docID);
        }

        internal Document[] GetDocument(string[] docID)
        {
            return list.GetDocument(docID);
        }

        /// <summary>
        /// Get all documents
        /// </summary>
        /// <returns></returns>
        internal Document[] GetDocument()
        {
            return list.GetDocument();
        }
        internal string[] GetKeyImage(SemanticGroup semanticGroup)
        {
            string[] result;
            //List<string> result = new List<string>();
            Dictionary<string, double> imagesShown = new Dictionary<string, double>();

            var tklist = semanticGroup.Topic.GetToken();
            foreach (Token tk in tklist)
            {
                foreach (string docID in semanticGroup.DocList.Keys)
                {
                    Document doc = GetDocument( docID );
                    List<ImageVector> vectors = doc.GetImageVector( this );
                    foreach (ImageVector iv in vectors)
                    {
                        //iv.Id;//jpg name
                        foreach (KeyValuePair<string, double> pair in iv.List)
                        {
                            if( pair.Key.Contains( tk.StemmedWord ) )
                            {
                                imagesShown.Add( iv.Id, pair.Value );
                            }
                            //pair.Key;
                            //pair.Value;
                        }
                    }
                }

            }
            var imageListOrdered = imagesShown.OrderByDescending( e => e.Value ).ToList();
            if ( imageListOrdered.Count > 0 )
            {
                int thr = semanticGroup.DocList.Keys.Count / 10;
                int imagenum;
                // Determine number of images to be shown
                if ( thr > 3 )
                {
                    if( imageListOrdered.Count >= 3 )
                    {
                        imagenum = 3;
                    }
                    else
                    {
                        imagenum = imageListOrdered.Count;
                    }                   
                }
                else
                {
                    if ( imageListOrdered.Count >= 3 )
                    {
                        imagenum = thr;
                    }
                    else
                    {
                        if( thr < imageListOrdered.Count )
                        {
                            imagenum = thr;
                        }
                        else
                        {
                            imagenum = imageListOrdered.Count;
                        }                      
                    }
                }

                result = new string[ imagenum ];
                for (int i = 0; i < imagenum ; i++)
                {
                    result[i] = imageListOrdered[i].Key;
                }
            }
            else
            {
                result = null;
            }
            return result;
        }
        /// <summary>
        /// Find the existed word in documents. 
        /// </summary>
        /// <param name="word"></param>
        /// <param name="documents"></param>
        /// <returns></returns>
        internal Token FindToken(string word, Document[] documents)
        {
            foreach (Document doc in documents)
            {
                Token result = doc.FindToken(word);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }

        internal Task Init(string newsArticle, object imageCSV)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace CoLocatedCardSystem.CollaborationWindow.DocumentModule
{
    class DocumentList
    {
        Dictionary<string, Document> list=new Dictionary<string, Document>();
        /// <summary>
        /// Add a document from a json line
        /// </summary>
        /// <param name="jsonLine"></param>
        internal void AddDocument(Document doc) {
            list.Add(doc.DocID, doc);
        }
        /// <summary>
        /// Remove the document with "docID" from the list
        /// </summary>
        /// <param name="docID"></param>
        internal void RemoveDocument(string docID) {
            //To do
        }
        /// <summary>
        /// Remove all documents
        /// </summary>
        internal void Clear() {
            list.Clear();
        }
        /// <summary>
        /// Find the document by "docID"
        /// </summary>
        /// <param name="docID"></param>
        /// <returns></returns>
        internal Document GetDocument(string docID) {
            //To do
            return null;
        }
        /// <summary>
        /// Get all documents
        /// </summary>
        /// <returns></returns>
        internal Document[] GetDocument() {
            return list.Values.ToArray();
        }
        /// <summary>
        /// Get the serialized json of all the documents
        /// </summary>
        /// <returns></returns>
        internal String[] GetJsonDocument() {
            String[] slist = list.Values.Select(d => d.ToJson()).ToArray();
            return slist;
        }
    }
}

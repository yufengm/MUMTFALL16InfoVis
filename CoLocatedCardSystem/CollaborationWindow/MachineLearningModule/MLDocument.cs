using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.CollaborationWindow.MachineLearningModule
{
    public class MLDocument
    {
        public int[] Words;
        public String RawDocument;
        public int Length;

        public MLDocument()
        {
            Words = null;
            RawDocument = "";
            Length = 0;
        }

        public MLDocument(int length, int[] words)
        {
            Words = new int[words.Length];
            Array.Copy(words, Words, words.Length);
            Length = length;
        }

        public MLDocument(int length, int[] words, string rawDocument)
        {
            Words = new int[words.Length];
            Array.Copy(words, Words, words.Length);
            Length = length;
            RawDocument = rawDocument;
        }

        public MLDocument(IEnumerable<int> ids, string rawDocument)
            : this(ids.Count(), ids.ToArray(), rawDocument)
        {

        }

        public MLDocument(IEnumerable<int> ids)
            : this(ids, "")
        {

        }


    }
}

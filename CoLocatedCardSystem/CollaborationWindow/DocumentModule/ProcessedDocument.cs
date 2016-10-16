using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.CollaborationWindow.DocumentModule
{
    class ProcessedDocument
    {
        Token[] list;

        internal Token[] List
        {
            get
            {
                return list;
            }

            set
            {
                list = value;
            }
        }

        /// <summary>
        /// Initialize the token list with the content. Processed with token makers.
        /// </summary>
        /// <param name="content"></param>
        internal void InitTokens(string content) {
            Regex r = new Regex("([\\s{}():;., \"“”])");
            List<Token> tokenList = new List<Token>();
            string stack = "";
            //Partition the content with the regular expression.
            for (int i = 0; i < content.Length; i++)
            {
                if (r.IsMatch("" + content[i]))//Add a spliter
                {
                    Token tk = new Token();
                    tk.OriginalWord = "" + content[i];
                    tokenList.Add(tk);
                    if (stack.Length > 0)
                    {
                        Token token = new Token();
                        token.OriginalWord = stack;
                        tokenList.Add(token);
                        stack = "";
                    }
                }
                else//Add the letter to the word stack
                {
                    stack += content[i];
                }
            }
            if (stack.Length>0) {
                Token token = new Token();
                token.OriginalWord = stack;
                tokenList.Add(token);
            }
            foreach (Token tk in tokenList) {
                tk.Process();
            }
            list = tokenList.ToArray<Token>();
        }
        /// <summary>
        /// Return the number of tokens whose original word is the same with the key work
        /// </summary>
        /// <param name="key">key word</param>
        /// <returns></returns>
        internal int CountToken(string key) {
            return 0;
        }
        internal string[] GetJsonList() {
            string[] result = list.Select(t => t.ToJson()).ToArray();
            return result;
        }
        /// <summary>
        /// Check if the document contains the keyword.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool IsContainToken(Token newToken)
        {
            foreach (Token tk in List)
            {
                if (tk.EqualContent(newToken)) {
                    return true;
                }
            }
            return false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.CollaborationWindow.DocumentModule
{
    enum WordType
    {
        DEFAULT,
        PUNCTUATION,
        STOPWORD,
        IRREGULAR,
        REGULAR,
        //Line break
        LINEBREAK,
        //Default types
        DATE,
        LOCACTION,
        MONEY,
        ORGANIZATION,
        PERSON,
        TIME
    }
}

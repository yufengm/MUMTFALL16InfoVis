using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.CollaborationWindow.TableModule
{
    public class DataCell
    {
        internal DataAttribute Attribute { get; set; }
        internal String StringData { get; set; }
        private double value;

        internal double Value
        {
            get
            {
                if (Attribute.Type == ATTRIBUTETYPE.Categorical) {
                    value = Attribute.GetCountIndex(StringData);
                }
                return value;
            }

            set
            {
                this.value = value;
            }
        }

    }
}

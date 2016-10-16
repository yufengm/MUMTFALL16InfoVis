using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.CollaborationWindow.TableModule
{
    public class DataRow
    {
        internal Dictionary<DataAttribute, DataCell> cellList;
        internal String UUID { get; set; }
        internal int Index { get; set; }
        internal DataRow()
        {
            cellList = new Dictionary<DataAttribute, DataCell>();
            UUID = Guid.NewGuid().ToString();
        }

        internal void SetAttributes(IEnumerable<DataAttribute> attrs) {
            cellList = new Dictionary<DataAttribute, DataCell>();
            foreach (DataAttribute att in attrs) {
                DataCell cell = new DataCell();
                cell.Attribute = att;
                cellList.Add(att, cell);
            }
        }

        internal void SetCell(DataAttribute attr, string value) {
            if (cellList != null) {
                if (cellList.Keys.Contains(attr)) {
                    cellList[attr].StringData=value;
                }
            }
        }

        internal DataCell GetCell(DataAttribute attr) {
            return cellList[attr];
        }

        internal string GetIndex() {
            return ""+Index;
        }

        internal string GetAll() {
            string result = "";
            foreach (KeyValuePair<DataAttribute, DataCell> pair in cellList) {
                result += "" + pair.Key.Name + ":" + pair.Value.StringData+"\n";
            }
            return result;
        }
    }
}

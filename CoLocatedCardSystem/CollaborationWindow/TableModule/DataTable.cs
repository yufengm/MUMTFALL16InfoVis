using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoLocatedCardSystem.CollaborationWindow.TableModule
{
    public class DataTable
    {
        private Dictionary<string, DataRow> dataRows;//key is the row unique id.
        private Dictionary<string, DataAttribute> attributeList;//key is the attribute name 

        public DataTable()
        {
            Init();
        }

        public void Init()
        {
            dataRows = new Dictionary<string, DataRow>();
            attributeList = new Dictionary<string, DataAttribute>();
        }

        public void DeInit()
        {
            dataRows.Clear();
            dataRows = null;
            attributeList.Clear();
            attributeList = null;
        }

        /// <summary>
        /// Adds an attribute to the dictionary list 
        /// </summary>
        /// <param name="attr"></param>
        public bool addColumn(string attr)
        {
            if (!attributeList.Keys.Contains(attr))
            {
                DataAttribute attribute = new DataAttribute();
                attribute.Type = ATTRIBUTETYPE.None;
                attribute.Name = attr;
                attributeList.Add(attr, attribute);
                return true;
            }
            return false;
        }

        internal IEnumerable<DataCell> GetDataCell(DataAttribute attr)
        {
            List<DataCell> cells = new List<DataCell>();
            foreach (DataRow row in dataRows.Values) {
                cells.Add(row.GetCell(attr));
            }
            return cells;
        }

        internal DataAttribute[] GetAttribute()
        {
            return attributeList.Values.ToArray();
        }

        public bool AddRow(DataRow row)
        {
            int id = dataRows.Count();
            row.Index = id;
            if (row != null)
            {
                dataRows.Add(row.UUID, row);
                return true;
            }
            return false;
        }


        internal void ParseAttribute()
        {
            foreach (DataRow row in dataRows.Values) {
                foreach (DataAttribute attr in attributeList.Values) {
                    DataCell cell = row.GetCell(attr);
                    if (cell.Attribute.Type == ATTRIBUTETYPE.None 
                        || cell.Attribute.Type != ATTRIBUTETYPE.Categorical) {
                        cell.Attribute.Type = AttributeHelper.ParseType(cell.StringData);
                    }
                }
            }
            foreach (DataRow row in dataRows.Values)
            {
                foreach (DataAttribute attr in attributeList.Values)
                {
                    DataCell cell = row.GetCell(attr);
                    if (attr.Type == ATTRIBUTETYPE.Numerical)
                    {
                        double v = 0;
                        double.TryParse(cell.StringData, out v);
                        cell.Value = v;
                        if (v > attr.Max)
                        {
                            attr.Max = v;
                        }
                        if (v < attr.Min)
                        {
                            attr.Min = v;
                        }
                    }
                    else if (attr.Type == ATTRIBUTETYPE.Categorical) {
                        attr.AddCategorical(cell.StringData);
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Windows.Storage;
using System.Text.RegularExpressions;

namespace CoLocatedCardSystem.CollaborationWindow.TableModule
{
    public class TableController
    {
        CentralControllers controllers;
        DataTable dataTable;

        public TableController(CentralControllers ctrls)
        {
            this.controllers = ctrls;
        }

        /// <summary>
        /// Creates itemController and attributeList, and parses through CSV file
        /// </summary>
        internal async Task Init(String csvFile)
        {
            dataTable = new DataTable();      
            await CsvParser(csvFile);
        }
        /// <summary>
        /// Get a list of data cell from attribute
        /// </summary>
        /// <param name="attr"></param>
        /// <returns></returns>
        internal IEnumerable<DataCell> GetValueWithAttribute(DataAttribute attr)
        {
            return dataTable.GetDataCell(attr);
        }

        /// <summary>
        /// Clears all objects and sets objects to null
        /// </summary>
        internal void Deinit()
        {
            dataTable.DeInit();
            dataTable = null;
        }
        /// <summary>
        /// Parses through csv file and adds items to attributeList and itemList
        /// </summary>
        /// <param name="filePath"></param>
        internal async Task CsvParser(String filePath)
        {
            StorageFolder folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            StorageFile file = await folder.GetFileAsync(filePath);
            using (var inputStream = await file.OpenReadAsync())
            using (var classicStream = inputStream.AsStreamForRead())
            using (var sr = new StreamReader(classicStream))
            {
                int row = 0;
                int counter = 0;
                while (!sr.EndOfStream)
                {
                    String[] line = Regex.Split(sr.ReadLine(), ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                    if (row == 0)
                    {
                        foreach (String str in line)
                        {
                            dataTable.addColumn(str);
                        }
                    }
                    else
                    {
                        counter = 0;
                        DataRow dataRow = new DataRow();
                        DataAttribute[] attrs = dataTable.GetAttribute();
                        dataRow.SetAttributes(attrs);
                        foreach (DataAttribute att in attrs) {
                            dataRow.SetCell(att, line[counter]);
                            counter++;
                        }
                        dataTable.AddRow(dataRow);
                    }
                    row++;
                }
            }
            dataTable.ParseAttribute();
        }

        internal DataAttribute[] GetAttribute()
        {
            return dataTable.GetAttribute();
        }
    }
}

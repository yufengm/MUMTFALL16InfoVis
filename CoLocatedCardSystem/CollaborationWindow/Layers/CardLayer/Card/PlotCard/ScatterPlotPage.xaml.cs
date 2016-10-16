using CoLocatedCardSystem.CollaborationWindow.Layers.Card_Layer;
using CoLocatedCardSystem.CollaborationWindow.TableModule;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CoLocatedCardSystem.CollaborationWindow.Layers.CardLayer.Card.PlotCard
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ScatterPlotPage : Page
    {
        public Chart chart;
        public ScatterPlotPage()
        {
            this.InitializeComponent();
            //LoadChartContents();
            //chart = ScatterChart;
        }

        public async void LoadChartContents()
        {
            String CSVFile = FilePath.CSVFile;// @"Assets\data\titanic.csv"; //the path of the csv file
            List<Records> records = new List<Records>();
            TableController tableController = new TableController();
            await tableController.Init(CSVFile);

            Dictionary<String, Item> itemList = tableController.itemList.itemList;

            foreach (Item item in itemList.Values)
            {
                Records rec = new Records();
                String str = item.cellList[tableController.attributeList.attributeList["age"]].data;
                double val;
                if (double.TryParse(str, out val))
                {
                    rec.double1 = val.ToString();
                }

                str = item.cellList[tableController.attributeList.attributeList["fare"]].data;
                if (double.TryParse(str, out val))
                {
                    rec.double2= val.ToString();
                }
                records.Add(rec);

            }

            (ScatterChart.Series[0] as ScatterSeries).ItemsSource = records;
            //ScatterChart.Width = 100;
            //ScatterChart.Height = 100;
        }
    }
}

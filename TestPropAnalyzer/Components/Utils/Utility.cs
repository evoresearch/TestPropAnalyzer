using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestPropAnalyzer.Components.Utils
{
   public class Utility
    {
        public static void DisplayErrorMessage(Exception ex)
        {
            DevExpress.XtraEditors.XtraMessageBox.Show(ex.Message, "TeCap Analyzer", MessageBoxButtons.OK, MessageBoxIcon.Stop);

            // DevExpress.XtraEditors.XtraMessageBox.Show(string.Format("{0}\n{1}",ex.Message,ex.StackTrace), "Gladtidings ESchool", MessageBoxButtons.OK, MessageBoxIcon.Stop);

        }
        public static void DisplayErrorMessage(string message)
        {
            DevExpress.XtraEditors.XtraMessageBox.Show(message, "TeCap Analyzer", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }
        public static void DisplayInfoMessage(string message)
        {
            DevExpress.XtraEditors.XtraMessageBox.Show(message, "TeCap Analyzer", MessageBoxButtons.OK);
        }

        public static string[] GetMatchingGroups()
        {
            return new string[] { "100", "90-100", "80-89", "70-79", "60-69", "50-59", "40-49", "30-39", "20-29", "10-19", "0-9" };
        }
        public static string GetMatchingGroup(double percValue)
        {
            string group = null;
            if (percValue == 100)
            {
                group = "100";
            }
            else if (percValue >= 90)
            {
                group = "90-100";
            }else if (percValue >= 80)
            {
                group = "80-89";
            }else if (percValue >= 70)
            {
                group = "70-79";
            }else if (percValue >= 60)
            {
                group = "60-69";
            }else if (percValue >= 50)
            {
                group = "50-59";
            }else if (percValue >= 40)
            {
                group = "40-49";
            }else if (percValue >= 30)
            {
                group = "30-39";
            }else if (percValue >= 20)
            {
                group = "20-29";
            }else if (percValue >= 10)
            {
                group = "10-19";
            }
            else
            {
                group = "0-9";
            }
            return group;
        }

        public static double GetRating(string rating1,string rating2)
        {
            double rating = 0;
            //if both ratings are empty
            if (string.IsNullOrWhiteSpace(rating1) && string.IsNullOrWhiteSpace(rating2))
            {
                rating = 0;
            }else if (!string.IsNullOrWhiteSpace(rating1) && string.IsNullOrWhiteSpace(rating2)&&Convert.ToInt32(rating2)>=0)
            {
                rating = Convert.ToDouble(rating1.Trim());
            }
            else if (string.IsNullOrWhiteSpace(rating1) && !string.IsNullOrWhiteSpace(rating2) && Convert.ToInt32(rating1) >= 0)
            {
                rating = Convert.ToDouble(rating2.Trim());
            }
            else
            {
                rating = (Convert.ToDouble(rating1.Trim())+Convert.ToDouble(rating2.Trim()))/2;
            }

            return rating;
        }

        public static void ChangeChartType(string chartType, ChartControl chartControl1)
        {
            switch (chartType)
            {

                case "Bar":
                    foreach (SeriesBase series in chartControl1.Series)
                    {
                        series.ChangeView(ViewType.Bar);
                    }
                    break;
                case "Spline":
                    foreach (SeriesBase series in chartControl1.Series)
                    {
                        series.ChangeView(ViewType.Spline);
                    }
                    break;
                case "Line":
                    foreach (SeriesBase series in chartControl1.Series)
                    {
                        series.ChangeView(ViewType.Line);
                    }
                    break;
                case "Step Line":
                    foreach (SeriesBase series in chartControl1.Series)
                    {
                        series.ChangeView(ViewType.StepLine);
                    }
                    break;
                case "Scatter Line":
                    foreach (SeriesBase series in chartControl1.Series)
                    {
                        series.ChangeView(ViewType.ScatterLine);
                    }
                    break;
                case "Swift Plot":
                    foreach (SeriesBase series in chartControl1.Series)
                    {
                        series.ChangeView(ViewType.SwiftPlot);
                    }
                    break;
                case "Doughnut":
                    foreach (SeriesBase series in chartControl1.Series)
                    {
                        series.ChangeView(ViewType.Doughnut);
                    }
                    break;
                case "Funnel":
                    foreach (SeriesBase series in chartControl1.Series)
                    {
                        series.ChangeView(ViewType.Funnel);
                    }
                    break;
                case "Point":
                    foreach (SeriesBase series in chartControl1.Series)
                    {
                        series.ChangeView(ViewType.Point);
                    }
                    break;
                case "Bubble":
                    foreach (SeriesBase series in chartControl1.Series)
                    {
                        series.ChangeView(ViewType.Bubble);
                    }
                    break;
                case "Area":
                    foreach (SeriesBase series in chartControl1.Series)
                    {
                        series.ChangeView(ViewType.Area);
                    }
                    break;
                case "Step Area":
                    foreach (SeriesBase series in chartControl1.Series)
                    {
                        series.ChangeView(ViewType.StepArea);
                    }
                    break;
                case "Spline Area":
                    foreach (SeriesBase series in chartControl1.Series)
                    {
                        series.ChangeView(ViewType.SplineArea);
                    }
                    break;
                case "Range Bar":
                    foreach (SeriesBase series in chartControl1.Series)
                    {
                        series.ChangeView(ViewType.RangeBar);
                    }
                    break;
                case "Range Area":
                    foreach (SeriesBase series in chartControl1.Series)
                    {
                        series.ChangeView(ViewType.RangeArea);
                    }
                    break;
                case "Radar Point":
                    foreach (SeriesBase series in chartControl1.Series)
                    {
                        series.ChangeView(ViewType.PolarPoint);
                    }
                    break;
                case "Candle Stick":
                    foreach (SeriesBase series in chartControl1.Series)
                    {
                        series.ChangeView(ViewType.CandleStick);
                    }
                    break;
                case "Gantt":
                    foreach (SeriesBase series in chartControl1.Series)
                    {
                        series.ChangeView(ViewType.Gantt);
                    }
                    break;
                case "Stock":
                    foreach (SeriesBase series in chartControl1.Series)
                    {
                        series.ChangeView(ViewType.Stock);
                    }
                    break;
                case "Polar Line":
                    foreach (SeriesBase series in chartControl1.Series)
                    {
                        series.ChangeView(ViewType.PolarLine);
                    }
                    break;
                case "Radar Line":
                    foreach (SeriesBase series in chartControl1.Series)
                    {
                        series.ChangeView(ViewType.RadarLine);
                    }
                    break;
                case "Pie":


                    foreach (SeriesBase series1 in chartControl1.Series)
                    {
                        series1.ChangeView(ViewType.Pie);

                        //series1.PointOptions.PointView = PointView.Values;

                        //series1.PointOptions.ValueNumericOptions.Format = NumericFormat.Percent;
                        //series1.PointOptions.ValueNumericOptions.Precision = 0;

                        //    ((PieSeriesLabel)series1.Label).ResolveOverlappingMode = ResolveOverlappingMode.Default;

                        //PieSeriesView pieView = (PieSeriesView)series1.View;

                        ////title
                        //pieView.Titles.Add(new SeriesTitle());
                        //pieView.Titles[0].Text = series1.Name;

                        ////data filter
                        //pieView.ExplodedPointsFilters.Add(new SeriesPointFilter(SeriesPointKey.Value_1, DataFilterCondition.GreaterThanOrEqual, 9));
                        //pieView.ExplodedPointsFilters.Add(new SeriesPointFilter(SeriesPointKey.Argument, DataFilterCondition.NotEqual, "Others"));
                        //pieView.ExplodeMode = PieExplodeMode.UseFilters;
                        //pieView.ExplodedDistancePercentage = 30;
                        //pieView.RuntimeExploding = true;
                        //pieView.HeightToWidthRatio = 99;
                    }
                    break;
                case "Stacked Bar":
                    foreach (SeriesBase series in chartControl1.Series)
                    {
                        series.ChangeView(ViewType.StackedBar);
                        //series.PointOptions.PointView = PointView.
                    }
                    break;
                case "Side By Side Stacked Bar":
                    foreach (SeriesBase series in chartControl1.Series)
                    {
                        series.ChangeView(ViewType.SideBySideStackedBar);
                    }
                    break;
                default:
                    foreach (SeriesBase series in chartControl1.Series)
                    {
                        series.ChangeView(ViewType.Bar);
                    }
                    break;
            }
        }

        public static List<TestCaseUUTPair> GetRandomElements(IEnumerable<TestCaseUUTPair> list, int elementsCount)
        {
            return list.OrderBy(arg => Guid.NewGuid()).Take(elementsCount).ToList();
        }
    }
}

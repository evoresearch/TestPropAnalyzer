using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestPropAnalyzer.Components;
using TestPropAnalyzer.Components.Utils;

namespace TestPropAnalyzer.Controls
{
    public partial class PivotForm : DevExpress.XtraEditors.XtraForm
    {
        public PivotForm(List<TestCaseUUTPair> uutPairs)
        {
            InitializeComponent();
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            pairsDS.DataSource = uutPairs;
            pivotGridControl1.DataSource = pairsDS;
            pivotGridControl1.RefreshData();
        }

        private void btnPrintPivotTable_Click(object sender, EventArgs e)
        {
            try
            {
                pivotGridControl1.ShowPrintPreview();
            }
            catch (Exception ex)
            {
                Utility.DisplayErrorMessage(ex);
            }
        }

        private void btnPrintGraph_Click(object sender, EventArgs e)
        {
            try
            {
                chartControl1.ShowPrintPreview();
            }
            catch (Exception ex)
            {
                Utility.DisplayErrorMessage(ex);
            }
        }

        private void btnPrintPieChart_Click(object sender, EventArgs e)
        {
            try
            {
                chartControl2.ShowPrintPreview();
            }
            catch (Exception ex)
            {
                Utility.DisplayErrorMessage(ex);
            }
        }

        private void ddlPieChartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CreatePieChart();
            }
            catch (Exception ex)
            {
                Utility.DisplayErrorMessage(ex);
            }
        }

        private void CreatePieChart()
        {
            chartControl2.SeriesTemplate.Label.Visible = chkShowLabelsPie.Checked;
            Utility.ChangeChartType(ddlPieChartType.EditValue == null ? "Pie" : Convert.ToString(ddlPieChartType.EditValue), chartControl2);
        }
        private void CreateBarChart()
        {
            chartControl1.SeriesTemplate.Label.Visible = chkShowLabelsBar.Checked;
            Utility.ChangeChartType(Convert.ToString(ddlBarChartType.EditValue), chartControl1);
        }
        private void chkShowLabelsPie_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CreatePieChart();
            }
            catch (Exception ex)
            {
                Utility.DisplayErrorMessage(ex);
            }
        }

        private void ddlBarChartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CreateBarChart();
            }
            catch (Exception ex)
            {
                Utility.DisplayErrorMessage(ex);
            }
        }

        private void chkShowLabelsBar_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CreateBarChart();
            }
            catch (Exception ex)
            {
                Utility.DisplayErrorMessage(ex);
            }
        }

        private void pivotGridControl1_Click(object sender, EventArgs e)
        {
            try
            {
                CreatePieChart();
                CreateBarChart();
            }
            catch (Exception ex)
            {
                Utility.DisplayErrorMessage(ex);
            }
        }
    }
}
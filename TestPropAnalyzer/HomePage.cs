using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
//using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Alphaleonis.Win32.Filesystem;
using DevExpress.CodeParser;
using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Layout;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.XtraRichEdit.Services;
using DiffPlex.WindowsForms.Controls;
using TestPropAnalyzer.Components;
using TestPropAnalyzer.Components.Utils;
using TestPropAnalyzer.Controls;

namespace TestPropAnalyzer
{
    public partial class HomePage : DevExpress.XtraEditors.XtraForm
    {
        public List<TestCaseUUTPair> UUTPairs { get; set; }
        public List<TestCaseUUTPair> TestCaseUUTPairs { get; set; }
        public List<TestCaseUUTPair> SampledUUTPairs { get; set; }
        public List<TestCaseUUTPair> AutoSampledUUTPairs { get; set; }
        public string SourceFileText { get; set; }
        public string TargetFileText { get; set; }
        public int ChangeCount { get; set; }
        public string DataFilePath { get; set; }
        public string SourceMappedFile { get; set; }
        public string SourceUnMappedFile { get; set; }
        public string TargetUnMappedFile { get; set; }
        public GridView ClickedGrid { get; set; }
        public HomePage()
        {
            try
            {
                InitializeComponent();
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
                UUTPairs = new List<TestCaseUUTPair>();


            }
            catch (Exception ex)
            {
                Utility.DisplayErrorMessage(ex);
            }

        }


        public void SetEditorProperties(RichEditControl richEditControl, string path)
        {
            richEditControl.ReplaceService<ISyntaxHighlightService>(new MySyntaxHighlightService(richEditControl));
            richEditControl.LoadDocument(path);
            richEditControl.Views.SimpleView.Padding = new DevExpress.Portable.PortablePadding(60, 4, 4, 0); //new System.Windows.Forms.Padding(60, 4, 4, 0);
            richEditControl.Views.DraftView.Padding = new DevExpress.Portable.PortablePadding(60, 4, 4, 0);
            richEditControl.Views.SimpleView.AllowDisplayLineNumbers = true;
            richEditControl.Views.DraftView.AllowDisplayLineNumbers = true;

            richEditControl.Document.Sections[0].LineNumbering.Start = 1;
            richEditControl.Document.Sections[0].LineNumbering.CountBy = 1;
            richEditControl.Document.Sections[0].LineNumbering.Distance = 25f;
            richEditControl.Document.Sections[0].LineNumbering.RestartType = DevExpress.XtraRichEdit.API.Native.LineNumberingRestart.Continuous;

            richEditControl.Document.CharacterStyles["Line Number"].FontName = "Courier";
            richEditControl.Document.CharacterStyles["Line Number"].FontSize = 10;
            richEditControl.Document.CharacterStyles["Line Number"].ForeColor = Color.DarkGray;
            richEditControl.Document.CharacterStyles["Line Number"].Bold = true;
        }

        private void btnLoadPairs_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {

                DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
                String pairsFIle = null;

                if (result == DialogResult.OK)
                {

                    pairsFIle = openFileDialog1.FileName;
                    DataFilePath = pairsFIle;
                    LoadPairs(pairsFIle, UUTPairs, gcAllPairs, gridViewAllPairs, pairsDS, true);
                }



            }
            catch (Exception ex)
            {

                Utility.DisplayErrorMessage(ex);
            }
        }
        private void LoadPairs(string fileName, List<TestCaseUUTPair> pairsList, GridControl grid, GridView gridView, BindingSource ds, bool all)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            pairsList = new List<TestCaseUUTPair>();

            string[] lines = File.ReadAllLines(fileName);
            for (int i = 1; i < lines.Length; i++)//skip header
            {
                string[] p = lines[i].Split(';');
                TestCaseUUTPair pair = new TestCaseUUTPair
                {
                    EcosystemID = Convert.ToInt32(p[0]),
                    Ecosystem = p[1],
                    SourceUUTProject = p[2],
                    TestCaseSourceFilePath = p[3],
                    TestcaseFullMethodName = p[4],
                    TestCaseMethodName = p[5],
                    SourceUUTFilePath = p[6],
                    SourceUUTFullMethodName = p[7],
                    SourceUUTMethodName = p[8],
                    SourceUUTStart = Convert.ToInt32(p[9]),
                    SourceUUTEnd = Convert.ToInt32(p[10]),
                    SourceCloneStart = Convert.ToInt32(p[11]),
                    SourceCloneEnd = Convert.ToInt32(p[12]),
                    TargetUUTProject = p[13],
                    TargetUUTFilePath = p[14],
                    TargetMethodFullName = p[15],
                    TargetMethodName = p[16],
                    TargetMethodStart = Convert.ToInt32(p[17]),
                    TargetMethodEnd = Convert.ToInt32(p[18]),
                    TargetCloneStart = Convert.ToInt32(p[19]),
                    TargetCloneEnd = Convert.ToInt32(p[20]),
                    IntersetStart = Convert.ToInt32(p[21]),
                    IntersectEnd = Convert.ToInt32(p[22]),
                    Fingerprint = p[23],
                    Editscript = Convert.ToInt32(p[24]),
                    SourceTokens = Convert.ToInt32(p[25]),
                    SourceMatched = Convert.ToInt32(p[26]),
                    SourceUnmatched = Convert.ToInt32(p[27]),
                    SourceMatchedPercent = Convert.ToDouble(p[28]),
                    SourceUnMatchedPercent = Convert.ToDouble(p[29]),
                    TargetTokens = Convert.ToInt32(p[30]),
                    TargetMatched = Convert.ToInt32(p[31]),
                    TargetUnmatched = Convert.ToInt32(p[32]),
                    TargetMatchedPercent = Convert.ToDouble(p[33]),
                    TargetUnMatchedPercent = Convert.ToDouble(p[34]),
                    SourceMappingsFile = p[35],
                    SourceUnmappedPairsFile = p[36],
                    TargetUnmappedPairsFile = p[37],
                    PairdID = i,
                    PairCount = 1
                };
                if (p.Length > 46)
                {
                    //additonal columns for raters

                    pair.Rating1 = string.IsNullOrWhiteSpace(p[42].Trim()) ? -1 : Convert.ToInt32(p[42].Trim());
                    pair.Rating2 = string.IsNullOrWhiteSpace(p[43].Trim()) ? -1 : Convert.ToInt32(p[43].Trim());
                    pair.Rating = Utility.GetRating(p[42], p[43]);//44
                    pair.Rater1Comment = p[45].Trim();
                    pair.Rater2Comment = p[46].Trim();
                    pair.Rater1CloneType = string.IsNullOrWhiteSpace(p[47].Trim()) ? -1 : Convert.ToInt32(p[47].Trim());
                    pair.Rater2CloneType = string.IsNullOrWhiteSpace(p[48].Trim()) ? -1 : Convert.ToInt32(p[48].Trim());
                    pair.CloneType = (int)Math.Round(Utility.GetRating(p[47], p[48]));//49
                    pair.AcceptStatus = p[50].Trim();
                }
                pairsList.Add(pair);
            }

            ds.DataSource = pairsList;
            grid.DataSource = pairsList;
            gridView.RefreshData();
            grid.RefreshDataSource();
            grid.UseEmbeddedNavigator = true;
            if (all)
            {
                UUTPairs = pairsList;
            }
            else
            {
                SampledUUTPairs = pairsList;

            }
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {

        }

        private void gridControl1_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridViewAllPairs.FocusedRowHandle < 0)
                {
                    return;
                }
                string SourceUUTFilePath = Convert.ToString(gridViewAllPairs.GetRowCellValue(gridViewAllPairs.FocusedRowHandle, "SourceUUTFilePath"));
                uint sourceUUTStart = Convert.ToUInt32(gridViewAllPairs.GetRowCellValue(gridViewAllPairs.FocusedRowHandle, "SourceUUTStart"));
                string TargetUUTFilePath = Convert.ToString(gridViewAllPairs.GetRowCellValue(gridViewAllPairs.FocusedRowHandle, "TargetUUTFilePath"));
                string testFullName = Convert.ToString(gridViewAllPairs.GetRowCellValue(gridViewAllPairs.FocusedRowHandle, "TestcaseFullMethodName"));
                string sourceProject = Convert.ToString(gridViewAllPairs.GetRowCellValue(gridViewAllPairs.FocusedRowHandle, "SourceUUTProject"));
                string targetProject = Convert.ToString(gridViewAllPairs.GetRowCellValue(gridViewAllPairs.FocusedRowHandle, "TargetUUTProject"));
                uint targetStart = Convert.ToUInt32(gridViewAllPairs.GetRowCellValue(gridViewAllPairs.FocusedRowHandle, "TargetMethodStart"));
                SourceUnMappedFile = Convert.ToString(gridViewAllPairs.GetRowCellValue(gridViewAllPairs.FocusedRowHandle, "SourceUnmappedPairsFile"));
                TargetUnMappedFile = Convert.ToString(gridViewAllPairs.GetRowCellValue(gridViewAllPairs.FocusedRowHandle, "TargetUnmappedPairsFile"));
                SourceMappedFile = Convert.ToString(gridViewAllPairs.GetRowCellValue(gridView2.FocusedRowHandle, "SourceMappingsFile"));
                //test case

                setTestCaseContent(gridViewAllPairs);
                //get rating values
                setRatings(gridViewAllPairs);
                SourceFileText = File.ReadAllText(SourceUUTFilePath);
                TargetFileText = File.ReadAllText(TargetUUTFilePath);

                if (!string.IsNullOrWhiteSpace(SourceUUTFilePath))
                {
                    //set diffs
                    var diffView = new DiffViewer
                    {
                        Margin = new Padding(0),
                        Dock = DockStyle.Fill,
                        OldText = SourceFileText,
                        NewText = TargetFileText,
                        OldTextHeader = SourceUUTFilePath,
                        NewTextHeader = TargetUUTFilePath
                    };
                    sccMain.Panel1.Controls.Clear();
                    sccMain.Panel1.Controls.Add(diffView);

                    
                    string[] sourceLines = rblSOurceTokens.SelectedIndex == 1 ? File.ReadAllLines(SourceMappedFile) : File.ReadAllLines(SourceUnMappedFile);
                    string[] targetLines = File.ReadAllLines(TargetUnMappedFile);
                    lstSourceUnMatched.DataSource = sourceLines.Distinct();
                    lstTargetUnMatched.DataSource = targetLines.Distinct();
                    //load sub table to show details for all pairs of the test case to which this pair belongs

                    TestCaseUUTPairs = UUTPairs.Where(u => u.SourceUUTProject.Trim() == sourceProject.Trim() && u.TestcaseFullMethodName.Trim() == testFullName.Trim() && u.TargetUUTProject.Trim() == targetProject.Trim()).ToList();
                    pairDS.DataSource = TestCaseUUTPairs;
                    gcTestCasePairs.DataSource = TestCaseUUTPairs;
                    gridView2.RefreshData();
                    gcTestCasePairs.RefreshDataSource();
                    gcTestCasePairs.UseEmbeddedNavigator = true;
                    //SetEditorProperties(sourceEditControl, SourceUUTFilePath);
                    //ResetFocusLine(sourceUUTStart, sourceEditControl);

                    //label1.Text = SourceUUTFilePath;
                    //SetEditorProperties(targetEditControl, TargetUUTFilePath);
                    //label2.Text = TargetUUTFilePath;
                    //ResetFocusLine(targetStart, targetEditControl);
                }

            }
            catch (Exception ex)
            {
                Utility.DisplayErrorMessage(ex);
            }
        }

        private void ResetFocusLine(uint lineNum, RichEditControl richEditControl)
        {
            var richEdit = richEditControl;
            DocumentRange range = null; ;
            while (!richEdit.DocumentLayout.IsDocumentFormattingCompleted)
            {
                //wait
            }
            if (richEdit.DocumentLayout.IsDocumentFormattingCompleted)
            {
                var visitor = new CustomLayoutVisitor(lineNum, richEditControl.Document);
                for (int i = 0; i < richEdit.DocumentLayout.GetPageCount(); i++)
                {
                    LayoutPage page = richEdit.DocumentLayout.GetPage(i);
                    visitor.Visit(page);
                    if (visitor.IsFound)
                    {
                        range = visitor.LineRange;
                        break;
                    }
                }
            }
            if (range != null)
            {
                richEdit.Document.CaretPosition = range.Start;
                richEditControl.ScrollToCaret();
            }
        }

        private void gridControl2_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridView2.FocusedRowHandle < 0)
                {
                    return;
                }
                string SourceUUTFilePath = Convert.ToString(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "SourceUUTFilePath"));
                uint sourceUUTStart = Convert.ToUInt32(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "SourceUUTStart"));
                string TargetUUTFilePath = Convert.ToString(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "TargetUUTFilePath"));
                string testFullName = Convert.ToString(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "TestcaseFullMethodName"));
                string sourceProject = Convert.ToString(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "SourceUUTProject"));
                string targetProject = Convert.ToString(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "TargetUUTProject"));
                uint targetStart = Convert.ToUInt32(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "TargetMethodStart"));

                SourceUnMappedFile = Convert.ToString(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "SourceUnmappedPairsFile"));
                TargetUnMappedFile = Convert.ToString(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "TargetUnmappedPairsFile"));
                SourceMappedFile = Convert.ToString(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "SourceMappingsFile"));
                //test case

                setTestCaseContent(gridViewSamples);

                setRatings(gridView2);
                SourceFileText = File.ReadAllText(SourceUUTFilePath);
                TargetFileText = File.ReadAllText(TargetUUTFilePath);
                if (!string.IsNullOrWhiteSpace(SourceUUTFilePath))
                {
                    //set diffs
                    var diffView = new DiffViewer
                    {
                        Margin = new Padding(0),
                        Dock = DockStyle.Fill,
                        OldText = SourceFileText,
                        NewText = TargetFileText,
                        OldTextHeader = SourceUUTFilePath,
                        NewTextHeader = TargetUUTFilePath
                    };
                    sccMain.Panel1.Controls.Clear();
                    sccMain.Panel1.Controls.Add(diffView);

                    
                    string[] sourceLines = rblSOurceTokens.SelectedIndex == 1 ? File.ReadAllLines(SourceMappedFile) : File.ReadAllLines(SourceUnMappedFile);
                    string[] targetLines = File.ReadAllLines(TargetUnMappedFile);
                    lstSourceUnMatched.DataSource = sourceLines.Distinct();
                    lstTargetUnMatched.DataSource = targetLines.Distinct();

                    //txtSOurceUnMatched.Text = File.ReadAllText(sourceUnMatchedPath);
                    //txtTargetUnMatched.Text = File.ReadAllText(targetUnMatchedPath);

                    //get specific charcaters missing
                }

            }
            catch (Exception ex)
            {
                Utility.DisplayErrorMessage(ex);
            }
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lstSourceUnMatched_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //get selected line
                string selectedLine = Convert.ToString(lstSourceUnMatched.SelectedValue);
                if (!string.IsNullOrWhiteSpace(selectedLine))
                {
                    SetSelectedText(selectedLine, txtSOurceUnMatched, SourceFileText);
                }

            }
            catch (Exception ex)
            {
                Utility.DisplayErrorMessage(ex);
            }
        }

        private void SetSelectedText(string selectedLine, RichEditControl txtViewer, string testToSearch)
        {
            if (string.IsNullOrWhiteSpace(selectedLine))
            {
                return;
            }
            string[] parts = selectedLine.Split(new string[] { "==>" }, StringSplitOptions.None);
            string firstHalf = parts[0].Trim();
            int startIndex = -1;
            for (int i = firstHalf.Length - 1; i >= 0; i--)
            {
                if (firstHalf[i] == '[')
                {
                    startIndex = i;
                    break;//weve found our start index
                }

            }
            string positions = firstHalf.Substring(startIndex).Replace("[", "").Replace("]", "");
            int start = Convert.ToInt32(positions.Split(',')[0].Trim());
            int end = Convert.ToInt32(positions.Split(',')[1].Trim());
            //now get test at the positions
            txtViewer.Text = testToSearch.Substring(start, end - start);
        }

        private void lstTargetUnMatched_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //get selected line
                string selectedLine = Convert.ToString(lstTargetUnMatched.SelectedValue);
                SetSelectedText(selectedLine, txtTargetUnMatched, TargetFileText);
            }
            catch (Exception ex)
            {
                Utility.DisplayErrorMessage(ex);
            }
        }

        private void setRatings(GridView gridView)
        {
            int applicability = 0;
            int cloneType = 0;
            string comment = null;
            string acceptPair = null;
            acceptPair = Convert.ToString(gridView.GetRowCellValue(gridView.FocusedRowHandle, "AcceptStatus"));
            if (rblRater.SelectedIndex == 1)
            {
                applicability = Convert.ToInt32(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Rating2"));
                cloneType = Convert.ToInt32(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Rater2CloneType"));
                comment = Convert.ToString(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Rater2Comment"));

            }
            else
            {//rater 1
                applicability = Convert.ToInt32(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Rating1"));
                cloneType = Convert.ToInt32(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Rater1CloneType"));
                comment = Convert.ToString(gridView.GetRowCellValue(gridView.FocusedRowHandle, "Rater1Comment"));

            }
            rblApplicability.EditValue = applicability;
            rblCloneType.EditValue = cloneType;
            txtComment.EditValue = comment;
            rblAcceptPair.EditValue = acceptPair;
        }
        private void btnSaveRating_Click(object sender, EventArgs e)
        {
            try
            {
                if (rblRater.EditValue == null)
                {
                    throw new Exception("Please select whether you're rater 1 or 2");
                }
                if (rblApplicability.EditValue == null)
                {
                    throw new Exception("Please rate the applicability of the this test case based on all UUT pairs listed in table 2 above");
                }
                if (rblCloneType.EditValue == null)
                {
                    throw new Exception("Please select the clone type for the pair selected in table 2 above");
                }
                int applicability = Convert.ToInt32(rblApplicability.EditValue);
                int cloneType = Convert.ToInt32(rblCloneType.EditValue);
                string comment = Convert.ToString(txtComment.Text);
                string acceptPair = Convert.ToString(rblAcceptPair.EditValue);

                if (string.IsNullOrWhiteSpace(txtComment.Text) && (applicability == 0 || applicability == 1))
                {
                    throw new Exception("Please comment on why the test case is not applicable or applicable with modification");
                }
                int PairdID = Convert.ToInt32(gridViewSamples.GetRowCellValue(gridViewSamples.FocusedRowHandle, "PairdID"));

                //save
                //find record that matches selected row
                TestCaseUUTPair pair = SampledUUTPairs.Where(p => p.PairdID == PairdID).FirstOrDefault();
                pair.AcceptStatus = acceptPair;
                if (rblRater.SelectedIndex == 0)
                {//rater 1
                    pair.Rating1 = applicability;
                    pair.Rater1Comment = comment;
                    pair.Rater1CloneType = cloneType;

                }
                else
                {
                    pair.Rating2 = applicability;
                    pair.Rater2Comment = comment;
                    pair.Rater2CloneType = cloneType;

                }

                ChangeCount++;
            }
            catch (Exception ex)
            {
                Utility.DisplayErrorMessage(ex);
            }
        }



        private void HomePage_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveAllData();
        }

        private void SaveAllData()
        {
            try
            {
                //save data before closing
                if (ChangeCount > 0)
                {
                    string dataFilePath = DataFilePath.Replace("_UPDATED", "").Replace(".csv", "_UPDATED.csv");
                    if (File.Exists(dataFilePath))
                    {
                        File.Delete(dataFilePath);
                    }
                    System.IO.StreamWriter w = File.AppendText(dataFilePath);
                    w.WriteLine("ecosystemID;ecosystem;sourceUUTProject;testCaseSourceFilePath;testcaseFullMethodName;testCaseMethodName;sourceUUTFilePath;sourceUUTFullMethodName;sourceUUTMethodName;sourceUUTStart;sourceUUTEnd;sourceCloneStart;sourceCloneEnd;targetUUTProject;targetUUTFilePath;targetMethodFullName;targetMethodName;targetMethodStart;targetMethodEnd;targetCloneStart;targetCloneEnd;intersetStart;intersectEnd;fingerprint;editscript;sourceTokens;sourceMatched;sourceUnmatched;sourceMatchedPercent;sourceUnMatchedPercent;targetTokens;targetMatched;targetUnmatched;targetMatchedPercent;targetUnMatchedPercent;sourceMappingsFile;sourceUnmappedPairsFile;targetUnmappedPairsFile;SourceMatchingGroup;SourceUnMatchingGroup;TargetMatchingGroup;TargetUnMatchingGroup;Rating1;Rating2;Rating;Rater1Comment;Rater2Comment;Rater1CloneType;Rater2CloneType;CloneType;AcceptStatus");
                    foreach (TestCaseUUTPair p in SampledUUTPairs)
                    {
                        w.WriteLine($"{p.EcosystemID};{p.Ecosystem};{p.SourceUUTProject};{p.TestCaseSourceFilePath};{p.TestcaseFullMethodName};{p.TestCaseMethodName};{p.SourceUUTFilePath};{p.SourceUUTFullMethodName};{p.SourceUUTMethodName};{p.SourceUUTStart};{p.SourceUUTEnd};{p.SourceCloneStart};{p.SourceCloneEnd};{p.TargetUUTProject};{p.TargetUUTFilePath};{p.TargetMethodFullName};{p.TargetMethodName};{p.TargetMethodStart};{p.TargetMethodEnd};{p.TargetCloneStart};{p.TargetCloneEnd};{p.IntersetStart};{p.IntersectEnd};{p.Fingerprint};{p.Editscript};{p.SourceTokens};{p.SourceMatched};{p.SourceUnmatched};{p.SourceMatchedPercent};{p.SourceUnMatchedPercent};{p.TargetTokens};{p.TargetMatched};{p.TargetUnmatched};{p.TargetMatchedPercent};{p.TargetUnMatchedPercent};{p.SourceMappingsFile};{p.SourceUnmappedPairsFile};{p.TargetUnmappedPairsFile};{p.SourceMatchingGroup};{p.SourceUnMatchingGroup};{p.TargetMatchingGroup};{p.TargetUnMatchingGroup};{p.Rating1};{p.Rating2};{p.Rating};{p.Rater1Comment};{p.Rater2Comment};{p.Rater1CloneType};{p.Rater2CloneType};{p.CloneType};{p.AcceptStatus}");
                        w.Flush();

                    }

                    w.Close();


                }
            }
            catch (Exception ex)
            {
                Utility.DisplayErrorMessage(ex);
            }
        }
        private void lbtnPivotGrid_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (UUTPairs == null || UUTPairs.Count <= 0)
                {
                    throw new Exception("Please load data first");
                }
                PivotForm form = new PivotForm(UUTPairs);
                form.Show(this);
            }
            catch (Exception ex)
            {
                Utility.DisplayErrorMessage(ex);
            }
        }

        private void btnSamplePairs_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (UUTPairs == null || UUTPairs.Count <= 0)
                {
                    throw new Exception("Please load data first");
                }
                SampledUUTPairs = new List<TestCaseUUTPair>();

                int sample100 = Convert.ToInt32(btnSample100.EditValue);
                int sampleBelow = Convert.ToInt32(txtSamplePercentage.EditValue);
                //sample 100%
                string[] groups = Utility.GetMatchingGroups();
                int sampleSize = 0;
                foreach (string group in groups)
                {
                    if (group == "100")
                    {
                        sampleSize = sample100;
                    }
                    else
                    {
                        sampleSize = sampleBelow;
                    }
                    List<TestCaseUUTPair> list = UUTPairs.Where(p => p.SourceMatchingGroup.Trim() == group.Trim()).ToList();
                    List<TestCaseUUTPair> sampleList = Utility.GetRandomElements(list, sampleSize);
                    SampledUUTPairs.AddRange(sampleList);
                }

                //bind datasource
                sampleDS.DataSource = SampledUUTPairs;
                gcSamples.DataSource = SampledUUTPairs;
                gridViewSamples.RefreshData();
                gcSamples.RefreshDataSource();
                gcSamples.UseEmbeddedNavigator = true;

                //save the samples
                string datafilePath = DataFilePath;
                datafilePath = datafilePath.Replace(".csv", "_SAMPLED.csv");
                if (File.Exists(datafilePath))
                {
                    File.Delete(datafilePath);
                }
                System.IO.StreamWriter w = File.AppendText(datafilePath);
                w.WriteLine("ecosystemID;ecosystem;sourceUUTProject;testCaseSourceFilePath;testcaseFullMethodName;testCaseMethodName;sourceUUTFilePath;sourceUUTFullMethodName;sourceUUTMethodName;sourceUUTStart;sourceUUTEnd;sourceCloneStart;sourceCloneEnd;targetUUTProject;targetUUTFilePath;targetMethodFullName;targetMethodName;targetMethodStart;targetMethodEnd;targetCloneStart;targetCloneEnd;intersetStart;intersectEnd;fingerprint;editscript;sourceTokens;sourceMatched;sourceUnmatched;sourceMatchedPercent;sourceUnMatchedPercent;targetTokens;targetMatched;targetUnmatched;targetMatchedPercent;targetUnMatchedPercent;sourceMappingsFile;sourceUnmappedPairsFile;targetUnmappedPairsFile;SourceMatchingGroup;SourceUnMatchingGroup;TargetMatchingGroup;TargetUnMatchingGroup;Rating1;Rating2;Rating;Rater1Comment;Rater2Comment;Rater1CloneType;Rater2CloneType;CloneType;AcceptStatus");
                foreach (TestCaseUUTPair p in SampledUUTPairs)
                {
                    w.WriteLine($"{p.EcosystemID};{p.Ecosystem};{p.SourceUUTProject};{p.TestCaseSourceFilePath};{p.TestcaseFullMethodName};{p.TestCaseMethodName};{p.SourceUUTFilePath};{p.SourceUUTFullMethodName};{p.SourceUUTMethodName};{p.SourceUUTStart};{p.SourceUUTEnd};{p.SourceCloneStart};{p.SourceCloneEnd};{p.TargetUUTProject};{p.TargetUUTFilePath};{p.TargetMethodFullName};{p.TargetMethodName};{p.TargetMethodStart};{p.TargetMethodEnd};{p.TargetCloneStart};{p.TargetCloneEnd};{p.IntersetStart};{p.IntersectEnd};{p.Fingerprint};{p.Editscript};{p.SourceTokens};{p.SourceMatched};{p.SourceUnmatched};{p.SourceMatchedPercent};{p.SourceUnMatchedPercent};{p.TargetTokens};{p.TargetMatched};{p.TargetUnmatched};{p.TargetMatchedPercent};{p.TargetUnMatchedPercent};{p.SourceMappingsFile};{p.SourceUnmappedPairsFile};{p.TargetUnmappedPairsFile};{p.SourceMatchingGroup};{p.SourceUnMatchingGroup};{p.TargetMatchingGroup};{p.TargetUnMatchingGroup};{p.Rating1};{p.Rating2};{p.Rating};{p.Rater1Comment};{p.Rater2Comment};{p.Rater1CloneType};{p.Rater2CloneType};{p.CloneType};{p.AcceptStatus}");
                    w.Flush();

                }

                w.Close();

            }
            catch (Exception ex)
            {
                Utility.DisplayErrorMessage(ex);
            }
        }

        private void setTestCaseContent(GridView gridView)
        {
            string TestCaseFilePath = Convert.ToString(gridView.GetRowCellValue(gridView.FocusedRowHandle, "TestCaseSourceFilePath"));
            string testCaseMethodName = Convert.ToString(gridView.GetRowCellValue(gridView.FocusedRowHandle, "TestCaseMethodName"));
            SetEditorProperties(txtTestCaseContent, TestCaseFilePath);
            lblTestCase.Text = testCaseMethodName;
        }
        private void gcSamples_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridViewSamples.FocusedRowHandle < 0)
                {
                    return;
                }
                string SourceUUTFilePath = Convert.ToString(gridViewSamples.GetRowCellValue(gridViewSamples.FocusedRowHandle, "SourceUUTFilePath"));

                uint sourceUUTStart = Convert.ToUInt32(gridViewSamples.GetRowCellValue(gridViewSamples.FocusedRowHandle, "SourceUUTStart"));
                string TargetUUTFilePath = Convert.ToString(gridViewSamples.GetRowCellValue(gridViewSamples.FocusedRowHandle, "TargetUUTFilePath"));
                string testFullName = Convert.ToString(gridViewSamples.GetRowCellValue(gridViewSamples.FocusedRowHandle, "TestcaseFullMethodName"));
                string sourceProject = Convert.ToString(gridViewSamples.GetRowCellValue(gridViewSamples.FocusedRowHandle, "SourceUUTProject"));
                string targetProject = Convert.ToString(gridViewSamples.GetRowCellValue(gridViewSamples.FocusedRowHandle, "TargetUUTProject"));
                uint targetStart = Convert.ToUInt32(gridViewSamples.GetRowCellValue(gridViewSamples.FocusedRowHandle, "TargetMethodStart"));
                SourceUnMappedFile = Convert.ToString(gridViewSamples.GetRowCellValue(gridViewSamples.FocusedRowHandle, "SourceUnmappedPairsFile"));
                TargetUnMappedFile = Convert.ToString(gridViewSamples.GetRowCellValue(gridViewSamples.FocusedRowHandle, "TargetUnmappedPairsFile"));
                SourceMappedFile = Convert.ToString(gridViewSamples.GetRowCellValue(gridViewSamples.FocusedRowHandle, "SourceMappingsFile"));
                setRatings(gridViewSamples);
                setTestCaseContent(gridViewSamples);
                SourceFileText = File.ReadAllText(SourceUUTFilePath);
                TargetFileText = File.ReadAllText(TargetUUTFilePath);

                if (!string.IsNullOrWhiteSpace(SourceUUTFilePath))
                {
                    //set diffs
                    var diffView = new DiffViewer
                    {
                        Margin = new Padding(0),
                        Dock = DockStyle.Fill,
                        OldText = SourceFileText,
                        NewText = TargetFileText,
                        OldTextHeader = SourceUUTFilePath,
                        NewTextHeader = TargetUUTFilePath
                    };
                    sccMain.Panel1.Controls.Clear();
                    sccMain.Panel1.Controls.Add(diffView);

                    
                    string[] sourceLines = rblSOurceTokens.SelectedIndex == 1 ? File.ReadAllLines(SourceMappedFile) : File.ReadAllLines(SourceUnMappedFile);
                    string[] targetLines = File.ReadAllLines(TargetUnMappedFile);
                    lstSourceUnMatched.DataSource = sourceLines.Distinct();
                    lstTargetUnMatched.DataSource = targetLines.Distinct();
                    //load sub table to show details for all pairs of the test case to which this pair belongs

                    TestCaseUUTPairs = UUTPairs.Where(u => u.SourceUUTProject.Trim() == sourceProject.Trim() && u.TestcaseFullMethodName.Trim() == testFullName.Trim() && u.TargetUUTProject.Trim() == targetProject.Trim()).ToList();
                    pairDS.DataSource = TestCaseUUTPairs;
                    gcTestCasePairs.DataSource = TestCaseUUTPairs;
                    gridView2.RefreshData();
                    gcTestCasePairs.RefreshDataSource();
                    gcTestCasePairs.UseEmbeddedNavigator = true;

                }

            }
            catch (Exception ex)
            {
                Utility.DisplayErrorMessage(ex);
            }
        }

        private void btnImportSamples_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {

                DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
                String pairsFIle = null;

                if (result == DialogResult.OK)
                {

                    pairsFIle = openFileDialog1.FileName;
                    DataFilePath = pairsFIle;
                    LoadPairs(pairsFIle, SampledUUTPairs, gcSamples, gridViewSamples, sampleDS, false);
                }



            }
            catch (Exception ex)
            {

                Utility.DisplayErrorMessage(ex);
            }
        }

        private void lbtnAUtoSample_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (UUTPairs == null || UUTPairs.Count <= 0)
                {
                    throw new Exception("Please load data first");
                }
                AutoSampledUUTPairs = new List<TestCaseUUTPair>();
                string samplesizes = txtAutoSample.EditValue.ToString();
                int s100 = Convert.ToInt32(samplesizes.Split(',')[0]);
                int sOther = Convert.ToInt32(samplesizes.Split(',')[1]);

                //sample 100%
                string[] groups = Utility.GetMatchingGroups();
                int sampleSize = 0;
                //get unique ecosystems
                List<string> ecosystems = (from eco in UUTPairs
                                           select eco.Ecosystem).Distinct().ToList();
                foreach (string ecosystem in ecosystems)
                {
                    foreach (string group in groups)
                    {
                        if (group == "100")
                        {
                            sampleSize = s100;
                        }
                        else
                        {
                            sampleSize = sOther;
                        }
                        List<TestCaseUUTPair> list = UUTPairs.Where(p => p.SourceMatchingGroup.Trim() == group.Trim() && p.Ecosystem.Trim() == ecosystem.Trim()).ToList();
                        int lastIndex = list.Count - 1;
                        int sampleIndex = -1;
                        if (lastIndex >= 0)
                        {
                            Random random = new Random();
                            for (int p = 0; p < sampleSize; p++)
                            {
                                sampleIndex = random.Next(0, lastIndex);
                                AutoSampledUUTPairs.Add(list[sampleIndex]);
                            }
                        }

                    }
                }

                //bind datasource
                autoSampleDS.DataSource = AutoSampledUUTPairs;
                gcAutoSampled.DataSource = AutoSampledUUTPairs;
                gridViewAutoSampled.RefreshData();
                gcAutoSampled.RefreshDataSource();
                gcAutoSampled.UseEmbeddedNavigator = true;

                //save the samples
                string datafilePath = DataFilePath;
                datafilePath = datafilePath.Replace(".csv", "_AUTOSAMPLED.csv");
                if (File.Exists(datafilePath))
                {
                    File.Delete(datafilePath);
                }
                System.IO.StreamWriter w = File.AppendText(datafilePath);
                w.WriteLine("ecosystemID;ecosystem;sourceUUTProject;testCaseSourceFilePath;testcaseFullMethodName;testCaseMethodName;sourceUUTFilePath;sourceUUTFullMethodName;sourceUUTMethodName;sourceUUTStart;sourceUUTEnd;sourceCloneStart;sourceCloneEnd;targetUUTProject;targetUUTFilePath;targetMethodFullName;targetMethodName;targetMethodStart;targetMethodEnd;targetCloneStart;targetCloneEnd;intersetStart;intersectEnd;fingerprint;editscript;sourceTokens;sourceMatched;sourceUnmatched;sourceMatchedPercent;sourceUnMatchedPercent;targetTokens;targetMatched;targetUnmatched;targetMatchedPercent;targetUnMatchedPercent;sourceMappingsFile;sourceUnmappedPairsFile;targetUnmappedPairsFile;SourceMatchingGroup;SourceUnMatchingGroup;TargetMatchingGroup;TargetUnMatchingGroup;Rating1;Rating2;Rating;Rater1Comment;Rater2Comment;Rater1CloneType;Rater2CloneType;CloneType;AcceptStatus");
                foreach (TestCaseUUTPair p in AutoSampledUUTPairs)
                {
                    w.WriteLine($"{p.EcosystemID};{p.Ecosystem};{p.SourceUUTProject};{p.TestCaseSourceFilePath};{p.TestcaseFullMethodName};{p.TestCaseMethodName};{p.SourceUUTFilePath};{p.SourceUUTFullMethodName};{p.SourceUUTMethodName};{p.SourceUUTStart};{p.SourceUUTEnd};{p.SourceCloneStart};{p.SourceCloneEnd};{p.TargetUUTProject};{p.TargetUUTFilePath};{p.TargetMethodFullName};{p.TargetMethodName};{p.TargetMethodStart};{p.TargetMethodEnd};{p.TargetCloneStart};{p.TargetCloneEnd};{p.IntersetStart};{p.IntersectEnd};{p.Fingerprint};{p.Editscript};{p.SourceTokens};{p.SourceMatched};{p.SourceUnmatched};{p.SourceMatchedPercent};{p.SourceUnMatchedPercent};{p.TargetTokens};{p.TargetMatched};{p.TargetUnmatched};{p.TargetMatchedPercent};{p.TargetUnMatchedPercent};{p.SourceMappingsFile};{p.SourceUnmappedPairsFile};{p.TargetUnmappedPairsFile};{p.SourceMatchingGroup};{p.SourceUnMatchingGroup};{p.TargetMatchingGroup};{p.TargetUnMatchingGroup};{p.Rating1};{p.Rating2};{p.Rating};{p.Rater1Comment};{p.Rater2Comment};{p.Rater1CloneType};{p.Rater2CloneType};{p.CloneType};{p.AcceptStatus}");
                    w.Flush();

                }

                w.Close();

            }
            catch (Exception ex)
            {
                Utility.DisplayErrorMessage(ex);
            }
        }

        private void gcAutoSampled_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridViewAutoSampled.FocusedRowHandle < 0)
                {
                    return;
                }
                string SourceUUTFilePath = Convert.ToString(gridViewAutoSampled.GetRowCellValue(gridViewAutoSampled.FocusedRowHandle, "SourceUUTFilePath"));
                uint sourceUUTStart = Convert.ToUInt32(gridViewAutoSampled.GetRowCellValue(gridViewAutoSampled.FocusedRowHandle, "SourceUUTStart"));
                string TargetUUTFilePath = Convert.ToString(gridViewAutoSampled.GetRowCellValue(gridViewAutoSampled.FocusedRowHandle, "TargetUUTFilePath"));
                string testFullName = Convert.ToString(gridViewAutoSampled.GetRowCellValue(gridViewAutoSampled.FocusedRowHandle, "TestcaseFullMethodName"));
                string sourceProject = Convert.ToString(gridViewAutoSampled.GetRowCellValue(gridViewAutoSampled.FocusedRowHandle, "SourceUUTProject"));
                string targetProject = Convert.ToString(gridViewAutoSampled.GetRowCellValue(gridViewAutoSampled.FocusedRowHandle, "TargetUUTProject"));
                uint targetStart = Convert.ToUInt32(gridViewAutoSampled.GetRowCellValue(gridViewAutoSampled.FocusedRowHandle, "TargetMethodStart"));
                SourceUnMappedFile= Convert.ToString(gridViewAutoSampled.GetRowCellValue(gridViewAutoSampled.FocusedRowHandle, "SourceUnmappedPairsFile"));
                TargetUnMappedFile = Convert.ToString(gridViewAutoSampled.GetRowCellValue(gridViewAutoSampled.FocusedRowHandle, "TargetUnmappedPairsFile"));
                SourceFileText = File.ReadAllText(SourceUUTFilePath);
                TargetFileText = File.ReadAllText(TargetUUTFilePath);

                if (!string.IsNullOrWhiteSpace(SourceUUTFilePath))
                {
                    //set diffs
                    var diffView = new DiffViewer
                    {
                        Margin = new Padding(0),
                        Dock = DockStyle.Fill,
                        OldText = SourceFileText,
                        NewText = TargetFileText,
                        OldTextHeader = SourceUUTFilePath,
                        NewTextHeader = TargetUUTFilePath
                    };
                    sccMain.Panel1.Controls.Clear();
                    sccMain.Panel1.Controls.Add(diffView);

                    SourceMappedFile = Convert.ToString(gridViewAutoSampled.GetRowCellValue(gridViewAutoSampled.FocusedRowHandle, "SourceMappingsFile"));
                    string[] sourceLines = rblSOurceTokens.SelectedIndex == 1 ? File.ReadAllLines(SourceMappedFile) : File.ReadAllLines(SourceUnMappedFile);
                    string[] targetLines = File.ReadAllLines(TargetUnMappedFile);
                    lstSourceUnMatched.DataSource = sourceLines.Distinct();
                    lstTargetUnMatched.DataSource = targetLines.Distinct();
                    //load sub table to show details for all pairs of the test case to which this pair belongs

                    TestCaseUUTPairs = UUTPairs.Where(u => u.SourceUUTProject.Trim() == sourceProject.Trim() && u.TestcaseFullMethodName.Trim() == testFullName.Trim() && u.TargetUUTProject.Trim() == targetProject.Trim()).ToList();
                    pairDS.DataSource = TestCaseUUTPairs;
                    gcTestCasePairs.DataSource = TestCaseUUTPairs;
                    gridView2.RefreshData();
                    gcTestCasePairs.RefreshDataSource();
                    gcTestCasePairs.UseEmbeddedNavigator = true;

                }

            }
            catch (Exception ex)
            {
                Utility.DisplayErrorMessage(ex);
            }
        }

        private void btnSaveAll_Click(object sender, EventArgs e)
        {
            SaveAllData();
        }

        private void rblSOurceTokens_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                
                string[] sourceLines = rblSOurceTokens.SelectedIndex == 1 ? File.ReadAllLines(SourceMappedFile) : File.ReadAllLines(SourceUnMappedFile);
                string[] targetLines = File.ReadAllLines(TargetUnMappedFile);
                lstSourceUnMatched.DataSource = sourceLines.Distinct();
                lstTargetUnMatched.DataSource = targetLines.Distinct();
            }
            catch (Exception ex)
            {
                Utility.DisplayErrorMessage(ex);
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {

              
                    gcAllPairs.ShowRibbonPrintPreview();
               
            }
            catch (Exception ex)
            {
                Utility.DisplayErrorMessage(ex);
            }
        }
    }


    public class CustomLayoutVisitor : LayoutVisitor
    {
        string lineNum;
        bool found = false;
        DocumentRange range;
        Document document;
        public bool IsFound { get { return found; } }
        public DocumentRange LineRange { get { return range; } }
        public CustomLayoutVisitor(uint lineNum, Document document)
        {
            this.lineNum = lineNum.ToString();
            this.document = document;
        }
        protected override void VisitLineNumberBox(LineNumberBox lineNumberBox)
        {
            if (lineNumberBox.Text == lineNum)
            {
                found = true;
                FixedRange fRange = lineNumberBox.Row.Range;
                range = document.CreateRange(fRange.Start, fRange.Length);
            }
            base.VisitLineNumberBox(lineNumberBox);
        }
    }


    /// <summary>
    ///  This class implements the Execute method of the ISyntaxHighlightService interface to parse and colorize text.
    /// </summary>
    public class MySyntaxHighlightService : ISyntaxHighlightService
    {
        readonly RichEditControl syntaxEditor;
        SyntaxColors syntaxColors;
        SyntaxHighlightProperties commentProperties;
        SyntaxHighlightProperties keywordProperties;
        SyntaxHighlightProperties stringProperties;
        SyntaxHighlightProperties xmlCommentProperties;
        SyntaxHighlightProperties textProperties;

        public MySyntaxHighlightService(RichEditControl syntaxEditor)
        {
            this.syntaxEditor = syntaxEditor;
            syntaxColors = new SyntaxColors(UserLookAndFeel.Default);
        }

        void HighlightSyntax(TokenCollection tokens)
        {
            commentProperties = new SyntaxHighlightProperties();
            commentProperties.ForeColor = syntaxColors.CommentColor;

            keywordProperties = new SyntaxHighlightProperties();
            keywordProperties.ForeColor = syntaxColors.KeywordColor;

            stringProperties = new SyntaxHighlightProperties();
            stringProperties.ForeColor = syntaxColors.StringColor;

            xmlCommentProperties = new SyntaxHighlightProperties();
            xmlCommentProperties.ForeColor = syntaxColors.XmlCommentColor;

            textProperties = new SyntaxHighlightProperties();
            textProperties.ForeColor = syntaxColors.TextColor;

            Document document = syntaxEditor.Document;

            List<SyntaxHighlightToken> syntaxTokens = new List<SyntaxHighlightToken>(tokens.Count);
            foreach (Token token in tokens)
            {
                var categorizedToken = token as CategorizedToken;
                if (categorizedToken != null)
                    HighlightCategorizedToken(categorizedToken, syntaxTokens);
            }
            if (syntaxTokens.Count > 0)
            {
                document.ApplySyntaxHighlight(syntaxTokens);
            }
        }
        void HighlightCategorizedToken(CategorizedToken token, List<SyntaxHighlightToken> syntaxTokens)
        {
            Color backColor = syntaxEditor.ActiveView.BackColor;
            TokenCategory category = token.Category;
            switch (category)
            {
                case TokenCategory.Comment:
                    syntaxTokens.Add(SetTokenColor(token, commentProperties, backColor));
                    break;
                case TokenCategory.Keyword:
                    syntaxTokens.Add(SetTokenColor(token, keywordProperties, backColor));
                    break;
                case TokenCategory.String:
                    syntaxTokens.Add(SetTokenColor(token, stringProperties, backColor));
                    break;
                case TokenCategory.XmlComment:
                    syntaxTokens.Add(SetTokenColor(token, xmlCommentProperties, backColor));
                    break;
                default:
                    syntaxTokens.Add(SetTokenColor(token, textProperties, backColor));
                    break;
            }
        }
        SyntaxHighlightToken SetTokenColor(Token token, SyntaxHighlightProperties foreColor, Color backColor)
        {
            if (syntaxEditor.Document.Paragraphs.Count < token.Range.Start.Line)
                return null;
            int paragraphStart = syntaxEditor.Document.Paragraphs[token.Range.Start.Line - 1].Range.Start.ToInt();
            int tokenStart = paragraphStart + token.Range.Start.Offset - 1;
            if (token.Range.End.Line != token.Range.Start.Line)
                paragraphStart = syntaxEditor.Document.Paragraphs[token.Range.End.Line - 1].Range.Start.ToInt();

            int tokenEnd = paragraphStart + token.Range.End.Offset - 1;
            Debug.Assert(tokenEnd > tokenStart);
            return new SyntaxHighlightToken(tokenStart, tokenEnd - tokenStart, foreColor);
        }

        #region #ISyntaxHighlightServiceMembers
        public void Execute()
        {
            string newText = syntaxEditor.Text;
            // Determine the language by file extension.
            string ext = System.IO.Path.GetExtension(syntaxEditor.Options.DocumentSaveOptions.CurrentFileName);
            ParserLanguageID lang_ID = ParserLanguage.FromString("Csharp");// ParserLanguage.FromFileExtension(ext);
            // Do not parse HTML or XML.
            if (lang_ID == ParserLanguageID.Html ||
                lang_ID == ParserLanguageID.Xml ||
                lang_ID == ParserLanguageID.None) return;
            // Use DevExpress.CodeParser to parse text into tokens.
            ITokenCategoryHelper tokenHelper = TokenCategoryHelperFactory.CreateHelper(lang_ID);
            if (tokenHelper != null)
            {
                TokenCollection highlightTokens = tokenHelper.GetTokens(newText);
                if (highlightTokens != null && highlightTokens.Count > 0)
                {
                    HighlightSyntax(highlightTokens);
                }

            }
        }

        public void ForceExecute()
        {
            Execute();
        }
        #endregion #ISyntaxHighlightServiceMembers
    }
    /// <summary>
    ///  This class defines colors to highlight tokens.
    /// </summary>
    public class SyntaxColors
    {
        static Color DefaultCommentColor { get { return Color.Green; } }
        static Color DefaultKeywordColor { get { return Color.Blue; } }
        static Color DefaultStringColor { get { return Color.Brown; } }
        static Color DefaultXmlCommentColor { get { return Color.Gray; } }
        static Color DefaultTextColor { get { return Color.Black; } }
        UserLookAndFeel lookAndFeel;

        public Color CommentColor { get { return GetCommonColorByName(CommonSkins.SkinInformationColor, DefaultCommentColor); } }
        public Color KeywordColor { get { return GetCommonColorByName(CommonSkins.SkinQuestionColor, DefaultKeywordColor); } }
        public Color TextColor { get { return GetCommonColorByName(CommonColors.WindowText, DefaultTextColor); } }
        public Color XmlCommentColor { get { return GetCommonColorByName(CommonColors.DisabledText, DefaultXmlCommentColor); } }
        public Color StringColor { get { return GetCommonColorByName(CommonSkins.SkinWarningColor, DefaultStringColor); } }

        public SyntaxColors(UserLookAndFeel lookAndFeel)
        {
            this.lookAndFeel = lookAndFeel;
        }

        Color GetCommonColorByName(string colorName, Color defaultColor)
        {
            Skin skin = CommonSkins.GetSkin(lookAndFeel);
            if (skin == null)
                return defaultColor;
            return skin.Colors[colorName];
        }
    }
}

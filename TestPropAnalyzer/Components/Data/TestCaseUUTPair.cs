using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestPropAnalyzer.Components.Utils;

namespace TestPropAnalyzer.Components
{
    public class TestCaseUUTPair
    {
        private int ecosystemID, sourceUUTStart,
   sourceUUTEnd,
   sourceCloneStart,
   sourceCloneEnd, targetMethodStart,
    targetMethodEnd,
    targetCloneStart,
    targetCloneEnd,
    intersetStart,
    intersectEnd, editscript,
    sourceTokens,
    sourceMatched,
    sourceUnmatched, targetTokens,
    targetMatched,
    targetUnmatched,rating1,rating2,pairdID,rater1CloneType,rater2CloneType, cloneType,pairCount;
        private string ecosystem,
      sourceUUTProject,
      testCaseSourceFilePath,
      testcaseFullMethodName,
      testCaseMethodName,
      sourceUUTFilePath,
      sourceUUTFullMethodName,
      sourceUUTMethodName,

      targetUUTProject,
      targetUUTFilePath,
      targetMethodFullName,
      targetMethodName,

      fingerprint,


      sourceMappingsFile,
      sourceUnmappedPairsFile,
      targetUnmappedPairsFile,sourceMatchingGroup,sourceUnMatchingGroup,targetMatchingGroup,targetUnMatchingGroup,rater1Comment,rater2Comment,acceptStatus;
        private double sourceMatchedPercent,
sourceUnMatchedPercent,

targetMatchedPercent,
targetUnMatchedPercent,rating;

        public int EcosystemID { get => ecosystemID; set => ecosystemID = value; }
        public int SourceUUTStart { get => sourceUUTStart; set => sourceUUTStart = value; }
        public int SourceUUTEnd { get => sourceUUTEnd; set => sourceUUTEnd = value; }
        public int SourceCloneStart { get => sourceCloneStart; set => sourceCloneStart = value; }
        public int SourceCloneEnd { get => sourceCloneEnd; set => sourceCloneEnd = value; }
        public int TargetMethodStart { get => targetMethodStart; set => targetMethodStart = value; }
        public int TargetMethodEnd { get => targetMethodEnd; set => targetMethodEnd = value; }
        public int TargetCloneStart { get => targetCloneStart; set => targetCloneStart = value; }
        public int TargetCloneEnd { get => targetCloneEnd; set => targetCloneEnd = value; }
        public int IntersetStart { get => intersetStart; set => intersetStart = value; }
        public int IntersectEnd { get => intersectEnd; set => intersectEnd = value; }
        public int Editscript { get => editscript; set => editscript = value; }
        public int SourceTokens { get => sourceTokens; set => sourceTokens = value; }
        public int SourceMatched { get => sourceMatched; set => sourceMatched = value; }
        public int SourceUnmatched { get => sourceUnmatched; set => sourceUnmatched = value; }
        public int TargetTokens { get => targetTokens; set => targetTokens = value; }
        public int TargetMatched { get => targetMatched; set => targetMatched = value; }
        public int TargetUnmatched { get => targetUnmatched; set => targetUnmatched = value; }
        public string Ecosystem { get => ecosystem; set => ecosystem = value; }
        public string SourceUUTProject { get => sourceUUTProject; set => sourceUUTProject = value; }
        public string TestCaseSourceFilePath { get => testCaseSourceFilePath; set => testCaseSourceFilePath = value; }
        public string TestcaseFullMethodName { get => testcaseFullMethodName; set => testcaseFullMethodName = value; }
        public string TestCaseMethodName { get => testCaseMethodName; set => testCaseMethodName = value; }
        public string SourceUUTFilePath { get => sourceUUTFilePath; set => sourceUUTFilePath = value; }
        public string SourceUUTFullMethodName { get => sourceUUTFullMethodName; set => sourceUUTFullMethodName = value; }
        public string SourceUUTMethodName { get => sourceUUTMethodName; set => sourceUUTMethodName = value; }
        public string TargetUUTProject { get => targetUUTProject; set => targetUUTProject = value; }
        public string TargetUUTFilePath { get => targetUUTFilePath; set => targetUUTFilePath = value; }
        public string TargetMethodFullName { get => targetMethodFullName; set => targetMethodFullName = value; }
        public string TargetMethodName { get => targetMethodName; set => targetMethodName = value; }
        public string Fingerprint { get => fingerprint; set => fingerprint = value; }
        public string SourceMappingsFile { get => sourceMappingsFile; set => sourceMappingsFile = value; }
        public string SourceUnmappedPairsFile { get => sourceUnmappedPairsFile; set => sourceUnmappedPairsFile = value; }
        public string TargetUnmappedPairsFile { get => targetUnmappedPairsFile; set => targetUnmappedPairsFile = value; }
        public double SourceMatchedPercent { get => sourceMatchedPercent; set => sourceMatchedPercent = value; }
        public double SourceUnMatchedPercent { get => sourceUnMatchedPercent; set => sourceUnMatchedPercent = value; }
        public double TargetMatchedPercent { get => targetMatchedPercent; set => targetMatchedPercent = value; }
        public double TargetUnMatchedPercent { get => targetUnMatchedPercent; set => targetUnMatchedPercent = value; }
        public string SourceMatchingGroup { get => Utility.GetMatchingGroup(SourceMatchedPercent); set => sourceMatchingGroup = value; }
        public string SourceUnMatchingGroup { get => Utility.GetMatchingGroup(SourceUnMatchedPercent); set => sourceUnMatchingGroup = value; }
        public string TargetMatchingGroup { get => Utility.GetMatchingGroup(TargetMatchedPercent); set => targetMatchingGroup = value; }
        public string TargetUnMatchingGroup { get => Utility.GetMatchingGroup(TargetUnMatchedPercent); set => targetUnMatchingGroup = value; }
        public int Rating1 { get => rating1; set => rating1 = value; }
        public int Rating2 { get => rating2; set => rating2 = value; }
        public double Rating { get => rating; set => rating = value; }
        public string Rater1Comment { get => rater1Comment; set => rater1Comment = value; }
        public string Rater2Comment { get => rater2Comment; set => rater2Comment = value; }
        public string AcceptStatus { get => acceptStatus; set => acceptStatus = value; }
       
        public int PairdID { get => pairdID; set => pairdID = value; }
        public int Rater1CloneType { get => rater1CloneType; set => rater1CloneType = value; }
        public int Rater2CloneType { get => rater2CloneType; set => rater2CloneType = value; }
        public int CloneType { get => cloneType; set => cloneType = value; }
        public int PairCount { get => pairCount; set => pairCount = value; }
    }
}

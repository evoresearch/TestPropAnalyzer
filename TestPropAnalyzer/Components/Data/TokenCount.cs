using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPropAnalyzer.Components.Data
{
   public class TokenCount
    {
        private int ecosystemID , SourceTokens ,
sourceMatched ,
sourceUnMatched ,targetTokens ,
targetMatched ,
targetUnMatched ;
        private string
ecosystem,
testCaseFullName,
sourceProject,
sourceClass,
sourceMethodName,
targetProject,
targetClass,
targetMethodName,
tokenType;

            private double
sourceMatchedPerc,
sourceUnMatchedPerc,

targetMatchedPerc,
targetUnMatchedPerc;

        public int EcosystemID { get => ecosystemID; set => ecosystemID = value; }
        public int SourceTokens1 { get => SourceTokens; set => SourceTokens = value; }
        public int SourceMatched { get => sourceMatched; set => sourceMatched = value; }
        public int SourceUnMatched { get => sourceUnMatched; set => sourceUnMatched = value; }
        public int TargetTokens { get => targetTokens; set => targetTokens = value; }
        public int TargetMatched { get => targetMatched; set => targetMatched = value; }
        public int TargetUnMatched { get => targetUnMatched; set => targetUnMatched = value; }
        public string Ecosystem { get => ecosystem; set => ecosystem = value; }
        public string TestCaseFullName { get => testCaseFullName; set => testCaseFullName = value; }
        public string SourceProject { get => sourceProject; set => sourceProject = value; }
        public string SourceClass { get => sourceClass; set => sourceClass = value; }
        public string SourceMethodName { get => sourceMethodName; set => sourceMethodName = value; }
        public string TargetProject { get => targetProject; set => targetProject = value; }
        public string TargetClass { get => targetClass; set => targetClass = value; }
        public string TargetMethodName { get => targetMethodName; set => targetMethodName = value; }
        public string TokenType { get => tokenType; set => tokenType = value; }
        public double SourceMatchedPerc { get => sourceMatchedPerc; set => sourceMatchedPerc = value; }
        public double SourceUnMatchedPerc { get => sourceUnMatchedPerc; set => sourceUnMatchedPerc = value; }
        public double TargetMatchedPerc { get => targetMatchedPerc; set => targetMatchedPerc = value; }
        public double TargetUnMatchedPerc { get => targetUnMatchedPerc; set => targetUnMatchedPerc = value; }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MyVote.Repository
{
    [GeneratedCode("DbContext 1.0.2.0", "EF 4.3.1")]
    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
    public partial class MVReportedPollStateOption
    {
        [GeneratedCode("DbContext 1.0.2.0", "EF 4.3.1")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
        public MVReportedPollStateOption()
        {
            this.MVReportedPolls = new HashSet<MVReportedPoll>();
        }
    
        public int ReportedPollStateOptionID { get; set; }
        public string ReportedPollStateName { get; set; }
        public string ReportedPollStateComments { get; set; }
    
        public virtual ICollection<MVReportedPoll> MVReportedPolls { get; set; }
    }
    
}
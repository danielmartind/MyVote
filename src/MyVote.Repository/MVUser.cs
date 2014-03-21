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
    public partial class MVUser
    {
        [GeneratedCode("DbContext 1.0.2.0", "EF 4.3.1")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly")]
        public MVUser()
        {
            this.MVPolls = new HashSet<MVPoll>();
            this.MVPollComments = new HashSet<MVPollComment>();
            this.MVPollResponses = new HashSet<MVPollResponse>();
            this.MVPollSubmissions = new HashSet<MVPollSubmission>();
            this.MVReportedPolls = new HashSet<MVReportedPoll>();
            this.MVReportedPolls1 = new HashSet<MVReportedPoll>();
            this.MVReportedPollStateLogs = new HashSet<MVReportedPollStateLog>();
        }
    
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string ProfileID { get; set; }
        public string ProfileAuthToken { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string PostalCode { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public Nullable<int> UserRoleID { get; set; }
        public Nullable<System.DateTime> AuditCreateDate { get; set; }
        public Nullable<System.DateTime> AuditModifyDate { get; set; }
    
        public virtual ICollection<MVPoll> MVPolls { get; set; }
        public virtual ICollection<MVPollComment> MVPollComments { get; set; }
        public virtual ICollection<MVPollResponse> MVPollResponses { get; set; }
        public virtual ICollection<MVPollSubmission> MVPollSubmissions { get; set; }
        public virtual ICollection<MVReportedPoll> MVReportedPolls { get; set; }
        public virtual ICollection<MVReportedPoll> MVReportedPolls1 { get; set; }
        public virtual ICollection<MVReportedPollStateLog> MVReportedPollStateLogs { get; set; }
        public virtual MVUserRole MVUserRole { get; set; }
    }
    
}
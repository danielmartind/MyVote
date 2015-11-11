﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Csla;
using MyVote.BusinessObjects.Attributes;
using MyVote.BusinessObjects.Contracts;
using MyVote.BusinessObjects.Core;

namespace MyVote.BusinessObjects
{
	[System.Serializable]
	internal sealed class PollSubmissionCommand
		: CommandBaseScopeCore<PollSubmissionCommand>, IPollSubmissionCommand
	{

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [RunLocal]
        private void DataPortal_Create() { }

#if !NETFX_CORE && !MOBILE
        protected override void DataPortal_Execute()
		{
			var submissionExists = (from s in this.Entities.MVPollSubmissions
											where (s.UserID == this.UserID &&
											s.PollID == this.PollID)
											select s.PollSubmissionID).Any();

			if (!submissionExists)
			{
				this.Submission = this.Factory.Create(
					new PollSubmissionCriteria(this.PollID, this.UserID));
			}
		}
#endif

		public static PropertyInfo<int> PollIDProperty =
			PollSubmissionCommand.RegisterProperty<int>(_ => _.PollID);
		public int PollID
		{
			get { return this.ReadProperty(PollSubmissionCommand.PollIDProperty); }
			set { this.LoadProperty(PollSubmissionCommand.PollIDProperty, value); }
		}

		public static PropertyInfo<int> UserIDProperty =
			PollSubmissionCommand.RegisterProperty<int>(_ => _.UserID);
		public int UserID
		{
			get { return this.ReadProperty(PollSubmissionCommand.UserIDProperty); }
			set { this.LoadProperty(PollSubmissionCommand.UserIDProperty, value); }
		}

		public static PropertyInfo<IPollSubmission> SubmissionProperty =
			PollSubmissionCommand.RegisterProperty<IPollSubmission>(_ => _.Submission);
		public IPollSubmission Submission
		{
			get { return this.ReadProperty(PollSubmissionCommand.SubmissionProperty); }
			private set { this.LoadProperty(PollSubmissionCommand.SubmissionProperty, value); }
		}

#if !NETFX_CORE && !MOBILE
        [NonSerialized]
		private IObjectFactory<IPollSubmission> factory;
		[Dependency]
		public IObjectFactory<IPollSubmission> Factory
		{
			get { return this.factory; }
			set { this.factory = value; }
		}
#endif
	}
}

﻿using Csla;
using MyVote.BusinessObjects.Contracts;
using MyVote.BusinessObjects.Core;
using System.Linq;

#if !NETFX_CORE && !WINDOWS_PHONE && !ANDROID && !IOS
using System;
using MyVote.BusinessObjects.Attributes;
#endif

namespace MyVote.BusinessObjects
{
#if (!NETFX_CORE && !WINDOWS_PHONE) || ANDROID || IOS
	[System.Serializable]
#else
	[Csla.Serialization.Serializable]
#endif
	internal sealed class PollResults
		: BusinessBaseScopeCore<PollResults>, IPollResults
	{
#if !NETFX_CORE && !WINDOWS_PHONE && !ANDROID && !IOS
		private void DataPortal_Fetch(PollResultsCriteria criteria)
		{
			using (this.BypassPropertyChecks)
			{
				this.PollID = criteria.PollID;
				this.PollDataResults = this.PollDataResultsFactory.FetchChild(criteria.PollID);
				this.PollComments = this.PollCommentsFactory.FetchChild(criteria.PollID);

				var pollData = (from p in this.Entities.MVPolls
									 where p.PollID == criteria.PollID
									 select new
									 {
										 p.UserID,
										 IsDeleted = (bool)(p.PollDeletedFlag ?? false),
										 p.PollStartDate,
										 p.PollEndDate,
										 p.PollImageLink
									 }).Single();

				this.IsActive = !pollData.IsDeleted && pollData.PollStartDate < DateTime.UtcNow && pollData.PollEndDate > DateTime.UtcNow;
				this.PollImageLink = pollData.PollImageLink;

				if (criteria.UserID != null)
				{
					this.IsPollOwnedByUser = pollData.UserID == criteria.UserID.Value;
				}
			}
		}

		protected override void DataPortal_Update()
		{
			this.FieldManager.UpdateChildren();
		}
#endif // !NETFX_CORE && !WINDOWS_PHONE && !ANDROID && !IOS

		public static PropertyInfo<int> PollIDProperty =
			PollResults.RegisterProperty<int>(_ => _.PollID);
		public int PollID
		{
			get { return this.ReadProperty(PollResults.PollIDProperty); }
			private set { this.LoadProperty(PollResults.PollIDProperty, value); }
		}

		public static PropertyInfo<bool> IsActiveProperty =
			PollResults.RegisterProperty<bool>(_ => _.IsActive);
		public bool IsActive
		{
			get { return this.ReadProperty(PollResults.IsActiveProperty); }
			private set { this.LoadProperty(PollResults.IsActiveProperty, value); }
		}

		public static PropertyInfo<bool> IsPollOwnedByUserProperty =
			PollResults.RegisterProperty<bool>(_ => _.IsPollOwnedByUser);
		public bool IsPollOwnedByUser
		{
			get { return this.ReadProperty(PollResults.IsPollOwnedByUserProperty); }
			private set { this.LoadProperty(PollResults.IsPollOwnedByUserProperty, value); }
		}

		public static PropertyInfo<IPollDataResults> PollDataResultsProperty =
			PollResults.RegisterProperty<IPollDataResults>(_ => _.PollDataResults);
		public IPollDataResults PollDataResults
		{
			get { return this.ReadProperty(PollResults.PollDataResultsProperty); }
			private set { this.LoadProperty(PollResults.PollDataResultsProperty, value); }
		}

		public static PropertyInfo<string> PollImageLinkProperty =
			PollResults.RegisterProperty<string>(_ => _.PollImageLink);
		public string PollImageLink
		{
			get { return this.ReadProperty(PollResults.PollImageLinkProperty); }
			private set { this.LoadProperty(PollResults.PollImageLinkProperty, value); }
		}

		public static PropertyInfo<IPollComments> PollCommentsProperty =
			PollResults.RegisterProperty<IPollComments>(_ => _.PollComments);
		public IPollComments PollComments
		{
			get { return this.ReadProperty(PollResults.PollCommentsProperty); }
			private set { this.LoadProperty(PollResults.PollCommentsProperty, value); }
		}

#if !NETFX_CORE && !WINDOWS_PHONE && !ANDROID && !IOS
		[NonSerialized]
		private IObjectFactory<IPollDataResults> pollDataResultsFactory;
		[Dependency]
		public IObjectFactory<IPollDataResults> PollDataResultsFactory
		{
			get { return this.pollDataResultsFactory; }
			set { this.pollDataResultsFactory = value; }
		}

		[NonSerialized]
		private IObjectFactory<IPollComments> pollCommentsFactory;
		[Dependency]
		public IObjectFactory<IPollComments> PollCommentsFactory
		{
			get { return this.pollCommentsFactory; }
			set { this.pollCommentsFactory = value; }
		}
#endif
	}
}
﻿using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Csla;
using MyVote.BusinessObjects.Contracts;
using MyVote.BusinessObjects.Core;

namespace MyVote.BusinessObjects
{
	[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
	[System.Serializable]
	internal sealed class CategoryCollection
		: ReadOnlyListBaseScopeCore<CategoryCollection, ICategory>, ICategoryCollection
	{
#if !NETFX_CORE && !MOBILE
        protected override void DataPortal_Fetch()
		{
			this.IsReadOnly = false;

			try
			{
				foreach (var category in (from c in this.Entities.MVCategories
												  select c).ToList())
				{
					this.Add(DataPortal.FetchChild<Category>(category));
				}
			}
			finally
			{
				this.IsReadOnly = true;
			}
		}
#endif
	}
}

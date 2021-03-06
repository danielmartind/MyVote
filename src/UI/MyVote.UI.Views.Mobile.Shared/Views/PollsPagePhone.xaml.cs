﻿using System;
using System.Linq;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using MyVote.BusinessObjects;
using MyVote.UI.ViewModels;
using Xamarin.Forms;

namespace MyVote.UI.Views
{
	public partial class PollsPagePhone : TabbedPage
	{
		public PollsPagePhone()
		{
            InitializeComponent();
            var toolbar = this.ToolbarItems.FirstOrDefault();
            if (toolbar != null)
            {
#if ANDROID
                toolbar.Icon = "MoreMenu.png";
#elif IOS
		        toolbar.Text = "...";
#endif
            }
		}

	    protected override void OnBindingContextChanged()
	    {
	        if (Children != null && this.BindingContext != null)
	        {
	            var homePage = Children.Single(c => c.Title == "Home");
	            homePage.PropertyChanged += Child_PropertyChanged;
	            homePage.BindingContext = BindingContext;
	            var categoriesPage = Children.Single(c => c.Title == "Categories");
                categoriesPage.PropertyChanged += Child_PropertyChanged;
                categoriesPage.BindingContext = BindingContext;
                var searchPage = Children.Single(c => c.Title == "Search");
                searchPage.PropertyChanged += Child_PropertyChanged;
                searchPage.BindingContext = BindingContext;
                var addPollPage = Children.Single(c => c.Title == "New Poll");
                addPollPage.PropertyChanged += Child_PropertyChanged;
#if IOS
                SetIconPictures();
#endif
                SetupNewPollViewModel();
	        }
            else if (Children != null)
            {
                var addPollPage = Children.Single(c => c.Title == "New Poll");
                var addPollviewModel = addPollPage.BindingContext as AddPollPageViewModel;
                if (addPollviewModel != null)
                {
                    addPollviewModel.PollAdded -= AddPollviewModel_PollAdded;
                }
            }
	    }

        private void Child_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsBusy")
			{
                this.IsBusy = ((Page)sender).IsBusy;
			}
        }

	    private void SetupNewPollViewModel()
	    {
            var addPollPage = Children.Single(c => c.Title == "New Poll");
            var viewModelLoader = Mvx.Resolve<IMvxViewModelLoader>();
            var addPollviewModel = (AddPollPageViewModel)viewModelLoader.LoadViewModel(new MvxViewModelRequest(typeof(AddPollPageViewModel), new MvxBundle(), new MvxBundle(), new MvxRequestedBy(MvxRequestedByType.UserAction)), null);
            addPollviewModel.PollAdded += AddPollviewModel_PollAdded;
            addPollPage.BindingContext = addPollviewModel;
	    }

	    private void AddPollviewModel_PollAdded(object sender, AddPollEventArgs addPollEventArgs)
	    {
            Device.BeginInvokeOnMainThread(() =>
            {
                var home = Children.Single(c => c.Title == "Home");
                this.SelectedItem = home;
            });
	    }

        public async void ActionsClick(object sender, EventArgs e)
        {
            const string editProfile = "Edit Profile";
            const string logout = "Logout";

            var result = await this.DisplayActionSheet(null, null, null, editProfile, logout);
            var viewModel = ((PollsPageViewModel)this.BindingContext);

            switch (result)
            {
                case editProfile:
                    viewModel.EditProfile.Execute(null);
                    break;
                case logout:
                    viewModel.Logout.Execute(null);
                    break;
            }
        }

	    private void SetIconPictures()
	    {
            var homePage = Children.Single(c => c.Title == "Home");
            var categoriesPage = Children.Single(c => c.Title == "Categories");
            var searchPage = Children.Single(c => c.Title == "Search");
            var addPollPage = Children.Single(c => c.Title == "New Poll");
#if IOS
            homePage.Icon = categoriesPage.IsFocused? "HomeSelected.png": "Home.png";
            categoriesPage.Icon = categoriesPage.IsFocused? "CategoriesSelected.png": "Categories.png";
            searchPage.Icon = searchPage.IsFocused?  "SearchSelected.png" : "Search.png";
            addPollPage.Icon = addPollPage.IsFocused? "NewSelected.png": "New.png";
#endif
        }
	}
}

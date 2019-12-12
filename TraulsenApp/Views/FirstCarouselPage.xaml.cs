
namespace TraulsenApp.Views
{
    using System;
    using System.Collections.Generic;
    using ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FirstCarouselPage : ContentPage
    {
        private FirstCarouselPageViewModel firstCarouselPageViewModel;

        public FirstCarouselPage()
        {
            InitializeComponent();

            firstCarouselPageViewModel = new FirstCarouselPageViewModel();
            BindingContext = firstCarouselPageViewModel;
        }

        #region Page Events
        protected override void OnAppearing()
        {
            base.OnAppearing();
            firstCarouselPageViewModel.OnPageAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            firstCarouselPageViewModel.OnPageDissapearing();
        }
        #endregion
    }
}



namespace TraulsenApp.Views
{
    using System;
    using Xamarin.Forms;
    using System.Collections.Generic;
    using Xamarin.Forms.Xaml;
    using TraulsenApp.ViewModels;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeviceSelectorPage : ContentPage
    {
        private DeviceSelectorViewModel deviceSelectorPageViewModel;
        public DeviceSelectorPage()
        {
            InitializeComponent();

            deviceSelectorPageViewModel = new DeviceSelectorViewModel();
            BindingContext = deviceSelectorPageViewModel;
        }

        public async void ConnectToDevice(object sender, EventArgs e)
        {
            await deviceSelectorPageViewModel.ConnectToDevice();
        }
    }
}

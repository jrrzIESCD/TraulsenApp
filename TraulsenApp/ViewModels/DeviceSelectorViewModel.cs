namespace TraulsenApp.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using TraulsenApp.Views;
    using Xamarin.Forms;
    using XBeeLibrary.Core.Exceptions;
    using XBeeLibrary.Xamarin;

    public class DeviceSelectorViewModel : BaseViewModel
    {
        #region Attributes
        private String deviceMacAddress;
        private String devicePassword;
        #endregion

        #region Properties
        public String DeviceMACAddress
        {
            get { return this.deviceMacAddress; }
            set { SetValue(ref this.deviceMacAddress, value); }
        }

        public String DevicePassword
        {
            get { return this.devicePassword; }
            set { SetValue(ref this.devicePassword, value); }
        }

        #endregion

        #region Actions

        public async Task ConnectToDevice()
        {
            if (string.IsNullOrEmpty(this.DeviceMACAddress))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Specify a MAC Address",
                    "OK");
            }

            await Task.Run(() =>
             {
                 try
                 {
                     MainViewModel.GetInstance().currentXBeeDevice = new XBeeBLEDevice(this.DeviceMACAddress, this.DevicePassword);
                     MainViewModel.GetInstance().currentXBeeDevice.Open();
                     Device.BeginInvokeOnMainThread(() =>
                     {
                         // If the open method did not throw an exception, the connection is open.
                         Device.BeginInvokeOnMainThread(async () =>
                         {
                            // Load the Temperature page.
                             await Application.Current.MainPage.Navigation.PushAsync(new ViewsCarouselPage());
                         });

                     });
                 }
                 catch (BluetoothAuthenticationException e)
                 {
                    // Error authenticating the device, check password.
                    Device.BeginInvokeOnMainThread(() =>
                     {
                         MainViewModel.GetInstance().currentXBeeDevice = null;
                        // TODO: Show invalid password alert
                    });
                 }
                 catch (XBeeException e)
                 {
                    // Error opening the connection with the device.
                    Device.BeginInvokeOnMainThread(() =>
                     {
                         MainViewModel.GetInstance().currentXBeeDevice = null;
                        // TODO: Show connection error alert.
                    });
                 }
             });
        }

        #endregion

        #region Constructor
        public DeviceSelectorViewModel()
        {
            // TODO: Remove these lines, used only for development
            this.DeviceMACAddress = "D0:CF:5E:87:2F:02";
            this.DevicePassword = "12345";
        }
        #endregion

    }
}

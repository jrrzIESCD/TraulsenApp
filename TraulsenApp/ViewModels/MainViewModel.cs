
namespace TraulsenApp.ViewModels
{
    using System;
    using XBeeLibrary.Xamarin;

    public class MainViewModel
    {

        #region Properties
        public XBeeBLEDevice currentXBeeDevice = null;
        public DeviceSelectorViewModel DeviceSelector
        {
            get;
            set;
        }
        #endregion

        #region Constructor
        public MainViewModel()
        {
            instance = this;
            this.DeviceSelector = new DeviceSelectorViewModel();
        }
        #endregion

        #region Singleton
        public static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }
            return instance;

        }

        #endregion
    }
}

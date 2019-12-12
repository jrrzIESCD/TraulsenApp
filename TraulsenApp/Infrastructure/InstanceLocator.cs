namespace TraulsenApp.Infrastructure
{
    using System;
    using ViewModels;

    public class InstanceLocator
    {
        #region Properties
        public MainViewModel Main
        {
            get;
            set;
        }
        #endregion

        #region Constructor

        #endregion
        public InstanceLocator()
        {
            this.Main = new MainViewModel();
        }
    }
}

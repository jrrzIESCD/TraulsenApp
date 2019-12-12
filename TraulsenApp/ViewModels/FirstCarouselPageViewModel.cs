using System;
namespace TraulsenApp.ViewModels
{
    using System;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using XBeeLibrary.Core.Events.Relay;

    public class FirstCarouselPageViewModel : BaseViewModel
    {
        #region Constants
        private static readonly string[] SEPARATOR = { "@@@" };
        private static readonly int ACK_TIMEOUT = 5000;
        #endregion

        #region Variables
        private bool ackReceived = false;
        object ackLock = new object();
        #endregion

        #region Attributes
        private string tempValue;
        private string humidityValue;
        #endregion

        #region Properties
        public bool IsRunning
        {
            get;
            set;
        }

        public String TemperatureValue
        {
            get { return this.tempValue; }
            set { SetValue(ref this.tempValue, value); }
        }

        public String HumidityValue
        {
            get { return this.humidityValue; }
            set { SetValue(ref this.humidityValue, value); }
        }

        #endregion

        #region OnPageEvents
        public void OnPageAppearing()
        {
            RegisterEventHandler();
            StartProcess(5);
        }

        public void OnPageDissapearing()
        {
            UnregisterEventHandler();
        }

        #endregion

        #region EventHandler Subscription
        /// <summary>
        /// Registers the event handler to be notified when new data from the MicroPython interface
        /// is received.
        /// </summary>
        public void RegisterEventHandler()
        {
            MainViewModel.GetInstance().currentXBeeDevice.MicroPythonDataReceived += BLEDataReceived;
        }

        /// <summary>
        /// Unregisters the event handler to be notified when new data from the MicroPython
        /// interface is received.
        /// </summary>
        public void UnregisterEventHandler()
        {
            MainViewModel.GetInstance().currentXBeeDevice.MicroPythonDataReceived -= BLEDataReceived;
        }
        #endregion

        #region DataHandling
        /// <summary>
        /// Starts the reading process of the temperature and humidity.
        /// </summary>
        /// <param name="rate">Refresh rate.</param>
        /// <returns></returns>
        public Task<bool> StartProcess(int rate)
        {
            return StartStopProcess(true, rate);
        }

        /// <summary>
        /// Stops the reading process of the temperature and humidity.
        /// </summary>
        /// <returns></returns>
        public Task<bool> StopProcess()
        {
            return StartStopProcess(false, -1);
        }

        private Task<bool> StartStopProcess(bool start, int rate)
        {
            var tcs = new TaskCompletionSource<bool>();

            IsRunning = start;
            string data = start ? ("ON@@@" + rate) : "OFF";
            Task.Run(() =>
            {
                // Send a message to the MicroPython interface with the action and refresh time.
                bool ackReceived = SendDataAndWaitResponse(Encoding.Default.GetBytes(data));
                tcs.SetResult(ackReceived);
            });

            return tcs.Task;


        }

        private bool SendDataAndWaitResponse(byte[] data)
        {
            ackReceived = false;
            // Send the data.
            MainViewModel.GetInstance().currentXBeeDevice.SendMicroPythonData(data);
            // Wait until the ACK is received.
            lock (ackLock)
            {
                Monitor.Wait(ackLock, ACK_TIMEOUT);
            }
            // If the ACK was not received, show an error.
            if (!ackReceived)
            {
                //ShowErrorDialog("Response not received", "Could not communicate with MicroPython. Please ensure you have the appropriate application running on it.");
                return false;
            }

            return true;
        }

        private void BLEDataReceived(object sender, MicroPythonDataReceivedEventArgs e)
        {
            string MSG_ACK = "OK";

            string data = Encoding.Default.GetString(e.Data);

            // If the response is "OK", notify the lock to continue the process.
            if (data.Equals(MSG_ACK))
            {
                ackReceived = true;
                lock (ackLock)
                {
                    Monitor.Pulse(ackLock);
                }
            }
            else
            {
                // If the process is stopped, do nothing.
                if (!IsRunning)
                    return;

                // Get the temperature and humidity from the received data.
                string[] dataArray = data.Split(SEPARATOR, StringSplitOptions.None);
                if (dataArray.Length != 2)
                    return;

                // Update the values of the temperature and humidity.
                TemperatureValue = dataArray[0];
                HumidityValue = dataArray[1];

                // Make the texts blink for a short time.

                Task.Delay(200).Wait();

            }
        }
        #endregion

        public FirstCarouselPageViewModel()
        {
        }
    }
}

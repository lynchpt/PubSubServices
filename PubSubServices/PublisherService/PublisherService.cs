using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceProcess;
using System.Text;

namespace PubSubServices.PublisherService
{
    public class PublisherService : ServiceBase
    {
        #region Class Variables

        private static System.Timers.Timer _scheduler;

        private bool _isProcessing = false;
        #endregion

        #region Constructors
        public PublisherService()
        {
            ServiceName = "PublisherService";

            //timer
            InitializeTimer();
        }

        private static void OnElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //_scheduler is already disabled at this point; no more events will come through until we reset it
            //by _scheduler.Enabled = true;
            try
            {
                if ( DateTime.UtcNow.Ticks % 7 == 0 )
                {
                    throw new DivideByZeroException("Divide By Zero");
                }
                if (DateTime.UtcNow.Ticks % 2 == 0)
                {
                    throw new Exception("Handled Error");
                }

                string filename = CheckFileExists();
                File.AppendAllText(filename, $"{DateTime.Now} processed.{Environment.NewLine}" );
                //good for next event
                _scheduler.Enabled = true;
            }
            catch ( DivideByZeroException dbze )
            {
                //example of error where we want to stop the process from trying to run again.
                //this is done by NOT setting enabled to be true

                //handle 
                string filename = CheckFileExists();
                File.AppendAllText(filename, $"{DateTime.Now} Fatal Error {dbze.Message}.{Environment.NewLine}");
            }
            catch ( Exception ex )
            {
                //example of error where we want the process try again.
                //this is done by setting _scheduler.Enabled = true;

                //handle
                string filename = CheckFileExists();
                File.AppendAllText(filename, $"{DateTime.Now} Warning {ex.Message}.{Environment.NewLine}");

                _scheduler.Enabled = true;
            }


            //if (!_isProcessing )
            //{
            //    _isProcessing = true;
            //    //process
            //    string filename = CheckFileExists();
            //    File.AppendAllText(filename, $"{DateTime.Now} processed.{Environment.NewLine}");
            //    _isProcessing = false;
            //    _scheduler.Enabled = true;
            //    //bool enabled = _scheduler.Enabled;
            //}
        }
        #endregion

        #region ServiceBase Overrides
        protected override void OnStart(string[] args)
        {
            string filename = CheckFileExists();
            File.AppendAllText(filename, $"{DateTime.Now} started.{Environment.NewLine}");
            base.OnStart(args);
        }

        protected override void OnStop()
        {
            string filename = CheckFileExists();
            File.AppendAllText(filename, $"{DateTime.Now} stopped.{Environment.NewLine}");
        }
        #endregion

        #region Public Methods

        public void StartAsConsole(string[] args)
        {
            this.OnStart(args);
        }
        #endregion

        #region Private Methods

        private static void InitializeTimer()
        {
            _scheduler = new System.Timers.Timer();
            _scheduler.Interval = 5000;
            _scheduler.AutoReset = false;
            _scheduler.Elapsed += OnElapsed;
            _scheduler.Enabled = true;
        }


        private static string CheckFileExists()
        {
            string filename = @"c:\temp\MyService.txt";
            if (!File.Exists(filename))
            {
                File.Create(filename);
            }

            return filename;
        }
        #endregion


    }
}

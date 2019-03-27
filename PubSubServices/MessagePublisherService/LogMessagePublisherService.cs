using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceProcess;
using System.Text;

namespace MessagePublisherService
{
    public class LogMessagePublisherService : IMessagePublisherService
    {
        #region Constructors

        public LogMessagePublisherService()
        {
            
        }
        #endregion

        #region IMessagePublisherService Implementation
        public void PublishMessages()
        {
            //_scheduler is already disabled at this point; no more events will come through until we reset it
            //by _scheduler.Enabled = true;
            try
            {
                if (DateTime.UtcNow.Ticks % 11 == 0)
                {
                    throw new DivideByZeroException("Divide By Zero");
                }
                if (DateTime.UtcNow.Ticks % 3 == 0)
                {
                    throw new Exception("Handled Error");
                }

                string filename = CheckFileExists();
                File.AppendAllText(filename, $"{DateTime.Now} processed.{Environment.NewLine}");

            }
            catch (DivideByZeroException dbze)
            {
                string filename = CheckFileExists();
                File.AppendAllText(filename, $"{DateTime.Now} Fatal Error {dbze.Message}.{Environment.NewLine}");

                //example of error where we want to stop the process from trying to run again.
                //this is done by throwing the exception, which will result in the scheduler
                //not resetting the timer event

                throw;
            }
            catch (Exception ex)
            {
                //example of error where we want the process try again.
                //this is done by handling the error so the containing scheduler sees no error
                //and resetting the timer event

                //handle
                string filename = CheckFileExists();
                File.AppendAllText(filename, $"{DateTime.Now} Warning {ex.Message}.{Environment.NewLine}");
            }
        }
        #endregion

        #region Private Methods

        private string CheckFileExists()
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

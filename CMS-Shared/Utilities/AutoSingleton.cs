using CMS_Shared.Keyword;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace CMS_Shared.Utilities
{
    public sealed class AutoSingleton
    {
        private static AutoSingleton instance = null;
        private static readonly object padlock = new object();

        AutoSingleton()
        {
        }

        public static AutoSingleton Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new AutoSingleton();

                            /* auto run with timer */
                            AutoFunction();
                        }
                    }
                }
                return instance;
            }
        }
        
        public static void AutoFunction()
        {
            /* set timer event */
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 60 * 1000; /* (second*1000) = milisecond */
            aTimer.Enabled = true;
        }

        // Specify what you want to happen when the Elapsed event is raised.
        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex) { LogHelper.WriteLogs("ErrorTimer", ""); };

            /* log data */
            LogHelper.WriteLogs("Timer", "");
        }
        
        
    }
}

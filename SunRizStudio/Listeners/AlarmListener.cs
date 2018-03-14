﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SunRizStudio.Listeners
{
    /// <summary>
    /// 监听新报警
    /// </summary>
    public class AlarmListener
    {
        enum AlarmListenerState
        {
            Stopped = 0,
            Running = 1,
            Pause = 2
        }
        static AlarmListenerState State = AlarmListenerState.Stopped;
        public static EventHandler Alarmed;
        public static void Pause()
        {
            State = AlarmListenerState.Pause;
        }

        public static void Start()
        {
            if(State != AlarmListenerState.Stopped)
            {
                return;
            }
            State = AlarmListenerState.Running;
            DateTime? lastAlarmTime = null;

            Task.Run(() => {
                while(true)
                {
                    Thread.Sleep(2000);
                    try
                    {
                        if(State == AlarmListenerState.Running)
                        {
                            Helper.Remote.Invoke<DateTime?>("GetLastAlarmTime", (ret, err) => {
                                if(err == null)
                                {
                                    lastAlarmTime = ret;
                                    if(Alarmed != null && ret != null)
                                    {
                                        Alarmed(null, null);
                                    }
                                }
                            }, lastAlarmTime);
                        }
                    }
                    catch
                    {

                    }
                }
            });
        }
    }
}

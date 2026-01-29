using NUnit.Framework;
using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;

public class WorldTimeWatcher : MonoBehaviour
{
    [SerializeField]
    private WorldTime worldtime;

    [SerializeField]
    private List<Schedule> schedule;

    private void Start()
    {
        worldtime.WorldTimeChanged += CheckSchedule;
    }

    private void OnDestroy()
    {
        worldtime.WorldTimeChanged -= CheckSchedule;
    }
    private void CheckSchedule(object sender, TimeSpan newtime)
    {
        var _schedule =
            schedule.FirstOrDefault(s =>
                s.hour == newtime.Hours &&
                s.minute == newtime.Minutes);

        _schedule?.action.Invoke();
    }

    [Serializable]
    private class Schedule
    {
        public int hour;
        public int minute;
        public UnityEvent action;
    }

   
}

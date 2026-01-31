using System;
using UnityEngine;
using System.Collections;

public class WorldTime : MonoBehaviour
{
    public event EventHandler<TimeSpan> WorldTimeChanged;
    public event Action<bool> DayNightChanged; // true = day, false = night

    public float daylength = 600f; // real seconds per full day
    public float minutesInDay = 1440f;

    private TimeSpan currentTime;
    private float minuteLength;
    private bool isDay;

    private void Start()
    {
        currentTime = TimeSpan.FromHours(6); // start at morning
        minuteLength = daylength / minutesInDay;
        UpdateDayNight();
        StartCoroutine(TimeTick());
    }

    private IEnumerator TimeTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(minuteLength);

            currentTime += TimeSpan.FromMinutes(1);
            if (currentTime.TotalMinutes >= minutesInDay)
                currentTime = TimeSpan.Zero;

            WorldTimeChanged?.Invoke(this, currentTime);
            UpdateDayNight();
        }
    }

    private void UpdateDayNight()
    {
        bool newIsDay = currentTime.Hours >= 6 && currentTime.Hours < 18;

        if (newIsDay != isDay)
        {
            isDay = newIsDay;
            DayNightChanged?.Invoke(isDay);
        }
    }
}

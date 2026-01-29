using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class WorldLight : MonoBehaviour
{
    public Light2D light;
    public float minutesinday = 1440;
    public WorldTime worldtime;

    [SerializeField]
    private Gradient gradient;

    private void Awake()
    {
        light = GetComponent<Light2D>();
        worldtime.WorldTimeChanged += OnWorldTimeChanged;
    }

    private void OnDestroy()
    {
        worldtime.WorldTimeChanged -= OnWorldTimeChanged;
    }

    private void OnWorldTimeChanged(object sender, TimeSpan newtime)
    {
        light.color = gradient.Evaluate(percentofday(newtime));
    }

    private float percentofday(TimeSpan timespan)
    { 
        return (float)timespan.TotalMinutes%minutesinday / minutesinday;
    }


}

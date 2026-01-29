using System;
using UnityEngine;
using System.Collections;


    public class WorldTime : MonoBehaviour
    {
    public event EventHandler<TimeSpan> WorldTimeChanged;
        public float daylength; //in seconds
        private TimeSpan currentTime;
        public float minutesinday= 1440;
        private float minutelength; //1440 minutes in a day

         private void Start()
         {
            minutelength = daylength / minutesinday;
            StartCoroutine(Addminute());
         }

    private IEnumerator Addminute() 
        {
            currentTime += TimeSpan.FromMinutes(1);
            WorldTimeChanged?.Invoke(this, currentTime);
            yield return new WaitForSeconds(minutelength);
            StartCoroutine(Addminute());
        }
    
    }

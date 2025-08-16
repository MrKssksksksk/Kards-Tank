using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimerScript : MonoBehaviour
{
    List<UnityEvent> events = new List<UnityEvent>();
    List<float> time = new List<float>(); 
    List<float> timer = new List<float>();

    private void Update()
    {
        for (int i = 0; i < events.Count; i++)
        {
            timer[i] += Time.deltaTime;
            if (timer[i] > time[i])
            {
                events[i].Invoke();
                events.RemoveAt(i);
                time.RemoveAt(i);
                timer.RemoveAt(i);
            }
        }
    }

    public int addTimer(float t, UnityAction method)
    {
        UnityEvent e = new UnityEvent();
        e.AddListener(method);
        events.Add(e);
        time.Add(t);
        timer.Add(0);
        return events.Count - 1;
    }

    public void endTimer(int index)
    {
        events.RemoveAt(index);
        time.RemoveAt(index);
        timer.RemoveAt(index);
    }

}

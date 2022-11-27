using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public struct SimpleTimer
{
    public float interval;
    public bool enabled;

    private float duration;

    public SimpleTimer(float interval)
    {
        this.interval = interval;
        duration = 0;
        enabled = false;
    }

    public void Start()
    {
        duration = 0;
        enabled = true;
    }

    public void Stop()
    {
        enabled = false;
    }


    /// <summary>
    /// Shows wheather the Timer was stopped.
    /// </summary>
    /// <returns></returns>
    public bool Update()
    {
        if (!enabled) return true;

        duration += Time.smoothDeltaTime;
        if (duration < interval) return false;

        enabled = false;
        return true;
    }
}


public struct RepeatTicker
{
    public float interval;

    private float duration;

    public RepeatTicker(float interval)
    {
        this.interval = interval;
        duration = 0;
    }

    public int Update(float speed)
    {
        duration += Time.smoothDeltaTime * speed;
        if (duration < interval) return 0;

        int times = Mathf.FloorToInt((duration - interval) / interval) + 1;
        duration -= times * interval;
        return times;
    }
}

public struct DelayedRepeatTicker
{
    public float delay;
    public float interval;

    private float duration;
    private float nextTime;
    private bool wasPressing;

    public DelayedRepeatTicker(float delay, float interval)
    {
        this.delay = delay;
        this.interval = interval;
        duration = nextTime = 0;
        wasPressing = false;

    }

    public int Update(bool isPressing)
    {
        if (!wasPressing && isPressing)
        {
            wasPressing = true;
            return 1;
        }

        if (wasPressing && !isPressing)
        {
            wasPressing = false;
            duration = 0f;
            nextTime = Mathf.Max(delay, interval);
            return 0;
        }

        if (!isPressing) return 0;

        duration += Time.smoothDeltaTime;
        if (duration < nextTime) return 0;

        int times = Mathf.FloorToInt((duration - nextTime) / interval) + 1;
        duration -= times * interval;
        return times;

    }
}

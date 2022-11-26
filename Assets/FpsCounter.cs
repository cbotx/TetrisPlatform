using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FpsCounter : MonoBehaviour
{
    [SerializeField] private Text _fpsText;

    public float updateInterval = 0.5f; //How often should the number update

    int frames = 0;
    float timeleft;
    float fps;

    void Start()
    {
        timeleft = updateInterval;
    }

    // Update is called once per frame
    void Update()
    {
        timeleft -= Time.unscaledDeltaTime;
        ++frames;

        // Interval ended - update GUI text and start new interval
        if (timeleft <= 0.0)
        {
            // display two fractional digits (f2 format)
            fps = frames / (updateInterval - timeleft);
            timeleft = updateInterval;
            frames = 0;
            _fpsText.text = $"fps: {fps:F0}";
        }
    }
}
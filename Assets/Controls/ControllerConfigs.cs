using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ControllerConfigs
{
    /// <summary>
    /// Automatic Repeat Rate. (30ms in default)
    /// </summary>
    public float ARR;

    /// <summary>
    /// Delayed Auto Shift. (150ms in default)
    /// </summary>
    public float DAS;

    /// <summary>
    /// DAS Cut delay, after Dropping. (17ms in default)
    /// </summary>
    public float DCD;

    /// <summary>
    /// Soft Drop Factor. (30x in default)
    /// </summary>
    public float SDF;

    /// <summary>
    /// Auto Cut Hard drop, after auto freeze. (50ms in default)
    /// </summary>
    public float ACH;

    /// <summary>
    /// DAS Cut delay, after Rotating . (50ms in default)
    /// </summary>
    public float DCR;

    public static ControllerConfigs Default = new()
    {
        ARR = 30 * 0.001f,
        DAS = 150 * 0.001f,
        DCD = 17 * 0.001f,
        SDF = 30,
        ACH = 50 * 0.001f,
        DCR = 50 * 0.001f,
    };

}



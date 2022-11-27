using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class KeyConfigDefinitions
{
    public static KeyConfigs cirq = new()
    {
        main = new KeyBindings()
        {
            MoveLeft = KeyCode.LeftArrow,
            MoveRight = KeyCode.RightArrow,
            SoftDrop = KeyCode.UpArrow,
            HardDrop = KeyCode.Space,
            Hold = KeyCode.LeftShift,
            RotateRight = KeyCode.D,
            RotateLeft = KeyCode.A,
            Rotate180 = KeyCode.W,
            Exit = KeyCode.Escape,
            Retry = KeyCode.BackQuote,
        }
    };

}

public static class ControllerConfigDefinitions
{
    public static ControllerConfigs cirq = new()
    {
        ARR = 3 * 0.001f,
        DAS = 117 * 0.001f,
        DCD = 17 * 0.001f,
        SDF = 10000,
        ACH = 50 * 0.001f,
        DCR = 50 * 0.001f,
    };

}
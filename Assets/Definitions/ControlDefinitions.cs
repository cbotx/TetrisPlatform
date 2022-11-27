using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class ControlDefinitions
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct KeyBinding
{
    public KeyCode MoveLeft;
    public KeyCode MoveRight;

    public KeyCode SoftDrop;
    public KeyCode HardDrop;

    public KeyCode Hold;

    public KeyCode RotateRight;
    public KeyCode RotateLeft;
    public KeyCode Rotate180;

    public KeyCode Exit;
    public KeyCode Retry;

}



[Serializable]
public class KeyConfigs
{
    public KeyBinding main;
    public KeyBinding sub;
    public KeyBinding third;

}
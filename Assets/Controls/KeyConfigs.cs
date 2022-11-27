using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


[Serializable]
public class KeyBindings
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


public enum Op
{
    MoveLeft, MoveRight,
    SoftDrop, HardDrop, 
    Hold,
    RotateLeft, RotateRight, Rotate180,
    Exit, Retry,
}



public class InputOperation
{

    const BindingFlags ISPNP = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
    static readonly FieldInfo[] fi_keys = typeof(KeyBindings).GetFields(ISPNP);

    readonly Dictionary<Op, KeyCode[]> map = new();

    public InputOperation(KeyConfigs conf)
    {
        KeyBindings main = conf.main;
        KeyBindings sub = conf.sub;
        KeyBindings third = conf.third;

        foreach (FieldInfo field in fi_keys)
        {
            map[field.Name.ToEnum<Op>()] = new KeyCode[] { 
                (KeyCode)field.GetValue(main), 
                (KeyCode)field.GetValue(sub), 
                (KeyCode)field.GetValue(third), 
            };

        }
    }

    public bool GetKeyDown(Op op)
    {
        if (!map.TryGetValue(op, out KeyCode[] c)) return false;
        return Input.GetKeyDown(c[0]) || Input.GetKeyDown(c[1]) || Input.GetKeyDown(c[2]);
    }

    public bool GetKeyUp(Op op)
    {
        if (!map.TryGetValue(op, out KeyCode[] c)) return false;
        return Input.GetKeyUp(c[0]) || Input.GetKeyDown(c[1]) || Input.GetKeyDown(c[2]);
    }

    public bool GetKey(Op op)
    {
        if (!map.TryGetValue(op, out KeyCode[] c)) return false;
        return Input.GetKey(c[0]) || Input.GetKeyDown(c[1]) || Input.GetKeyDown(c[2]);
    }

}



[Serializable]
public class KeyConfigs
{
    public KeyBindings main = new();
    public KeyBindings sub = new();
    public KeyBindings third = new();

    private InputOperation _input;

    public InputOperation Input
    {
        get
        {
            if (_input is null) _input = new InputOperation(this);
            return _input;
        }
    }

}
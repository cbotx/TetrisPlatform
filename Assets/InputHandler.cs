using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class InputHandler: MonoBehaviour
{
    public static float s_FallInterval = 1f;
    public static float s_RepeatInterval = 0.03f;
    public static float s_WaitBeforeRepeatInterval = 0.15f;
    public static float s_TimeTillFreeze = 1.5f;


    private float _prevUpdateTime;
    private float _prevFallTime;
    private float _freezeTime;
    private float _prevBottomTouchTime;

    private bool _moveLeft = false;
    private bool _moveRight = false;
    private bool _repeat = false;
    private bool _isBottomTouched = false;


    private Playfield _playfield;

    private void Awake()
    {
        _playfield = GetComponentInParent<Playfield>();
    }

    private void Update()
    { }



}
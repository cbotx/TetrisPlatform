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


    public InputOperation input;

    private float _prevUpdateTime;
    private float _prevFallTime;
    private float _freezeTime;
    private float _prevBottomTouchTime;

    private bool _moveLeft = false;
    private bool _moveRight = false;
    private bool _repeat = false;
    private bool _isBottomTouched = false;


    private Playfield _playfield;
    private PieceEntity _piece;

    private void Awake()
    {
        _playfield = GetComponentInParent<Playfield>();
        input = _playfield.control.Input;
    }

    public void Start()
    {
    }

    private void Update()
    {
        if (!_piece)
        {
            _piece = _playfield.FieldPiece;
            if (!_piece) return;
        }

        if (input.GetKeyDown(Op.Retry))
        {
            _playfield.Restart();
            return;
        }

        if (input.GetKeyDown(Op.HardDrop))
        {
            _piece = _piece.HardDrop();
        }
        if (input.GetKeyDown(Op.Hold))
        {
            _piece = _playfield.Swap();
        }
        if (input.GetKeyDown(Op.MoveLeft))
        {
            _moveLeft = true;
            _moveRight = false;
            _repeat = false;
            _piece.Move(-1, 0);
            _prevUpdateTime = Time.time;
        }
        if (input.GetKeyDown(Op.MoveRight))
        {
            _moveLeft = false;
            _moveRight = true;
            _repeat = false;
            _piece.Move(1, 0);
            _prevUpdateTime = Time.time;
        }
        if (input.GetKeyDown(Op.RotateRight))
        {
            _piece.Rotate(1);
        }
        if (input.GetKeyDown(Op.RotateLeft))
        {
            _piece.Rotate(3);
        }
        if (input.GetKeyDown(Op.Rotate180))
        {
            _piece.Rotate(2);
        }
        if (input.GetKeyDown(Op.SoftDrop))
        {
            _piece.Move(0, -1);
            _prevFallTime = Time.time;
        }

        if (!input.GetKey(Op.MoveLeft)) _moveLeft = false;
        if (!input.GetKey(Op.MoveRight)) _moveRight = false;

        if (_moveLeft || _moveRight)
        {
            int move_x = _moveLeft ? -1 : 1;
            if (_repeat)
            {
                if (Time.time - _prevUpdateTime >= s_RepeatInterval)
                {
                    _piece.Move(move_x, 0);
                    _prevUpdateTime += s_RepeatInterval;
                }
            }
            else
            {
                if (Time.time - _prevUpdateTime >= s_WaitBeforeRepeatInterval)
                {
                    _piece.Move(move_x, 0);
                    _prevUpdateTime += s_WaitBeforeRepeatInterval;
                    _repeat = true;
                }
            }
        }
        else
        {
            _repeat = false;
        }

        float interval = input.GetKey(Op.SoftDrop) ? s_RepeatInterval : s_FallInterval;
        if (Time.time - _prevFallTime >= interval)
        {
            _piece.Move(0, -1);
            _prevFallTime += interval;
        }
        if (_isBottomTouched && Time.time - _prevBottomTouchTime + _freezeTime >= s_TimeTillFreeze)
        {
            _piece = _playfield.FreezePiece();
        }

    }



}
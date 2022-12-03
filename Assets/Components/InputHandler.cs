using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using Assets.AI;

public class InputHandler: MonoBehaviour
{

    public InputOperation input;

    private Playfield _playfield;
    private PieceEntity _piece;

    public MovementController controller;

    private SimpleTimer autoFrozen;

    private void Awake()
    {
        _playfield = GetComponentInParent<Playfield>();
    }

    public void Start()
    {
        input = _playfield.control.Input;
        controller = new MovementController(_playfield.handling, _playfield.game, (x,y) => _piece.Move(x,y));
        autoFrozen = new SimpleTimer(controller.config.ACH);
    }

    public void AutoFrozen()
    {
        controller.Dropped();
        autoFrozen.Start();
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

        if (autoFrozen.Update() && input.GetKeyDown(Op.HardDrop))
        {
            controller.Dropped();
            _piece = _piece.HardDrop();
        }
        if (input.GetKeyDown(Op.Hold))
        {
            controller.Swapped();
            var res = _playfield.Swap();
            if (res is not null) _piece = res;
        }
        if (input.GetKeyDown(Op.MoveLeft))
        {
            controller.MoveLeft();
        }
        if (input.GetKeyDown(Op.MoveRight))
        {
            controller.MoveRight();
        }
        if (input.GetKeyDown(Op.RotateRight))
        {
            controller.Rotated();
            _piece.Rotate(1);
        }
        if (input.GetKeyDown(Op.RotateLeft))
        {
            controller.Rotated();
            _piece.Rotate(3);
        }
        if (input.GetKeyDown(Op.Rotate180))
        {
            controller.Rotated();
            _piece.Rotate(2);
        }
        if (input.GetKey(Op.MoveLeftFast)) {
            controller.MoveFast();
            while (_piece.Move(-1, 0)) ;
        }
        if (input.GetKey(Op.MoveRightFast)) {
            controller.MoveFast();
            while (_piece.Move(1, 0)) ;
        }
        if (input.GetKey(Op.SoftDropFast)) {
            while (_piece.Move(0, -1)) ;
        }

        bool leftMove = input.GetKey(Op.MoveLeft);
        bool rightMove = input.GetKey(Op.MoveRight);
        bool softDrop = input.GetKey(Op.SoftDrop);

        controller.Update(leftMove, rightMove, softDrop);

    }



}
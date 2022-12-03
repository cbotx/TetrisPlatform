using System.Collections;
using System.Collections.Generic;
using Assets.AI.Structures;
using System;
using UnityEngine;
using Assets.AI.PathFinder;

namespace Assets.AI
{
    public class AIHandler : MonoBehaviour
    {
        public float Interval = 0.1f;
        private float _lastCommandTime;
        private IAI _ai;
        private Queue<Op> _commandsToBeExecuted = new();
        private Playfield _playfield;

        private void Awake()
        {
            _playfield = GetComponentInParent<Playfield>();
        }
        private void Start()
        {
            _lastCommandTime = Time.time;
        }
        private void Update()
        {
            if (_commandsToBeExecuted.Count == 0) return;
            if (Time.time - _lastCommandTime < Interval) return;
            _lastCommandTime = Time.time;
            ExecuteCommand();
        }
        public void SetAI(IAI ai)
        {
            _ai = ai;
            _ai.Initialize();
        }
        
        public async void UpdateBoard(UpdateEvent e, BoardState state)
        {
            if (e == UpdateEvent.NextPieceEvent)
            {
                _ai.CancelCalculation();
                BoardAction action = await _ai.CalculateBestMove(state);
                if (action != null)
                {
                    List<Op> ops = GenerateCommands(state, action);
                    foreach (var item in ops)
                    {
                        Debug.Log(item);
                        _commandsToBeExecuted.Enqueue(item);
                    }
                }
            }
        }

        private List<Op> GenerateCommands(BoardState state, BoardAction action)
        {
            int pieceId = action.doSwap ? state.pieces[0] : state.pieces[1];
            List<Op> commands = PathFinder.PathFinder.Search(pieceId, action.posX, action.posY, action.direction, state.field);
            if (action.doSwap) commands.Insert(0, Op.Hold);
            return commands;
        }

        private void ExecuteCommand()
        {
            Op op = _commandsToBeExecuted.Peek();
            if (op != Op.SoftDrop) _commandsToBeExecuted.Dequeue();
            PieceEntity piece = _playfield.FieldPiece;
            if (op == Op.Retry)
            {
                _ai.SoftReset();
                _playfield.Restart();
            }
            else if (op == Op.HardDrop)
            {
                piece.HardDrop();
            }
            else if (op == Op.Hold)
            {
                _playfield.Swap();
            }
            else if (op == Op.MoveLeft)
            {
                piece.Move(-1, 0);
            }
            else if (op == Op.MoveRight)
            {
                piece.Move(1, 0);
            }
            else if (op == Op.RotateRight)
            {
                piece.Rotate(1);
            }
            else if (op == Op.RotateLeft)
            {
                piece.Rotate(3);
            }
            else if (op == Op.Rotate180)
            {
                piece.Rotate(2);
            }
            else if (op == Op.MoveLeftFast)
            {
                while (piece.Move(-1, 0)) ;
            }
            else if (op == Op.MoveRightFast)
            {
                while (piece.Move(1, 0)) ;
            }
            else if (op == Op.SoftDropFast)
            {
                while (piece.Move(0, -1)) ;
            }
            else if (op == Op.SoftDrop)
            {
                // TODO: Implement this (Drop till 1 block above ground)
                throw new NotImplementedException();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.AI.PathFinder
{
    public class Simulator
    {
        public int direction;
        public int x;
        public int y;
        public int PieceId;
        public PieceShape Shape;
        private KickTable _kickTable;
        private PieceShape[] _shapes;
        private bool[,] _field;
        public Simulator(int pieceId, bool[,] field)
        {
            PieceId = pieceId;
            _field = field;
            var rule = GameDefinitions.Tetris_SRS_Plus;
            _kickTable = rule.kickTableOfPiece[pieceId];
            _shapes = rule.shapesOfPiece[pieceId];
            Reset();
        }

        public void Reset()
        {
            Shape = _shapes[0];
            x = 4;
            y = 20;
        }

        public void Init(int x, int y, int direction)
        {
            this.x = x;
            this.y = y;
            this.direction = direction;
            Shape = _shapes[direction];
        }
        public bool Act(Op op)
        {
            if (op == Op.RotateLeft || op == Op.RotateRight || op == Op.Rotate180)
            {
                int rotation = 3;
                if (op == Op.RotateRight) rotation = 1;
                if (op == Op.Rotate180) rotation = 2;
                int newDirection = (direction + rotation) % 4;
                Vector2Int[] attempts = _kickTable[rotation, direction];
                PieceShape newShape = _shapes[newDirection];

                foreach (Vector2Int attempt in attempts)
                {
                    if (!HitTest(x, y, newShape, attempt))
                    {
                        Shape = newShape;
                        x += attempt.x;
                        y += attempt.y;
                        direction = newDirection;
                        return true;
                    }
                }
            }
            else if (op == Op.MoveLeft)
            {
                if (!HitTest(x, y, Shape, new Vector2Int(-1, 0)))
                {
                    x--;
                    return true;
                }
            }
            else if (op == Op.MoveRight)
            {
                if (!HitTest(x, y, Shape, new Vector2Int(1, 0)))
                {
                    x++;
                    return true;
                }
            }
            else if (op == Op.MoveLeftFast)
            {
                while (!HitTest(x, y, Shape, new Vector2Int(-1, 0)))
                {
                    x--;
                }
                return true;
            }
            else if (op == Op.MoveRightFast)
            {
                while (!HitTest(x, y, Shape, new Vector2Int(1, 0)))
                {
                    x++;
                }
                return true;
            }
            else if (op == Op.SoftDropFast || op == Op.HardDrop)
            {
                while (!HitTest(x, y, Shape, new Vector2Int(0, -1)))
                {
                    y--;
                }
                return true;
            }
            else if (op == Op.SoftDrop)
            {
                while (!HitTest(x, y, Shape, new Vector2Int(0, -2)))
                {
                    y--;
                }
                return true;
            }
            return false;
        }
        public bool HitTest(int posX, int posY, PieceShape shape, Vector2Int offset)
        {
            int cx = posX + offset.x;
            int cy = posY + offset.y;

            foreach (var pos in shape.blocks)
            {
                int x = cx + pos.x;
                int y = cy + pos.y;
                if (x < 0 || x >= Playfield.s_Width || y < 0 || y >= Playfield.s_Height) return true;
                if (_field[x, y]) return true;
            }

            return false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.AI.PathFinder
{
    public static class PathFinder
    {
        class BfsItem
        {
            public readonly int x;
            public readonly int y;
            public readonly int direction;
            public readonly Op command;
            public readonly int depth;
            public readonly BfsItem prev;
            public BfsItem(int x, int y, int direction, Op command, int depth, BfsItem prev)
            {
                this.x = x;
                this.y = y;
                this.direction = direction;
                this.command = command;
                this.depth = depth;
                this.prev = prev;
            }
        };
        private static List<Op> _instructions = new() { Op.MoveLeft, Op.MoveRight, Op.MoveLeftFast, Op.MoveRightFast, Op.RotateLeft, Op.RotateRight, Op.Rotate180, Op.HardDrop, Op.SoftDropFast, Op.SoftDrop };
        //private static List<Op> _instructions = new() { Op.RotateRight};
        private static Dictionary<Op, List<Op>> _allowedNext = new()
        {
            {
                Op.MoveLeft,
                new List<Op> { Op.MoveLeft, Op.RotateLeft, Op.RotateRight, Op.Rotate180, Op.HardDrop, Op.SoftDropFast, Op.SoftDrop }
            },
            {
                Op.MoveRight,
                new List<Op> { Op.MoveRight, Op.RotateLeft, Op.RotateRight, Op.Rotate180, Op.HardDrop, Op.SoftDropFast, Op.SoftDrop }
            },
            {
                Op.MoveLeftFast,
                new List<Op> { Op.MoveRight, Op.RotateLeft, Op.RotateRight, Op.Rotate180, Op.HardDrop, Op.SoftDropFast, Op.SoftDrop }
            },
            {
                Op.MoveRightFast,
                new List<Op> { Op.MoveLeft, Op.RotateLeft, Op.RotateRight, Op.Rotate180, Op.HardDrop, Op.SoftDropFast, Op.SoftDrop }
            },
            {
                Op.RotateLeft,
                new List<Op> { Op.MoveLeft, Op.MoveRight, Op.MoveLeftFast, Op.MoveRightFast, Op.RotateLeft, Op.Rotate180, Op.HardDrop, Op.SoftDropFast, Op.SoftDrop }
            },
            {
                Op.RotateRight,
                new List<Op> { Op.MoveLeft, Op.MoveRight, Op.MoveLeftFast, Op.MoveRightFast, Op.RotateRight, Op.Rotate180, Op.HardDrop, Op.SoftDropFast, Op.SoftDrop }
            },
            {
                Op.Rotate180,
                new List<Op> { Op.MoveLeft, Op.MoveRight, Op.MoveLeftFast, Op.MoveRightFast, Op.RotateLeft, Op.RotateRight, Op.HardDrop, Op.SoftDropFast, Op.SoftDrop }
            },
            {
                Op.SoftDrop,
                new List<Op> { Op.MoveLeft, Op.MoveRight, Op.MoveLeftFast, Op.MoveRightFast, Op.RotateLeft, Op.RotateRight, Op.Rotate180 }
            },
            {
                Op.SoftDropFast,
                new List<Op> { Op.MoveLeft, Op.MoveRight, Op.MoveLeftFast, Op.MoveRightFast, Op.RotateLeft, Op.RotateRight, Op.Rotate180 }
            },
            {
                Op.HardDrop,
                new List<Op>()
            }
        };

        // Symmetrical piece can have two possible directions at the same position, e.g. I, Z, S
        // Key = PieceId, Value = Array [ Dim0 = First direction, Dim1 = Offset of second direction ]
        private static readonly Dictionary<int, int[,]> AlternativeTarget = new()
        {
            {
                0, // Z
                new int[2, 2] { { 0, 1 }, { 1, 0 } }
            },
            {
                3, // S
                new int[2, 2] { { 0, 1 }, { 1, 0 } }
            },
            {
                4, // I
                new int[2, 2] { { 0, 1 }, { 1, 0 } }
            }
        };
        public static List<Op> Search(int pieceId, int targetX, int targetY, int targetDirection, bool[,] field)
        {
            Simulator sim = new Simulator(pieceId, field);
            int altX = targetX;
            int altY = targetY;
            int altDirection = targetDirection;
            if (AlternativeTarget.ContainsKey(pieceId))
            {
                altX += AlternativeTarget[pieceId][targetDirection, 0];
                altY += AlternativeTarget[pieceId][targetDirection, 1];
                altDirection += 2;
            }


            // BFS
            Queue<BfsItem> queue = new();
            Queue<BfsItem> softDropQueue = new();
            foreach (var _item in _instructions)
            {
                if (_item == Op.SoftDrop)
                {
                    softDropQueue.Enqueue(new BfsItem(4, 20, 0, _item, 3, null));
                }
                else
                {
                    queue.Enqueue(new BfsItem(4, 20, 0, _item, 0, null));
                }
            }
            int maxSearch = 1000000;
            while (queue.Count > 0)
            {
                maxSearch -= 1;
                if (maxSearch == 0)
                {
                    break;
                }
                var item = queue.Dequeue();
                sim.Init(item.x, item.y, item.direction);
                if (sim.Act(item.command))
                {
                    if ((sim.x == targetX && sim.y == targetY && sim.direction == targetDirection)
                        || (sim.x == altX && sim.y == altY && sim.direction == altDirection))
                    {
                        if (item.command != Op.HardDrop)
                        {
                            queue.Enqueue(new BfsItem(sim.x, sim.y, sim.direction, Op.HardDrop, item.depth + 1, item));
                            continue;
                        }
                        List<Op> ops = new();
                        while (item != null)
                        {
                            ops.Add(item.command);
                            item = item.prev;
                        }
                        ops.Reverse();
                        return ops;
                    }
                    foreach (var _next in _allowedNext[item.command])
                    {
                        if (_next == Op.SoftDrop)
                        {
                            softDropQueue.Enqueue(new BfsItem(sim.x, sim.y, sim.direction, _next, item.depth + 3, item));
                        }
                        else
                        {
                            queue.Enqueue(new BfsItem(sim.x, sim.y, sim.direction, _next, item.depth + 1, item));
                        }
                    }
                    while (softDropQueue.Count > 0 && softDropQueue.Peek().depth <= item.depth + 1)
                    {
                        queue.Enqueue(softDropQueue.Dequeue());
                    }
                }
            }
            return null;
        }
    }
}

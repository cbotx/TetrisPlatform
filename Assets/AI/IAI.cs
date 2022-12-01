using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.AI.Structures;
using System.Threading.Tasks;

namespace Assets.AI
{
    public interface IAI
    {
        public void Initialize();
        public Task<BoardAction> CalculateBestMove(BoardState state);

        public void Destroy();
    }
}
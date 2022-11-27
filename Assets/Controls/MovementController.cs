using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class MovementController
{
    public delegate bool Move(int x, int y);

    public ControllerConfigs config;
    private readonly TetrisGameplay game;
    public Move move;

    private DelayedRepeatTicker moveLeft, moveRight;
    private RepeatTicker drop;

    private SimpleTimer dropped, rotated;

    public MovementController(ControllerConfigs config, TetrisGameplay game, Move move)
    {
        this.config = config;
        this.game = game;
        this.move = move;

        moveLeft = new DelayedRepeatTicker(config.DAS, config.ARR);
        moveRight = new DelayedRepeatTicker(config.DAS, config.ARR);
        drop = new RepeatTicker(game.FallInterval);

        dropped = new SimpleTimer(config.DCD);
        rotated = new SimpleTimer(config.DCR);
    }


    public void Dropped()
    {
        dropped.Start();
        rotated.Stop();
    }
    public void Rotated()
    {
        rotated.Start();
    }
    public void Swapped()
    {
        dropped.Start();
    }

    public void MoveLeft()
    {
        rotated.Stop();
        isLeftLast = true;
        move(-1, 0);
    }

    public void MoveRight()
    {
        rotated.Stop();
        isLeftLast = false;
        move(1, 0);
    }


    private bool isLeftLast;

    public void Update(bool left, bool right, bool softDrop)
    {
        drop.interval = game.FallInterval;

        int leftTimes = moveLeft.Update(left);
        int rightTimes = moveRight.Update(right);
        int dropTimes = drop.Update(softDrop ? config.SDF : 1f);

        bool canDAS = dropped.Update() && rotated.Update();


        if (left && (isLeftLast || !right) && canDAS && leftTimes > 0)
            for (int i = 0; i < leftTimes; i++)
                if (!move(-1, 0)) break;

        if (right && (!isLeftLast || !left) && canDAS && rightTimes > 0)
            for (int i = 0; i < rightTimes; i++)
                if (!move(1, 0)) break;

        for (int i = 0; i < dropTimes; i++)
            if (!move(0, -1)) break;

    }

}

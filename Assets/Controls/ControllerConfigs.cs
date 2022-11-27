using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public struct ControllerConfigs
{
    /// <summary>
    /// Automatic Repeat Rate. (30ms in default)
    /// </summary>
    public float ARR;

    /// <summary>
    /// Delayed Auto Shift. (150ms in default)
    /// </summary>
    public float DAS;

    /// <summary>
    /// DAS Cut delay, after Dropping. (17ms in default)
    /// </summary>
    public float DCD;

    /// <summary>
    /// Soft Drop Factor. (30x in default)
    /// </summary>
    public float SDF;

    /// <summary>
    /// Auto Cut Hard drop, after auto freeze. (100ms in default)
    /// </summary>
    public float ACH;

    /// <summary>
    /// DAS Cut delay, after Rotating . (17ms in default)
    /// </summary>
    public float DCR;

    public static ControllerConfigs Default = new()
    {
        ARR = 30 * 0.001f,
        DAS = 150 * 0.001f,
        DCD = 17 * 0.001f,
        SDF = 30,
        ACH = 100 * 0.001f,
        DCR = 17 * 0.001f,
    };

}



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

    public void MoveKeyDown(bool isLeft)
    {
        rotated.Stop();
        isLeftLast = isLeft;
    }

    private bool isLeftLast;

    public void Update(bool left, bool right, bool softDrop)
    {
        drop.interval = game.FallInterval;

        int leftTimes = moveLeft.Update(left);
        int rightTimes = moveRight.Update(right);
        int dropTimes = drop.Update(softDrop ? config.SDF : 1f);

        bool canDAS = dropped.Update() && rotated.Update();
        bool isOverlayed = left && right;


        if (left && (isLeftLast || !isOverlayed) && canDAS && leftTimes > 0)
            for (int i = 0; i < leftTimes; i++)
                if (!move(-1, 0)) break;

        if (right && (!isLeftLast || !isOverlayed) && canDAS && rightTimes > 0)
            for (int i = 0; i < rightTimes; i++)
                if (!move(1, 0)) break;

        for (int i = 0; i < dropTimes; i++)
            if (!move(0, -1)) break;

    }

}

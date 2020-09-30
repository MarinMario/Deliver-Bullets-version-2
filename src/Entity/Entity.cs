using Godot;
using System;
using System.Diagnostics;

public abstract class Entity : KinematicBody2D
{
    protected int health = 100;

    protected enum AnimationState { Idle, Walk }

    public override void _Process(float delta)
    {
        if (health <= 0)
            Die();

        Process(delta);
    }

    /// <summary>
    /// method to be overriden if you want to extend the _Process method
    /// </summary>
    protected virtual void Process(float delta) { }
    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    protected void Die()
    {
        QueueFree();
    }

    protected static void Flip(Vector2 motionVector, Node2D nodeToFlip)
    {
        if (motionVector.x > 0)
            nodeToFlip.Scale = new Vector2(1, 1);
        else if (motionVector.x < 0)
            nodeToFlip.Scale = new Vector2(-1, 1);
    }
}

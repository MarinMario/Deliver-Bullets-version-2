using Godot;
using System;
using System.Diagnostics;

public class Entity : KinematicBody2D
{
    [Export]
    protected int health = 100;

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

}

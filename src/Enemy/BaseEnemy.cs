using Godot;
using System;


public class BaseEnemy : KinematicBody2D
{
    public int health = 100;

    public override void _Process(float delta)
    {
        if (health <= 0)
            Die();

        Process2(delta);
    }

    // method to be overriden if you want to exted the _Process method
    protected virtual void Process2(float delta)
    {

    }
    public void TakeDamange(int dmg)
    {
        health -= dmg;
    }
    protected void Die()
    {
        QueueFree();
    }
}

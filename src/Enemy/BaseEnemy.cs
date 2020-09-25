using Godot;
using System;


public class BaseEnemy : KinematicBody2D
{
    [Export]
    public int health = 100;
    private float randomDirectionTimer = 0f;
    private Vector2 target;
    private Vector2 directionToFollow = Vector2.Zero;

    public override void _Process(float delta)
    {
        if (health <= 0)
            Die();

        MoveAtRandom(delta);


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

    private void MoveAtRandom(float delta)
    {
        randomDirectionTimer += delta;
        if (randomDirectionTimer > 3f || GetSlideCount() > 0)
        {
            randomDirectionTimer = 0f;

            var rand = new Random();

            directionToFollow.x = rand.Next(-1, 2);
            directionToFollow.y = rand.Next(-1, 2);
        }


        MoveAndSlide(directionToFollow * 100);
     
    }
}

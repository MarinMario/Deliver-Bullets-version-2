using Godot;
using System;


public class BaseEnemy : KinematicBody2D
{
    [Export]
    public int health = 100;
    private float randomDirectionTimer = 0f;
    private KinematicBody2D target;
    private Vector2 directionToFollow = Vector2.Zero;

    public override void _Ready()
    {
        var targetDetector = GetNode<Area2D>("TargetDetector");
        targetDetector.Connect("body_entered", this, "PotentialTargetDetected");
        targetDetector.Connect("body_exited", this, "TargetLeft");
    }
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

    private void PotentialTargetDetected(KinematicBody2D body)
    {
        if (body.IsInGroup("EnemyTarget"))
            target = body;

        GD.Print($"{body.Name} entered");
    }

    private void TargetLeft(KinematicBody2D body)
    {
        if (body.IsInGroup("EnemyTarget"))
            target = null;

        GD.Print($"{body.Name} exited");
    }
}

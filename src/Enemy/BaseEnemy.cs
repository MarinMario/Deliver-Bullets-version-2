using Godot;
using System;


public class BaseEnemy : KinematicBody2D
{
    public int health = 100;
    public int speed = 200;
    private KinematicBody2D target;
    private Vector2 directionToFollow = Vector2.Zero;

    public override void _Ready()
    {
        var targetDetector = GetNode<Area2D>("TargetDetector");
        targetDetector.Connect("body_entered", this, "PotentialTargetDetected");
        targetDetector.Connect("body_exited", this, "TargetLeft");
        AddToGroup("Enemies");
    }
    public override void _Process(float delta)
    {
        if (health <= 0)
            Die();

        if (target == null)
            MoveAtRandom(delta);
        else
            FollowTarget(delta);

        MoveAndSlide(directionToFollow * speed);


        Process2(delta);
    }

    // method to be overriden if you want to exted the _Process method
    protected virtual void Process2(float delta) { }
    public void TakeDamange(int dmg)
    {
        health -= dmg;
    }
    protected void Die()
    {
        QueueFree();
    }


    private float updateTargetDirectionTimer = 0f;
    private void FollowTarget(float delta)
    {
        updateTargetDirectionTimer += delta;
        if (updateTargetDirectionTimer > 0.5f)
        {
            directionToFollow = (target.Position - Position).Normalized();
            updateTargetDirectionTimer = 0f;
        }
    }


    private float randomDirectionTimer = 0f;
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

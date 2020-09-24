using Godot;
using System;

public class Bullet : Area2D
{
    Vector2 targetDirection;
    float despawnTimer = 0f;

    [Export]
    int speed = 1000;
    public override void _Ready()
    {
        targetDirection = (GetGlobalMousePosition() - Position).Normalized();
        LookAt(GetGlobalMousePosition());
    }
    public override void _Process(float delta)
    {
        despawnTimer += delta;
        if (despawnTimer > 5)
            QueueFree();

        Position += targetDirection * speed * delta;
    }

    public void Init(Vector2 initialPosition)
    {
        Position = initialPosition;
    }

    // this is called by body_entered signal
    private void hit(KinematicBody2D body)
    {
        if (body.IsInGroup("enemies"))
        {
            (body as BaseEnemy).TakeDamange(100);
            QueueFree();
        }
        
    }
}

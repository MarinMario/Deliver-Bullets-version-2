using Godot;
using System;

public class Bullet : Area2D
{
    Vector2 targetDirection;
    float despawnTimer = 0f;
    string targetGroup;

    [Export]
    int speed = 1000;
    public override void _Ready()
    {
        Rotation = (float)Math.Atan2(targetDirection.y, targetDirection.x);
    }

    public override void _Process(float delta)
    {
        despawnTimer += delta;
        if (despawnTimer > 5)
            QueueFree();

        Position += targetDirection * speed * delta;
    }

    public void Init(Vector2 position, Vector2 targetDirection, string targetGroup)
    {
        this.Position = position;
        this.targetDirection = targetDirection;
        this.targetGroup = targetGroup;
    }

    // this is called by body_entered signal
    void Hit(Entity body)
    {
        if (body.IsInGroup(targetGroup))
        {
            body.TakeDamage(100);
            QueueFree();
        }
        
    }
}

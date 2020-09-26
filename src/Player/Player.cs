using Godot;
using System;

public class Player : Entity
{
    [Export]
    int maxSpeed = 400;
    [Export]
    int acceleration = 500;
    [Export]
    int friction = 1000;
    Vector2 velocity;

    protected override void Process(float delta)
    {
        //player movement
        Vector2 motionVector;
        motionVector.x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
        motionVector.y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");

        if (motionVector != Vector2.Zero)
            velocity = velocity.MoveToward(motionVector * maxSpeed, acceleration * delta);
        else 
            velocity = velocity.MoveToward(Vector2.Zero, friction * delta);

        velocity = MoveAndSlide(velocity);

        //shoot
        if (Input.IsActionJustPressed("shoot")) Shoot();
    }

    private void Shoot()
    {
        var bullet = (Bullet)(GetNode("/root/Resources").Get("BULLET") as PackedScene).Instance();
        bullet.Init(Position, (GetGlobalMousePosition() - Position).Normalized(), "Enemies");
        GetParent().AddChild(bullet);
    }
}

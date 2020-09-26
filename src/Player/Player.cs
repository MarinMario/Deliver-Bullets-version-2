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

    enum AnimationState { Idle, Walk }

    AnimationState animationState = AnimationState.Idle;
    Vector2 velocity;

    protected override void Process(float delta)
    {
        var motionVector = Vector2.Zero;
        var sprites = GetNode<Node2D>("Sprites");

        motionVector.x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
        motionVector.y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");

        if (motionVector != Vector2.Zero)
        {
            velocity = velocity.MoveToward(motionVector * maxSpeed, acceleration * delta);
            animationState = AnimationState.Walk;

            //I have to also check with an else if so sprites.Scale won't be set to (-1, 1) 
            //when the player moves up or down only
            if (motionVector.x > 0)
                sprites.Scale = new Vector2(1, 1);
            else if (motionVector.x < 0)
                sprites.Scale = new Vector2(-1, 1);
        }
        else
        {
            velocity = velocity.MoveToward(Vector2.Zero, friction * delta);
            animationState = AnimationState.Idle;
        }

        MoveAndSlide(velocity);
        sprites.GetNode<AnimatedSprite>("AnimatedSprite").Play(animationState.ToString());

        if (Input.IsActionJustPressed("shoot")) Shoot();

    }

    private void Shoot()
    {
        var bullet = (Bullet)(GetNode("/root/Resources").Get("BULLET") as PackedScene).Instance();
        bullet.Init(Position, (GetGlobalMousePosition() - Position).Normalized(), "Enemies");
        GetParent().AddChild(bullet);
    }
}

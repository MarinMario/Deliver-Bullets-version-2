using Godot;
using System;

public class Player : Entity
{
    [Export]
    readonly int maxSpeed = 400;
    [Export]
    readonly int acceleration = 500;
    [Export]
    readonly int friction = 1000;

    AnimationState animationState = AnimationState.Idle;
    Vector2 velocity;

    public override void _Ready()
    {
        health = 100000;
    }

    protected override void Process(float delta)
    {
        var motionVector = Vector2.Zero;

        motionVector.x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
        motionVector.y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");

        if (motionVector != Vector2.Zero)
        {
            velocity = velocity.MoveToward(motionVector * maxSpeed, acceleration * delta);
            animationState = AnimationState.Walk;

            Flip(motionVector, GetNode<Node2D>("Animatable"));
        }
        else
        {
            velocity = velocity.MoveToward(Vector2.Zero, friction * delta);
            animationState = AnimationState.Idle;
        }

        MoveAndSlide(velocity);

        GetNode<AnimatedSprite>("Animatable/AnimatedSprite").Play(animationState.ToString());

        if (Input.IsActionJustPressed("shoot")) 
            GetNode<Weapon>("Animatable/Hand/Weapon").Shoot((GetGlobalMousePosition() - Position).Normalized(), "Enemies");

        GetNode<Sprite>("Animatable/Hand").LookAt(GetGlobalMousePosition());

    }
}

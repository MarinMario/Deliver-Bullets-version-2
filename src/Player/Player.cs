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

    Node2D nAnimatable;
    AnimatedSprite nAnimatedSprite;
    Weapon nWeapon;
    Sprite nHand;
    Position2D nShootPosition;

    AnimationState animationState = AnimationState.Idle;
    Vector2 velocity;

    public override void _Ready()
    {
        health = 100000;
        nAnimatable = GetNode<Node2D>("Animatable");
        nAnimatedSprite = GetNode<AnimatedSprite>("Animatable/AnimatedSprite");
        nWeapon = GetNode<Weapon>("Animatable/Hand/Weapon");
        nHand = GetNode<Sprite>("Animatable/Hand");
        nShootPosition = GetNode<Position2D>("Animatable/Hand/Weapon/Position2D");
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

            nAnimatable.Scale = motionVector.x > 0 ? Vector2.One : new Vector2(-1, 1);
        }
        else
        {
            velocity = velocity.MoveToward(Vector2.Zero, friction * delta);
            animationState = AnimationState.Idle;
        }

        MoveAndSlide(velocity);

        nAnimatedSprite.Play(animationState.ToString());

        if (Input.IsActionJustPressed("shoot")) 
            nWeapon.Shoot((GetGlobalMousePosition() - nShootPosition.GlobalPosition).Normalized(), "Enemies");

        nHand.LookAt(GetGlobalMousePosition());

    }
}

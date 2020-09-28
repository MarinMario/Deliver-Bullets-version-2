using Godot;
using System;

public class Player : Entity
{
    [Export]
    private readonly int maxSpeed = 400;
    [Export]
    private readonly int acceleration = 500;
    [Export]
    private readonly int friction = 1000;

    enum AnimationState { Idle, Walk }

    AnimationState animationState = AnimationState.Idle;
    Vector2 velocity;

    protected override void Process(float delta)
    {
        var motionVector = Vector2.Zero;
        var sprite = GetNode<Node2D>("Sprite");

        motionVector.x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
        motionVector.y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");

        if (motionVector != Vector2.Zero)
        {
            velocity = velocity.MoveToward(motionVector * maxSpeed, acceleration * delta);
            animationState = AnimationState.Walk;

            //I have to also check with an else if so sprite.Scale won't be set to (-1, 1) 
            //when the player moves up or down only
            if (motionVector.x > 0)
                sprite.Scale = new Vector2(1, 1);
            else if (motionVector.x < 0)
                sprite.Scale = new Vector2(-1, 1);
        }
        else
        {
            velocity = velocity.MoveToward(Vector2.Zero, friction * delta);
            animationState = AnimationState.Idle;
        }

        MoveAndSlide(velocity);
        sprite.GetNode<AnimatedSprite>("AnimatedSprite").Play(animationState.ToString());

        if (Input.IsActionJustPressed("shoot")) 
            GetNode<Weapon>("Sprite/Hand/Weapon").Shoot((GetGlobalMousePosition() - Position).Normalized(), "Enemies");

        GetNode<Sprite>("Sprite/Hand").LookAt(GetGlobalMousePosition());

    }
}

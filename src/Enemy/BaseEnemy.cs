using Godot;
using System;


public class BaseEnemy : Entity
{
    [Export]
    public BaseEnemyResource enemy;
    [Export]
    public WeaponResource weapon;

    KinematicBody2D target;
    Vector2 directionToFollow = Vector2.Zero;
    AnimationState animationState = AnimationState.Idle;

    public override void _Ready()
    {
        GetNode<AnimatedSprite>("Animatable/AnimatedSprite").Frames = enemy.animations;
        health = enemy.health;
    }
    protected override void Process(float delta)
    {
        if (target != null)
        {
            FollowTarget(delta);
            FindAttackOpportunity(delta);
        }
        else
            MoveAtRandom(delta);

        MoveAndSlide(directionToFollow * enemy.speed);

        GetNode<Weapon>("Animatable/Weapon").weapon = weapon;

        animationState = directionToFollow != Vector2.Zero 
            ? AnimationState.Walk 
            : AnimationState.Idle;
        GetNode<AnimatedSprite>("Animatable/AnimatedSprite").Play(animationState.ToString());

        Flip(directionToFollow, GetNode<Node2D>("Animatable"));
    }

    float updateTargetDirectionTimer = 0f;
    void FollowTarget(float delta)
    {
        updateTargetDirectionTimer += delta;
        if (updateTargetDirectionTimer > 0.5f)
        {
            directionToFollow = (target.Position - Position).Normalized();
            updateTargetDirectionTimer = 0f;
        }
    }


    float randomDirectionTimer = 0f;
    void MoveAtRandom(float delta)
    {
        randomDirectionTimer += delta;
        if (randomDirectionTimer > enemy.changeDirectionTime || GetSlideCount() > 0)
        {
            randomDirectionTimer = 0f;

            var rand = new Random();

            directionToFollow.x = rand.Next(-1, 2);
            directionToFollow.y = rand.Next(-1, 2);
            directionToFollow = directionToFollow.Normalized();
        }
    }

    void PotentialTargetDetected(KinematicBody2D body)
    {
        if (body.IsInGroup("EnemyTarget"))
            target = body;

        //GD.Print($"{body.Name} entered");
    }

    void PotentialTargetExited(KinematicBody2D body)
    {
        if (body.IsInGroup("EnemyTarget"))
            target = null;

        //GD.Print($"{body.Name} exited");
    }

    float attackTimer = 0f;
    void FindAttackOpportunity(float delta)
    {
        attackTimer += delta;
        if (attackTimer > enemy.attackTime)
        {
            attackTimer = 0f;

            GetNode<Weapon>("Animatable/Weapon").Shoot(directionToFollow, "EnemyTarget");
            //var bullet = (Bullet)GetNode<Resources>("/root/Resources").Bullet.Instance();
            //bullet.Init(Position, directionToFollow, "EnemyTarget");
            //GetParent().AddChild(bullet);
        }
    }
}

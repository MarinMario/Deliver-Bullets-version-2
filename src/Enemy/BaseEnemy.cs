using Godot;
using System;


public class BaseEnemy : Entity
{
    [Export]
    public BaseEnemyResource enemy;
    [Export]
    public WeaponResource weapon;

    KinematicBody2D nTarget;
    Weapon nWeapon;
    AnimatedSprite nAnimatedSprite;
    Node2D nAnimatable;
    Sprite nHand;

    Vector2 directionToFollow = Vector2.Zero;
    AnimationState animationState = AnimationState.Idle;
    int initialSpeed;

    public override void _Ready()
    {
        nWeapon = GetNode<Weapon>("Animatable/Hand/Weapon");
        nAnimatedSprite = GetNode<AnimatedSprite>("Animatable/AnimatedSprite");
        nAnimatable = GetNode<Node2D>("Animatable");
        nHand = GetNode<Sprite>("Animatable/Hand");

        health = enemy.health;
        initialSpeed = enemy.speed;
        nAnimatedSprite.Frames = enemy.animations;
        nHand.Texture = enemy.handTexture;
    }
    protected override void Process(float delta)
    {
        nWeapon.weapon = weapon;
        
        if (nTarget != null)
        {
            FollowTarget(delta);
            FindAttackOpportunity(delta);
        }
        else
            MoveAtRandom(delta);

        MoveAndSlide(directionToFollow * enemy.speed);

        animationState = directionToFollow != Vector2.Zero && enemy.speed != 0
            ? AnimationState.Walk 
            : AnimationState.Idle;
        nAnimatedSprite.Play(animationState.ToString());

        nAnimatable.Scale = directionToFollow.x > 0 ? Vector2.One : new Vector2(-1, 1);
    }

    float updateTargetDirectionTimer = 0f;
    void FollowTarget(float delta)
    {
        updateTargetDirectionTimer += delta;
        if (updateTargetDirectionTimer > 0.5f)
        {
            directionToFollow = (nTarget.GlobalPosition - GlobalPosition).Normalized();

            var dis = GlobalPosition.DistanceSquaredTo(nTarget.GlobalPosition);
            enemy.speed = dis > 50000 ? initialSpeed : 0;

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
            nTarget = body;

        //GD.Print($"{body.Name} entered");
    }

    void PotentialTargetExited(KinematicBody2D body)
    {
        if (body.IsInGroup("EnemyTarget"))
            nTarget = null;

        //GD.Print($"{body.Name} exited");
    }

    float attackTimer = 0f;
    void FindAttackOpportunity(float delta)
    {
        attackTimer += delta;
        if (attackTimer > enemy.attackTime && directionToFollow != Vector2.Zero)
        {
            attackTimer = 0f;
            nWeapon.Shoot(directionToFollow, "EnemyTarget");

            var rotation = (float)Math.Atan2(directionToFollow.y, directionToFollow.x);
            nHand.Rotation = directionToFollow.x >= 0 ? rotation : 1/rotation;
        }
    }
}

using Godot;
using System;


public class BaseEnemy : Entity
{
    [Export]
    public BaseEnemyResource enemy;
    [Export]
    public WeaponResource weapon;

    private KinematicBody2D target;
    private Vector2 directionToFollow = Vector2.Zero;

    public override void _Ready()
    {
        var targetDetector = GetNode<Area2D>("TargetDetector");
        GetNode<Sprite>("Sprite").Texture = enemy.texture;
        targetDetector.Connect("body_entered", this, nameof(PotentialTargetDetected));
        targetDetector.Connect("body_exited", this, nameof(PotentialTargetExited));
        CollisionLayer = 2;
        CollisionMask = 2;
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

        GetNode<Weapon>("Weapon").weapon = weapon;
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
        if (randomDirectionTimer > enemy.changeDirectionTime || GetSlideCount() > 0)
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

        //GD.Print($"{body.Name} entered");
    }

    private void PotentialTargetExited(KinematicBody2D body)
    {
        if (body.IsInGroup("EnemyTarget"))
            target = null;

        //GD.Print($"{body.Name} exited");
    }

    private float attackTimer = 0f;
    private void FindAttackOpportunity(float delta)
    {
        attackTimer += delta;
        if (attackTimer > enemy.attackTime)
        {
            attackTimer = 0f;

            GetNode<Weapon>("Weapon").Shoot(directionToFollow, "EnemyTarget");
            //var bullet = (Bullet)GetNode<Resources>("/root/Resources").Bullet.Instance();
            //bullet.Init(Position, directionToFollow, "EnemyTarget");
            //GetParent().AddChild(bullet);
        }
    }
}

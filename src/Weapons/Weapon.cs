using Godot;
using System;

public class Weapon : Node2D
{
    [Export]
    WeaponResource weapon;
    public override void _Ready()
    {
        GetNode<Sprite>("Sprite").Texture = weapon.texture;
    }

    public void Shoot(Vector2 targetDirection, string targetGroup)
    {
        var bullet = (Bullet)ResourceLoader.Load<PackedScene>("res://Bullet/Bullet.tscn").Instance();
        bullet.Init(GetNode<Position2D>("Position2D").GlobalPosition, targetDirection, targetGroup);
        GetNode<GameManager>("/root/GameManager").world.AddChild(bullet);
    }
}

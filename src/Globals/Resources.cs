using Godot;
using System;

public class Resources : Node
{
    public readonly PackedScene BULLET = ResourceLoader.Load<PackedScene>("res://Bullet/Bullet.tscn");
}

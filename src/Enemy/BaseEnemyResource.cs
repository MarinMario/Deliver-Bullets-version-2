using Godot;
using System;

public class BaseEnemyResource : Resource
{
    [Export]
    public SpriteFrames animations;
    [Export]
    public int speed = 200;
    [Export]
    public float changeDirectionTime = 3f;
    [Export]
    public float attackTime = 2f;
    [Export]
    public int health = 100;
}

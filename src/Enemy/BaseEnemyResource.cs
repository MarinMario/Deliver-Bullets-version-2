using Godot;
using System;

public class BaseEnemyResource : Resource
{
    [Export]
    public Texture texture;
    [Export]
    public int speed = 200;
    [Export]
    public float changeDirectionTime = 3f;
    [Export]
    public float attackTime = 2f;
}

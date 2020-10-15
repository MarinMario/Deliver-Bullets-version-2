using Godot;
using System;

public class World : YSort
{
    public override void _Ready()
    {
        GetNode<GameManager>("/root/GameManager").world = this;
    }
}
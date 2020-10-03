using Godot;
using System;

public class GameManager : Node
{
    public YSort world;

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("ui_accept"))
            GetTree().ReloadCurrentScene();
    }
}

using Godot;
using System;

public class Item : Area2D
{
    [Export]
    public ItemResource itemRes;
    bool mouseOver = false;
    bool followMouse = false;
    public override void _Ready()
    {
        GetNode<Sprite>("Sprite").Texture = itemRes.texture;
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("click") && mouseOver)
            followMouse = true;
        if (Input.IsActionJustReleased("click"))
            followMouse = false;

        if (followMouse)
            GlobalPosition = GetGlobalMousePosition();
    }

    void OnMouseEntered()
    {
        mouseOver = true;
    }

    void OnMouseExited()
    {
        mouseOver = false;
    }
}

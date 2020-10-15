using Godot;
using System;

public class InventoryCell : Area2D
{
    public Item item;
    Color color = Color.ColorN("white");

    public override void _Process(float delta)
    {
        GetNode<ColorRect>("ColorRect").Color = color;
        if (item != null)
           color = Color.ColorN("red");
    }

    void OnItemEntered(Area2D area)
    {
        color = Color.ColorN("blue");
    }

    void OnItemExited(Area2D area)
    {
        color = Color.ColorN("white");
    }
}

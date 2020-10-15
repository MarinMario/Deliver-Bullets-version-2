using Godot;
using System;

public class Item : Area2D
{
    [Export]
    public ItemResource itemRes;

    bool mouseOver = false;
    bool followMouse = false;
    InventoryCell hoveredSlot;
    InventoryCell usedSlot;
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

        if (!followMouse && hoveredSlot != null && hoveredSlot.item == null)
        {
            usedSlot = hoveredSlot;
            usedSlot.item = this;
        }

        if (followMouse)
        {
            GlobalPosition = GetGlobalMousePosition();
            if (usedSlot != null)
                usedSlot.item = null;
        }
        else if (usedSlot != null)
            GlobalPosition = usedSlot.GlobalPosition;

        if (GetOverlappingAreas().Count > 0)
            hoveredSlot = GetOverlappingAreas()[0] as InventoryCell;
    }

    void OnMouseEntered()
    {
        mouseOver = true;
    }

    void OnMouseExited()
    {
        mouseOver = false;
    }

    void PotentialSlotEntered(InventoryCell slot)
    {
        //hoveredSlot = slot;
    }

    void PotentialSlotExited(InventoryCell slot)
    {
        if (slot == usedSlot)
            usedSlot.item = null;
    }
}

using Godot;
using System;

public class WeaponResource : Resource
{
    [Export]
    public int maxAmmo = 30;
    [Export]
    public Texture texture;

    [Export]
    public WeaponType type = WeaponType.Pistol;
}

public enum WeaponType { Pistol, MachineGun, ShotGun }

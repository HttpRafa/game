using Godot;
using Godot.Collections;
using System;

public partial class WeaponSystem : Node3D
{

	[ExportGroup("Weapon Settings")]
	[Export]
	private int _currentWeaponIndex = 0; // Index of the currently active weapon
	[Export]
	private Array<Weapon> _weapons = new Array<Weapon>();

	private float _lastFireTime = 0.0f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (_weapons.Count == 0)
		{
			GD.PrintErr("No weapons assigned to the WeaponSystem.");
			return;
		}
		else
		{
			for (int i = 0; i < _weapons.Count; i++)
			{
				_weapons[i].SetProcess(i == _currentWeaponIndex); // Disable processing for all weapons except the current one
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		var weapon = _weapons[_currentWeaponIndex];

		// Increment the last fire time
		_lastFireTime += (float)delta;

		// Check if the fire action is pressed and if enough time has passed since the last fire
		if (Input.IsActionPressed("fire") && _lastFireTime >= (60.0f / weapon.FireRate))
		{
			_lastFireTime = 0.0f; // Reset the last fire time
			weapon.Fire();
		}
	}
	
	public void SwitchWeapon(int index)
	{
		if (index < 0 || index >= _weapons.Count)
		{
			GD.PrintErr("Invalid weapon index: " + index);
			return;
		}

		// Disable the current weapon
		_weapons[_currentWeaponIndex].SetProcess(false);

		// Update the current weapon index
		_currentWeaponIndex = index;

		// Enable the new weapon
		_weapons[_currentWeaponIndex].SetProcess(true);
		
		GD.Print("Switched to weapon: " + _weapons[_currentWeaponIndex].Name);
	}
}

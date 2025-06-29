using Godot;
using System;

public partial class Weapon : Node3D
{

	[ExportGroup("Spawning")]
	[Export]
	public int _firerate = 550; // Projectiles per minute
	[Export]
	private Node3D _muzzle;

	[ExportGroup("Projectile")]
	[Export]
	private PackedScene _projectileScene;
	[Export]
	private float _projectileSpeed = 40.0f;


	public void Fire()
	{
		// Create the projectile instance
		var projectile = _projectileScene.Instantiate<Projectile>();
		if (projectile != null)
		{
			// Add the projectile to the scene tree
			GetTree().Root.AddChild(projectile);

			// Set the position and rotation of the projectile
			projectile.GlobalTransform = _muzzle.GlobalTransform;

			// Set the speed of the projectile
			projectile.Direction = projectile.Transform.Basis.Z;
			projectile.Speed = _projectileSpeed;
		}
	}
	
	public int FireRate
	{
		get => _firerate;
		set => _firerate = value;
	}
}

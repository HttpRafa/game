using Godot;
using System;

public partial class Weapon : Node3D
{

	[ExportGroup("Spawning")]
	[Export]
	private Node3D _spawnPoint;

	[ExportGroup("Projectile")]
	[Export]
	private PackedScene _projectileScene;
	[Export]
	private float _projectileSpeed = 40.0f;

	public override void _PhysicsProcess(double delta)
	{
		if (Input.IsActionJustPressed("fire"))
		{
			if (_spawnPoint != null && _projectileScene != null)
			{
				// Create the projectile instance
				var projectile = _projectileScene.Instantiate<Projectile>();
				if (projectile != null)
				{
					// Add the projectile to the scene tree
					GetTree().Root.AddChild(projectile);

					// Set the position and rotation of the projectile
					projectile.GlobalTransform = _spawnPoint.GlobalTransform;

					// Set the speed of the projectile
					projectile.Direction = projectile.Transform.Basis.Z;
					projectile.Speed = _projectileSpeed;
				}
			}
		}
	}
}

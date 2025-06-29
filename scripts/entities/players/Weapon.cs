using Godot;
using System;

public partial class Weapon : Node3D
{

	[ExportGroup("Spawning")]
	[Export]
	private int _ppm = 550; // Projectiles per minute
	[Export]
	private Node3D _spawnPoint;

	[ExportGroup("Projectile")]
	[Export]
	private PackedScene _projectileScene;
	[Export]
	private float _projectileSpeed = 40.0f;

	private float _lastFireTime = 0.0f;

	public override void _PhysicsProcess(double delta)
	{
		// Increment the last fire time
		_lastFireTime += (float)delta;

		if (Input.IsActionPressed("fire") && _lastFireTime >= (60.0f / _ppm))
		{
			_lastFireTime = 0.0f; // Reset the last fire time
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

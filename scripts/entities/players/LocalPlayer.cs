using Godot;
using Microsoft.VisualBasic;
using System;

public partial class LocalPlayer : CharacterBody3D
{

	[ExportGroup("Targeting")]
	[Export]
	private Node3D _target;
	[Export]
	private Node3D _weapon;

	[ExportGroup("Movement")]
	[Export]
	private float _speed = 5.0f;
	[Export]
	private float _jumpVelocity = 4.5f;

	public override void _Process(double delta)
	{
		_weapon.LookAt(_target.GlobalPosition + new Vector3(0, 0.25f, 0), Vector3.Up, useModelFront: true);

		var velocity = Velocity;
		var rotation = Rotation;

		// Rotate the player to face the target.
		if (_target != null)
		{
			Vector3 targetDirection = (_target.GlobalPosition - GlobalPosition).Normalized();
			if (targetDirection != Vector3.Zero)
			{
				rotation.Y = Mathf.Atan2(targetDirection.X, targetDirection.Z);
			}
		}

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = _jumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("left", "right", "up", "down");
		Vector3 direction = new Vector3(inputDir.X, 0, inputDir.Y).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * _speed;
			velocity.Z = direction.Z * _speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, _speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, _speed);
		}

		Velocity = velocity;
		Rotation = rotation;
		MoveAndSlide();
	}
}

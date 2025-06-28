using Godot;
using System;

public partial class Projectile : Area3D
{

	public Vector3 Direction { get; set; } = Vector3.Forward;
	public float Speed { get; set; } = 40.0f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Connect the body entered signal to handle collisions
		BodyEntered += OnBodyEntered;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		// Move the projectile in the specified direction at the specified speed
		GlobalPosition += Direction * Speed * (float)delta;
	}

	private void OnBodyEntered(Node3D body)
	{
		if (body is LocalPlayer)
			return; // Ignore collisions with the player
			
		QueueFree();
	}

}

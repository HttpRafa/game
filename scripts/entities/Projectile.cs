using Godot;
using System;

public partial class Projectile : Area3D
{

	public Vector3 Direction { get; set; } = Vector3.Forward;
	public float Speed { get; set; } = 40.0f;

	[ExportGroup("Lifetime")]
	[Export]
	private float _lifetime = 5.0f; // Lifetime in seconds

	private Timer _lifetimeTimer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Connect the body entered signal to handle collisions
		BodyEntered += OnBodyEntered;

		// Create a timer to handle the lifetime of the projectile
		_lifetimeTimer = new Timer();
		AddChild(_lifetimeTimer); // Add the timer to the projectile
		_lifetimeTimer.WaitTime = _lifetime; // Set the lifetime to 5 seconds
		_lifetimeTimer.OneShot = true; // Ensure the timer only runs
		_lifetimeTimer.Timeout += () => QueueFree(); // Free the projectile when the timer
		_lifetimeTimer.Start(); // Start the timer
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

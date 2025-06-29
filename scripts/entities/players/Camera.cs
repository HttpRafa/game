using Godot;
using System;

public partial class Camera : Camera3D
{

	[ExportGroup("Targeting")]
	[Export]
	private Node3D _player;
	[Export]
	private Node3D _target;

	[ExportGroup("Positioning")]
	[Export]
	private Vector3 _offset = new(0, 15, 4.5f);
	[Export]
	private float _vectorMagnitude = 0.15f;
	[Export]
	private float _speed = 4.0f;

	[ExportGroup("Target Camera")]
	[Export]
	private Camera3D _targetCamera;

	public override void _Ready()
	{
		if (_player == null)
		{
			GD.PrintErr("Player node is not set for Camera.");
		}
		if (_target == null)
		{
			GD.PrintErr("Target node is not set for Camera.");
		}
		if (_targetCamera == null)
		{
			GD.PrintErr("Target camera node is not set for Camera.");
		}
	}

	public override void _Process(double delta)
	{
		if (_player == null || _target == null || _targetCamera == null)
			return;

		// Copy transform from the this camera to target camera
		_targetCamera.GlobalTransform = GlobalTransform;

		// Calculate the position to look at based on the target and player positions
		var cameraLookAtPosition = (_target.GlobalPosition - _player.GlobalPosition) * _vectorMagnitude + _player.GlobalPosition;

		// Calculate the new global position of the camera
		// Lerp towards the target position with the offset applied
		GlobalPosition = GlobalPosition.Lerp(cameraLookAtPosition + _offset, (float)delta * _speed);
	}
}

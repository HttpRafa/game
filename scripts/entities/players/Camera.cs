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
	private Vector3 _offset = new(0, 15, 3);
	[Export]
	private float _vectorMagnitude = 0.20f;
	[Export]
	private float _speed = 4.0f;

	public override void _Process(double delta)
	{
		if (_player == null || _target == null)
			return;

		// Calculate the position to look at based on the target and player positions
		var cameraLookAtPosition = (_target.GlobalPosition - _player.GlobalPosition) * _vectorMagnitude + _player.GlobalPosition;

		// Calculate the new global position of the camera
		// Lerp towards the target position with the offset applied
		GlobalPosition = GlobalPosition.Lerp(cameraLookAtPosition + _offset, (float)delta * _speed);
	}
}

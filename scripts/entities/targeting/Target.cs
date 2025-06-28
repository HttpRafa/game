using Godot;
using Microsoft.VisualBasic;
using System;

public partial class Target : Node3D
{
	[ExportGroup("Required")]
	[Export]
	private Camera3D _camera;

	[ExportGroup("Raycasting")]
	[Export]
	private float _rayLength = 1000.0f;

	public override void _Process(double delta)
	{
		if (_camera == null)
			return;

		var state = GetWorld3D().DirectSpaceState;

		var mousePosition = GetViewport().GetMousePosition();
		var from = _camera.ProjectRayOrigin(mousePosition);
		var to = from + _camera.ProjectRayNormal(mousePosition) * _rayLength;
		var query = PhysicsRayQueryParameters3D.Create(from, to);
		var result = state.IntersectRay(query);
		if (result.Count > 0)
		{
			GlobalPosition = (Vector3)result["position"];
		}
		
	}
}

using Godot;
using Godot.Collections;
using System;

public partial class World : Node3D
{

	[ExportGroup("Player Settings")]
	[Export]
	private Node3D _localPlayer; // Scene for remote players

	[ExportGroup("Levels")]
	[Export]
	private Node3D _levelRoot; // Root node for levels, if needed
	[Export]
	private Array<PackedScene> _levels = new Array<PackedScene>();

	private Node3D _currentLevel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_localPlayer.SetProcess(false); // Disable processing for the local player until explicitly enabled

		if (_levels.Count == 0)
		{
			GD.PrintErr("No levels assigned to the World.");
			return;
		}
	}

	public void LoadLevel(int index)
	{
		if (index < 0 || index >= _levels.Count)
		{
			GD.PrintErr("Invalid level index: " + index);
			return;
		}

		// Remove the current level if it exists
		if (_currentLevel != null)
		{
			_currentLevel.QueueFree();
			_currentLevel = null;
			GD.Print("Unloaded previous level.");
		}

		_currentLevel = _levels[index].Instantiate<Node3D>();
		if (_currentLevel != null)
		{
			_levelRoot.AddChild(_currentLevel);
			GD.Print("Loaded level: " + _levels[index].ResourceName);
		}
		else
		{
			GD.PrintErr("Failed to instantiate level at index: " + index);
		}
	}

	public void EnableLocalPlayer()
	{
		if (_localPlayer == null)
		{
			GD.PrintErr("Local player is not assigned.");
			return;
		}

		_localPlayer.SetProcess(true); // Enable processing for the local player
	}

}

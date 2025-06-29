using Godot;
using System;

public partial class Manager : Node3D
{

	[ExportGroup("Menu")]
	[Export]
	private PackedScene _mainMenuScene;

	[ExportGroup("World")]
	[Export]
	private PackedScene _worldScene;

	[ExportGroup("Multiplayer")]
	[Export]
	private PackedScene _remotePlayerScene;

	[ExportGroup("Server")]
	[Export]
	private string _bind = "0.0.0.0"; // Default port for the server
	[Export]
	private int _port = 7777; // Default port for the server
	[Export]
	private int _maxClients = 32; // Maximum number of clients

	[ExportGroup("Client")]
	private string _serverAddress = "127.0.0.1"; // Default server address for clients
	[Export]
	private int _serverPort = 7777; // Default server port for clients

	private ENetMultiplayerPeer _multiplayerPeer;
	private World _world;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Initializing Manager...");
		if (OS.HasFeature("dedicated_server"))
		{
			GD.Print("Running as a dedicated server.");
			StartServer(_bind, _port);
		}
		else
		{
			GD.Print("Running as a client.");
			Connect(_serverAddress, _serverPort);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public Error Connect(string address, int port)
	{
		if (_multiplayerPeer == null)
		{
			_multiplayerPeer = new ENetMultiplayerPeer();
			var error = _multiplayerPeer.CreateClient(address, port);
			if (error != Error.Ok)
			{
				return error;
			}
			Multiplayer.MultiplayerPeer = _multiplayerPeer;
			GD.Print("Preparing world...");
			_world = _worldScene.Instantiate<World>();
			AddChild(_world); // Add the world to the scene tree
			if (_world != null)
			{
				_world.LoadLevel(0); // Load the first level by default
				_world.EnableLocalPlayer(); // Spawn the local player in the world
			}
			else
			{
				GD.PrintErr("World is not assigned in the Manager.");
			}
			GD.Print($"Connecting to server at {address}:{port}...");
		}
		else
		{
			GD.Print("Already connected or trying to connect.");
		}
		return Error.Ok;
	}

	public Error StartServer(string bind, int port)
	{
		if (_multiplayerPeer == null)
		{
			_multiplayerPeer = new ENetMultiplayerPeer();
			_multiplayerPeer.SetBindIP(bind);
			var error = _multiplayerPeer.CreateServer(port, _maxClients);
			if (error != Error.Ok)
			{
				return error;
			}
			Multiplayer.MultiplayerPeer = _multiplayerPeer;
			GD.Print("Preparing world...");
			_world = _worldScene.Instantiate<World>();
			AddChild(_world); // Add the world to the scene tree
			if (_world != null)
			{
				_world.LoadLevel(0); // Load the first level by default
			}
			else
			{
				GD.PrintErr("World is not assigned in the Manager.");
			}
			GD.Print($"Server started on {bind}:{port} with a maximum of {_maxClients} clients.");
		}
		else
		{
			GD.Print("Server is already running or trying to start.");
		}
		return Error.Ok;
	}
}

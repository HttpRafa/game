using Godot;

namespace Game.scripts.entities.player;

public partial class LocalPlayer : Node3D
{

	[Signal]
	public delegate void MouseInputStateChangedEventHandler(bool enabled);

	public static RemotePlayer PlayerBody;

	[Export] private float _gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
	[Export] private float _speed = 6.0f;
	[Export] private float _jumpVelocity = 4.5f;
	[Export] private float _rotationSpeed = 15f;

	[Export] private Crosshair _crosshair;
	
	private bool _isUsingMouse = true;
	
	private Vector3 _rotationTarget;

	public override void _Ready()
	{
		_crosshair.RotationTargetChanged += target =>
		{
			_rotationTarget = target;
		};
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_isUsingMouse && (Input.IsActionPressed("look_up") || Input.IsActionPressed("look_down") || Input.IsActionPressed("look_right") || Input.IsActionPressed("look_left")))
		{
			_isUsingMouse = false;
			_crosshair.Hide();
			
			EmitSignal(SignalName.MouseInputStateChanged, false);
		} else if (!_isUsingMouse && (Input.GetLastMouseVelocity() > Vector2.Zero))
		{
			_isUsingMouse = true;
			_crosshair.Show();

			EmitSignal(SignalName.MouseInputStateChanged, true);
		}

		if (PlayerBody != null && PlayerBody.IsMultiplayerAuthority())
		{
			MovePlayer(delta);
			RotatePlayer(delta);	
		}
	}

	private void RotateWeapon()
	{
		if(_isUsingMouse)
		{
			_crosshair.GlobalTransform = _crosshair.GlobalTransform.LookingAt(_rotationTarget, Vector3.Up);
		}
		else
		{
			Rotation = new Vector3(0f, 0f, 0f);
		}
	}
	
	private void RotatePlayer(double delta)
	{
		if (_isUsingMouse)
		{
			Vector3 target = _rotationTarget;
			target.Y = PlayerBody.Position.Y;
			Quaternion rotation = PlayerBody.Transform.LookingAt(target, Vector3.Up).Basis.GetRotationQuaternion();
			PlayerBody.Rotation = Quaternion.FromEuler(PlayerBody.Rotation).Slerp(rotation, _rotationSpeed * (float)delta).GetEuler();
		}
		else
		{
			Vector2 gamepadInput = Input.GetVector("look_left", "look_right", "look_up", "look_down");
			if (gamepadInput != Vector2.Zero)
			{
				Vector3 gamepadLook = new Vector3(gamepadInput.X, 0, gamepadInput.Y) + PlayerBody.Position;
				Quaternion rotation = PlayerBody.Transform.LookingAt(gamepadLook, Vector3.Up).Basis.GetRotationQuaternion();
				PlayerBody.Rotation = Quaternion.FromEuler(PlayerBody.Rotation).Slerp(rotation, _rotationSpeed * (float)delta).GetEuler();	
			}
		}
	}

	private void MovePlayer(double delta)
	{
		Vector3 velocity = PlayerBody.Velocity;		
		
		// Add the gravity
		if (!PlayerBody.IsOnFloor())
			velocity.Y -= _gravity * (float)delta;

		// Handle Jump
		if (Input.IsActionJustPressed("jump") && PlayerBody.IsOnFloor())
			velocity.Y = _jumpVelocity;
		
		Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_back");
		Vector3 direction = new Vector3(inputDir.X, 0, inputDir.Y).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * _speed;
			velocity.Z = direction.Z * _speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(PlayerBody.Velocity.X, 0, _speed);
			velocity.Z = Mathf.MoveToward(PlayerBody.Velocity.Z, 0, _speed);
		}

		PlayerBody.Velocity = velocity;
		PlayerBody.MoveAndSlide();
	}
	
}
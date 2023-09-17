using Godot;

public partial class Player : CharacterBody2D
{
	private Sprite2D _sprite = default!;
	private PackedScene _flameScene = default!;
	private bool _facingLeft;

	[Export] public int Speed { get; set; } = 128;

	public override void _Ready()
	{
		_sprite = GetNode<Sprite2D>("Sprite2D");
		_flameScene = GD.Load<PackedScene>("res://Scenes/Entities/Flame.tscn");

		var flameTimer = GetNode<Timer>("FlameTimer");
		flameTimer.Timeout += OnFlameTimeout;
	}

	private void OnFlameTimeout()
	{
		// Only add flame when moving
		if (Velocity != Vector2.Zero)
		{
			var instance = _flameScene.Instantiate<Node2D>();
			instance.Transform = Transform;
			AddSibling(instance);
		}
	}

	public override void _Process(double delta)
	{
		GetInput();
		FaceDirection();
		MoveAndSlide();
	}

	private void GetInput()
	{
		// Movement
		var inputVector = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		Velocity = inputVector * Speed;

		// Facing
		var fireLeft = Input.IsActionPressed("fire_left");
		var fireRight = Input.IsActionPressed("fire_right");
		if (fireLeft && !fireRight) _facingLeft = true;
		if (fireRight && !fireLeft) _facingLeft = false;
	}

	private void FaceDirection()
	{
		_sprite.FlipH = !_facingLeft;
	}
}

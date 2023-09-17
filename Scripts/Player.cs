using Godot;

public partial class Player : CharacterBody2D
{
	private Sprite2D _sprite = default!;
	private bool _facingLeft;

	[Export] public int Speed { get; set; } = 128;

	public override void _Ready()
	{
		_sprite = GetNode<Sprite2D>("Sprite2D");
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
		var spriteLeft = _sprite.RegionRect.Position.X == 0;
		if (spriteLeft && !_facingLeft)
		{
			_sprite.RegionRect = _sprite.RegionRect with { Position = new Vector2(13, 0) };
		}
		else if (!spriteLeft && _facingLeft)
		{
			_sprite.RegionRect = _sprite.RegionRect with { Position = new Vector2(0, 0) };
		}
	}
}

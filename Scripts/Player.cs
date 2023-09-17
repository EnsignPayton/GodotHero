using Godot;

public partial class Player : CharacterBody2D
{
	[Export] public int Speed { get; set; } = 128;

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		var inputVector = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		Velocity = inputVector * Speed;
		MoveAndSlide();
	}
}

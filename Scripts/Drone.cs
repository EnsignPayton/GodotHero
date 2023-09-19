using System.Linq;
using Godot;

public partial class Drone : Node2D
{
	private Player _player = default!;
	private DroneState _state = DroneState.Blinking;
	private Vector2 _direction = Vector2.Zero;

	[Export] public int Speed { get; set; } = 96;

	public override void _Ready()
	{
		var timer = GetNode<Timer>("Timer");
		timer.Timeout += OnTimeout;

		_player = GetTree().GetNodesInGroup("Player").OfType<Player>().First();
	}

	private void OnTimeout()
	{
		if (_state is DroneState.Blinking)
		{
			_state = DroneState.Chasing;
			_direction = (_player.Position - Position).Normalized();
		}
		else if (_state is DroneState.Chasing)
		{
			_direction = (_player.Position - Position).Normalized();
		}
	}

	public override void _Process(double delta)
	{
		Translate((float)delta * Speed * _direction);
	}

	private enum DroneState
	{
		Blinking,
		Chasing,
	}
}

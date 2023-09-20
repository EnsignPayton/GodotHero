namespace GodotHero.Scenes.Entities;

public partial class Drone : Node2D
{
	private Player _player = default!;
	private DroneState _state = DroneState.Blinking;
	private Vector2 _direction = Vector2.Zero;
	private int _currentHealth;

	[Export] public int Speed { get; set; } = 96;
	[Export] public int MaxHealth { get; set; } = 2;

	public override void _Ready()
	{
		_currentHealth = MaxHealth;

		var timer = GetNode<Timer>("Timer");
		timer.Timeout += OnTimeout;

		_player = GetTree().GetNodesInGroup("Player").OfType<Player>().First();

		var area = GetNode<Area2D>("Area2D");
		area.BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body)
	{
		GD.Print(body.Name);
		if (body is TileMap)
		{
			var apos = GlobalPosition;
			var bpos = body.GlobalPosition;
			var dir = (apos - bpos).Normalized();
			_direction = (-dir);
		}
	}

	private void OnTimeout()
	{
		if (_state is DroneState.Blinking)
		{
			_state = DroneState.Chasing;
			_direction = (_player.GlobalPosition - GlobalPosition).Normalized();
		}
		else if (_state is DroneState.Chasing)
		{
			_direction = (_player.GlobalPosition - GlobalPosition).Normalized();
		}
	}

	public override void _Process(double delta)
	{
		Translate((float)delta * Speed * _direction);
	}

	public void Hit()
	{
		_currentHealth--;
		if (_currentHealth < 1)
			QueueFree();
	}

	private enum DroneState
	{
		Blinking,
		Chasing,
	}
}

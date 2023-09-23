namespace GodotHero.Scenes.Entities;

public partial class Drone : CharacterBody2D
{
	private CollisionShape2D _collisionShape = default!;
	private Sprite2D _sprite = default!;
	private AnimatedSprite2D _deathSprite = default!;
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

		_collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
		_sprite = GetNode<Sprite2D>("Sprite2D");
		_deathSprite = GetNode<AnimatedSprite2D>("DeathSprite");
		_player = GetTree().GetNodesInGroup("Player").OfType<Player>().First();
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
		else if (_state is DroneState.Dying)
		{
			_direction = Vector2.Zero;
		}
	}

	public override void _Process(double delta)
	{
		if (_state is not DroneState.Dying)
		{
			var collision = MoveAndCollide((float)delta * Speed * _direction);
			if (collision is not null)
			{
				_direction = _direction.Bounce(collision.GetNormal());
			}
		}
	}

	public void Hit()
	{
		_currentHealth--;
		if (_currentHealth < 1)
			Die();
	}

	private void Die()
	{
		_state = DroneState.Dying;
		_collisionShape.SetDeferred("disabled", true);
		_sprite.Visible = false;
		_deathSprite.Visible = true;
		_deathSprite.Play();
		_deathSprite.AnimationFinished += OnAnimationFinished;
	}

	private void OnAnimationFinished()
	{
		QueueFree();
	}

	private enum DroneState
	{
		Blinking,
		Chasing,
		Dying,
	}
}

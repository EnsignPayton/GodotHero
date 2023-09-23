namespace GodotHero.Scenes.Entities;

public partial class Reaver : CharacterBody2D, IEnemy
{
	private Player _player = default!;
	private EnemyState _state = EnemyState.Blinking;
	private Vector2 _direction = Vector2.Zero;
	private int _currentHealth;

	[Export] public int Speed { get; set; } = 64;
	[Export] public int MaxHealth { get; set; } = 4;

	[Export] public AnimatedSprite2D BlinkSprite { get; set; } = default!;
	[Export] public Sprite2D MainSprite { get; set; } = default!;
	[Export] public AnimatedSprite2D DeathSprite { get; set; } = default!;
	[Export] public Timer BlinkTimer { get; set; } = default!;
	[Export] public Timer TargetTimer { get; set; } = default!;
	[Export] public CollisionShape2D CollisionShape { get; set; } = default!;

	public override void _Ready()
	{
		_currentHealth = MaxHealth;
		_player = (Player)GetTree().GetFirstNodeInGroup("Player");

		BlinkTimer.Timeout += BlinkTimerOnTimeout;
		TargetTimer.Timeout += TargetTimerOnTimeout;
	}

	private void BlinkTimerOnTimeout()
	{
		_state = EnemyState.Chasing;
		_direction = (_player.GlobalPosition - GlobalPosition).Normalized();

		BlinkSprite.Stop();
		BlinkSprite.Visible = false;
		MainSprite.Visible = true;
		TargetTimer.Start();
		FaceDirection();
	}

	private void TargetTimerOnTimeout()
	{
		_direction = (_player.GlobalPosition - GlobalPosition).Normalized();
		FaceDirection();
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_state is not EnemyState.Chasing) return;

		var collision = MoveAndCollide((float)delta * Speed * _direction);
		if (collision is not null)
		{
			_direction = _direction.Bounce(collision.GetNormal());
			FaceDirection();
		}
	}

	private void FaceDirection()
	{
		MainSprite.FlipH = _direction.X > 0;
	}

	public void Hit()
	{
		_currentHealth--;
		if (_currentHealth < 1)
			Die();
	}

	private void Die()
	{
		_state = EnemyState.Dying;
		CollisionShape.SetDeferred("disabled", true);
		MainSprite.Visible = false;
		DeathSprite.Visible = true;
		DeathSprite.Play();
		DeathSprite.AnimationFinished += OnAnimationFinished;
	}

	private void OnAnimationFinished()
	{
		QueueFree();
	}
}

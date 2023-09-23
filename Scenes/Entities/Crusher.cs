namespace GodotHero.Scenes.Entities;

public partial class Crusher : CharacterBody2D, IEnemy
{
    private EnemyState _state = EnemyState.Blinking;
    private Vector2 _direction = Vector2.Zero;
    private int _currentHealth;

    [Export] public int Speed { get; set; } = 48;
    [Export] public int MaxHealth { get; set; } = 8;

    [Export] public AnimatedSprite2D MainSprite { get; set; } = default!;
    [Export] public AnimatedSprite2D DeathSprite { get; set; } = default!;
    [Export] public Timer BlinkTimer { get; set; } = default!;
    [Export] public Timer TargetTimer { get; set; } = default!;
    [Export] public CollisionShape2D CollisionShape { get; set; } = default!;

    public override void _Ready()
    {
        _currentHealth = MaxHealth;

        BlinkTimer.Timeout += BlinkTimerOnTimeout;
        TargetTimer.Timeout += TargetTimerOnTimeout;
    }

    private void BlinkTimerOnTimeout()
    {
        _state = EnemyState.Chasing;
        _direction = (Player.Instance.GlobalPosition - GlobalPosition).Normalized();

        MainSprite.Stop();
        TargetTimer.Start();
    }

    private void TargetTimerOnTimeout()
    {
        _direction = (Player.Instance.GlobalPosition - GlobalPosition).Normalized();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_state is not EnemyState.Chasing) return;

        var collision = MoveAndCollide((float)delta * Speed * _direction);
        if (collision is not null)
        {
            _direction = _direction.Bounce(collision.GetNormal());
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

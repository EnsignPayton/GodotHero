namespace GodotHero.Scenes.Entities;

public partial class GeneratorCore : CharacterBody2D, IEnemy
{
    private int _currentHealth;
    private int _currentShot;

    [Export] public int MaxHealth { get; set; } = 32;
    [Export] public int ShotCycleLength { get; set; } = 5;
    [Export] public int ExplodeShots { get; set; } = 9;
    [Export] public int ShotSpeed { get; set; } = 64;
    [Export] public int ShotSpeedFast { get; set; } = 96;

    [Export] public AnimatedSprite2D MainSprite { get; set; } = default!;
    [Export] public AnimatedSprite2D DeathSprite { get; set; } = default!;
    [Export] public Timer ShotTimer { get; set; } = default!;
    [Export] public PackedScene ShotScene { get; set; } = default!;

    public override void _Ready()
    {
        _currentHealth = MaxHealth;
        ShotTimer.Timeout += OnTimeout;
    }

    private void OnTimeout()
    {
        var playerDirection = (Player.Instance.GlobalPosition - GlobalPosition).Normalized();

        if (_currentShot == 0)
        {
            // Explode
            for (var i = 0; i < ExplodeShots; i++)
            {
                var progress = (float)i / (ExplodeShots - 1);
                CreateShot(playerDirection.Rotated(ScaleAngle(progress)) * ShotSpeed);
            }
        }
        else
        {
            // Normal shot
            CreateShot(playerDirection * ShotSpeed);

            // Random faster one
            CreateShot(playerDirection.Rotated(ScaleAngle(GD.Randf())) * ShotSpeedFast);
        }

        _currentShot++;
        if (_currentShot >= ShotCycleLength)
            _currentShot = 0;
    }

    // Scale the range 0..1 to -Pi/4..+Pi/4
    private static float ScaleAngle(float input) => Mathf.Pi * (input - .5f) / 2f;

    private void CreateShot(Vector2 velocity)
    {
        var instance = ShotScene.Instantiate<Shot>();
        instance.Transform = Transform;
        instance.Velocity = velocity;
        instance.IsEnemy = true;
        AddSibling(instance);
    }

    public void Hit()
    {
        _currentHealth--;
        if (_currentHealth < 1)
            Die();
    }

    private void Die()
    {
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

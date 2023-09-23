namespace GodotHero.Scenes.Entities;

public partial class GeneratorCore : CharacterBody2D, IEnemy
{
    private int _currentHealth;

    [Export] public int MaxHealth { get; set; } = 64;

    [Export] public AnimatedSprite2D MainSprite { get; set; } = default!;
    [Export] public AnimatedSprite2D DeathSprite { get; set; } = default!;

    public override void _Ready()
    {
        _currentHealth = MaxHealth;
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

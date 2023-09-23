namespace GodotHero.Scenes.Entities;

public partial class Flame : Node2D
{
    [Export] public Timer Timer { get; set; } = default!;

    public override void _Ready()
    {
        Timer.Timeout += OnTimeout;
    }

    private void OnTimeout()
    {
        QueueFree();
    }
}

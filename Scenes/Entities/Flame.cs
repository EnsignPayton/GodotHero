namespace GodotHero.Scenes.Entities;

public partial class Flame : Node2D
{
    public override void _Ready()
    {
        var timer = GetNode<Timer>("Timer");
        timer.Timeout += OnTimeout;
    }

    private void OnTimeout()
    {
        QueueFree();
    }
}

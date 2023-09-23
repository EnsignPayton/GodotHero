namespace GodotHero.Scenes;

public partial class Main : Node
{
    [Export] public Timer FPSTimer { get; set; } = default!;

    public override void _Ready()
    {
        FPSTimer.Timeout += OnTimeout;
    }

    private static void OnTimeout()
    {
        var fps = (int)Engine.GetFramesPerSecond();
        DisplayServer.WindowSetTitle($"GodotHero ({fps} fps)");
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionPressed("quit"))
        {
            GetTree().Quit();
        }
    }
}

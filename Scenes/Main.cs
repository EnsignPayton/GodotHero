namespace GodotHero.Scenes;

public partial class Main : Node
{
	public override void _Ready()
	{
		var timer = GetNode<Timer>("FPSTimer");
		timer.Timeout += OnTimeout;
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

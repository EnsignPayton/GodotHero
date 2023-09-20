namespace GodotHero.Scenes;

public partial class Main : Node
{
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("quit"))
		{
			GetTree().Quit();
		}
	}
}

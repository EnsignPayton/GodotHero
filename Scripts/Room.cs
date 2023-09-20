public partial class Room : Node2D
{
	private Camera2D _camera = default!;

	public override void _Ready()
	{
		_camera = GetNode<Camera2D>("Camera2D");
		var area2d = GetNode<Area2D>("Area2D");

		area2d.BodyEntered += OnBodyEntered;
		area2d.BodyExited += OnBodyExited;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is Player)
		{
			GD.Print("Player Entered Room");
			_camera.Enabled = true;
		}
	}

	private void OnBodyExited(Node2D body)
	{
		if (body is Player)
		{
			GD.Print("Player Exited Room");
			_camera.Enabled = false;
		}
	}
}

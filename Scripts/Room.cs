using Godot;

public partial class Room : Node2D
{
	private Camera2D _camera = default!;

	public override void _Ready()
	{
		_camera = GetNode<Camera2D>("Camera2D");
		var area2d = GetNode<Area2D>("Area2D");

		area2d.BodyEntered += Area2dOnBodyEntered;
		area2d.BodyExited += Area2dOnBodyExited;
	}

	private void Area2dOnBodyEntered(Node2D body)
	{
		if (body is Player)
		{
			_camera.Enabled = true;
		}
	}

	private void Area2dOnBodyExited(Node2D body)
	{
		if (body is Player)
		{
			_camera.Enabled = false;
		}
	}
}

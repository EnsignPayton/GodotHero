public partial class Shot : Node2D
{
	[Export] public bool FacingLeft { get; set; }
	[Export] public int Speed { get; set; } = 128;

	public override void _Ready()
	{
		var area = GetNode<Area2D>("Area2D");
		area.AreaEntered += OnAreaEntered;
		area.AreaExited += OnAreaExited;
		area.BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is TileMap)
		{
			// Hit a wall, destroy
			QueueFree();
		}
	}

	private void OnAreaEntered(Area2D area)
	{
		if (area.GetParent() is Drone d)
		{
			d.Hit();
			QueueFree();
		}
	}

	private void OnAreaExited(Area2D area)
	{
		if (area.GetParent() is Room)
		{
			QueueFree();
		}
	}

	public override void _Process(double delta)
	{
		var unit = FacingLeft ? Vector2.Left : Vector2.Right;
		Translate((float)delta * Speed * unit);
	}
}

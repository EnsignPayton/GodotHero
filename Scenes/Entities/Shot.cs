namespace GodotHero.Scenes.Entities;

public partial class Shot : Node2D
{
    [Export] public bool FacingLeft { get; set; }
    [Export] public int Speed { get; set; } = 128;

    [Export] public Area2D Area { get; set; } = default!;

    public override void _Ready()
    {
        Area.AreaExited += OnAreaExited;
        Area.BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is TileMap)
        {
            // Hit a wall, destroy
            QueueFree();
        }
        else if (body is IEnemy enemy)
        {
            enemy.Hit();
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

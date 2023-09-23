namespace GodotHero.Scenes.Entities;

public partial class Shot : Node2D
{
    [Export] public bool IsEnemy { get; set; }
    [Export] public Vector2 Velocity { get; set; }

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
        else if (!IsEnemy)
        {
            if (body is IEnemy enemy)
            {
                enemy.Hit();
                QueueFree();
            }
            else if (body.GetParent().Name == "Generator")
            {
                // TODO: Do this in a better way. Check which collision mask we hit?
                QueueFree();
            }
        }
        else
        {
            if (body is Player)
            {
                // TODO: Damage
            }
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
        Translate((float)delta * Velocity);
    }
}

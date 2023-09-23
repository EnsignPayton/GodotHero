namespace GodotHero.Scenes.Entities;

public partial class Shot : Node2D
{
    [Export] public bool IsEnemy { get; set; }
    [Export] public Vector2 Velocity { get; set; }

    [Export] public Area2D Area { get; set; } = default!;
    [Export] public CollisionShape2D CollisionShape { get; set; } = default!;
    [Export] public Sprite2D MainSprite { get; set; } = default!;
    [Export] public AnimatedSprite2D ExplodeSprite { get; set; } = default!;

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
            Explode();
            // QueueFree();
        }
        else if (!IsEnemy)
        {
            if (body is IEnemy enemy)
            {
                enemy.Hit();
                Explode();
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

    private void Explode()
    {
        // Stop movement and collider, start explode
        Velocity = Vector2.Zero;
        CollisionShape.SetDeferred("disabled", true);
        MainSprite.Visible = false;
        ExplodeSprite.Visible = true;
        ExplodeSprite.Play();
        ExplodeSprite.AnimationFinished += OnAnimationFinished;
    }

    private void OnAnimationFinished()
    {
        QueueFree();
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

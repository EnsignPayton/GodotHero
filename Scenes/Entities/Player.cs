namespace GodotHero.Scenes.Entities;

public partial class Player : CharacterBody2D
{
    private bool _facingLeft;

    public static Player Instance { get; private set; } = default!;

    [Export] public int MaximumShots { get; set; } = 5;
    [Export] public int Speed { get; set; } = 128;

    [Export] public Sprite2D Sprite { get; set; } = default!;
    [Export] public Timer FlameTimer { get; set; } = default!;
    [Export] public PackedScene FlameScene { get; set; } = default!;
    [Export] public PackedScene ShotScene { get; set; } = default!;

    public override void _Ready()
    {
        Instance = this;
        FlameTimer.Timeout += OnFlameTimeout;
    }

    private void OnFlameTimeout()
    {
        // Only add flame when moving
        if (Velocity != Vector2.Zero)
        {
            var instance = FlameScene.Instantiate<Node2D>();
            instance.Transform = Transform;
            AddSibling(instance);
        }
    }

    public override void _Process(double delta)
    {
        GetInput();
        FaceDirection();
        MoveAndSlide();
    }

    private void GetInput()
    {
        // Movement
        var inputVector = Input.GetVector("move_left", "move_right", "move_up", "move_down");
        Velocity = inputVector * Speed;

        // Facing
        var fireLeft = Input.IsActionJustPressed("fire_left");
        var fireRight = Input.IsActionJustPressed("fire_right");
        if (fireLeft && !fireRight) _facingLeft = true;
        if (fireRight && !fireLeft) _facingLeft = false;
        if (fireLeft || fireRight)
        {
            if (ShotCount() < MaximumShots)
                CreateShot();
        }
    }

    private void FaceDirection()
    {
        Sprite.FlipH = !_facingLeft;
    }

    private void CreateFlame()
    {
        var instance = FlameScene.Instantiate<Flame>();
        instance.Transform = Transform;
        AddSibling(instance);
    }

    private void CreateShot()
    {
        var instance = ShotScene.Instantiate<Shot>();
        instance.Transform = Transform;
        instance.FacingLeft = _facingLeft;
        AddSibling(instance);
    }

    private int ShotCount() => GetParent().GetChildren().OfType<Shot>().Count();
}

using GodotHero.Scenes.Entities;

namespace GodotHero.Scenes;

public partial class Room : Node2D
{
    private readonly List<Transform2D> _droneTransforms = new();
    private readonly List<Transform2D> _reaverTransforms = new();
    private readonly List<Transform2D> _crusherTransforms = new();

    [Export] public Camera2D Camera { get; set; } = default!;
    [Export] public Area2D Area { get; set; } = default!;
    [Export] public PackedScene DroneScene { get; set; } = default!;
    [Export] public PackedScene ReaverScene { get; set; } = default!;
    [Export] public PackedScene CrusherScene { get; set; } = default!;

    public override void _Ready()
    {
        InitializeEnemies();
        FreeAllEnemies();
        InitializeAreaCallbacks();
    }

    private void InitializeEnemies()
    {
        var children = GetChildren();

        foreach (var node in children.OfType<Drone>())
        {
            _droneTransforms.Add(node.Transform);
        }

        foreach (var node in children.OfType<Reaver>())
        {
            _reaverTransforms.Add(node.Transform);
        }

        foreach (var node in children.OfType<Crusher>())
        {
            _crusherTransforms.Add(node.Transform);
        }
    }

    private void InitializeAreaCallbacks()
    {
        Area.BodyEntered += OnBodyEntered;
        Area.BodyExited += OnBodyExited;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is Player)
        {
            Activate();
        }
    }

    private void OnBodyExited(Node2D body)
    {
        if (body is Player)
        {
            Deactivate();
        }
    }

    private void Activate()
    {
        Camera.Enabled = true;
        FreeAllEnemies();
        InstantiateEnemies();
    }

    private void Deactivate()
    {
        Camera.Enabled = false;
        FreeAllEnemies();
    }

    private void FreeAllEnemies()
    {
        foreach (var node in GetChildren().OfType<IEnemy>())
        {
            node.QueueFree();
        }
    }

    private void InstantiateEnemies()
    {
        foreach (var transform in _droneTransforms)
        {
            var instance = DroneScene.Instantiate<Drone>();
            instance.Transform = transform;
            AddChild(instance);
        }

        foreach (var transform in _reaverTransforms)
        {
            var instance = ReaverScene.Instantiate<Reaver>();
            instance.Transform = transform;
            AddChild(instance);
        }

        foreach (var transform in _crusherTransforms)
        {
            var instance = CrusherScene.Instantiate<Crusher>();
            instance.Transform = transform;
            AddChild(instance);
        }
    }
}

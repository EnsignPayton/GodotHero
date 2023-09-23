namespace GodotHero.Scenes.Entities;

public partial class Generator : Node2D
{
    [Export] public Node? Barrier { get; set; }
    [Export] public GeneratorCore Core { get; set; } = default!;

    public override void _Ready()
    {
        Core.TreeExited += CoreOnTreeExited;
    }

    private void CoreOnTreeExited()
    {
        Barrier?.QueueFree();
    }
}

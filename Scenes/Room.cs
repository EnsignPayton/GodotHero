using System.Collections.Generic;
using GodotHero.Scenes.Entities;

namespace GodotHero.Scenes;

public partial class Room : Node2D
{
	private readonly List<Transform2D> _droneTransforms = new();
	private Camera2D _camera = default!;
	private PackedScene _droneScene = default!;

	public override void _Ready()
	{
		_camera = GetNode<Camera2D>("Camera2D");
		_droneScene = GD.Load<PackedScene>("res://Scenes/Entities/Drone.tscn");

		InitializeEnemies();
		InitializeAreaCallbacks();
	}

	private void InitializeEnemies()
	{
		foreach (var node in GetChildren().OfType<Drone>())
		{
			_droneTransforms.Add(node.Transform);
		}
	}

	private void InitializeAreaCallbacks()
	{
		var area2d = GetNode<Area2D>("Area2D");

		area2d.BodyEntered += OnBodyEntered;
		area2d.BodyExited += OnBodyExited;
	}

	private void OnBodyEntered(Node2D body)
	{
		if (body is Player)
		{
			GD.Print("Player Entered Room");
			Activate();
		}
	}

	private void OnBodyExited(Node2D body)
	{
		if (body is Player)
		{
			GD.Print("Player Exited Room");
			Deactivate();
		}
	}

	private void Activate()
	{
		_camera.Enabled = true;
		FreeAllEnemies();
		InstantiateEnemies();
	}

	private void Deactivate()
	{
		_camera.Enabled = false;
		FreeAllEnemies();
	}

	private void FreeAllEnemies()
	{
		foreach (var node in GetChildren().OfType<Drone>())
		{
			node.QueueFree();
		}
	}

	private void InstantiateEnemies()
	{
		foreach (var transform in _droneTransforms)
		{
			var instance = _droneScene.Instantiate<Drone>();
			instance.Transform = transform;
			AddChild(instance);
		}
	}
}

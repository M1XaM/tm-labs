using Godot;
using System;

public partial class GameManager : Node
{
	
	
	private static GameManager _instance;
	public static GameManager Instance => _instance;
	
	public WorldScene CurrentWorld { get; set; }

	
	public bool bossKilled = false;
	
	public override void _EnterTree()
	{
		if (_instance != null)
		{
			QueueFree(); // Prevent duplicate instances
			return;
		}
		_instance = this;
	}
	
	public override void _Ready()
	{
		// Also here would be nice to add logic of loading a saved game
		
	}
	
	public override void _PhysicsProcess(double delta)
	{
		
		if (bossKilled == true)
		{
			// Run final scene
		}
		
	}
	
	
	
}

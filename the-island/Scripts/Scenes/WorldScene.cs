using Godot;
using System;

public partial class WorldScene : Node2D
{
	public override void _Ready()
	{
		GameManager.Instance.CurrentWorld = this;
		
		ShowAllNodes(this);
		Spawn.Instance.StartSpawning();
		TimeManager.Instance.StartTimeSystem();
		
	}
	
	
	public override void _Process(double delta)
	{
	}
	
	public override void _ExitTree()
	{
		TimeManager.Instance.StopTimeSystem();
	}

	private void ShowAllNodes(Node node)
	{
		if (node is CanvasItem canvasItem && !canvasItem.Visible)
		{
			canvasItem.Visible = true;
		}

		foreach (Node child in node.GetChildren())
		{
			ShowAllNodes(child);
		}
	}
	

}

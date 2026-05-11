using Godot;
using System;

public partial class StartMenu : Control
{
		private Button _startButton;
	
		public override void _Ready()
	{
		_startButton = GetNode<Button>("StartBtn");
		_startButton.Pressed += OnStartPressed;
		
	}

	private void OnStartPressed()
	{
			GD.Print("start pressed");

		if (GameManager.Instance != null)
		{
			GetTree().ChangeSceneToFile("res://Scenes/WorldScene.tscn");
		}
		else
		{
			GD.PrintErr("GameManager is not initialized.");
		}
	}
}

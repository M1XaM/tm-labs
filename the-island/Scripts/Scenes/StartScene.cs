using Godot;
using System;

public partial class StartScene : Control
{
	private Button _startButton;
	private Button _exitButton;
	
	
	public override void _Ready()
	{
		_startButton = GetNode<Button>("StartButton");
		_startButton.Pressed += OnStartPressed;
		_exitButton = GetNode<Button>("ExitButton");
		_exitButton.Pressed += OnExitPressed;
	}

	private void OnStartPressed()
	{
		var sceneTree = (SceneTree)Engine.GetMainLoop();
		sceneTree.ChangeSceneToFile("res://Scenes/WorldScene.tscn");
	}
	
	private void OnExitPressed()
	{
		GetTree().Quit();
	}
}

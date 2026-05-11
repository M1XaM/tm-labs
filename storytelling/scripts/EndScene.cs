using Godot;
using System;

public class EndScene : Node2D
{
	private Button _restartButton;
	private Sprite _background;

	public override void _Ready()
	{
		_background = GetNode<Sprite>("Background");
		GetBackground();
		
		_restartButton = GetNode<Button>("RestartButton");
		_restartButton.Connect("pressed", this, nameof(OnWakeUpButton));
	}
	
	private void GetBackground()
	{
		int endId = SceneManager.Instance.endSceneId;
		if (endId == -1)
		{
			Texture texture = GD.Load<Texture>("res://images/endImage1.jpg");
			_background.Texture = texture;
		}
		else if (endId == -2)
		{
			Texture texture = GD.Load<Texture>("res://images/endImage2.jpg");
			_background.Texture = texture;
		}
		else if (endId == -3)
		{
			Texture texture = GD.Load<Texture>("res://images/endImage3.jpg");
			_background.Texture = texture;
		}
		else if (endId == -4)
		{
			Texture texture = GD.Load<Texture>("res://images/endImage4.jpg");
			_background.Texture = texture;
		}
	}

	private void OnWakeUpButton()
	{
		SceneManager.Instance.nextScene = 1;
		GetTree().ChangeScene("res://scenes/Start.tscn");
	}
}

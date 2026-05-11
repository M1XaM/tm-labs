using Godot;
using System;

public class Dead : Node2D
{
	private Sprite _wakeUpSprite;
	private Button _wakeUpButton;

	public override void _Ready()
	{
		_wakeUpSprite = GetNode<Sprite>("WakeUpSprite");
		_wakeUpButton = GetNode<Button>("WakeUpButton");

		_wakeUpButton.Hide();
		_wakeUpButton.Connect("pressed", this, nameof(OnWakeUpButton));
	}

	private void OnWakeUpButton()
	{
		GetTree().ChangeScene("res://scenes/Screen.tscn");
	}

	public override void _Process(float delta)
	{
		if (_wakeUpSprite.GetRect().HasPoint(_wakeUpSprite.ToLocal(GetGlobalMousePosition())))
			_wakeUpButton.Show();
		else
			_wakeUpButton.Hide();
	}
}

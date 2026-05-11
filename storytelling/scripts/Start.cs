using Godot;
using System;

public class Start : Node2D
{
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
		{
			GetTree().ChangeScene("res://scenes/Screen.tscn");
		}
	}
}

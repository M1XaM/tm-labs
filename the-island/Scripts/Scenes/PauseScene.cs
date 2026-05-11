using Godot;
using System;

public partial class PauseScene : Control
{
	private AnimationPlayer _animationPlayer;

	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		_animationPlayer.Play("RESET");
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("escape"))
		{
			if (GetTree().Paused)
				Resume();
			else
				Pause();
		}
	}
	
	private void Resume()
	{
		GetTree().Paused = false;
		_animationPlayer.PlayBackwards("blur");
	}
		
	private void Pause()
	{
		GetTree().Paused = true;
		_animationPlayer.Play("blur");
	}
	
	private void OnResumePressed()
	{
		Resume();
	}

	private void OnQuitPressed()
	{
		GetTree().Quit();
	}
}

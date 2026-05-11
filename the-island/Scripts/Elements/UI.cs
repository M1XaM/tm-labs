using Godot;
using System;

public partial class UI : Node2D
{
	private Label _fpsLabel;
	private Label _timeLabel;

	public override void _Ready()
	{
		_fpsLabel = GetNode<Label>("FPSLabel");
		_timeLabel = GetNode<Label>("TimeLabel");
	}
	public override void _PhysicsProcess(double delta)
	{
		_fpsLabel.Text = $"FPS: {Engine.GetFramesPerSecond()}";
		_timeLabel.Text = TimeManager.Instance.CurrentTimeString;
	}
	
		
		}
		
		

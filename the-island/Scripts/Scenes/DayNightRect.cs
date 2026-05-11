using Godot;
using System;

public partial class DayNightRect : ColorRect
{
	[Export] public float MaxOpacity = 0.7f; // Maximum alpha value at night
	private Color _originalColor;

	public override void _Ready()
	{
		_originalColor = Color;
	}

	public override void _Process(double delta)
	{
		if (TimeManager.Instance == null)
			return;

		int hour = TimeManager.Instance.Hours;
		int minute = TimeManager.Instance.Minutes;

		float currentTime = hour + minute / 60f;
		float alpha = 0f;

		if (currentTime >= 19f && currentTime < 20f)
		{
			// Fade in from 19:00 to 20:00
			float t = (currentTime - 19f) / 1f; // 0.0 to 1.0
			alpha = Mathf.Lerp(0f, MaxOpacity, t);
		}
		else if (currentTime >= 20f || currentTime < 5f)
		{
			// Fully opaque from 20:00 to 5:00
			alpha = MaxOpacity;
		}
		else if (currentTime >= 5f && currentTime < 6f)
		{
			// Fade out from 5:00 to 6:00
			float t = (currentTime - 5f) / 1f; // 0.0 to 1.0
			alpha = Mathf.Lerp(MaxOpacity, 0f, t);
		}
		else
		{
			// Daytime: fully transparent
			alpha = 0f;
		}

		Color newColor = _originalColor;
		newColor.A = alpha;
		Color = newColor;
	}
}

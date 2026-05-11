using Godot;
using System;

public class ClickToPlaySprite : Sprite
{
	public float FadeInDuration = 1.3f;
	public float VisibleDuration = 1.5f;
	public float FadeOutDuration = 1.3f;

	private Tween tween;

	public override void _Ready()
	{
		tween = GetNode<Tween>("ClickToPlayTween");
		StartPulsating();
	}

	private void StartPulsating()
	{
		tween.StopAll();
		Modulate = new Color(Modulate.r, Modulate.g, Modulate.b, 0); // Start fully transparent

		// Fade in
		tween.InterpolateProperty(this, "modulate:a", 0.0f, 1.0f, FadeInDuration,
			Tween.TransitionType.Sine, Tween.EaseType.Out);

		// Stay visible
		tween.InterpolateCallback(this, FadeInDuration + VisibleDuration, nameof(FadeOut));

		tween.Start();
	}

	private void FadeOut()
	{
		tween.InterpolateProperty(this, "modulate:a", 1.0f, 0.0f, FadeOutDuration,
			Tween.TransitionType.Sine, Tween.EaseType.In);

		// Restart animation
		tween.InterpolateCallback(this, FadeOutDuration, nameof(StartPulsating));

		tween.Start();
	}
}

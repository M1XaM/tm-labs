using Godot;
using System.Collections.Generic;

public class SceneManager : Node
{
	public static SceneManager Instance { get; private set; } // Singleton Pattern
	public int nextScene;
	public int endSceneId;
	
	public int? savedSceneId;
	
	// Reference to the AudioStreamPlayer for music
	private AudioStreamPlayer musicPlayer;
	
	[Export]
	public string musicPath = "res://sound/Ghost-Story(chosic.com).mp3";
	
	public override void _Ready()
	{
 		if (Instance == null)
		{
			Instance = this;
			nextScene = 1;
			savedSceneId = null;
		}
		else
			QueueFree(); // Prevent duplicate instances
		
		GD.Print("CurrentSceneData Initialized!");
		
		InitializeMusic();
	}
	
	private void InitializeMusic()
	{
		// Create a new AudioStreamPlayer if it doesn't exist in the scene
		musicPlayer = new AudioStreamPlayer();
		AddChild(musicPlayer);

		// Load the music file from the given path
		var musicStream = ResourceLoader.Load<AudioStream>(musicPath);
		if (musicStream == null)
		{
			GD.PrintErr("Failed to load music from path: " + musicPath);
			return;
		}

		// Assign the music stream to the AudioStreamPlayer
		musicPlayer.Stream = musicStream;
		musicPlayer.VolumeDb = -10.0f; 

		// Play music if it isn't already playing
		if (!musicPlayer.Playing)
		{
			musicPlayer.Play();
		}
	}
}

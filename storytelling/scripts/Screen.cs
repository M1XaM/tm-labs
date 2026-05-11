using Godot;
using System;
using System.Collections.Generic;

public class Screen : Node
{
	private ProcessJson json;
	private Sprite background;
	private Label mainText;
	private List<Button> buttons = new List<Button>();
	
	private Button _saveButton;
	private Button _loadButton;
	private Sprite _line;
	private int _currentSceneId;

	public override void _Ready()
	{

		// Load JSON
		json = new ProcessJson();
		json.Initialization();
		
		_saveButton = GetNode<Button>("SaveButton");
		_loadButton = GetNode<Button>("LoadButton");
		_line = GetNode<Sprite>("Line");
		
		_line.Hide();
		_loadButton.Hide();
		_saveButton.Connect("pressed", this, nameof(SaveScene));
		_loadButton.Connect("pressed", this, nameof(LoadSavedScene));
		
		// Get references to child nodes
		background = GetNode<Sprite>("Background");
		mainText = GetNode<Label>("MainTextBox/MainText"); // Get reference to the Label
		mainText.Autowrap = true;
		
		var fontMainText = new DynamicFont();
		var fontButtons = new DynamicFont();
		fontMainText.FontData = (DynamicFontData) GD.Load("res://fonts/Roboto.ttf"); 
		fontMainText.Size = 34; 
		mainText.AddColorOverride("font_color", new Color(1, 1, 1)); // Red color (RGB)


		fontButtons.FontData = (DynamicFontData) GD.Load("res://fonts/Roboto.ttf"); 
		fontButtons.Size = 24; 
		
		mainText.AddFontOverride("font", fontMainText);

		for (int i = 1; i <= 3; i++)
		{
			Button button = GetNode<Button>($"Button{i}");
			button.AddFontOverride("font", fontButtons);
			buttons.Add(button);
		}
		
		AdjustBackground();
		LoadScene(SceneManager.Instance.nextScene);
	}
	
	public override void _Process(float delta)
	{
		CheckLoadButtonAppearance();
	}

	private void AdjustBackground()
	{
		if (background == null || background.Texture == null)
			return;

		Vector2 screenSize = GetViewport().Size; // Get screen size
		Vector2 textureSize = background.Texture.GetSize(); // Get original image size

		// Scale to fit screen
		float scaleX = screenSize.x / textureSize.x;
		float scaleY = screenSize.y / textureSize.y;

		background.Scale = new Vector2(scaleX, scaleY);
		background.Position = screenSize / 2; // Center the background
	}
	
	private void AdjustText()
	{
		Vector2 screenSize = GetViewport().Size;
		
		mainText.RectMinSize = new Vector2(screenSize.x * 0.8f, 0); // Set max width (80% of screen width)
		mainText.RectPosition = new Vector2((screenSize.x - mainText.RectSize.x) / 2, screenSize.y * 0.1f); // Center horizontally and move down slightly
	}

private void LoadScene(int sceneId)
{
	SceneData scene = json.GetSceneById(sceneId);
	if (scene == null)
	{
		GD.PrintErr($"Scene with ID {sceneId} not found.");
		return;
	}
	
	_currentSceneId = sceneId;
	CheckLoadButtonAppearance();
	
	// Handle You got killed scene separate (because it looks more beautiful)
	// The scene after is loaded as usual (from plot.json)
	if (scene.MainText == "You got killed")
	{
		SceneManager.Instance.nextScene = scene.Options[0].LeadToId;
		GetTree().ChangeScene("res://scenes/Dead.tscn");
	} 
	else if (scene.MainText == "End")
	{
		SceneManager.Instance.endSceneId = scene.Id;
		GetTree().ChangeScene("res://scenes/EndScene.tscn");
	}

	Texture texture = GD.Load<Texture>($"res://images/{scene.BackgroundImage}");
	mainText.Text = scene.MainText;
	background.Texture = texture;
	AdjustBackground();
	
	// Update buttons
	for (int i = 0; i < buttons.Count; i++)
		{
			if (i < scene.Options.Count)
			{
				buttons[i].Text = scene.Options[i].OptionText;
				buttons[i].Visible = true;
				int nextSceneId = scene.Options[i].LeadToId;

				// Disconnect any existing connections before reconnecting to prevent multiple connections
				if (buttons[i].IsConnected("pressed", this, nameof(OnButtonPressed)))
					buttons[i].Disconnect("pressed", this, nameof(OnButtonPressed));

				// Connect signal using Godot.Collections.Array for passing arguments
				buttons[i].Connect("pressed", this, nameof(OnButtonPressed), new Godot.Collections.Array() { nextSceneId });
			}
			else
			{
				buttons[i].Visible = false; // Hide unused buttons
			}
		}
	}
	
	private void OnButtonPressed(int nextSceneId)
	{
		LoadScene(nextSceneId);
	}
	
	private void CheckLoadButtonAppearance()
	{
		if (SceneManager.Instance.savedSceneId != null)
		{
			_loadButton.Show();
			_line.Show();
		}
	}
	
	private void SaveScene()
	{
		SceneManager.Instance.savedSceneId = _currentSceneId;
	}
	
	private void LoadSavedScene()
	{
		LoadScene(SceneManager.Instance.savedSceneId ?? 1);
	}
}

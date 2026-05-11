using Godot;
using System;
using System.Collections.Generic;
using Newtonsoft.Json; // Add this to use Newtonsoft.Json

public class ProcessJson : Node
{
	public List<SceneData> Scenes { get; private set; } = new List<SceneData>();

	public override void _Ready()
	{
	}
	
	public void Initialization()
	{
		string jsonPath = "res://scripts/plot.json";
		string jsonString = LoadJson(jsonPath);
		ParseJson(jsonString);
	}

	private string LoadJson(string filePath)
	{
		File file = new File();
		var error = file.Open(filePath, File.ModeFlags.Read);  // Open file in read mode

		if (error != Error.Ok)
		{
			GD.PrintErr("JSON file not found or failed to open: " + filePath);
			return null;
		}

		string content = file.GetAsText();
		file.Close();  // Close the file after reading
		return content;
	}

	private void ParseJson(string jsonString)
	{
		if (string.IsNullOrEmpty(jsonString))
		{
			GD.PrintErr("JSON string is empty.");
			return;
		}

		try
		{
			// Deserialize using Newtonsoft.Json
			Scenes = JsonConvert.DeserializeObject<List<SceneData>>(jsonString);  
			GD.Print("Loaded " + Scenes.Count + " scenes.");
		}
		catch (Exception e)
		{
			GD.PrintErr("Failed to parse JSON: " + e.Message);
		}
	}

	public SceneData GetSceneById(int id)
	{
		return Scenes.Find(scene => scene.Id == id);
	}
}

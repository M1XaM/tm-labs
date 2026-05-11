using Godot;
using System.Collections.Generic;
using Newtonsoft.Json; // Use Newtonsoft.Json instead of System.Text.Json

public class SceneData
{
	[JsonProperty("id")]
	public int Id { get; set; }

	[JsonProperty("mainText")]
	public string MainText { get; set; }

	[JsonProperty("backgroundImage")]
	public string BackgroundImage { get; set; }

	[JsonProperty("options")]
	public List<OptionData> Options { get; set; }
}

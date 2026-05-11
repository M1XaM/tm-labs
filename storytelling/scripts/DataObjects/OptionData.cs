using Godot;
using System.Collections.Generic;
using Newtonsoft.Json; // Use Newtonsoft.Json instead of System.Text.Json

public class OptionData
{
	[JsonProperty("optionText")] // Use JsonProperty attribute from Newtonsoft.Json
	public string OptionText { get; set; }

	[JsonProperty("leadToId")]
	public int LeadToId { get; set; }
}

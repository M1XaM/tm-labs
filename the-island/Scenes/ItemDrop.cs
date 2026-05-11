using Godot;

public partial class ItemDrop : Area2D
{
	[Export]
	public string ItemName { get; set; } = "Default Item";

	[Export]
	public int Quantity { get; set; } = 1;

	[Export]
	public Texture2D Icon { get; set; }

	public override void _Ready()
	{
		// Set sprite texture if Icon is assigned
		var sprite = GetNodeOrNull<Sprite2D>("Sprite2D");
		if (Icon != null && sprite != null)
		{
			sprite.Texture = Icon;
		}
	}


	public void OnPickup()
	{
		// Optional: Add sound effect, animation, etc.
		QueueFree(); // Removes item from the scene
	}
}

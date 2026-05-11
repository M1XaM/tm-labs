using Godot;

public partial class Item : Area2D
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

		// Connect the body entered signal to detect the player
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node body)
	{
		if (body is MainCharacter mainCharacter)
		{
			GD.Print($"Item '{ItemName}' picked up!");

			// Add the item to the player's inventory
			mainCharacter.AddItemToInventory(ItemName, Quantity);

			// Optional: Play pickup animation or sound here

			// Remove this item from the scene
			QueueFree();
		}
	}

	public void OnPickup()
	{
		// Optional: Add sound effect, animation, etc.
		QueueFree(); // Removes item from the scene
	}
}

extends Panel

var ItemClass = preload("res://Scenes/Item.tscn")
var item = null

var default_tex = preload("res://Assets/inventory/1 Sprites/Book Desk/5.png")
var empty_tex = preload("res://Assets/inventory/1 Sprites/Book Desk/7.png")

var default_style: StyleBoxTexture = null
var empty_style: StyleBoxTexture = null


# Called when the node enters the scene tree for the first time.
func _ready():
	default_style = StyleBoxTexture.new()
	empty_style = StyleBoxTexture.new()
	default_style.texture = default_tex
	empty_style.texture = empty_tex
	
	if randi() % 2 == 0:
		item = ItemClass.instantiate()
		add_child(item)
	refresh_style()
	
func refresh_style():
	if item == null:
		set("theme_override_styles/panel", empty_style)
	else:
		set("theme_override_styles/panel", default_style)
	

func pickFromSlot():
	remove_child(item)
	var invetoryNode = find_parent("Inventory")
	#find_parent("UserInterface").add_child(item)
	invetoryNode.add_child(item)
	item = null
	refresh_style()
	
func putIntoSlot(new_item):
	item = new_item
	item.position = Vector2(0, 0)
	var invetoryNode = find_parent("Inventory")
	#find_parent("UserInterface").remove_child(item)
	invetoryNode.remove_child(item)
	add_child(item)
	refresh_style()

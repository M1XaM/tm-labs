extends Node

var item_data: Dictionary

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	item_data = LoadData("res://Scripts/InventoryData/ItemData.json")

# Called every frame. 'delta' is the elapsed time since the previous frame.
func LoadData(file_path):
	var file_data  = FileAccess.open(file_path, FileAccess.READ)
	var json_data  = JSON.new()
	json_data .parse(file_data .get_as_text())
	file_data .close()
	return json_data .get_data()

extends Node2D

const SlotClass = preload("res://Scripts/Scenes/Slot.gd")

var holding_item = null
@onready var inventory_slots = $GridContainer
#onready var equip_slots = $EquipSlots.get_children()

func _ready():
	for inv_slot in inventory_slots.get_children():
		inv_slot.gui_input.connect(slot_gui_input.bind(inv_slot))
	
	#for i in range(slots.size()):
		#slots[i].connect("gui_input", self, "slot_gui_input", [slots[i]])
		#slots[i].slot_index = i
		#slots[i].slotType = SlotClass.SlotType.INVENTORY
		#
	#for i in range(equip_slots.size()):
		#equip_slots[i].connect("gui_input", self, "slot_gui_input", [equip_slots[i]])
		#equip_slots[i].slot_index = i
	#equip_slots[0].slotType = SlotClass.SlotType.SHIRT
	#equip_slots[1].slotType = SlotClass.SlotType.PANTS
	#equip_slots[2].slotType = SlotClass.SlotType.SHOES
	#
	#initialize_inventory()
	#initialize_equips()

#func initialize_inventory():
	#var slots = inventory_slots.get_children()
	#for i in range(slots.size()):
		#if PlayerInventory.inventory.has(i):
			#slots[i].initialize_item(PlayerInventory.inventory[i][0], PlayerInventory.inventory[i][1])
#
#func initialize_equips():
	#for i in range(equip_slots.size()):
		#if PlayerInventory.equips.has(i):
			#equip_slots[i].initialize_item(PlayerInventory.equips[i][0], PlayerInventory.equips[i][1])
	
func slot_gui_input(event: InputEvent, slot: SlotClass):
	if event is InputEventMouseButton:
		if event.button_index == MOUSE_BUTTON_LEFT and event.pressed:
			if holding_item != null:
				if !slot.item:
					slot.putIntoSlot(holding_item)
					holding_item = null
				else:
					if holding_item.item_name != slot.item.item_name:
						var temp_item = slot.item
						slot.pickFromSlot()
						temp_item.global_position = event.global_position
						slot.putIntoSlot(holding_item)
						holding_item = temp_item
					else:
						var stack_size = int(JsonData.item_data[slot.item.item_name]["StackSize"])
						var able_to_add = stack_size - slot.item.item_quantity
						if able_to_add >= holding_item.item_quantity:
							slot.item.add_item_quantity(holding_item.item_quantity)
							holding_item.queue_free()
							holding_item = null
						else:
							slot.item.add_item_quantity(able_to_add)
							holding_item.decrease_item_quantity(able_to_add)
			elif slot.item:
				holding_item = slot.item
				slot.pickFromSlot()
				holding_item.global_position = get_global_mouse_position()
				
func _input(event):
	if holding_item:
		holding_item.global_position = get_global_mouse_position()
	#if find_parent("UserInterface").holding_item:
		#find_parent("UserInterface").holding_item.global_position = get_global_mouse_position()
		
		
#func able_to_put_into_slot(slot: SlotClass):
	#var holding_item = find_parent("UserInterface").holding_item
	#if holding_item == null:
		#return true
	#var holding_item_category = JsonData.item_data[holding_item.item_name]["ItemCategory"]
	#
	#if slot.slotType == SlotClass.SlotType.SHIRT:
		#return holding_item_category == "Shirt"
	#elif slot.slotType == SlotClass.SlotType.PANTS:
		#return holding_item_category == "Pants"
	#elif slot.slotType == SlotClass.SlotType.SHOES:
		#return holding_item_category == "Shoes"
	#return true
		#
#func left_click_empty_slot(slot: SlotClass):
	#if able_to_put_into_slot(slot):
		#PlayerInventory.add_item_to_empty_slot(find_parent("UserInterface").holding_item, slot)
		#slot.putIntoSlot(find_parent("UserInterface").holding_item)
		#find_parent("UserInterface").holding_item = null
	#
#func left_click_different_item(event: InputEvent, slot: SlotClass):
	#if able_to_put_into_slot(slot):
		#PlayerInventory.remove_item(slot)
		#PlayerInventory.add_item_to_empty_slot(find_parent("UserInterface").holding_item, slot)
		#var temp_item = slot.item
		#slot.pickFromSlot()
		#temp_item.global_position = event.global_position
		#slot.putIntoSlot(find_parent("UserInterface").holding_item)
		#find_parent("UserInterface").holding_item = temp_item
#
#func left_click_same_item(slot: SlotClass):
	#if able_to_put_into_slot(slot):
		#var stack_size = int(JsonData.item_data[slot.item.item_name]["StackSize"])
		#var able_to_add = stack_size - slot.item.item_quantity
		#if able_to_add >= find_parent("UserInterface").holding_item.item_quantity:
			#PlayerInventory.add_item_quantity(slot, find_parent("UserInterface").holding_item.item_quantity)
			#slot.item.add_item_quantity(find_parent("UserInterface").holding_item.item_quantity)
			#find_parent("UserInterface").holding_item.queue_free()
			#find_parent("UserInterface").holding_item = null
		#else:
			#PlayerInventory.add_item_quantity(slot, able_to_add)
			#slot.item.add_item_quantity(able_to_add)
			#find_parent("UserInterface").holding_item.decrease_item_quantity(able_to_add)
		#
#func left_click_not_holding(slot: SlotClass):
	#PlayerInventory.remove_item(slot)
	#find_parent("UserInterface").holding_item = slot.item
	#slot.pickFromSlot()
	#find_parent("UserInterface").holding_item.global_position = get_global_mouse_position()

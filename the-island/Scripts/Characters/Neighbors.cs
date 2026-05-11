using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

public partial class Neighbors : CharacterBody2D
{
	public Area2D DetectionArea;
	public AnimatedSprite2D AnimatedSprite;

	protected bool _playerInRange;
	private bool _isDialogLoaded = false;
	protected virtual string DialogFilePath => "res://dialogs/default.json";
	private List<DialogEntry> _dialogEntries = new();
	private bool _isDialogActive = false;
	private List<int> _currentOptionIndices = new();

	// Key handling refactored to use collections
	private readonly List<Key> _dialogKeys = new() { Key.Key1, Key.Key2, Key.Key3 };
	private Dictionary<Key, bool> _previousKeyStates = new();

	public override void _Ready()
	{
		DetectionArea = GetNode<Area2D>("Area2D");
		AnimatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		DetectionArea.BodyEntered += OnBodyEntered;
		DetectionArea.BodyExited += OnBodyExited;

		// Initialize key states
		foreach (var key in _dialogKeys)
		{
			_previousKeyStates[key] = false;
		}

		if (AnimatedSprite?.SpriteFrames?.HasAnimation("idle") == true)
			AnimatedSprite.Play("idle");
	}
	
	private void OnBodyEntered(Node body)
	{
		if (body.IsInGroup("Player"))
		{
			_playerInRange = true;
			AnimatedSprite.Modulate = Colors.Green;
		}
	}

	private void OnBodyExited(Node body)
	{
		if (body.IsInGroup("Player"))
		{
			_playerInRange = false;
			AnimatedSprite.Modulate = Colors.White;
			if (_isDialogActive)
			{
				_isDialogActive = false;
				GD.Print("\n[You moved away from the character. Conversation ended.]");
			}
		}
	}

	// Existing BodyEntered/Exited methods remain the same...

	public override void _Process(double delta)
	{
		if (_playerInRange && Input.IsActionJustPressed("e") && !_isDialogActive)
		{
			StartDialog();
		}

		if (_isDialogActive)
		{
			HandleKeyInputs();
		}

		HandleIdleAnimation();
	}

	private void HandleKeyInputs()
	{
		for (int i = 0; i < _dialogKeys.Count; i++)
		{
			var key = _dialogKeys[i];
			var currentState = Input.IsKeyPressed(key);
			
			if (currentState && !_previousKeyStates[key])
			{
				HandleDialogChoice(i);
			}
			_previousKeyStates[key] = currentState;
		}
	}

	private void HandleIdleAnimation()
	{
		if (AnimatedSprite != null && !AnimatedSprite.IsPlaying())
		{
			AnimatedSprite.Play("idle");
		}
	}

	protected virtual void StartDialog()
	{
		// Reset key states using collection
		foreach (var key in _dialogKeys)
		{
			_previousKeyStates[key] = Input.IsKeyPressed(key);
		}

		if (!_isDialogLoaded) LoadDialogFile();

		_isDialogActive = true;
		_currentOptionIndices.Clear();
		GD.Print("\nChoose a question:");

		ShowDialogOptions();
	}

	private void LoadDialogFile()
	{
		using var file = FileAccess.Open(DialogFilePath, FileAccess.ModeFlags.Read);
		if (file == null)
		{
			GD.PrintErr("Failed to open dialog file: ", DialogFilePath);
			return;
		}

		try
		{
			_dialogEntries = JsonSerializer.Deserialize<List<DialogEntry>>(file.GetAsText());
			_isDialogLoaded = true;
		}
		catch (Exception e)
		{
			GD.PrintErr("Failed to parse dialog JSON: ", e.Message);
		}
	}

	private void ShowDialogOptions()
	{
		var eligibleIndices = _dialogEntries
			.Select((entry, index) => new { entry, index })
			.Where(x => !x.entry.Passed || x.entry.CanRepeat)
			.Select(x => x.index)
			.ToList();

		// Show up to 2 questions
		foreach (var index in eligibleIndices.Take(2))
		{
			_currentOptionIndices.Add(index);
			GD.Print($"{_currentOptionIndices.Count}. {_dialogEntries[index].Question}");
		}

		// Add exit option
		_currentOptionIndices.Add(-1);
		GD.Print($"{_currentOptionIndices.Count}. Exit conversation");
	}

	// Existing HandleDialogChoice and DialogEntry class remain the same...


	private void HandleDialogChoice(int index) {
		if (index >= _currentOptionIndices.Count) return;

		var entryIndex = _currentOptionIndices[index];

		// Handle exit choice
		if (entryIndex == -1) {
			GD.Print("\n[Conversation ended]");
			_isDialogActive = false;
			return;
		}

		var entry = _dialogEntries[entryIndex];

		GD.Print($"\nYou asked: {entry.Question}");
		GD.Print($"Response: {entry.Response}");

		if (!entry.CanRepeat)
			_dialogEntries[entryIndex].Passed = true;

		// Restart dialog
		StartDialog();
	}

	public class DialogEntry
	{
		public string Question { get; set; }
		public string Response { get; set; }
		public bool CanRepeat { get; set; }
		public bool Passed { get; set; }
	}
}

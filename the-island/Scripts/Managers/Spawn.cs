using Godot;

public partial class Spawn : Node
{
	// Singleton instance
	private static Spawn _instance;
	public static Spawn Instance => _instance;

	[Export] PackedScene EnemyScene;
	[Export] float SpawnInterval = 2.0f;
	
	private Timer _spawnTimer;

	public override void _EnterTree()
	{
		if (_instance != null)
		{
			QueueFree(); // Prevent duplicate instances
			return;
		}
		_instance = this;
	}

	public override void _Ready()
	{
		// Load enemy scene if not set in inspector
		if(EnemyScene == null)
		{
			EnemyScene = ResourceLoader.Load<PackedScene>("res://Prefabs/Enemy.tscn");
		}

		_spawnTimer = new Timer();
		AddChild(_spawnTimer);
		_spawnTimer.WaitTime = SpawnInterval;
		_spawnTimer.Timeout += OnSpawnTimerTimeout;
	}

	public void StartSpawning()
	{
		_spawnTimer.Start();
	}

	public void StopSpawning()
	{
		_spawnTimer.Stop();
	}

	private void OnSpawnTimerTimeout()
	{
		SpawnEnemy();
	}

	private void SpawnEnemy()
	{
		if(EnemyScene == null)
		{
			GD.PrintErr("Enemy scene not loaded!");
			return;
		}

		var enemy = EnemyScene.Instantiate();
		GetTree().Root.AddChild(enemy);
	}
}

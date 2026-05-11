using Godot;

public partial class Enemy : CharacterBody2D
{
	[Export]
	public int Speed { get; set; } = 200;
	[Export]
	public int ChaseSpeed { get; set; } = 300;
	[Export]
	public float AttackRange { get; set; } = 50f;
	[Export]
	public float DetectionRange { get; set; } = 400f;
	[Export]
	public float VisionAngle { get; set; } = 45f;

	private AnimatedSprite2D _animationPlayer;
	private RayCast2D _rayCast;
	private Area2D _detectionArea;
	private Node2D _player;
	private Timer _attackCooldown;
	private bool _canAttack = true;
	
	private AudioStreamPlayer2D _slashSound;

	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_rayCast = GetNode<RayCast2D>("RayCast2D");
		_detectionArea = GetNode<Area2D>("Area2D");
		_attackCooldown = GetNode<Timer>("AttackCooldown");
		
		// Set detection area shape radius
		var collisionShape = _detectionArea.GetNode<CollisionShape2D>("CollisionShape2D");
		var circleShape = new CircleShape2D();
		circleShape.Radius = DetectionRange;
		collisionShape.Shape = circleShape;
		
		// Connect area signals
		_detectionArea.BodyEntered += OnBodyEnteredDetectionArea;
		_detectionArea.BodyExited += OnBodyExitedDetectionArea;
		_attackCooldown.Timeout += OnAttackCooldownTimeout;

		_rayCast.Enabled = true;
		
		_slashSound = GetNode<AudioStreamPlayer2D>("/root/Enemy/SlashSound");
		// if (_slashSound != null){
		// 	GD.Print("sound found.");
		// }
	}

	public override void _PhysicsProcess(double delta)
	{
		
		
		if (_player != null)
		{
			// Update raycast direction
			_rayCast.TargetPosition = _player.GlobalPosition - GlobalPosition;
			
			if (CanSeePlayer() && IsPlayerInAttackRange())
			{
				Attack();
			}
			else if (CanSeePlayer())
			{
				ChasePlayer(delta);
			}
			else
			{
				_animationPlayer.Play("idle");
			}
		}
	}

	private bool CanSeePlayer()
	{
		 if (_player == null) return false;

		Vector2 toPlayer = (_player.GlobalPosition - GlobalPosition).Normalized();
		Vector2 rayDirection = (_rayCast.TargetPosition).Normalized();

		// Check if player is inside vision angle
		float angleToPlayer = rayDirection.AngleTo(toPlayer);
		if (Mathf.Abs(angleToPlayer) > Mathf.DegToRad(VisionAngle))
		{
			// Player outside vision cone
			return false;
		}
		
		
		// Check if raycast hits player (line of sight)
		if (_rayCast.IsColliding() && _rayCast.GetCollider() == _player)
		{
			return true;
		}

		return false;
	}

	private bool IsPlayerInAttackRange()
	{
		return GlobalPosition.DistanceTo(_player.GlobalPosition) <= AttackRange;
	}

	private void ChasePlayer(double delta)
	{
		Vector2 direction = (_player.GlobalPosition - GlobalPosition).Normalized();
		Velocity = direction * ChaseSpeed;
		MoveAndSlide();
		
		// Update animation
		_animationPlayer.Play("run");
		UpdateSpriteDirection(direction);
		GD.Print($"{Name}: Chasing player.");
	}

	private void UpdateSpriteDirection(Vector2 direction)
	{
		if (direction.X < 0)
			_animationPlayer.FlipH = true;
		else if (direction.X > 0)
			_animationPlayer.FlipH = false;
	}

	private void Attack()
	{
		if (!_canAttack) return;
		
		_animationPlayer.Play("attack");
		  _slashSound?.Play();
		// Add your attack logic here (damage application, etc.)
		
		if (_player is MainCharacter player)
	{
		player.TakeDamage(10); // deal 10 damage on each attack
	}
	
		_canAttack = false;
		_attackCooldown.Start();
	}

	private void OnBodyEnteredDetectionArea(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			_player = body;
			GD.Print($"{Name}: Player entered detection area.");

		}
	}

	private void OnBodyExitedDetectionArea(Node2D body)
	{
		if (body == _player)
		{
			_player = null;
			Velocity = Vector2.Zero;
			_animationPlayer.Play("idle");
		}
	}

	private void OnAttackCooldownTimeout()
	{
		_canAttack = true;
		GD.Print($"{Name}: Attack cooldown reset.");

	}
}

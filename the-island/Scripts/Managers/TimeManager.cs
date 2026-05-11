using Godot;

public partial class TimeManager : Node
{
	// Singleton instance
	public static TimeManager Instance { get; private set; }

	// Time tracking - these are now your primary time attributes
	public int Minutes { get; private set; }
	public int Hours { get; private set; }
	public int Days { get; private set; }

	// Time Accessing (string format remains unchanged)
	public string CurrentTimeString => $"Day {Days} - {Hours:D2}:{Minutes:D2}";

	// Configuration
	[Export] public double RealTimePerGameDay = 10f;  // real seconds = game day
	private double _timePerMinute;
	private double _accumulatedTime;

	public override void _Ready()
	{
		// Singleton initialization
		if (Instance != null) 
		{
			QueueFree();
			return;
		}
		Instance = this;
		
		CalculateTimePerMinute();
		SetProcess(false);
	}

	public override void _Process(double delta)
	{
		_accumulatedTime += delta;
		while (_accumulatedTime >= _timePerMinute)
		{
			_accumulatedTime -= _timePerMinute;
			AdvanceTime();
		}
	}

	private void AdvanceTime()
	{
		Minutes++;
		if (Minutes >= 60)
		{
			Minutes = 0;
			Hours++;
			if (Hours >= 24)
			{
				Hours = 0;
				Days++;
			}
		}
	}

	public void StartTimeSystem() => SetProcess(true);
	public void StopTimeSystem() => SetProcess(false);

	private void CalculateTimePerMinute() 
		=> _timePerMinute = RealTimePerGameDay / (24f * 60f);

	public void SetDayDuration(double realSecondsPerGameDay)
	{
		RealTimePerGameDay = realSecondsPerGameDay;
		CalculateTimePerMinute();
	}
}

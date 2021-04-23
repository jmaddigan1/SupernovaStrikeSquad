public class Timer
{
	public float Time;
	public float Duration;

	System.Action callback;

	public Timer(float duration, System.Action callback)
	{
		this.Duration = duration;
		this.callback = callback;
	}

	public void IncrementTime()
	{
		Time += UnityEngine.Time.deltaTime;
		if (Time >= Duration)
		{
			callback?.Invoke();
			Time = 0;
		}
	}
}

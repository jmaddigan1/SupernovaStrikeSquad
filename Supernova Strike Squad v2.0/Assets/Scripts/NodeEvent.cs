public abstract class NodeEvent
{
	public string Name = "";
	public string Description = "";

	public EnvironmentData Environment;

	public virtual void OnStartEvent() { }

	public virtual void OnEndEvent() { }

	public virtual bool IsOver()
	{
		return true;
	}
}

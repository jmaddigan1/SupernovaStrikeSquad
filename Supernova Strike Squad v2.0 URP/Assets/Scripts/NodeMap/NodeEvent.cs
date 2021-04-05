public abstract class NodeEvent
{
	public string EventName;
	public string EventDescription;

	public NodeEvent()
	{
	}

	public abstract bool IsEventOver();

	public virtual void OnEventStart() { }
	public virtual void OnEventEnd() { }
}

public static class NodeEventPresets
{
	public static NodeEvent TestEvent()
	{
		NodeEvent nodeEvent = new NodeEvent_Arena()
		{
			EventName = "Test Event",

			EventDescription = "A description for the test Node"
		};

		return nodeEvent;
	}
}
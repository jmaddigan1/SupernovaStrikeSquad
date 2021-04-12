public static class NodeEventPresets
{
	public static NodeEvent TestAreana()
	{
		NodeEvent nodeEvent = new NodeEvent_Arena()
		{
			EventName = "Test Areana",

			EventDescription = "A description for the test Areana"
		};

		return nodeEvent;
	}
	
	public static NodeEvent TestRunner()
	{
		NodeEvent nodeEvent = new NodeEvent_Runner()
		{
			EventName = "Test Runner",

			EventDescription = "A description for the test Runner"
		};

		return nodeEvent;
	}

	public static NodeEvent TestBoss()
	{
		NodeEvent nodeEvent = new NodeEvent_Boss()
		{
			EventName = "Test Boss",

			EventDescription = "A description for the test Boss"
		};

		return nodeEvent;
	}
}
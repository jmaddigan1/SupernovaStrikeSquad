using System.Collections.Generic;

[System.Serializable]
public class NodeMapData
{
	public string Name = "";
	public string Description = "";

	public int CurrentDepth = 0;

	public GameModeType CampaignType;

	public List<NodeData> Nodes = new List<NodeData>();

	public int Depth;

	public bool Completed()
	{
		return CurrentDepth >= Depth;
	}
}

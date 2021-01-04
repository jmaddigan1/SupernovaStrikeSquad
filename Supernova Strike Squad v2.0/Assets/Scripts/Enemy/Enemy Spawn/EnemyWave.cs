using System.Collections.Generic;

public class EnemyWave
{
	public List<SpawnParameters> Enemies = new List<SpawnParameters>();

	public int GetEnemyCount()
	{
		int count = 0;

		foreach (SpawnParameters enemy in Enemies)
		{
			count += enemy.EnemyCount;
		}

		return count;
	}
}

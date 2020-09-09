using UnityEngine;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Level")]
[System.Serializable]
public class Levels : ScriptableObject
{
	public string levelName;
	public int levelIndex;
	public int maxLevelScore;
	public GameObject[] spawnableTiles;
}

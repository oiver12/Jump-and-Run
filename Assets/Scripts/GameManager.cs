using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

//public enum Schools
//{
//	Kindergarten = 0,
//	Primar = 1, 
//	Sekundar = 2,
//	Gym = 3,
//	Univerität = 4
//}

public class GameManager : MonoBehaviour
{
	public Levels[] allLevels;
	public static GameManager instance;
	Levels levelNow;
	int levelIndexNow = 0;
	public float playerSpeed = 3f;
	public Scrollbar scrollbar;
	public TextMeshProUGUI textField;
	public TextMeshProUGUI scoreField;
	public TextMeshProUGUI nextScoreField;
	int tilesPlaced = 0;
	int tilesInLastLevels = 0;

	private void Start()
	{
		LevelTiles levelTiles = GetComponent<LevelTiles>();
		levelTiles.allTiles = new List<GameObject[]>();
		for (int i = 0; i < allLevels.Length; i++)
		{
			levelTiles.allTiles.Add(allLevels[i].spawnableTiles);
		}
		instance = this;
		levelNow = allLevels[0];
		MovementTiles.speed = playerSpeed;
		textField.text = levelNow.name;
		nextScoreField.text = levelNow.maxLevelScore.ToString();
		levelTiles.Inizialize();
	}

	private void Update()
	{
		 float tilesPLacedSinceLastLevel = tilesPlaced - tilesInLastLevels;
		float maxScoreSinceLastLevel = levelNow.maxLevelScore - tilesInLastLevels;
		//Debug.Log((tilesPlaced - tilesInLastLevels) / (levelNow.maxLevelScore - tilesInLastLevels));
		scrollbar.size = tilesPLacedSinceLastLevel / maxScoreSinceLastLevel;
		if(tilesPlaced == levelNow.maxLevelScore)
		{
			NextLevel();
		}
	}

	void NextLevel()
	{
		tilesInLastLevels = tilesPlaced;
		levelIndexNow++;
		levelNow = allLevels[levelIndexNow];
		textField.text = levelNow.name;
		LevelTiles.instance.NewLevel();
		nextScoreField.text = levelNow.maxLevelScore.ToString();
	}

	public void NextTilePlaced()
	{
		tilesPlaced++;
		scoreField.text = tilesPlaced + "/";
	}
}

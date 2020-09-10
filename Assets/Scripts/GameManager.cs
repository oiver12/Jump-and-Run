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
	public TextMeshProUGUI scoreDeadTextField;
	public TextMeshProUGUI highScoreTextField;
	float timeInLastLevel;
	float timeSinceStartTheGame = 0f;
	float scoreNow;

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
		nextScoreField.text = levelNow.levelTime.ToString();
		levelTiles.Inizialize();
	}

	private void InternalRestart()
	{
		levelIndexNow = 0;
		playerSpeed = 3f;
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
		nextScoreField.text = levelNow.levelTime.ToString();
		levelTiles.Inizialize();
		timeInLastLevel = 0f;
		timeSinceStartTheGame = Time.time;
	}

	private void Update()
	{
		scoreNow = Time.time - timeSinceStartTheGame;
		scoreField.text = (int)(scoreNow) + "/";
		scrollbar.size = (scoreNow - timeInLastLevel) / (levelNow.levelTime - timeInLastLevel);
		if(scoreNow >= levelNow.levelTime)
		{
			NextLevel();
		}
	}

	void NextLevel()
	{
		levelIndexNow++;
		levelNow = allLevels[levelIndexNow];
		textField.text = levelNow.name;
		LevelTiles.instance.NewLevel();
		nextScoreField.text = levelNow.levelTime.ToString();
		timeInLastLevel = Time.time;
	}

	public void Restart()
	{
		InternalRestart();
		LevelGenerator.instance.Restart();
		UIManager.instance.Restart();
		UIManager.instance.player.Restart();
	}

	public void Die()
	{
		scoreDeadTextField.text = ((int)scoreNow).ToString();
		int highScore = 0;
		Debug.Log(PlayerPrefs.HasKey("HighScore"));
		if (PlayerPrefs.HasKey("HighScore"))
			highScore = PlayerPrefs.GetInt("HighScore");
		else
		{
			highScore = (int)scoreNow;
		}
		if (highScore >= scoreNow)
		{
			PlayerPrefs.SetInt("HighScore", highScore);
			PlayerPrefs.Save();
		}

		Debug.Log(highScore);
		highScoreTextField.text = highScore.ToString();
		UIManager.instance.Die();
	}

	//public void NextTilePlaced(float width, GameObject nextTilePlace)
	//{
	//	nextTile = nextTilePlace.transform;
	//	widthNextTile = width;
	//	tilesPlaced++;
	//	scoreField.text = tilesPlaced + "/";
	//}
}

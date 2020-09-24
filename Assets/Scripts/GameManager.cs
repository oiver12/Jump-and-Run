using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public string[] alleNoten;
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
	public TextMeshProUGUI noteTextField;
	float timeInLastLevel;
	float timeSinceStartTheGame = 0f;
	float scoreNow;
	int noteNow;

	private void Start()
	{
		noteNow = alleNoten.Length - 1;
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
		StartCoroutine(reduceTtroopDamageOvertime());
		noteTextField.text = alleNoten[noteNow];
	}

	private void InternalRestart()
	{
		noteNow = alleNoten.Length - 1;
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
		textField.text = levelNow.levelName;
		nextScoreField.text = levelNow.levelTime.ToString();
		levelTiles.Inizialize();
		timeInLastLevel = 0f;
		timeSinceStartTheGame = Time.time;
		noteTextField.text = alleNoten[noteNow];
		//StartCoroutine(reduceTtroopDamageOvertime());
		noteTextField.text = alleNoten[noteNow];
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

	public void ReduceNote()
	{
		noteNow--;
		if (noteNow < 0f)
			Die();
		else
			noteTextField.text = alleNoten[noteNow];
	}

	void NextLevel()
	{
		noteNow = alleNoten.Length - 1;
		levelIndexNow++;
		if (levelIndexNow >= alleNoten.Length)
			levelIndexNow--;

		levelNow = allLevels[levelIndexNow];
		textField.text = levelNow.levelName;
		LevelTiles.instance.NewLevel();
		nextScoreField.text = levelNow.levelTime.ToString();
		timeInLastLevel = Time.time;
		noteTextField.text = alleNoten[noteNow];
	}

	public void Restart()
	{
		InternalRestart();
		LevelSpawner.instance.Restart();
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

	IEnumerator reduceTtroopDamageOvertime()
	{
		yield return new WaitForSeconds(0.4f);
		while(true)
		{
			float secondsToWait = levelNow.levelTime / (alleNoten.Length - 1);
			Debug.Log(secondsToWait);
			yield return new WaitForSeconds(secondsToWait);
			ReduceNote();
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

	public static UIManager instance;

	public GameObject inGameUI;
	public GameObject dieMenu;
	public GameObject pauseMenu;
	public PlayerMovement player;

    // Start is called before the first frame update
    void Start()
    {
		instance = this;
		inGameUI.SetActive(true);
		pauseMenu.SetActive(false);
		dieMenu.SetActive(false);
    }

	public void Restart()
	{
		inGameUI.SetActive(true);
		pauseMenu.SetActive(false);
		dieMenu.SetActive(false);
	}

	public void PauseGame()
	{
		player.gamePause = true;
		inGameUI.SetActive(false);
		pauseMenu.SetActive(true);
		MovementTiles.speed = 0f;
	}

	public void ReplayGame()
	{
		player.gamePause = false;
		inGameUI.SetActive(true);
		pauseMenu.SetActive(false);
		MovementTiles.speed = 3f;
	}

	public void Die()
	{
		inGameUI.SetActive(false);
		pauseMenu.SetActive(false);
		dieMenu.SetActive(true);
		player.gamePause = true;
	}
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Schools
{
	Kindergarten = 0,
	Primar = 1, 
	Sekundar = 2,
	Gym = 3,
	Univerität = 4
}

public class GameManager : MonoBehaviour
{
	public Schools schoolNow;
	public float playerSpeed = 3f;
	public Scrollbar scrollbar;
	public TextMeshProUGUI textField;

	private void Start()
	{
		schoolNow = 0;
		MovementTiles.speed = playerSpeed;
		textField.text = schoolNow.ToString();
	}

	private void Update()
	{
		scrollbar.size = Time.time / (15f + (15f * (int)schoolNow));

		if (Time.time >= 15f + (15f * (int)schoolNow))
		{
			schoolNow++;
			textField.text = schoolNow.ToString();
			LevelTiles.instance.NewLevel();
		}
	}
}

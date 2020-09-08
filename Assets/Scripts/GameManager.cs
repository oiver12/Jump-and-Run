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
		//Debug.Log(Time.time);
		Debug.Log(10f + (10 * (int)schoolNow));
		scrollbar.size = Time.time / (10f + (10f * (int)schoolNow));

		if (Time.time >= 10f + (10f * (int)schoolNow))
		{
			schoolNow++;
			textField.text = schoolNow.ToString();
		}
	}
}

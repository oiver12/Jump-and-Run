using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{

	public GameObject background;
	GameObject background1;
	GameObject activeBackground;
	public static BackgroundManager instance;
	float speed;

    // Start is called before the first frame update
    void Start()
    {
		instance = this;
		background1 = Instantiate(background);
		background1.SetActive(false);
		activeBackground = background;
		speed = MovementTiles.speed;
    }

    // Update is called once per frame
    void Update()
    {
		activeBackground.transform.Translate(new Vector3(-speed * Time.deltaTime, 0f, 0f));
		if (activeBackground.transform.position.x <= 0f)
			ShowNewBackground();
    }

	void ShowNewBackground()
	{

	}
}

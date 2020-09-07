using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTiles : MonoBehaviour
{
	public float speed;

	private void Update()
	{
		//bewegen von der Map gegen die Laufrichtung vom Player
		transform.Translate(new Vector3(-speed * Time.deltaTime, 0f, 0f));
	}
}

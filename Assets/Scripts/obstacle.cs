using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacle : MonoBehaviour
{
	public float t = 1f;
	public float l = 10f;
	float posX;

	private void Start()
	{
		posX = transform.localPosition.x;
	}

	private void Update()
	{
		Vector3 pos = new Vector3(posX + Mathf.PingPong(t * Time.time, l), transform.localPosition.y, transform.localPosition.z);
		
		transform.localPosition = pos;
	}
}

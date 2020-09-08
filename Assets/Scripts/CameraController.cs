using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

	public Transform m_Target;
	float currentVel;
	public float smoothTime;

	void Update()
	{
		float posy = Mathf.SmoothDamp(transform.position.y, m_Target.position.y, ref currentVel, smoothTime);
		transform.position = new Vector3(transform.position.x, posy, transform.position.z);
	}
}

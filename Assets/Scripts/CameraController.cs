using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

	//offset from the viewport center to fix damping
	public Transform m_Target;
	Vector2 offset;
	float currentVel;

	private void Start()
	{
		offset = m_Target.position - transform.position;
	}

	void Update()
	{
		Mathf.SmoothDamp(transform.position, m_Target.position, ref currentVel, )
		//float targetX = m_Target.position.x + m_XOffset;
		//float targetY = m_Target.position.y + m_YOffset;

		//if (Mathf.Abs(transform.position.x - targetX) > margin)
		//	targetX = Mathf.Lerp(transform.position.x, targetX, 1 / m_DampTime * Time.deltaTime);

		//if (Mathf.Abs(transform.position.y - targetY) > margin)
		//	targetY = Mathf.Lerp(transform.position.y, targetY, m_DampTime * Time.deltaTime);

		transform.position = new Vector3(targetX, targetY, transform.position.z);
	}
}

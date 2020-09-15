using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

//regeln des Player Movements 
public class PlayerMovement : MonoBehaviour
{
	public bool gamePause = false;
	public float angleThreshold = 40f;
	public float jumpForce;
	public float fallMultiplier = 2.5f;
	public float lowJumpMultiplier = 2f;
	public float timeToDieAfterLastGroundContact = 10f;

	//wie viel mal sind wir gesprungen
	int jumpCounter = 0;
	bool isgrounded;
	float lastGroundContact;
	BoxCollider2D boxCollider2D;
	Rigidbody2D rig;
	Animator animator;

	private void Start()
	{
		//bekommen von Rigidbody
		rig = GetComponent<Rigidbody2D>();
		boxCollider2D = GetComponent<BoxCollider2D>();
		animator = transform.GetChild(0).GetComponent<Animator>();
		animator.SetBool("isWalking", true);
		lastGroundContact = Time.time;
	}

	public void Restart()
	{
		gamePause = false;
		animator.SetBool("isDead", false);
		transform.position = new Vector3(0f, 1.85f, 0f);
	}

	// Update is called once per frame
	private void Update()
    {
		if (gamePause)
			return;

		//wennn timeToDieAfterLastGroundContact der Boden nicht brührt wurde --> sterben
		if (Time.time - lastGroundContact >= timeToDieAfterLastGroundContact)
			Dead();

		bool wasgrounded = isgrounded;
		isgrounded = getIsGrounded();
		//wenn man im letzten Frame nicht am Boden war, aber jetzt schon, ist man jetzt gerade gelandet
		if (wasgrounded != isgrounded)
		{
			if (isgrounded)
				jumpCounter = 0;

			if (!isgrounded)
			{
				animator.SetBool("isJumping", false);
				if (jumpCounter == 0)
					jumpCounter = 1;
			}

			animator.SetBool("isGrounded", isgrounded);
		}
		if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
		{
			if (isgrounded)
			{
				animator.SetBool("isJumping", true);
				Jump();
			}
			else if(jumpCounter < 2)
			{
				//mid air jump
				StartCoroutine(doubleJump());
				Jump();
			}
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (isgrounded)
			{
				animator.SetBool("isJumping", true);
				Jump();
				
			}
			else if(jumpCounter < 2)
			{
				Debug.Log("ok");
				StartCoroutine(doubleJump());
				Jump();
			}
		}

		//gehen wir runter --> y ist minus
		if (rig.velocity.y < 0)
			//schenller fallen mit einem fallMultiplier
			rig.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
		//gehen wir rauf und sind nicht gerade gesprungen
		else if (rig.velocity.y > 0 && !(Input.GetMouseButton(0) | Input.GetKeyDown(KeyCode.Space)))
			//weniger schnell nach oben
			rig.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;

		//Screentshot in editor
		if (Input.GetKey(KeyCode.A) && Input.GetKeyDown(KeyCode.F))
		{
			ScreenCapture.CaptureScreenshot(Application.dataPath + "/Screenshoot.png");
			Debug.Log(Application.dataPath);
		}
	}

	//wenn wir einen Mid Air Jump machen, dann müssen wir 0.5 Sekunden warten bevor wir wieder doubleJump auf false setzten, damit der Character nicht die ganze Zeit einen double Jump ausführt
	IEnumerator doubleJump()
	{
		animator.SetBool("doubleJump", true);
		yield return new WaitForSeconds(0.5f);
		animator.SetBool("doubleJump", false);
	}

	/// <summary>
	/// sind wir auf dem Boden
	/// </summary>
	public bool getIsGrounded()
	{
		//checken mit einer Box ob man auf dem Boden ist
		RaycastHit2D hit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, 0.2f, 1 << 8);
		return hit.collider != null;
	}

	//wenn man in eine Object hineingelaufen ist
	void OnCollisionEnter2D(Collision2D collision)
	{
		//was ist es für ein Object

		//ist es ein Obstacle --> layer 9
		if (collision.gameObject.layer == 9)
		{
			//Debug.Log(Vector2.Angle((Vector2)transform.position - collision.contacts[0].point, Vector2.up));
			//den Winkel zum aufrechten Winkel ausrechen und wenn der Winkel unter 40 ist, dann kommen wir von oben
			if (Mathf.Abs(Vector2.Angle((Vector2)transform.position - collision.contacts[0].point, Vector2.up)) < angleThreshold)
				Destroy(collision.gameObject);
			//wenn wir von der Seite kommen, dann sterben wir
			else
				Dead();
		}
		//ist der Boden --> layer 8
		if(collision.gameObject.layer == 8)
		{
			lastGroundContact = Time.time;
			//Debug.Log(Vector2.Angle((Vector2)transform.position - collision.contacts[0].point, Vector2.up));
			//wenn wir von der Seite kommen, sterben wir
			if (Mathf.Abs(Vector2.Angle((Vector2)transform.position - collision.contacts[0].point, Vector2.up)) > angleThreshold)
				Dead();
		}
	}

	/// <summary>
	/// wir sind gestorben
	/// </summary>
	public void Dead()
	{
		MovementTiles.speed = 0f;
		animator.SetBool("isDead", true);
		GameManager.instance.Die();
	}

	//springe nach oben
	void Jump()
	{
		//jumpTimer plus 1, um nur zwei mal zu springen
		jumpCounter++;
		//rig.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
		//die geschwindigkeit des Characters nach oben setzen
		rig.velocity = new Vector2(0f, jumpForce);
	}

	//public void NextYValueTile(float yValue)
	//{
	//	if(yValue <= lowestYValue)
	//		lowestYValue = yValue
	//}
}


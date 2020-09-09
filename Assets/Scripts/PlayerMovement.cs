using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//regeln des Player Movements 
public class PlayerMovement : MonoBehaviour
{
	public int jumpTime = 0;
	public float angleThreshold = 40f;
	public float jumpForce;
	public float fallMultiplier = 2.5f;
	public float lowJumpMultiplier = 2f;

	bool isgrounded;
	BoxCollider2D boxCollider2D;
	Rigidbody2D rig;
	Animator animator;

	private void Start()
	{
		//bekommen von Rigedbody
		rig = GetComponent<Rigidbody2D>();
		boxCollider2D = GetComponent<BoxCollider2D>();
		animator = transform.GetChild(0).GetComponent<Animator>();
		animator.SetBool("isWalking", true);
	}

	// Update is called once per frame
	private void Update()
    {
		bool wasgrounded = isgrounded;
		isgrounded = getIsGrounded();
		//wenn man im letzten Frame nicht am Boden war, aber jetzt schon, ist man jetzt gerade gelandet
		if (wasgrounded != isgrounded)
		{
			if (isgrounded)
				jumpTime = 0;

			if(!isgrounded)
				animator.SetBool("isJumping", false);

			animator.SetBool("isGrounded", isgrounded);
		}
		if (Input.GetMouseButtonDown(0))
		{
			if (isgrounded)
			{
				animator.SetBool("isJumping", true);
				jumpTime++;
				rig.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
			}
			else if(jumpTime < 2)
			{
				//mid air jump
				jumpTime++;
				rig.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
			}
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (isgrounded)
			{
				animator.SetBool("isJumping", true);
				jumpTime++;
				rig.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
			}
			else if(jumpTime < 2)
			{
				jumpTime++;
				rig.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
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
			Debug.Log(Vector2.Angle((Vector2)transform.position - collision.contacts[0].point, Vector2.up));
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
			Debug.Log(Vector2.Angle((Vector2)transform.position - collision.contacts[0].point, Vector2.up));
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
		rig.velocity = new Vector2(-MovementTiles.speed, 0f);
		animator.SetBool("isDead", true);
	}
}


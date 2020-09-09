using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
		animator.SetBool("isDoubleJump", false);
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
		if (Input.GetMouseButtonDown(0) && isgrounded || Input.GetMouseButtonDown(0) && jumpTime < 2)
		{
			animator.SetBool("isJumping", true);
			jumpTime++;
			rig.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
		}

		if (Input.GetKeyDown(KeyCode.Space) && isgrounded || Input.GetKeyDown(KeyCode.Space) && jumpTime < 2)
		{
			jumpTime++;
			rig.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
		}

		if (rig.velocity.y < 0)
			rig.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
		else if (rig.velocity.y > 0 && !(Input.GetMouseButton(0) | Input.GetKeyDown(KeyCode.Space)))
			rig.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;

		//if(Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Began && isgrounded || Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Began && jumpTime < 2)
		//{
		//	Debug.Log("Jetzt");
		//	jumpTime++;
		//	rig.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
		//}
	}

	public bool getIsGrounded()
	{
		RaycastHit2D hit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, 0.5f, 1 << 8);
		return hit.collider != null;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		//obstacle
		if (collision.gameObject.layer == 9)
		{
			Debug.Log(Vector2.Angle((Vector2)transform.position - collision.contacts[0].point, Vector2.up));
			if (Mathf.Abs(Vector2.Angle((Vector2)transform.position - collision.contacts[0].point, Vector2.up)) < angleThreshold)
				Destroy(collision.gameObject);
			else
				Dead();
		}
		if(collision.gameObject.layer == 8)
		{
			Debug.Log(Vector2.Angle((Vector2)transform.position - collision.contacts[0].point, Vector2.up));
			if (Mathf.Abs(Vector2.Angle((Vector2)transform.position - collision.contacts[0].point, Vector2.up)) > angleThreshold)
				Dead();
		}
	}

	public void Dead()
	{
		//rig.velocity = new Vector2(0)
		rig.velocity = new Vector2(-MovementTiles.speed, 0f);
		animator.SetBool("isDead", true);
	}

	void Jump()
	{
		rig.velocity = new Vector2(rig.velocity.x, 0);
		rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
	}
}


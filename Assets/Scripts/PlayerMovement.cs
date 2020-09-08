using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	BoxCollider2D boxCollider2D;
	Rigidbody2D rig;
	Animator animator;
	public float jumpForce;
	bool isgrounded;
	public int jumpTime = 0;
	public float angleThreshold = 40f;

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
		if (wasgrounded != isgrounded)
		{
			if (isgrounded)
				jumpTime = 0;

			animator.SetBool("isJumping", !isgrounded);
		}
		if (Input.GetMouseButtonDown(0) && isgrounded || Input.GetMouseButtonDown(0) && jumpTime < 2)
		{
			jumpTime++;
			rig.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
		}

		if (Input.GetKeyDown(KeyCode.Space) && isgrounded || Input.GetKeyDown(KeyCode.Space) && jumpTime < 2)
		{
			jumpTime++;
			rig.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
		}
	}

	public bool getIsGrounded()
	{
		RaycastHit2D hit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, 0.3f, 1 << 8);
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
		MovementTiles.instance.speed = 0f;
		animator.SetBool("isDead", true);
	}

	void Jump()
	{
		rig.velocity = new Vector2(rig.velocity.x, 0);
		rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
	}
}


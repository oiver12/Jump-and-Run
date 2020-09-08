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

	private void Start()
	{
		//bekommen von Rigedbody
		rig = GetComponent<Rigidbody2D>();
		boxCollider2D = GetComponent<BoxCollider2D>();
		animator = transform.GetChild(0).GetComponent<Animator>();
		animator.SetBool("isWalking", true);
	}

	// Update is called once per frame
	private void FixedUpdate()
    {
		bool wasgrounded = isgrounded;
		isgrounded = getIsGrounded();
		if (wasgrounded != isgrounded)
		{
			Debug.Log("Jetzt");
			animator.SetBool("isJumping", !isgrounded);
		}
		if (Input.GetMouseButtonDown(0) && isgrounded || Input.GetKeyDown(KeyCode.Space) && isgrounded)
		{
			rig.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
		}
    }

	public bool getIsGrounded()
	{
		RaycastHit2D hit = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, 0.1f, 1 << 8);
		return hit.collider != null;
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer == 9)
		{
			Debug.Log(Vector2.Angle(transform.position - collision.transform.position, Vector2.up));
			if (Mathf.Abs(Vector2.Angle(transform.position - collision.transform.position, Vector2.up) - 90f) < 10f)
				Debug.Log("Good Kill it");
			else
				Debug.Log("Dead");
		}
	}

	void Jump()
	{
		rig.velocity = new Vector2(rig.velocity.x, 0);
		rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
	}
}


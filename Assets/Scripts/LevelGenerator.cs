using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

	public MovementTiles movment;
	public PlayerMovement player;
	public Vector2 vel;
	public GameObject prefabTile;
	public GameObject parentObject;
	public float startSeed;

	private void Start()
	{
		vel.x = movment.speed;
		vel.y = player.jumpForce;
		float maxHeight = vel.y * vel.y / (2 * -Physics2D.gravity.y);
		float maxWidth = 2 * vel.x * vel.y / (-Physics2D.gravity.y);
		Debug.Log(GetYPositionAtX(maxWidth, vel, -Physics.gravity.y));
		Debug.Log(maxHeight);
		Debug.Log(maxWidth);
	}

	float GetYPositionAtX(float positionx,Vector2 velocity, float gravityY)
	{
		float angle = Vector2.Angle(velocity, Vector2.right);
		angle *= Mathf.Deg2Rad;
		float cos = Mathf.Cos(angle);
		//y = x tanθ− - g/(2*vel^2*cos(angle))*x^2

		return positionx * Mathf.Tan(angle) - gravityY / (2 * vel.sqrMagnitude * cos * cos) * positionx * positionx;
	}
}

using Assets.Scripts.Level_Generator.Infinite;
using System.Collections.Generic;
using UnityEngine;

//generieren von endless Tiles
public class LevelSpawner : MonoBehaviour
{
	//public bool calc = true;
	//public MovementTiles movment;
	//public PlayerMovement player;
	//public GameObject prefabTile;
	//public Transform parentObject;
	//public float maxJumpTime;

	//Vector2 vel;
	//eine statische Instanz, weil es nur ein Level Genrator gibt
	public static LevelSpawner instance;
	//alle Tiles, welche im Inspector angegen werden
	//public List<GameObject> platformPrefab;
	//alle Teile, welche nicht sichtbar sind und somit noch platziert werden können, weil sie verfügbar sind
	//List<GameObject> notVisibelTilesInScene = new List<GameObject>();
	//alle Tiles welche gerade sichtbar 
	List<GameObject> activeTiles = new List<GameObject>();
	public InfiniteLevelSettings infiniteLevelGenerator;
	public GameObject startTile;
	Vector2 endPos;
	public float xSpawnPosition = 10f;

	private void Start()
	{
		instance = this;
		////setzte von jedem Object mehrere Tiles und mache sie nicht sichtbar um danach diese setzten zu können
		//for (int i = 0; i < platformPrefab.Count; i++)
		//{
		//	for (int y = 0; y < 5; y++)
		//	{
		//		//setzte alle und mache sie nicht sichtbar
		//		GameObject go = Instantiate(platformPrefab[i]);
		//		go.SetActive(false);
		//		notVisibelTilesInScene.Add(go);
		//	}
		//}
		//setzte das erste Teil auf 0, 0
		endPos = Vector2.zero;
		//setzte ein Teil
		SpawnStartTile();

		//float width = platformPrefab[0].GetComponent<SpriteRenderer>().bounds.size.x;
		//Debug.Log(width);
		//endPos = Vector2.zero;
		//for (int i = 0; i < 10; i++)
		//{
		//	GameObject go = Instantiate(platformPrefab[0], new Vector2(endPos.x + (width / 2), endPos.y), Quaternion.identity, parentObject);
		//	endPos = go.transform.GetChild(0).position;
		//}
	}

	public void Restart()
	{
		activeTiles = new List<GameObject>();
		endPos = Vector2.zero;
		SpawnTile();
	}

	private void Update()
	{
		if(activeTiles[0].transform.position.x <= -40f)
			DestroyLastObject();

		if (activeTiles[activeTiles.Count - 1].transform.position.x <= 40f)
		{
			infiniteLevelGenerator.SpawnSectionRight();
			//SpawnTile();
		}
	}

	void DestroyLastObject()
	{
		GameObject lastObject = activeTiles[0];
		activeTiles.RemoveAt(0);
		LevelTiles.instance.DeactivateTile(lastObject);
		//lastObject.SetActive(false);
		//notVisibelTilesInScene.Add(lastObject);
	}

	void SpawnTile()
	{
		//int tile = Random.Range(0, notVisibelTilesInScene.Count-1);
		//float width;
		//GameObject spawnedGameObject = notVisibelTilesInScene[tile];
		//notVisibelTilesInScene.RemoveAt(tile);
		GameObject spawnedGameObject = LevelTiles.instance.GetTile();
		spawnedGameObject.SetActive(true);
		float width;
		if (activeTiles.Count > 0)
		{
			endPos = activeTiles[activeTiles.Count - 1].transform.GetChild(1).position; //endPositionObject
																						//width = activeTiles[activeTiles.Count - 1].transform.GetChild(1).position.x - activeTiles[activeTiles.Count - 1].transform.GetChild(0).position.x;
			width = spawnedGameObject.transform.GetChild(1).position.x - spawnedGameObject.transform.GetChild(0).position.x;
		}
		else
		{
			width = spawnedGameObject.GetComponent<SpriteRenderer>().bounds.size.x;
		}

		spawnedGameObject.transform.position = new Vector2(endPos.x + (width / 2), endPos.y);
		activeTiles.Add(spawnedGameObject);
		//UIManager.instance.player.NextYValueTile(spawnedGameObject.transform.position.y);
		//GameManager.instance.NextTilePlaced(width, spawnedGameObject);
	}

	public void SpawnStartTile()
	{
		GameObject spawnedGameObject = startTile;
		spawnedGameObject.SetActive(true);
		float width = spawnedGameObject.transform.GetChild(1).position.x - spawnedGameObject.transform.GetChild(0).position.x;
		spawnedGameObject.transform.position = new Vector2(endPos.x + (width / 2), endPos.y);
		activeTiles.Add(spawnedGameObject);
		SpawnTile();
	}

	//private void CalcTiles()
	//{
	//	vel.x = movment.speed;
	//	vel.y = player.jumpForce;
	//	float widthTile = prefabTile.GetComponent<SpriteRenderer>().bounds.size.x;
	//	Debug.Log(widthTile);
	//	Vector2 endPosition = Vector2.zero;
	//	GameObject go = Instantiate(prefabTile, parentObject);
	//	go.transform.position = Vector2.zero;
	//	GameObject goasd = new GameObject("Test");
	//	endPosition.x -= widthTile / 2;
	//	goasd.transform.position = new Vector2(endPosition.x, endPosition.y);
	//	for (int i = 0; i < 10; i++)
	//	{
	//		//gehe zum Ende des Tiles
	//		endPosition.x += widthTile;
	//		//tile Position ist Anfang des Tiles
	//		Vector2 tilePosition = getTilePosition(endPosition, vel, maxJumpTime, Physics2D.gravity.y);
	//		GameObject tileObject = new GameObject("StartTile");
	//		tileObject.transform.position = tilePosition;
	//		Debug.LogFormat("Tile Position is at: {0} and endPosition is at: {1}", tilePosition, endPosition);
	//		endPosition = tilePosition;
	//		tilePosition.x += widthTile / 2;
	//		GameObject go1 = Instantiate(prefabTile, tilePosition, Quaternion.identity, parentObject);
	//		tileObject.transform.parent = go1.transform;

	//	}
	//}

	//Vector2 getTilePosition(Vector2 endPositionTile, Vector2 velocity, float _maxJumpTime, float gravityY)
	//{
	//	//s = v* t
	//	float distanceToJump = vel.x * _maxJumpTime;
	//	//nur horizontale Tiles
	//	Vector2 firstJumpingPoint = new Vector2(endPositionTile.x - distanceToJump, endPositionTile.y);
	//	//float maxWidth = 2 * vel.x * vel.y / (-Physics2D.gravity.y);
	//	float maxWidth = 2 * velocity.x * velocity.y / (-gravityY);
	//	//TODO distance Range hinzufügen
	//	float xPosition = Random.Range(maxWidth / 2, maxWidth);
	//	float yPosition = GetYPositionAtX(xPosition, velocity, gravityY);
	//	return new Vector2(firstJumpingPoint.x + xPosition, firstJumpingPoint.y + yPosition);
	//}

	//float GetYPositionAtX(float positionx,Vector2 velocity, float gravityY)
	//{
	//	float angle = Vector2.Angle(velocity, Vector2.right);
	//	angle *= Mathf.Deg2Rad;
	//	float cos = Mathf.Cos(angle);
	//	//y(x) = x tanθ− - g/(2*vel^2*cos(angle))*x^2
	//	return positionx * Mathf.Tan(angle) - gravityY / (2 * vel.sqrMagnitude * cos * cos) * positionx * positionx;
	//}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
//public class SpawnTilesInLevel
//{
//	public List<GameObject> tilesFromLevel;
//}

public class LevelTiles : MonoBehaviour
{
	public List<GameObject[]> allTiles;
	Dictionary<int, List<GameObject>> allActiveTiles = new Dictionary<int, List<GameObject>>();
	Transform parentInactiveObject;
	Transform parentActiveObject;
	int levelNow = -1;
	public static LevelTiles instance;

	public void Inizialize()
	{
		Restart();
		instance = this;
		parentActiveObject = new GameObject("ActiveObject").transform;
		levelNow = -1;
		NewLevel();
	}

	public GameObject GetTile()
	{
		int randTileIndex = Random.Range(-1, allActiveTiles.Count);
		List<GameObject> tiles = allActiveTiles[randTileIndex];
		if(tiles.Count == 0)
		{
			GameObject go = Instantiate(allTiles[levelNow][randTileIndex], parentActiveObject);
			go.name = randTileIndex.ToString();
			return go;
		}
		else
		{
			GameObject objectToPlace = tiles[0];
			objectToPlace.SetActive(true);
			objectToPlace.transform.parent = null;
			allActiveTiles[randTileIndex].RemoveAt(0);
			objectToPlace.transform.parent = parentActiveObject;
			return objectToPlace;
		}
	}

	public void DeactivateTile(GameObject tile)
	{
		int tileIndex = -1;
		try
		{
			tileIndex = int.Parse(tile.name);
		}
		catch
		{
			Destroy(tile);
			return;
		}
		if (tileIndex == -1)
		{
			Destroy(tile);
			return;
		}
		tile.SetActive(false);
		allActiveTiles[tileIndex].Add(tile);
		tile.transform.parent = parentInactiveObject;
	}

	public void NewLevel()
	{
		Debug.Log(allTiles.Count);
		levelNow++;
		if(parentInactiveObject != null)
			Destroy(parentInactiveObject.gameObject);
		allActiveTiles.Clear();
		for (int i = 0; i < allTiles[levelNow].Length; i++)
		{
			allActiveTiles.Add(i, new List<GameObject>());
		}
		parentInactiveObject = new GameObject("TilesParentObject").transform;
		for (int i = 0; i < parentActiveObject.childCount; i++)
		{
			parentActiveObject.GetChild(i).name = "-1";
		}
	}

	public void Restart()
	{
		if (parentActiveObject != null)
			Destroy(parentActiveObject.gameObject);
		if (parentInactiveObject != null)
			Destroy(parentInactiveObject.gameObject);
	}
}

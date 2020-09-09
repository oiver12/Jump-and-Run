﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnTilesInLevel
{
	public List<GameObject> tilesFromLevel;
}

public class LevelTiles : MonoBehaviour
{
	public List<SpawnTilesInLevel> allTiles;
	Dictionary<int, List<GameObject>> allActiveTiles = new Dictionary<int, List<GameObject>>();
	Transform parentInactiveObject;
	Transform parentActiveObject;
	int levelNow = -1;
	public static LevelTiles instance;

	private void Start()
	{
		instance = this;
		parentActiveObject = new GameObject("ActiveObject").transform;
		NewLevel();
	}

	public GameObject GetTile()
	{
		int randTileIndex = Random.Range(0, allActiveTiles.Count);
		List<GameObject> tiles = allActiveTiles[randTileIndex];
		if(tiles.Count == 0)
		{
			GameObject go = Instantiate(allTiles[levelNow].tilesFromLevel[randTileIndex], parentActiveObject);
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
		int tileIndex = int.Parse(tile.name);
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
		levelNow++;
		if(parentInactiveObject != null)
			Destroy(parentInactiveObject.gameObject);
		allActiveTiles.Clear();
		for (int i = 0; i < allTiles[levelNow].tilesFromLevel.Count; i++)
		{
			allActiveTiles.Add(i, new List<GameObject>());
		}
		parentInactiveObject = new GameObject("TilesParentObject").transform;
		for (int i = 0; i < parentActiveObject.childCount; i++)
		{
			parentActiveObject.GetChild(i).name = "-1";
		}
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System;

public class BoardManager : MonoBehaviour
{
    // Start is called before the first frame update
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;
        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 10;
    public int rows = 10;
    public int roadIndex;
    public Count treeCount = new Count(8, 10);
    public Count healthCount = new Count(2, 6);
    public Count enemyCount = new Count(5, 10);
    public GameObject[] floorTiles;
    public GameObject[] roadTiles;
    public GameObject[] healthTiles;
    public GameObject[] treeTiles;
    public GameObject[] enemyTiles;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();
    void InitialiseList()
    {
        gridPositions.Clear();
        for (int x = 1; x < columns - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;
        roadIndex = Random.Range(0, columns - 1);

        for (int x = -1; x < columns + 1; x++)
        {

            for (int y = -1; y < rows + 1; y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if (x == roadIndex || x == roadIndex + 1)
                    toInstantiate = roadTiles[Random.Range(0, roadTiles.Length)];

                var instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity);

                instance.transform.SetParent(boardHolder);

                if ((x == -1 || x == columns || y == -1 || y == rows) && x != roadIndex && x != roadIndex + 1)
                    toInstantiate = treeTiles[Random.Range(0, treeTiles.Length)];

                instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity);

                instance.transform.SetParent(boardHolder);
            }
        }
    }

    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    void LayoutObjectsAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum + 1);
        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }
 
    public void SetupScene()
    {
        BoardSetup();
        InitialiseList();
        LayoutObjectsAtRandom(treeTiles, treeCount.minimum, treeCount.maximum);
        LayoutObjectsAtRandom(healthTiles, healthCount.minimum, healthCount.maximum);
        LayoutObjectsAtRandom(enemyTiles, enemyCount.minimum, enemyCount.maximum);
    }
}

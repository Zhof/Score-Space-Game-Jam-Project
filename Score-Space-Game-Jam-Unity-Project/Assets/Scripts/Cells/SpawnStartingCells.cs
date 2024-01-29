using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnStartingCells : MonoBehaviour
{
    [SerializeField]
    int amountOfCellsToSpawn;
    [SerializeField]
    float timeToSplit;
    [SerializeField]
    float speed;
    [SerializeField]
    Transform playerTransform;


    [SerializeField]
    GameObject blockCellPrefab;

    GameObject currentlyInstantiating;

    //Spawns as many cells as defined in amountOfCellsToSpawn, with random properties for:
    //Position. Must be a whole number on the x and y,
    //Moves vert. Splits vert must be set to whatever movesVert isn't.
    //Color, either one or two.
    private void Start()
    {
        SpawnNewCells();
    }

    public void SpawnNewCells()
    {
        for (int i = 0; i < amountOfCellsToSpawn; i++)
        {
            currentlyInstantiating = Instantiate(blockCellPrefab);
            bool randomVert = Random.Range(0, 2) == 1 ? true : false;
            Vector2 randomPos = new Vector2(Random.Range(-7, 7), Random.Range(-5, 5));
            int color = Random.Range(0, 2);

            currentlyInstantiating.GetComponent<BlockCellInstantiate>().
                InstantiateBlock(randomVert, color, randomVert ? false : true, 1, 2, timeToSplit, speed);
            currentlyInstantiating.transform.position = randomPos;

            /*if(i == amountOfCellsToSpawn - 1)
            {
                playerTransform.position = randomPos + Vector2.up;
            }*/

            BlockCellsManager.Instance.blockCellsList.Add(currentlyInstantiating.transform);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnStartingCells : MonoBehaviour
{
    [SerializeField]
    GameObject blockCellPrefab;

    GameObject currentlyInstantiating;

    //Spawns as many cells as defined in amountOfCellsToSpawn, with random properties for:
    //Position. Must be a whole number on the x and y,
    //Moves vert. Splits vert must be set to whatever movesVert isn't.
    //Color, either one or two.
    private void Start()
    {
        
    }
}
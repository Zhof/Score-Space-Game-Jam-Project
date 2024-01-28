using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCellSplit : MonoBehaviour
{
    //Same system as splits vert; this bool represents the movement of this block cell.
    public bool movesVert;
    //If true, the cell is stacked like a sandwich, and when it splits, cells spawn above and below it.
    //If false, the cell is like a bookshelf, and when it splits, cells spawn to the left and right of it.
    public bool splitsVert;
    //Same system as color a and b; this int represents the movement/color of this block cell.
    public int color;
    //0 = grey (no movement,) 1 = blue (up or right,) 2 = red (left or down).
    //color A represents either the bottom colour or the left colour.
    //If expanding this script to support different amounts of splits,
    //turning this into an array would be a good idea.
    public int colorA;
    public int colorB;

    public float timerStartingValue;
    public float timeUntilSplit;

    BlockCellMovement blockCellMovement;

    private void Start()
    {
        blockCellMovement = GetComponent<BlockCellMovement>();
    }

    private void Update()
    {
        timeUntilSplit -= Time.deltaTime;

        if(timeUntilSplit <= 0)
        {
            SplitCell();
        }
    }

    GameObject currentlyInstantiating;
    //Spawns and instantiates two cells then deletes self.
    void SplitCell()
    {
        if (splitsVert)
        {
            //spawn block above this one
            currentlyInstantiating = Instantiate(PrefabManager.Instance.BlockCellPrefab, new Vector3(0, 0.55f, 0) + transform.position, Quaternion.identity);
            InstantiateNewBlock(true);
            //spawn block below this one
            currentlyInstantiating = Instantiate(PrefabManager.Instance.BlockCellPrefab, new Vector3(0, -0.55f, 0) + transform.position, Quaternion.identity);
            InstantiateNewBlock(false);
            //destroy self
            Destroy(gameObject);
        }
        else
        {
            //spawn block to the right of this one
            currentlyInstantiating = Instantiate(PrefabManager.Instance.BlockCellPrefab, new Vector3(0.55f, 0, 0) + transform.position, Quaternion.identity);
            InstantiateNewBlock(true);
            //spawn block to the left of this one
            currentlyInstantiating = Instantiate(PrefabManager.Instance.BlockCellPrefab, new Vector3(-0.55f, 0, 0) + transform.position, Quaternion.identity);
            InstantiateNewBlock(false);
            //destroy self
            Destroy(gameObject);
        }
    }
    void InstantiateNewBlock(bool usesColorA)
    {
        currentlyInstantiating.GetComponent<BlockCellInstantiate>().InstantiateBlock
                (movesVert, usesColorA ? colorA : colorB, splitsVert, colorA, colorB, timerStartingValue, blockCellMovement.speed);
    }
}

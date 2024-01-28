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

    public float timeGrowing; // how long to grow

    public float timerStartingValue;
    public float timeUntilSplit;

    //Once a new cell has spawned, splits paused is set to true if it was created in a period where all the other
    //Cells are in the process of splitting. In this case, skip this split by setting it back to false once time until split <= 0.
    public bool splitsPaused = false;

    BlockCellMovement blockCellMovement;

    private void Start()
    {
        blockCellMovement = GetComponent<BlockCellMovement>();

        if(timeUntilSplit <= timeGrowing)
        {
            splitsPaused = true;
        }
    }

    private void Update()
    {
        timeUntilSplit -= Time.deltaTime;


        transform.localScale = Vector3.one;
        //When the cells start scaling.
        if(timeUntilSplit <= timeGrowing && splitsPaused == false)
        {
            transform.localScale = ScaleFromTime();
        }

        if(timeUntilSplit <= 0)
        {
            if (splitsPaused == false)
                SplitCell();
            else
            {
                timeUntilSplit = timerStartingValue;
                splitsPaused = false;
            }
        }
    }
    Vector3 newScale;
    Vector3 ScaleFromTime()
    {
        newScale = Vector3.one;
        if (splitsVert)
        {
            newScale.y = Mathf.Lerp(1, 2, 1 - (timeUntilSplit / timeGrowing));
        }
        else
        {
            newScale.x = Mathf.Lerp(1, 2, 1 - (timeUntilSplit / timeGrowing));
        }
        return newScale;
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
            BlockCellsManager.Instance.blockCellsList.Remove(transform);
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
            BlockCellsManager.Instance.blockCellsList.Remove(transform);
            Destroy(gameObject);
        }

    }
    void InstantiateNewBlock(bool usesColorA)
    {
        currentlyInstantiating.GetComponent<BlockCellInstantiate>().InstantiateBlock
                (movesVert, usesColorA ? colorA : colorB, splitsVert, colorA, colorB, timerStartingValue, blockCellMovement.speed);
    }
}

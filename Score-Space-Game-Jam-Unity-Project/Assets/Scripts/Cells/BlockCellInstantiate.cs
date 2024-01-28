using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCellInstantiate : MonoBehaviour
{
    BlockCellSplit blockCellSplitScript;
    BlockCellMovement blockCellMovement;
    public void InstantiateBlock(bool movesVert, int color, bool splitsVert, int colorA, int colorB, float timerStartingValue, float speed)
    {
        if (BlockCellsManager.Instance.blockCellsList.Count > 100)
        {
            return;
        }

        BlockCellsManager.Instance.blockCellsList.Add(transform);

        blockCellSplitScript = GetComponent<BlockCellSplit>();
        blockCellMovement = GetComponent<BlockCellMovement>();

        //whichever way this cell splits, make the cells it create split the opposite way.
        blockCellSplitScript.splitsVert = splitsVert ? false : true;
        blockCellSplitScript.color = color;
        blockCellSplitScript.movesVert = movesVert ? false : true;
        blockCellMovement.speed = speed;

        blockCellSplitScript.colorA = colorA;
        blockCellSplitScript.colorB = colorB;

        blockCellSplitScript.timerStartingValue = timerStartingValue; //+ Random.Range(-0.1f, 0.1f);
        blockCellSplitScript.timeUntilSplit = blockCellSplitScript.timerStartingValue;

        InstantiateAppearance(movesVert, color, splitsVert, colorA, colorB);
    }
    //Alternate version used by blockCellDeath. Includes an extra variable which allows you to set timerStartingValue and timeUntilSplit
    //Independently. This allows you to sink up all of the cells together.
    public void InstantiateBlock(bool movesVert, int color, bool splitsVert, int colorA, int colorB, float timerStartingValue, float timerCurrentValue, float speed)
    {
        if(BlockCellsManager.Instance.blockCellsList.Count > 100)
        {
            return;
        }

        BlockCellsManager.Instance.blockCellsList.Add(transform);

        blockCellSplitScript = GetComponent<BlockCellSplit>();
        blockCellMovement = GetComponent<BlockCellMovement>();

        //whichever way this cell splits, make the cells it create split the opposite way.
        blockCellSplitScript.splitsVert = splitsVert ? false : true;
        blockCellSplitScript.color = color;
        blockCellSplitScript.movesVert = movesVert ? false : true;
        blockCellMovement.speed = speed;

        blockCellSplitScript.colorA = colorA;
        blockCellSplitScript.colorB = colorB;

        blockCellSplitScript.timerStartingValue = timerStartingValue; //+ Random.Range(-0.1f, 0.1f);
        blockCellSplitScript.timeUntilSplit = timerCurrentValue;

        InstantiateAppearance(movesVert, color, splitsVert, colorA, colorB);
    }

    //After the block is instantiated in the code, instantiate the way the block looks.
    void InstantiateAppearance(bool movesVert, int color, bool splitsVert, int colorA, int colorB)
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCellDeath : MonoBehaviour
{
    BlockCellSplit blockCellSplit;

    private void Awake()
    {
        blockCellSplit = GetComponent<BlockCellSplit>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Kill"))
        {
            Destroy(gameObject);
        }
    }

    //When two cells collide, merge them.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //We need some way to be able to tell only one cell to create the new cell. Do this by checking
        //x position, with the person on the right surviving
        if (collision.collider.CompareTag("Cell"))
        {
            GetComponent<BoxCollider2D>().enabled = false;
            collision.collider.GetComponent<BoxCollider2D>().enabled = false;

            Vector3 otherCellPos = collision.transform.position;
            BlockCellSplit otherCellSplitScript = collision.collider.GetComponent<BlockCellSplit>();
            int otherCellColor = otherCellSplitScript.color;
            float otherCellTimer = otherCellSplitScript.timerStartingValue;
            float otherCellSpeed = collision.collider.GetComponent<BlockCellMovement>().speed;

            if (transform.position.x > collision.transform.position.x)
            {
                Destroy(collision.gameObject);

                Debug.Log(blockCellSplit.color + " and " + otherCellColor);
                CreateNewCell(blockCellSplit.color, otherCellColor, transform.position, otherCellPos,
                    blockCellSplit.timerStartingValue, otherCellTimer, GetComponent<BlockCellMovement>().speed, otherCellSpeed);
                Destroy(gameObject);
            }
            //though if x is exactly equal use the y instead.
            else if(transform.position.x == collision.transform.position.x)
            {
                if(transform.position.y > collision.transform.position.y)
                {
                    Destroy(collision.gameObject);
                }
            }

        }
    }
    //If two blocks collide, create a new cell in between them.
    //color a comes from the cell that was on the left, color b comes from the one on the right.
    //the current color is set to grey.
    void CreateNewCell(int colorA, int colorB, Vector3 cellAPos, Vector3 cellBPos, float timerValueA, float timerValueB, float speedA, float speedB)
    {
        Vector3 inBetweenPos = new Vector3((cellAPos.x + cellBPos.x) * 0.5f, (cellAPos.y + cellBPos.y) * 0.5f, 0);
        GameObject currentlyInstantiating = Instantiate(PrefabManager.Instance.BlockCellPrefab, inBetweenPos, Quaternion.identity);

        //this is kind of a cruddy way to do it, but 0s spread for some reason and it might take a while to figure out why.
        //Instead, invalidate 0s and set the colors to something else.
        if (colorA == 0) colorA = 1;
        if (colorB == 0) colorB = 2;

        colorA = 1;
        colorB = 2;

        bool randomBool = Random.Range(0, 2) == 1 ? true : false;
        currentlyInstantiating.GetComponent<BlockCellInstantiate>().
            InstantiateBlock(randomBool, 0, randomBool ? false : true, colorA, colorB, 
            (timerValueA + timerValueB) * 0.5f, blockCellSplit.timeUntilSplit, (speedA + speedB) * 0.5f);
    }
}
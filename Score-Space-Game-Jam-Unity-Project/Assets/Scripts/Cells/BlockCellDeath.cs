using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCellDeath : MonoBehaviour
{
    [SerializeField]
    float timeToFinishAnim; //in seconds
    //Because it's lerping, this is between 0 (animation start) and 1 (animation finish)
    float timeIntoAnim;
    bool animating;

    //The secondary cell is the one which just handles itself, doesn't worry about instantiating the new cell.
    bool secondaryCell;

    Transform newCellTransform;

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

    private void Update()
    {
        if(animating == false)
        {
            timeIntoAnim = 0;
        }
        else
        {
            if(timeIntoAnim >= 1)
            {
                //Animations ended
                newCellTransform.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                newCellTransform.gameObject.GetComponent<SpriteRenderer>().enabled = true;

                Destroy(gameObject);
            }
            else
            {
                timeIntoAnim += Time.deltaTime * (1 / timeToFinishAnim);

                transform.position = new Vector3(Mathf.Lerp(originalPosition.x, newCellTransform.position.x, timeIntoAnim),
                    Mathf.Lerp(originalPosition.y, newCellTransform.position.y, timeIntoAnim), 0);
                transform.localScale = new Vector3(Mathf.Lerp(originalScale.x, newCellTransform.localScale.x, timeIntoAnim),
                    Mathf.Lerp(originalScale.y, newCellTransform.localScale.y, timeIntoAnim), 1);
            }
        }
    }


    GameObject collidedWith;


    //When two cells collide, merge them.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //cant die again
        if (animating) return;

        collidedWith = collision.collider.gameObject;

        //We need some way to be able to tell only one cell to create the new cell. Do this by checking
        //x position, with the person on the right surviving
        if (collision.collider.CompareTag("Cell"))
        {
            //Right now it disable the box collider. Instead of this, change this game objects layer to be CellsDontTouch,
            //A specific collision layer which interacts with the player but not other cells. This should allow the player to stand on merging cells
            //while allowing merging cells to pass through each other.
            //GetComponent<BoxCollider2D>().enabled = false;
            int cellDontTouchLayer = LayerMask.NameToLayer("CellsDontTouch");
            gameObject.layer = cellDontTouchLayer;

            Vector3 otherCellPos = collision.transform.position;
            BlockCellSplit otherCellSplitScript = collision.collider.GetComponent<BlockCellSplit>();
            int otherCellColor = otherCellSplitScript.color;
            float otherCellTimer = otherCellSplitScript.timerStartingValue;
            float otherCellSpeed = collision.collider.GetComponent<BlockCellMovement>().speed;

            BlockCellsManager.Instance.blockCellsList.Remove(transform);

            if (transform.position.x > collision.transform.position.x)
            {
                secondaryCell = false;

                Debug.Log("I got a larger x position, its " + transform.position.x + " while this other hoe got a x of " + collision.transform.position.x);
                StartAnimation();

                //OLD CODE. This script used to instant kill the other game object, then create the new cell, then destroy itself.
                //Destroy(collision.gameObject);
                CreateNewCell(blockCellSplit.color, otherCellColor, transform.position, otherCellPos,
                    blockCellSplit.timerStartingValue, otherCellTimer, GetComponent<BlockCellMovement>().speed, otherCellSpeed);
                //Destroy(gameObject);
            }
            //though if x is exactly equal use the y instead.
            else if(transform.position.x == collision.transform.position.x)
            {
                if(transform.position.y > collision.transform.position.y)
                {
                    secondaryCell = false;

                    StartAnimation();

                    //same as above
                    //Destroy(collision.gameObject);
                    CreateNewCell(blockCellSplit.color, otherCellColor, transform.position, otherCellPos,
                        blockCellSplit.timerStartingValue, otherCellTimer, GetComponent<BlockCellMovement>().speed, otherCellSpeed);
                    //Destroy(gameObject);
                }
            }

        }
    }

    Vector3 originalPosition;
    Vector3 originalScale;
    void StartAnimation()
    {
        //variables
        animating = true;
        timeIntoAnim = 0;
        //Disallow splitting and moving
        GetComponent<BlockCellSplit>().enabled = false;
        GetComponent<BlockCellMovement>().enabled = false;
        //Setting the positions at collision
        originalPosition = transform.position;
        originalScale = transform.localScale;
    }

    //For the cell which doesn't create the new neuron; the one not in charge.
    public void GetAccessToNewlyCreatedCell(Transform newCell)
    {
        secondaryCell = true;
        newCellTransform = newCell;

        StartAnimation();
    }


    GameObject currentlyInstantiating;
    //If two blocks collide, create a new cell in between them.
    //color a comes from the cell that was on the left, color b comes from the one on the right.
    //the current color is set to grey.
    void CreateNewCell(int colorA, int colorB, Vector3 cellAPos, Vector3 cellBPos, float timerValueA, float timerValueB, float speedA, float speedB)
    {
        if (BlockCellsManager.Instance.blockCellsList.Count > 100)
        {
            return;
        }

        Vector3 inBetweenPos = new Vector3((int)((cellAPos.x + cellBPos.x) * 0.5f), (int)((cellAPos.y + cellBPos.y) * 0.5f), 0);
        currentlyInstantiating = Instantiate(PrefabManager.Instance.BlockCellPrefab, inBetweenPos, Quaternion.identity);

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
        BlockCellsManager.Instance.blockCellsList.Add(currentlyInstantiating.transform);

        //Telling the secondary cell to start animating
        collidedWith.GetComponent<BlockCellDeath>().GetAccessToNewlyCreatedCell(currentlyInstantiating.transform);
        newCellTransform = currentlyInstantiating.transform;

        //The new block only pseudo-exists right now; it's scripts are all running and stuff but it can't collide with anything or be seen.
        //I'm doing it like this because we still need its transform
        currentlyInstantiating.GetComponent<BoxCollider2D>().enabled = false;
        currentlyInstantiating.GetComponent<SpriteRenderer>().enabled = false;
    }
}
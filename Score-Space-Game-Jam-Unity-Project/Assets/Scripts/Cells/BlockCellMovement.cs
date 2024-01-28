using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCellMovement : MonoBehaviour
{
    BlockCellSplit blockCellSplitScript;
    Rigidbody2D rb;

    [SerializeField]
    float randomAddSpeedPower = 1f;

    public float speed;
    //if moving up, there will be a very small random number which determines how the cell will also move
    //horizontally.
    public float randomAdditionalSpeed;

    public Vector2 velocity;

    private void Awake()
    {
        blockCellSplitScript = GetComponent<BlockCellSplit>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        randomAdditionalSpeed = Random.Range(-0.3f, 0.3f);
    }

    private void FixedUpdate()
    {
        if (blockCellSplitScript.movesVert)
        {
            velocity = new Vector2(randomAdditionalSpeed * randomAddSpeedPower, GetIntBasedOnColor(blockCellSplitScript.color)) * speed;
            rb.velocity = velocity;
        }
        else
        {
            velocity = new Vector2(GetIntBasedOnColor(blockCellSplitScript.color), randomAdditionalSpeed * randomAddSpeedPower) * speed;
            rb.velocity = velocity;
        }
    }

    //Get a direction int based on colour. Grey aka 0 will return 0,
    //blue aka 1 will return 1 (positive movement) and red aka 2 will return -1 (negative movement)
    int GetIntBasedOnColor(int colorValue)
    {
        switch (colorValue)
        {
            case 0:
                return 0;
            case 1:
                return 1;
            case 2:
                return -1;
        }
        return 0;
    }
}

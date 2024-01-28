using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Right now, there's no friction. If the ground moves, the player should move with it. That's what this script is for.
public class PlayerMoveWithGround : MonoBehaviour
{
    BlockCellMovement blockCellMovementCollidedWith;
    [SerializeField]
    CharacterController2D characterController2D;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Cell"))
        {
            blockCellMovementCollidedWith = collision.collider.GetComponent<BlockCellMovement>();
            //transform.parent = collision.collider.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Cell"))
        {
            if(collision.collider.GetComponent<BlockCellMovement>() == blockCellMovementCollidedWith)
            {
                blockCellMovementCollidedWith = null;
            }
        }
    }

    private void Update()
    {
        if(blockCellMovementCollidedWith != null)
        {
            transform.position += new Vector3(blockCellMovementCollidedWith.velocity.x, 0, 0) * Time.deltaTime;
        }
    }
}
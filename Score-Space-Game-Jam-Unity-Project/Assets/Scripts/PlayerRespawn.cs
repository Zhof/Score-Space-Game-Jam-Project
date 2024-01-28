using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TEST: This is just for when the player falls off the map, so i dont have to keep hitting unplay then play.
public class PlayerRespawn : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Kill"))
        {
            Death();
        }
    }
    public void Death()
    {
        transform.position = new Vector3(0, 6, 0);
    }
}

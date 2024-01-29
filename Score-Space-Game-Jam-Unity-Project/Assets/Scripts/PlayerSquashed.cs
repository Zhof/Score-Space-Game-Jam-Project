using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSquashed : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Cell") && GetComponentInParent<PlayerRespawn>().deathTimer < 0)
        {
            GetComponentInParent<PlayerRespawn>().Death();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Keeps track of all the block cells so we know how many we're dealing with.
//A failsafe in case theres too many block cells.
public class BlockCellsManager : MonoBehaviour
{
    public List<Transform> blockCellsList = new List<Transform>();

    private void Update()
    {
        for(int i = 0; i < blockCellsList.Count; i++)
        {
            if(blockCellsList[i] == null)
            {
                blockCellsList.RemoveAt(i);
            }
        }
    }

    public void DeleteAll()
    {
        foreach(Transform t in blockCellsList)
        {
            Destroy(t.gameObject);
        }
        blockCellsList.Clear();
    }

    private static BlockCellsManager m_Instance = null;
    public static BlockCellsManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = (BlockCellsManager)FindObjectOfType(typeof(BlockCellsManager));
            }
            return m_Instance;
        }
    }
}
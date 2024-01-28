using UnityEngine;

//This script is one i found online which fixes this weird issue associated with
//having an instance of a prefab store that prefab as a variable.
public class PrefabManager : MonoBehaviour
{
    // Assign the prefab in the inspector
    public GameObject BlockCellPrefab;
    //Singleton
    private static PrefabManager m_Instance = null;
    public static PrefabManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = (PrefabManager)FindObjectOfType(typeof(PrefabManager));
            }
            return m_Instance;
        }
    }
}
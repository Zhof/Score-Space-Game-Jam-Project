using UnityEngine;
using UnityEngine.SceneManagement;

//This script is one i found online which fixes this weird issue associated with
//having an instance of a prefab store that prefab as a variable.
public class Audio : MonoBehaviour
{
    public static Audio Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Destroy(gameObject);
        }
    }
}
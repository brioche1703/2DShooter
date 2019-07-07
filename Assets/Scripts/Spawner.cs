using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour
{

    public GameObject enemyPrefab;

    void Start()
    {
        // Spawning enemies randomly on the map
        for (int i = 0; i < PersistentManagerScript.Instance.enemiesNumber;
                i++)
        {
            Vector3 randomPosition = new Vector3(Random.Range(-8.0f, 8.0f), Random.Range(-9.0f, 8.0f), 0.0f);
            Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
        }
    }
}

using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("")]
public class BenchmarkNetworkManager : NetworkManager
{
    [Header("Spawns")]
    public GameObject spawnPrefab;
    public int spawnAmount = 5000;
    public float interleave = 1;

    void SpawnAll()
    {
        // calculate sqrt so we can spawn N * N = Amount
        float sqrt = Mathf.Sqrt(spawnAmount);

        // calculate spawn xz start positions
        // based on spawnAmount * distance
        float offset = -sqrt / 2 * interleave;

        Debug.Log("SpawnAll> offset" + offset);
        Debug.Log("SpawnAll> sqrt" + sqrt);

        // spawn exactly the amount, not one more.
        int spawned = 0;
        for (int spawnX = 0; spawnX < sqrt; ++spawnX)
        {
            for (int spawnZ = 0; spawnZ < sqrt; ++spawnZ)
            {
                // spawn exactly the amount, not any more
                // (our sqrt method isn't 100% precise)
                if (spawned < spawnAmount)
                {
                    Debug.Log(spawned + "| Spawned");

                    // instantiate & position
                    GameObject go = Instantiate(spawnPrefab);
                    float x = offset + spawnX * interleave;
                    float z = offset + spawnZ * interleave;
                    go.transform.position = new Vector3(x, 0, z);

                    // spawn
                    NetworkServer.Spawn(go);
                    ++spawned;
                }
            }
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        SpawnAll();

        Debug.Log("OnStartServer>> SpawnAll");
    }
}
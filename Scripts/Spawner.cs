using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Here is a fantastic entity spawner ! You can use it to spawn any types of GameObject.
 * Make sure to add slots as children of the Spawner prefab (A simple GameObject that holds this script).
 * If you have no slots, the spawner will spawn GameObjects at the Spawner prefab position.
 * If you have more than 1 slot, the script will randomly chose a slot everytime a Spawn() method will be called
 */ 
public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject m_prefabToSpawn;
    [SerializeField] private string m_spawnedGameObjectName;
    [SerializeField] private float m_spawnCooldownInSeconds;
    [SerializeField] private uint m_numberOfEntitiesToSpawn;

    private List<GameObject> m_slots = new List<GameObject>();
    private float m_cooldownTimer;
    private uint m_entitySpawned;

    private void Start()
    {
        ResetTimer();

        m_entitySpawned = 0;

        foreach (Transform child in transform)
        {
            m_slots.Add(child.gameObject);
        }
    }

    private void Update()
    {
        m_cooldownTimer -= Time.deltaTime;

        if (m_cooldownTimer <= 0)
        {
            if (CanSpawn())
                Spawn();
            ResetTimer();
        }
    }

    private bool CanSpawn()
    {
        return m_entitySpawned < m_numberOfEntitiesToSpawn;
    }

    private void ResetTimer()
    {
        m_cooldownTimer = m_spawnCooldownInSeconds;
    }

    /*
     * Can be used by any script to force the spawner to spawn something
     */ 
    public void Spawn()
    {
        Vector3 spawnPosition;
        if (m_slots.Count > 0)
            spawnPosition = m_slots[Random.Range(0, m_slots.Count - 1)].transform.position;
        else
            spawnPosition = transform.position;

        GameObject spawned = Instantiate(m_prefabToSpawn, spawnPosition, Quaternion.identity);
        spawned.name = m_spawnedGameObjectName;
        ++m_entitySpawned;
    }
}

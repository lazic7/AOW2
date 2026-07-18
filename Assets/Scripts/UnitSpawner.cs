using System.Collections;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [Header("Unit prefabs")]
    [SerializeField] private GameObject[] unitPrefabs;

    [Header("Spawn position")]
    [SerializeField] private Transform spawnPoint;

    [Header("Cooldown")]
    [SerializeField] private float spawnCooldown = 1f;

    private void Start()
    {
        StartCoroutine(SpawnUnitsAutomatically());
    }

    private IEnumerator SpawnUnitsAutomatically()
    {
        while (true)
        {
            SpawnRandomUnit();

            yield return new WaitForSeconds(spawnCooldown);
        }
    }

    private void SpawnRandomUnit()
    {
        if (unitPrefabs == null || unitPrefabs.Length == 0)
        {
            Debug.LogWarning("Unit prefabi nisu dodani u UnitSpawner.");
            return;
        }

        if (spawnPoint == null)
        {
            Debug.LogWarning("SpawnPoint nije postavljen.");
            return;
        }

        int randomIndex = Random.Range(0, unitPrefabs.Length);

        GameObject selectedUnit = unitPrefabs[randomIndex];

        Instantiate(
            selectedUnit,
            spawnPoint.position,
            Quaternion.identity
        );
    }
}
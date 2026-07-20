using UnityEngine;
using System.Collections.Generic;

public class UnitSpawner : MonoBehaviour
{
    [Header("Unit prefabs")]
    [SerializeField] private GameObject[] unitPrefabs;

    [Header("Spawn position")]
    [SerializeField] private Transform spawnPoint;

    [Header("Spawn limit")]
    [SerializeField] private int maxActiveUnits = 10;

    [Header("Cooldown")]
    [SerializeField] private float spawnCooldown = 1f;

    private float _nextSpawnTime;
    private readonly List<GameObject> _spawnedUnits = new();

    private void Awake()
    {
        if (!IsConfigured())
        {
            enabled = false;
        }
    }

    private void OnEnable()
    {
        _nextSpawnTime = Time.time + spawnCooldown;
    }

    private void Update()
    {
        if (Time.time < _nextSpawnTime)
        {
            return;
        }

        RemoveDestroyedUnits();

        if (_spawnedUnits.Count >= maxActiveUnits)
        {
            return;
        }

        int randomIndex = Random.Range(0, unitPrefabs.Length);
        GameObject selectedUnit = unitPrefabs[randomIndex];

        GameObject spawnedUnit = Instantiate(selectedUnit, spawnPoint.position, Quaternion.identity);
        _spawnedUnits.Add(spawnedUnit);
        _nextSpawnTime += spawnCooldown;
    }

    private void RemoveDestroyedUnits()
    {
        for (int i = _spawnedUnits.Count - 1; i >= 0; i--)
        {
            if (_spawnedUnits[i] == null)
            {
                _spawnedUnits.RemoveAt(i);
            }
        }
    }

    private bool IsConfigured()
    {
        if (unitPrefabs == null || unitPrefabs.Length == 0)
        {
            Debug.LogWarning("Unit prefabi nisu dodani u UnitSpawner.");
            return false;
        }

        if (spawnPoint == null)
        {
            Debug.LogWarning("SpawnPoint nije postavljen.");
            return false;
        }

        if (spawnCooldown <= 0f)
        {
            Debug.LogWarning("Spawn cooldown mora biti veci od 0.");
            return false;
        }

        if (maxActiveUnits <= 0)
        {
            Debug.LogWarning("Max active units mora biti veci od 0.");
            return false;
        }

        return true;
    }
}
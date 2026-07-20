using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [Header("Unit prefabs")]
    [SerializeField] private GameObject[] unitPrefabs;

    [Header("Spawn position")]
    [SerializeField] private Transform spawnPoint;

    [Header("Cooldown")]
    [SerializeField] private float spawnCooldown = 1f;

    private float _nextSpawnTime;

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

        int randomIndex = Random.Range(0, unitPrefabs.Length);
        GameObject selectedUnit = unitPrefabs[randomIndex];

        Instantiate(selectedUnit, spawnPoint.position, Quaternion.identity);
        _nextSpawnTime += spawnCooldown;
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

        return true;
    }
}
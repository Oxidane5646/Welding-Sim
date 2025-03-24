using UnityEngine;

public class newWeldManager : MonoBehaviour
{
    [SerializeField] WeldDatabase weldDatabase;
    [SerializeField] UIManager uiManager;
    [SerializeField] Transform weldObjectSpawn;
    [SerializeField] Transform weldSetupSpawn;

    private InputManager inputManager;

    // Script References
    private WeldSpawner currentWeldSpawner;
    private JoinPlates currentJoinPlates;
    private RayReference rayReference;
    private ParticleSystem weldParticles;

    // Temporary data references
    [SerializeField] private float resolution;
    [SerializeField] private float maxWeldDistance;

    // Local Data Cache
    private Vector3 spawnPoint = Vector3.zero;
    private RaycastHit hit;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();

        // Subscribe to UIManager event
        if (uiManager != null)
        {
            uiManager.onWeldSelectionComplete += GetWeldData;
        }
    }

    private void Start()
    {
        inputManager.OnWeldPressed += Welding;
    }

    private void GetWeldData(weldObjectType weldObjectType, weldSetupType weldSetupType)
    {
        if (weldDatabase == null) return;

        // Destroy old objects before spawning new ones
        DestroyExistingObjects();

        // Spawn and configure the weld object
        GameObject currentWeldObject = BuildObject(weldObjectType);
        currentJoinPlates = currentWeldObject?.GetComponent<JoinPlates>();

        // Spawn and configure the weld setup
        GameObject currentWeldSetup = BuildSetup(weldSetupType);
        if (currentWeldSetup != null)
        {
            currentWeldSpawner = currentWeldSetup.GetComponent<WeldSpawner>();
            rayReference = currentWeldSetup.GetComponentInChildren<RayReference>();
            weldParticles = currentWeldSetup.GetComponentInChildren<ParticleSystem>();
        }
    }

    private GameObject BuildObject(weldObjectType weldObjectType)
    {
        GameObject weldObjectPrefab = weldDatabase?.GetWeldObject(weldObjectType);
        return weldObjectPrefab != null ? Instantiate(weldObjectPrefab, weldObjectSpawn.position, Quaternion.identity) : null;
    }

    private GameObject BuildSetup(weldSetupType weldSetupType)
    {
        GameObject weldSetupPrefab = weldDatabase?.GetWeldSetup(weldSetupType);
        return weldSetupPrefab != null ? Instantiate(weldSetupPrefab, weldSetupSpawn.position, Quaternion.identity) : null;
    }

    private void DestroyExistingObjects()
    {
        if (currentWeldSpawner != null) Destroy(currentWeldSpawner.gameObject);
        if (currentJoinPlates != null) Destroy(currentJoinPlates.gameObject);
    }

    private void Welding()
    {
        if (rayReference == null) return;

        (spawnPoint, hit) = inputManager.GetPositionRaycastVR(rayReference.transform, 5f);

        if (hit.collider == null || spawnPoint == Vector3.zero) return;

        if (currentWeldSpawner != null && currentWeldSpawner.CanSpawnWeld(resolution, spawnPoint, hit.transform.tag))
        {
            currentWeldSpawner.SpawnWeld(spawnPoint);
            SpawnWeldParticle();
        }

        if (currentJoinPlates != null)
        {
            currentJoinPlates.UpdateWeldPoints(hit);

            if (currentJoinPlates.CanJoinPlates())
            {
                currentJoinPlates.ConnectPlates();
            }
        }
    }

    private void SpawnWeldParticle()
    {
        if (weldParticles != null)
        {
            weldParticles.Play();
        }
        else
        {
            Debug.Log("Weld Particles not found");
        }
    }

    private void OnDestroy()
    {
        inputManager.OnWeldPressed -= Welding;

        if (uiManager != null)
        {
            uiManager.onWeldSelectionComplete -= GetWeldData;
        }
    }
}



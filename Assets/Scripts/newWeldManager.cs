using UnityEngine;
using UnityEngine.Windows;

public class newWeldManager : MonoBehaviour
{
    [SerializeField] WeldDatabase weldDatabase;
    [SerializeField] UIManager uiManager;
    [SerializeField] Transform weldObjectSpawn;
    [SerializeField] Transform weldSetupSpawn;

    InputManager inputManager;

    //Script References
    WeldSpawner currentWeldSpawner;
    JoinPlates currentJoinPlates;
    RayReference rayReference;
    ParticleSystem weldParticles;


    //temporary data references
    [SerializeField] float resolution;
    [SerializeField] float maxWeldDistance;


    //Local Data Cache
    Vector3 spawnPoint = Vector3.zero;
    RaycastHit hit;

    private void Awake()
    {
      
    }

    private void Start()
    {
        GetWeldData(weldObjectType.parallelJoint, weldSetupType.MIG ,weldDatabase);
        inputManager = gameObject.GetComponent<InputManager>();

        //Subscribing to events
        inputManager.OnWeldPressed += WeldTesting;
    }

    void Update()
    {
      // SpawnWeldParticle();
    }

    private void WeldTesting()
    {
        if (rayReference == null) return;

        (spawnPoint, hit) = inputManager.GetPositionRaycastVR(rayReference.transform, 5f);

        if (hit.transform == null || hit.collider == null) return;
        if (spawnPoint == Vector3.zero) return;

        if (currentWeldSpawner.CanSpawnWeld(resolution, spawnPoint, hit.transform.tag))
        {
            currentWeldSpawner.SpawnWeld(spawnPoint);
            SpawnWeldParticle();
        }

        currentJoinPlates.UpdateWeldPoints(hit);

        if (currentJoinPlates.CanJoinPlates())
        {
            currentJoinPlates.ConnectPlates();
        }
    }

    private void SpawnWeldParticle()
    {
        if (weldParticles == null)
        {
            Debug.Log("Weld Particles not found");
            return;
        }

        weldParticles.Play(); 
    }

    private void GetWeldData(weldObjectType weldObjectType , weldSetupType weldSetypType , WeldDatabase database)
    {
        // Getting weldObject Data
        GameObject currentWeldObject = BuildObject(weldObjectType , database);
        currentJoinPlates = currentWeldObject.GetComponent<JoinPlates>();


        // Getting weldSetup Data
        GameObject currentWeldSetup = BuildSetup(weldSetypType , database);
        currentWeldSpawner = currentWeldSetup.GetComponent<WeldSpawner>();
        rayReference = currentWeldSetup.GetComponentInChildren<RayReference>();
        weldParticles = currentWeldSetup.GetComponentInChildren<ParticleSystem>();

    }

    private GameObject BuildObject(weldObjectType weldObjectType, WeldDatabase database)
    {
        GameObject weldObjectPrefab = database.GetWeldObject(weldObjectType);
        GameObject currentWeldObject = Instantiate(weldObjectPrefab, weldObjectSpawn.position, Quaternion.identity);
        return currentWeldObject;
    }

    private GameObject BuildSetup(weldSetupType weldSetupType, WeldDatabase database)
    {
        GameObject weldSetupPrefab = database.GetWeldSetup(weldSetupType);
        GameObject currentWeldSetup = Instantiate(weldSetupPrefab, weldSetupSpawn.position, Quaternion.identity);
        return currentWeldSetup;
    }

    private void OnDestroy()
    {
        inputManager.OnWeldPressed -= WeldTesting;
    }
}

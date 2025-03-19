using UnityEngine;
using UnityEngine.Windows;

public class newWeldManager : MonoBehaviour
{
    [SerializeField] WeldDatabase weldDatabase;

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

    private void SpawnWeldParticle()
    {
        if (weldParticles == null)
        {
            Debug.Log("Weld Particles not found");
            return;
        }

        weldParticles.Play(); 
    }

    private void GetWeldData(weldObjectType weldObjectType , weldSetupType weldSetypType ,  WeldDatabase database)
    {
        // Getting weldObject Data
        GameObject weldObjectPrefab = database.GetWeldObject(weldObjectType);
        GameObject currentWeldObject = Instantiate(weldObjectPrefab , Vector3.zero , Quaternion.identity);
        currentJoinPlates = currentWeldObject.GetComponent<JoinPlates>();


        // Getting weldSetup Data
        GameObject weldSetupPrefab = database.GetWeldSetup(weldSetypType);
        GameObject currentWeldSetup = Instantiate(weldSetupPrefab, Vector3.zero, Quaternion.identity);
        currentWeldSpawner = currentWeldSetup.GetComponent<WeldSpawner>();
        rayReference = currentWeldSetup.GetComponentInChildren<RayReference>();
        weldParticles = currentWeldSetup.GetComponentInChildren<ParticleSystem>();

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

    private void OnDestroy()
    {
        inputManager.OnWeldPressed -= WeldTesting;
    }
}

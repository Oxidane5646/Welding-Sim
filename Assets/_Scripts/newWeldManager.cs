using UI_Scripts;
using UnityEngine;

public class newWeldManager : MonoBehaviour
{
    /*
     * Note:-  
     *   change the dependency of the jointplates class to selfmanage itseld using events 
     *   create a more simple initialization method for the weld object and setup
     *   combine the buildweldobject and the buildweldsetup methods into a single method
     *   create a seperate class for the particle effects and audio effects 
     */


    [SerializeField] WeldDatabase weldDatabase;
    [SerializeField] UIManager uiManager; // Changed to a new script so this wont work

    [Header("Spawn Points")]
    [SerializeField] Transform weldObjectSpawn;
    [SerializeField] Transform weldSetupSpawn;

    private InputManager inputManager;

    // Script References
    private WeldSpawner currentWeldSpawner;
    private JoinPlates currentJoinPlates;
    private ParticleSystem weldParticles;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();

        // Subscribe to UIManager event
        //Make it into a new function and get the information using other methods cuz its aint accessible through ui manager for now 
        if (uiManager != null)
        {
            //uiManager.OnOnWeldSelectionComplete += InitializeWeldData;
        }
    }

    private void Start()
    {
        inputManager.OnWeldPressed += Welding;
        //Change the function of line 45
        //currentWeldSpawner.OnWeldRaycastHit += JoinPlatesFunction;
    }

    private void InitializeWeldData(WeldObjectType weldObjectType, WeldSetupType weldSetupType)
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
            weldParticles = currentWeldSetup.GetComponentInChildren<ParticleSystem>();
        }
    }

    private GameObject BuildObject(WeldObjectType weldObjectType)
    {
        GameObject weldObjectPrefab = weldDatabase?.GetWeldObject(weldObjectType);
        return weldObjectPrefab != null ? Instantiate(weldObjectPrefab, weldObjectSpawn.position, Quaternion.identity) : null;
    }

    private GameObject BuildSetup(WeldSetupType weldSetupType)
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
        currentWeldSpawner.SpawnWeld();
        SpawnWeldParticle();
    }

    private void JoinPlatesFunction(RaycastHit hit)
    {
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
        if (weldParticles)
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
        //The reference of this event is changed so make change this function as a whole
        if (uiManager != null)
        {
            //uiManager.OnOnWeldSelectionComplete -= InitializeWeldData;
        }
    }

    private void OnDisable()
    {
        inputManager.OnWeldPressed -= Welding;
        //currentWeldSpawner.OnWeldRaycastHit -= JoinPlatesFunction;
    }
}



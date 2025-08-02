using UnityEngine;

public class WeldObjectsInitializer : MonoBehaviour
{
    
    [SerializeField] WeldDatabase weldDatabase; 
    [SerializeField] newWeldManager newWeldManager; 
    
    
    [Header("Spawn Points")]
    [SerializeField] Transform weldObjectSpawn;  
    [SerializeField] Transform weldSetupSpawn; 
    
    
    private WeldSpawner currentWeldSpawner;
    private JoinPlates currentJoinPlates;
    private ParticleSystem weldParticles;
    

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
    
    //Move this function to the gameManager and give the gameobject spawned to the gamemanager to get the script references
    private void GetScriptReferences(GameObject currentWeldObject, GameObject currentWeldSetup)
    {
        currentJoinPlates = currentWeldObject?.GetComponent<JoinPlates>();
        currentWeldSpawner = currentWeldSetup.GetComponent<WeldSpawner>();
        weldParticles = currentWeldSetup.GetComponentInChildren<ParticleSystem>();
    }
    

    #region Public API

    
    public void InitializeWeldData(WeldObjectType weldObjectType, WeldSetupType weldSetupType)
    {
        if (weldDatabase == null) return;

        // Destroy old objects before spawning new ones
        DestroyExistingObjects();

        // Spawn and configure the weld object
        GameObject currentWeldObject = BuildObject(weldObjectType);
        
        // Spawn and configure the weld setup
        GameObject currentWeldSetup = BuildSetup(weldSetupType);

        if (currentWeldSetup && currentWeldObject)
        {
            GetScriptReferences(currentWeldObject, currentWeldSetup);
        }

        //this isint the work of the weldobject initializer , the game manager should handle this
        newWeldManager.SetScriptReferences(currentWeldSpawner, currentJoinPlates, weldParticles);
        
        if (currentJoinPlates == null) return;
        
        currentJoinPlates.SetScriptReferences(currentWeldSpawner);
    }
    
    #endregion
}

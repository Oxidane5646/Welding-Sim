using System;
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

    private InputManager inputManager;

    // Script References
    private WeldSpawner currentWeldSpawner;
    private JoinPlates currentJoinPlates;
    private ParticleSystem weldParticles;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
    }

    private void Start()
    {
        inputManager.OnWeldPressed += Welding;
        //Change the function of line 45
        //currentWeldSpawner.OnWeldRaycastHit += JoinPlatesFunction;
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
    }

    private void OnDisable()
    {
        inputManager.OnWeldPressed -= Welding;
        //currentWeldSpawner.OnWeldRaycastHit -= JoinPlatesFunction;
    }

    public void SetScriptReferences(WeldSpawner weldSpawner, JoinPlates joinPlates , ParticleSystem weldParticle)
    {
        currentWeldSpawner = weldSpawner;
        currentJoinPlates = joinPlates;
        weldParticles = weldParticle;
    }
}



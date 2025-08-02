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

    [SerializeField] InputManager inputManager;

    // Script References
    private WeldSpawner currentWeldSpawner;
    private JoinPlates currentJoinPlates;
    private ParticleSystem weldParticles;

    private void Welding()
    {
        currentWeldSpawner.SpawnWeld();
        SpawnWeldParticle();
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

    private void InitializeEventListeners()
    {
        inputManager.OnWeldPressed += Welding;
        //currentWeldSpawner.OnWeldRaycastHit += JoinPlatesFunction;
    }

    private void DisableEventListeners()
    {
        inputManager.OnWeldPressed -= Welding;
        //currentWeldSpawner.OnWeldRaycastHit -= JoinPlatesFunction;
    }



    #region Unity LifeCycle

    private void OnEnable()
    {
        InitializeEventListeners();
    }

    private void OnDisable()
    {
        DisableEventListeners();
    }

    #endregion


    #region Public API

    public void SetScriptReferences(WeldSpawner weldSpawner, JoinPlates joinPlates, ParticleSystem weldParticle)
    {
        currentWeldSpawner = weldSpawner;
        currentJoinPlates = joinPlates;
        weldParticles = weldParticle;
    }

    #endregion

}



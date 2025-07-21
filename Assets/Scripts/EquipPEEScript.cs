using Unity.VisualScripting;
using UnityEngine;

public class EquipPEEScript : MonoBehaviour
{
    [SerializeField] InputManager inputManager;
    [SerializeField] private Transform raycastPoint;
    [SerializeField] private string weldMaskTag = "WeldMask";
    [SerializeField] private string weldGlovesTag = "WeldGlovesTag";
    [SerializeField] private string weldApronTag = "WeldApronTag";
    
    private bool isWeldMaskEquipped = false;
    private bool isWeldGlovesEquipped = false;
    private bool isweldApronEquipped = false;

    private void EquipRaycast()
    {
        if (isWeldMaskEquipped && isWeldGlovesEquipped) return;
        
        RaycastHit hit;
        if (Physics.Raycast(raycastPoint.position, raycastPoint.forward, out hit, 10f))
        {
            if (hit.collider.CompareTag(weldMaskTag))
            {
                EquipFunction(hit); 
            }
            else if (hit.collider.CompareTag(weldGlovesTag))
            {
                EquipFunction(hit);
            }
        }
    }
    private void EquipFunction(RaycastHit hit)
    {
        //IF a button in the controller is pressed destroy the object and set a equip bool tag for the object to true
        //Not implemented yet
    }
}

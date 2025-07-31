using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text instructionText;
    
    [Header("Completion Check Settings")]
    public float checkInterval = 0.1f; // How often to check for completion
    
    [Header("Other script references")]
    [SerializeField] private EquipPEEScript equipScript;
    
    
    private string[] instructions = {
        "STEP 1: Equip your safety mask",
        "STEP 2: Put on protective gloves", 
        "STEP 3: Wear safety apron",
        "STEP 4: Grab the welding gun",
        "STEP 5: Insert the electrode",
        "STEP 6: Begin welding process"
    };
    
    private int currentStep = 0;
    private float checkTimer = 0f;
    private bool isSystemActive = true; //why?
    
    void Start()
    {
        UpdateInstruction();
    }
    
    void Update()
    {
        if (!isSystemActive) return; //why?
        
        // Check for completion at regular intervals
        checkTimer += Time.deltaTime;
        if (checkTimer >= checkInterval)
        {
            if (IsCurrentStepCompleted())
            {
                AdvanceToNextStep();
            }
            checkTimer = 0f;
        }
    }
    
    private bool IsCurrentStepCompleted()
    {
        switch (currentStep)
        {
            case 0: // Equip mask
                return equipScript.IsItemEquipped("WeldMask");
                
            case 1: // Equip gloves
                return equipScript.IsItemEquipped("WeldGloves");
                
            case 2: // Equip apron
                return equipScript.IsItemEquipped("WeldApron");
                
            case 3: // Grab weld gun
                return IsWeldGunGrabbed();
                
            case 4: // Insert electrode
                return IsElectrodeInserted();
                
            case 5: // Start welding
                return IsWeldingStarted();
                
            default:
                return false;
        }
    }
    
    private void AdvanceToNextStep()
    {
        if (currentStep < instructions.Length - 1)
        {
            currentStep++;
            UpdateInstruction();
            
            // Optional: Add completion feedback for previous step
            Debug.Log($"Step {currentStep} completed! Moving to next step.");
        }
        else
        {
            CompleteInstructions();
        }
    }
    
    private void UpdateInstruction()
    {
        // Update instruction text
        if (instructionText)
            instructionText.text = instructions[currentStep];
        
        Debug.Log($"Current Step: {instructions[currentStep]}");
    }
    
    //Modify this to remove the tutorial canvas or do some other thing 
    private void CompleteInstructions()
    {
        isSystemActive = false;
        
        if (instructionText)
            instructionText.text = "WELDING PREPARATION COMPLETE!\nYou may now begin welding safely.";
            
        Debug.Log("All welding instructions completed!");
        OnInstructionsComplete();
    }
    
    // COMPLETION CHECK METHODS - IMPLEMENT THESE BASED ON YOUR GAME LOGIC
    
    private bool IsWeldGunGrabbed()
    {
        // Implement your weld gun grab check here
        // return PlayerHands.Instance.isHoldingWeldGun;
        return false;
    }
    
    private bool IsElectrodeInserted()
    {
        // Implement your electrode insertion check here
        // return WeldGun.Instance.hasElectrode;
        return false;
    }
    
    private bool IsWeldingStarted()
    {
        // Implement your welding start check here
        // return WeldGun.Instance.isWelding;
        return false;
    }
    
    public void RestartInstructions()
    {
        currentStep = 0;
        isSystemActive = true;
        checkTimer = 0f;
        UpdateInstruction();
    }
    
    // Method to force advance (for testing)
    public void ForceNextStep()
    {
        if (isSystemActive)
            AdvanceToNextStep();
    }
    
    // Event for when instructions are completed
    private void OnInstructionsComplete()
    {
        // Add your completion logic here
        // Enable welding mechanics, show completion UI, unlock next level, etc.
    }
    
    // Get current step info
    public int GetCurrentStep() { return currentStep; }
    public string GetCurrentInstruction() { return instructions[currentStep]; }
    public bool IsSystemComplete() { return !isSystemActive; }
}

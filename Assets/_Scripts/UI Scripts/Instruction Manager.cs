using TMPro;
using UnityEngine;

public class InstructionsManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text instructionText;
    
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
    
    void Start()
    {
        UpdateInstruction();
    }
    
    void Update()
    {
        if (IsCurrentStepCompleted())
        {
            AdvanceToNextStep();
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
                return equipScript.IsItemEquipped("WeldTorch");
                
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
        {
            instructionText.text = instructions[currentStep];
        }   
    }
    
    //Modify this to remove the tutorial canvas or do some other thing 
    private void CompleteInstructions()
    {
        if (instructionText)
            instructionText.text = "WELDING PREPARATION COMPLETE!\nYou may now begin welding safely.";
        
        OnInstructionsComplete();
    }
    
    // COMPLETION CHECK METHODS - IMPLEMENT THESE BASED ON YOUR GAME LOGIC
    
    
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
        UpdateInstruction();
    }
    
    // Method to force advance (for testing)
    public void ForceNextStep()
    {
            AdvanceToNextStep();
    }
    
    
    private void OnInstructionsComplete()
    {
        // Add your completion logic here
        // Enable welding mechanics, show completion UI, unlock next level, etc.
    }
    
    // Get current step info
    public int GetCurrentStep() { return currentStep; }
    public string GetCurrentInstruction() { return instructions[currentStep]; }
}

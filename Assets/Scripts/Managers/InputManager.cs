// This script handles inputs for the player

using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float horizontal;                            //Float that stores horizontal Input
    public float vertical;                              //Float that stores vertical Input

    [HideInInspector] public bool jumpPressed;          //Bool that stores jump held
    public bool jumpHeld;                               //Bool that stores jump pressed

    [HideInInspector] public bool skillPressed;         //Bool that stores skill held
    public bool skillHeld;                              //Bool that stores skill pressed

    [HideInInspector] public bool defensePressed;       //Bool that stores defense held
    [HideInInspector] public bool defenseRelease;       //Bool that stores defense pressed
    public bool defenseHeld;                            //Bool that stores defense pressed

    bool readyToClear;                                  //Bool used to keep input in sync

    void Update()
    {
        //Clear out existing input values
        ClearInput();

        //Process jeyboard, mouse, gamepad (etc) inputs
        ProcessInputs();

        //Clamp the horizontal input to be between -1 and 1
        horizontal = Mathf.Clamp(horizontal, -1f, 1f);
        //Clamp the vertical input to be between -1 and 1
        vertical = Mathf.Clamp(vertical, -1f, 1f);
    }

    void FixedUpdate()
    {
        //In FixedUpdate() we sent a flag that lets inputs to be cleared out during the
        //next Update(). This ensures that all code gets to use the current inputs
        readyToClear = true;
    }

    void ClearInput()
    {
        //If we're not ready to clear input, exit
        if (!readyToClear)
            return;

        //Reset all inputs
        horizontal = 0f;
        vertical = 0f;

        jumpPressed = false;
        jumpHeld = false;

        skillPressed = false;
        skillHeld = false;

        defensePressed = false;
        defenseRelease = false;
        defenseHeld = false;

        readyToClear = false;
    }

    void ProcessInputs()
    {
        //Acumulate button axis inputs
        horizontal += Input.GetAxis("Horizontal");
        vertical += Input.GetAxis("Vertical");

        //Acumulate button inputs...
        jumpPressed = jumpPressed || Input.GetButtonDown("Jump");
        jumpHeld = jumpHeld || Input.GetButton("Jump");

        skillPressed = skillPressed || Input.GetButtonDown("Skill");
        skillHeld = skillHeld || Input.GetButton("Skill");

        defensePressed = defensePressed || Input.GetButtonDown("Defense");
        defenseRelease = defenseRelease || Input.GetButtonUp("Defense");
        defenseHeld = defenseHeld || Input.GetButton("Defense");
    }
}

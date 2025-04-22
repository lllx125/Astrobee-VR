using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;

public class Control : MonoBehaviour
{
    Astrobee astrobee;
    public XRInputValueReader<Vector2> l_StickInput = new XRInputValueReader<Vector2>("Thumbstick");
    public InputActionReference l_XButtonInput;
    public InputActionReference l_YButtonInput;
    public XRInputValueReader<Vector2> r_StickInput = new XRInputValueReader<Vector2>("RightThumbstick");
    public InputActionReference r_AButtonInput;
    public InputActionReference r_BButtonInput;

    // a length 12 array to store the state of the controller,
    // 0 is +x
    // 1 is -x
    // 2 is +y
    // 3 is -y
    // 4 is +z
    // 5 is -z
    // 6 is +rx
    // 7 is -rx
    // 8 is +ry
    // 9 is -ry
    // 10 is +rz
    // 11 is -rz
    private bool[] controllerState;
    private int[][] controlMap = new int[12][]{
        Parameters.CONTROL["+x"],
        Parameters.CONTROL["-x"],
        Parameters.CONTROL["+y"],
        Parameters.CONTROL["-y"],
        Parameters.CONTROL["+z"],
        Parameters.CONTROL["-z"],
        Parameters.CONTROL["+Rx1"],
        Parameters.CONTROL["-Rx1"],
        Parameters.CONTROL["+Ry1"],
        Parameters.CONTROL["-Ry1"],
        Parameters.CONTROL["+Rz1"],
        Parameters.CONTROL["-Rz1"],
    };

    void Start()
    {
        // initialize the controller state array
        controllerState = new bool[12];

        // Look for the Astrobee component in this GameObject
        astrobee = GetComponent<Astrobee>();

        // Check if the component was found
        if (astrobee == null)
        {
            Debug.LogError("Astrobee component not found on this GameObject.");
        }

        //subscribe to the input action events
        l_XButtonInput.action.started += (InputAction.CallbackContext context) => Open(5);
        l_XButtonInput.action.canceled += (InputAction.CallbackContext context) => Close(5);
        l_YButtonInput.action.started += (InputAction.CallbackContext context) => Open(4);
        l_YButtonInput.action.canceled += (InputAction.CallbackContext context) => Close(4);
        r_AButtonInput.action.started += (InputAction.CallbackContext context) => Open(10);
        r_AButtonInput.action.canceled += (InputAction.CallbackContext context) => Close(10);
        r_BButtonInput.action.started += (InputAction.CallbackContext context) => Open(11);
        r_BButtonInput.action.canceled += (InputAction.CallbackContext context) => Close(11);
    }

    // Update is called once per frame
    void Update()
    {
        // reads all the values from the controller
        Vector2 lstickVal = PosToState(l_StickInput.ReadValue());
        Vector2 rstickVal = PosToState(r_StickInput.ReadValue());
        bool[] currentState = GetCurrentState(lstickVal, rstickVal);
        for (int i = 0; i < controllerState.Length; i++)
        {
            if (currentState[i] != controllerState[i])
            {
                controllerState[i] = currentState[i];
                if (controllerState[i])
                {
                    Open(i);
                }
                else
                {
                    Close(i);
                }
            }
        }


    }

    // convert joystick position to state
    Vector2 PosToState(Vector2 input)
    {
        // If magnitude is too small, treat as “no movement”
        float magnitude = input.magnitude;

        if (magnitude < 0.5f)
        {
            return Vector2.zero;
        }

        float degrees = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;

        if (degrees > -45 && degrees <= 45)
            return new Vector2(1, 0);

        else if (degrees > 45 && degrees <= 135)
            return new Vector2(0, 1);

        else if (degrees > 135 || degrees <= -135)
            return new Vector2(-1, 0);

        else if (degrees > -135 && degrees <= -45)
            return new Vector2(0, -1);

        return Vector2.zero;
    }

    bool[] GetCurrentState(Vector2 lstickVal, Vector2 rstickVal)
    {
        bool[] currentState = new bool[12];
        if (lstickVal.x > 0)
        {
            currentState[1] = true;
        }
        else if (lstickVal.x < 0)
        {
            currentState[0] = true;
        }
        else
        {
            currentState[0] = false;
            currentState[1] = false;
        }
        if (lstickVal.y > 0)
        {
            currentState[2] = true;
        }
        else if (lstickVal.y < 0)
        {
            currentState[3] = true;
        }
        else
        {
            currentState[2] = false;
            currentState[3] = false;
        }


        if (rstickVal.x > 0)
        {
            currentState[6] = true;
        }
        else if (rstickVal.x < 0)
        {
            currentState[7] = true;
        }
        else
        {
            currentState[6] = false;
            currentState[7] = false;
        }
        if (rstickVal.y > 0)
        {
            currentState[8] = true;
        }
        else if (rstickVal.y < 0)
        {
            currentState[9] = true;
        }
        else
        {
            currentState[8] = false;
            currentState[9] = false;
        }
        return currentState;
    }

    void Open(int i)
    {
        foreach (int vent in controlMap[i])
        {
            astrobee.OpenVent(vent);
        }
    }
    void Close(int i)
    {
        foreach (int vent in controlMap[i])
        {
            astrobee.CloseVent(vent);
        }
    }


}


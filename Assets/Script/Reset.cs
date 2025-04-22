using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class Reset : MonoBehaviour
{
    public XRInputValueReader<float> l_triggerInput = new XRInputValueReader<float>("Trigger");
    private UnityEngine.UI.Image resetBar;
    public GameObject ResetBar;
    public AudioSource resetSound;
    public GameObject addCupObjects;
    Astrobee astrobee;
    bool continuousResetFlag = true;
    bool onPressFlag = false;

    void Start()
    {
        resetBar = ResetBar.GetComponent<UnityEngine.UI.Image>();
        astrobee = GetComponent<Astrobee>();
        continuousResetFlag = true;
        onPressFlag = false;
    }

    void Update()
    {
        // hide the bar if the grip is not pressed
        if (resetBar.fillAmount >= 0.001f)
        {
            ResetBar.SetActive(true);
        }
        else
        {
            ResetBar.SetActive(false);
        }

        if (l_triggerInput.ReadValue() > 0.01f)
        {
            if (!onPressFlag)
            {
                onPressFlag = true;
                resetSound.Play();
            }
            if (continuousResetFlag)
            {
                resetBar.fillAmount += l_triggerInput.ReadValue() / 2f * Time.deltaTime;
            }

        }
        else
        {
            continuousResetFlag = true;
            onPressFlag = false;
            resetSound.Stop();
            if (resetBar.fillAmount > 0.001f)
            {
                resetBar.fillAmount -= Time.deltaTime;
            }
            else
            {
                resetBar.fillAmount = 0f;
            }
        }

        if (resetBar.fillAmount >= 1f)
        {
            astrobee.ResetObject();
            resetBar.fillAmount = 0f;
            resetSound.Stop();
            continuousResetFlag = false;
            foreach (Transform child in addCupObjects.transform)
            {
                Destroy(child.gameObject);
            }
        }

    }
}

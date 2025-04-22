using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class AddCupObjects : MonoBehaviour
{
    public XRInputValueReader<float> r_triggerInput = new XRInputValueReader<float>("Trigger");
    private UnityEngine.UI.Image addBar;
    public GameObject AddBar;
    public AudioSource resetSound;

    bool continuousResetFlag = true;
    bool onPressFlag = false;
    public GameObject Grable;
    public GameObject[] objects;
    void Start()
    {
        addBar = AddBar.GetComponent<UnityEngine.UI.Image>();
        continuousResetFlag = true;
        onPressFlag = false;
    }

    void Update()
    {
        // hide the bar if the grip is not pressed
        if (addBar.fillAmount >= 0.001f)
        {
            AddBar.SetActive(true);
        }
        else
        {
            AddBar.SetActive(false);
        }

        if (r_triggerInput.ReadValue() > 0.01f)
        {
            if (!onPressFlag)
            {
                onPressFlag = true;
                resetSound.Play();
            }
            if (continuousResetFlag)
            {
                addBar.fillAmount += r_triggerInput.ReadValue() / 2f * Time.deltaTime;
            }

        }
        else
        {
            continuousResetFlag = true;
            onPressFlag = false;
            resetSound.Stop();
            if (addBar.fillAmount > 0.001f)
            {
                addBar.fillAmount -= Time.deltaTime;
            }
            else
            {
                addBar.fillAmount = 0f;
            }
        }

        if (addBar.fillAmount >= 1f)
        {
            addBar.fillAmount = 0f;
            resetSound.Stop();
            continuousResetFlag = false;
            Add();
        }
    }
    void Add()
    {
        int randomIndex = Random.Range(0, objects.Length);
        GameObject randomObject = objects[randomIndex];
        Instantiate(randomObject, randomObject.transform.position, randomObject.transform.rotation);
    }
}

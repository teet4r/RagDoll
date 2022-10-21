using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BacklightController : MonoBehaviour
{
    [SerializeField]
    GameObject leftBacklight;
    [SerializeField]
    GameObject rightBacklight;

    public bool isOn = false;

    public void TurnOn()
    {
        isOn = true;
        leftBacklight.SetActive(true);
        rightBacklight.SetActive(true);
    }

    public void TurnOff()
    {
        isOn = false;
        leftBacklight.SetActive(false);
        rightBacklight.SetActive(false);
    }
}

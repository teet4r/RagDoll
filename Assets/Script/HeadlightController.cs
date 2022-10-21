using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadlightController : MonoBehaviour
{
    [SerializeField]
    GameObject leftHeadlight;
    [SerializeField]
    GameObject rightHeadlight;

    public bool isOn = false;

    public void TurnOn()
    {
        isOn = true;
        leftHeadlight.SetActive(true);
        rightHeadlight.SetActive(true);
    }

    public void TurnOff()
    {
        isOn = false;
        leftHeadlight.SetActive(false);
        rightHeadlight.SetActive(false);
    }
}

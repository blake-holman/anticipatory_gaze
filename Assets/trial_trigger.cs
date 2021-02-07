using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
public class trial_trigger : MonoBehaviour
{
    // a reference to the action
public SteamVR_Action_Boolean trial_press;
// a reference to the hand
public SteamVR_Input_Sources handType;
//reference to the sphere
public GameObject Sphere;
    // Start is called before the first frame update
    void Start()
    {
        trial_press.AddOnStateDownListener(TriggerDown, handType);
        trial_press.AddOnStateUpListener(TriggerUp, handType);
    }
public void TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
{
    Debug.Log("Trigger is up");
    Sphere.GetComponent<MeshRenderer>().enabled = false;
}
public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
{
    Debug.Log("Trigger is down");
    Sphere.GetComponent<MeshRenderer>().enabled = true;
}

    // Update is called once per frame
    void Update()
    {
        
    }
}

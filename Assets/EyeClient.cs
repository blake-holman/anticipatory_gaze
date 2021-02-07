using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ROSBridgeLib;
using ROSBridgeLib.geometry_msgs;


public class EyeClient : MonoBehaviour {
    public ROSBridgeWebSocketConnection ros = null;
   

    protected Type _subscriber = null;

    public EyeClient(Type subscriber) {
        _subscriber = subscriber;
    }

    void Start() {
        ros = new ROSBridgeWebSocketConnection("ws://localhost", 9090);
        ros.AddSubscriber(_subscriber);
        ros.Connect();      
    }

    void OnApplcationQuit() {
        if(ros != null)
            ros.Disconnect();
    }

    void Update() {
        ros.Render();
    }

    public void Rotate(Quaternion rotation_angles) {
        transform.rotation = rotation_angles; 
    }
}
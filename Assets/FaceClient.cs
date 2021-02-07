using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ROSBridgeLib;
using ROSBridgeLib.geometry_msgs;




public class FaceClient : MonoBehaviour {
    private ROSBridgeWebSocketConnection ros = null;

    void Start() {
        ros = new ROSBridgeWebSocketConnection("ws://localhost", 9090);
        ros.AddSubscriber(typeof(FaceSubscriber));
        ros.Connect();
        
    }

    void OnApplcationQuit() {
        if(ros != null)
            ros.Disconnect();
    }

    void Update() {
        //Debug.Log("Rendering new frame..");
        ros.Render();
    }

    public void Rotate(Quaternion rotation_angles) {
        transform.rotation = rotation_angles; 
    }

    public void Rotate2(float rotationangle, Vector3 axis){
        transform.rotation = Quaternion.AngleAxis(rotationangle, axis);
    }
    
    public void RotateEye(float x, float y, float z){
      Debug.LogFormat("Moving to ({} {} {})",x,y,z);
      transform.Rotate(x,y,z);
    }

}

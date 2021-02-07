using UnityEngine;
using ROSBridgeLib;
using SimpleJSON;
using ROSBridgeLib.geometry_msgs;

public class FaceSubscriber : ROSBridgeSubscriber {
    
    public new static string GetMessageTopic() {
        return "/face_rotation";
    }

    public new static string GetMessageType() {
        return "geometry_msgs/Quaternion";
    }

    public new static ROSBridgeMsg ParseMessage(JSONNode msg){
        return new QuaternionMsg(msg);
    }

    public new static void CallBack(ROSBridgeMsg msg) {

        QuaternionMsg ms = (QuaternionMsg) msg;
        Debug.Log(ms.GetW());
        Quaternion q = new Quaternion( (float)ms.GetW(),(float)ms.GetX(), (float)ms.GetY(), (float)ms.GetZ() );
        GameObject obj = GameObject.FindGameObjectsWithTag("Face")[0];
        FaceClient c = obj.GetComponents(typeof(FaceClient))[0] as FaceClient;
       //Vector3 axis = new Vector3 (0.0f,-0.784151f,-0.62057f);
        c.Rotate(q);
        //c.Rotate2((float).284, axis);
    }

}

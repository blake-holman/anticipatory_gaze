using UnityEngine;
using ROSBridgeLib;
using SimpleJSON;
using ROSBridgeLib.geometry_msgs;

public class LeftEyeSubscriber : ROSBridgeSubscriber {
    
    public new static string GetMessageTopic() {
        return "/left_rotation";
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
        Quaternion q = new Quaternion( (float)ms.GetX(),(float)ms.GetY(), (float)ms.GetZ(), (float)ms.GetW() );
        GameObject obj = GameObject.FindGameObjectsWithTag("Left")[0];
        LeftEyeClient c = obj.GetComponents(typeof(LeftEyeClient))[0] as LeftEyeClient;
        c.Rotate(q);
    }

}

using UnityEngine;
using ROSBridgeLib;
using SimpleJSON;
using ROSBridgeLib.geometry_msgs;

public class CyclopeanpointSubscriber : ROSBridgeSubscriber {
    
    public new static string GetMessageTopic() {
        return "/cyclopean_rotation";
    }

    public new static string GetMessageType() {
        return "geometry_msgs/Quaternion";
    }

    public new static ROSBridgeMsg ParseMessage(JSONNode msg){
        return new QuaternionMsg(msg);
    }

    public new static void CallBack(ROSBridgeMsg msg) {

        QuaternionMsg ms = (QuaternionMsg) msg;
       // Debug.Log(ms.GetW());
        Quaternion q = new Quaternion( (float)ms.GetX(),(float)ms.GetY(), (float)ms.GetZ(), (float)ms.GetW() );
        GameObject obj = GameObject.FindGameObjectsWithTag("Cylopean")[0];
        //Debug.Log(obj.GetComponents())
        CyclopeanpointClient c = obj.GetComponents(typeof(CyclopeanpointClient))[0] as CyclopeanpointClient;
        c.Rotate(q);
    }

}

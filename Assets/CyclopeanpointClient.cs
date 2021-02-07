using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ROSBridgeLib;
using ROSBridgeLib.geometry_msgs;




public class CyclopeanpointClient : EyeClient {
   private ROSBridgeWebSocketConnection ros = null;

    public CyclopeanpointClient() : base(typeof(CyclopeanpointSubscriber)) {}
    
}

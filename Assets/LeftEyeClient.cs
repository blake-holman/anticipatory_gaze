using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ROSBridgeLib;
using ROSBridgeLib.geometry_msgs;




public class LeftEyeClient : EyeClient {
   private ROSBridgeWebSocketConnection ros = null;

    public LeftEyeClient() : base(typeof(LeftEyeSubscriber)) {}
    
}

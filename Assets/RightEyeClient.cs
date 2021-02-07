using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ROSBridgeLib;
using ROSBridgeLib.geometry_msgs;




public class RightEyeClient : EyeClient {
   private ROSBridgeWebSocketConnection ros = null;

    public RightEyeClient() : base(typeof(RightEyeSubscriber)) {}
    
}

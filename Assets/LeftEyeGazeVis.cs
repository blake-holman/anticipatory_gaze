//========= Copyright 2018, HTC Corporation. All rights reserved. ===========
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Assertions;
using Valve.VR;

namespace ViveSR
{
    namespace anipal
    {
        namespace Eye
        {
            public class LeftEyeGazeVis : MonoBehaviour
            {
                public int LengthOfRay = 25;
                [SerializeField] private LineRenderer LeftGazeRayRenderer;
                // [SerializeField] private LineRenderer RightGazeRayRenderer;

                private static EyeData_v2 eyeData = new EyeData_v2();
                private bool eye_callback_registered = false;
                private void Start()
                {
                    if (!SRanipal_Eye_Framework.Instance.EnableEye)
                    {
                        enabled = false;
                        return;
                    }
                    Assert.IsNotNull(LeftGazeRayRenderer);
                    // Assert.IsNotNull(RightGazeRayRenderer);

                }

                private void Update()
                {
                    if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING &&
                        SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.NOT_SUPPORT) return;

                    if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == true && eye_callback_registered == false)
                    {
                        SRanipal_Eye_v2.WrapperRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
                        eye_callback_registered = true;
                    }
                    else if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == false && eye_callback_registered == true)
                    {
                        SRanipal_Eye_v2.WrapperUnRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
                        eye_callback_registered = false;
                    }

                    Vector3 GazeOriginCombinedLocal, GazeDirectionCombinedLocal;
                    Vector3 GazeOriginLeft, GazeDirectionLeft;
                    Vector3 GazeOriginRight, GazeDirectionRight;

                    VerboseData data;
                    bool leftValid = false;
                    bool rightValid = false;
                    if (eye_callback_registered)
                    {
                        leftValid = SRanipal_Eye_v2.GetGazeRay(GazeIndex.LEFT, out GazeOriginLeft, out GazeDirectionLeft, eyeData);
                        rightValid = SRanipal_Eye_v2.GetGazeRay(GazeIndex.RIGHT, out GazeOriginRight, out GazeDirectionRight, eyeData);
                    }
                    else
                    {
                        leftValid = SRanipal_Eye_v2.GetGazeRay(GazeIndex.LEFT, out GazeOriginLeft, out GazeDirectionLeft);
                        rightValid = SRanipal_Eye_v2.GetGazeRay(GazeIndex.RIGHT, out GazeOriginRight, out GazeDirectionRight);
                    }
                    // system origin = camera.main.transform.position
                    // gazeoriginleft , with resepct to left lens origin
                    // we know the transfrom from left lens origin to system origin (14mm translation)
                    if(leftValid) {
                        Vector3 LeftGazeDirectionCombined = Camera.main.transform.TransformDirection(GazeDirectionLeft);
                        Vector3 GazeLeft = Camera.main.transform.TransformPoint(-GazeOriginLeft);
                        LeftGazeRayRenderer.SetPosition(0, GazeLeft);
                        LeftGazeRayRenderer.SetPosition(1, GazeLeft + LeftGazeDirectionCombined * LengthOfRay);
                    }
                    if(rightValid) {
                        // Vector3 RightGazeDirectionCombined = Camera.main.transform.TransformDirection(GazeDirectionRight);
                        // Vector3 GazeRight = Camera.main.transform.TransformPoint(GazeOriginRight);
                        // RightGazeRayRenderer.SetPosition(0, GazeRight);
                        // RightGazeRayRenderer.SetPosition(1, GazeRight + RightGazeDirectionCombined * LengthOfRay);
                    }
                }
                private void Release()
                {
                    if (eye_callback_registered == true)
                    {
                        SRanipal_Eye_v2.WrapperUnRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
                        eye_callback_registered = false;
                    }
                }
                private static void EyeCallback(ref EyeData_v2 eye_data)
                {
                    eyeData = eye_data;
                }
            }
        }
    }
}

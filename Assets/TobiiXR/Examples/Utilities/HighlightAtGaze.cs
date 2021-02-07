// Copyright © 2018 – Property of Tobii AB (publ) - All Rights Reserved

using Tobii.G2OM;
using UnityEngine;
using ViveSR.anipal.Eye;

namespace Tobii.XR.Examples
{
//Monobehaviour which implements the "IGazeFocusable" interface, meaning it will be called on when the object receives focus
    public class HighlightAtGaze : MonoBehaviour, IGazeFocusable
    {
        public Color HighlightColor = Color.red;
        public float AnimationTime = 0.1f;

        private Renderer _renderer;
        private Color _originalColor;
        private Color _targetColor;

        private LineRenderer _lineRenderer;

        //The method of the "IGazeFocusable" interface, which will be called when this object receives or loses focus
        public void GazeFocusChanged(bool hasFocus)
        {
            //If this object received focus, fade the object's color to highlight color
            if (hasFocus)
            {
                _targetColor = HighlightColor;
            }
            //If this object lost focus, fade the object's color to it's original color
            else
            {
                _targetColor = _originalColor;
            }
        }

        private void Start()
        {
            _renderer = GetComponent<Renderer>();
            _originalColor = _renderer.material.color;
            _targetColor = _originalColor;


            _lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        private void Update()
        {
            //This lerp will fade the color of the object
            if (_renderer.material.HasProperty(Shader.PropertyToID("_BaseColor"))) // new rendering pipeline (lightweight, hd, universal...)
            {
                _renderer.material.SetColor("_BaseColor", Color.Lerp(_renderer.material.GetColor("_BaseColor"), _targetColor, Time.deltaTime * (1 / AnimationTime)));
            }
            else // old standard rendering pipline
            {
                _renderer.material.color = Color.Lerp(_renderer.material.color, _targetColor, Time.deltaTime * (1 / AnimationTime));
            }


            // Get eye tracking data in world space
            var eyeTrackingData = TobiiXR.GetEyeTrackingData(TobiiXR_TrackingSpace.World);

            // Check if gaze ray is valid
            if(eyeTrackingData.GazeRay.IsValid)
            {
                // The origin of the gaze ray is a 3D point
                var rayOrigin = eyeTrackingData.GazeRay.Origin;

                // The direction of the gaze ray is a normalized direction vector
                var rayDirection = eyeTrackingData.GazeRay.Direction;

                _lineRenderer.SetVertexCount(2);
                _lineRenderer.SetPosition(0, rayOrigin);
                _lineRenderer.SetPosition(1, rayDirection * 20 + rayOrigin);

                if(eyeTrackingData.ConvergenceDistanceIsValid == false) {
                    // print("invalid");
                    print(eyeTrackingData.ConvergenceDistance);
                } else {
                    print("valid");
                }
                var convergence_distance = eyeTrackingData.ConvergenceDistance;
                transform.position = rayDirection * convergence_distance + rayOrigin;

            }




            // SRanipal_Eye.GetEyeData(ref eyeData);
            // SRanipal_Eye.GetVerboseData(out verboseData);

            // gazeOriginLeft = eyeData.verbose_data.left.gaze_origin_mm;
            // gazeOriginRight = eyeData.verbose_data.right.gaze_origin_mm;   

            // gazeDirectionLeft = eyeData.verbose_data.left.gaze_direction_normalized;    
            // gazeDirectionRight = eyeData.verbose_data.right.gaze_direction_normalized;    

            // _lineRenderer.SetVertexCount(2);
            // _lineRenderer.SetPosition(0, gazeOriginLeft);
            // _lineRenderer.SetPosition(1, gazeDirectionLeft * 20 + gazeOriginLeft);
        }
    }
}

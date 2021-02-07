using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EClient : MonoBehaviour {
    public Transform target;
    Vector3 objpt = new Vector3();
    Vector3 eyeOrient = new Vector3(1f,0f,0f);
    void Start() {
       
    }

   

    void Update() {
        //GameObject obj = GameObject.FindGameObjectsWithTag("Left")[0];
        Vector3 eye = new Vector3 (66.5f, -4.2f, -39.5f);
        Debug.Log("eye"+eye);
        // GameObject target = GameObject.FindGameObjectsWithTag("target")[0];
        // objpt = target.transform.position;
        Vector3 objpt = new Vector3(2165.63f*50f, 0f, 12500f*50f);
        Debug.Log(objpt);
        Vector3 eyeVect = objpt - eye; 
          Debug.Log("eyevect"+eyeVect);
        eyeVect = eyeVect.normalized;
            Debug.Log("eyevectorm"+eyeVect);
        Vector3 crossprod = Vector3.Cross(eyeOrient,eyeVect);
        Debug.Log("crossprod before norm" + crossprod);
        float dot = Vector3.Dot(eyeOrient,eyeVect);
        float crossprodnorm = crossprod.magnitude;
       float rotationangle =  Mathf.Atan2(crossprodnorm, dot);
       crossprod = crossprod.normalized;
              
         transform.rotation = Quaternion.AngleAxis( rotationangle, crossprod);
        Debug.Log("crossprodnorm" + crossprodnorm + "crossprod" + crossprod + "rotation"+transform.rotation);
        




         
        
    }

}
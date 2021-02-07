using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ROSBridgeLib;

public class EyeMovement : MonoBehaviour {



	public float speed = .005f;
	public float speedMouse = 7f;
	public float maxDistance = 0.5f;
	public Camera mainCamera ;
	private Vector3 _origin;
	//private var rotation_origin;
	private Vector3 parent_origin;
	public bool move = true;
	public int count = 0;
	public float startTime;

	public bool lookDown = true;
	public bool lookLeft = true;
	public bool goToMiddle = false;
	// Use this for initialization
	void Start () {
		mainCamera = Camera.main;
		_origin = transform.position;
		//rotation_origin = Quaternion.Euler(transform.rotation.x,transform.rotation.y, transform.rotation.z);
		//parent_origin = transform.parent.position; //test
		startTime = Time.time;

		InvokeRepeating ("LookUpDown", 0.5f, 1f);
		//InvokeRepeating ("LookLeftRight", 0.5f, 1.5f);

	}

	void TestHello()
	{
		Debug.Log ("\nHello");
	}

	void followMouse(){
		var mouseWorldCoordinates = mainCamera.ScreenPointToRay(Input.mousePosition).origin;

		var originToMouse = mouseWorldCoordinates - _origin;
		originToMouse = Vector3.ClampMagnitude (originToMouse, maxDistance);
		if (count < 300) {
			transform.position = Vector3.Lerp (transform.position, _origin + originToMouse, speedMouse*Time.deltaTime);
			count++;
		} else if(count==600){
			count = 0;
		}else {
			transform.position = _origin;
			count++;
		}

	}

	void LookUpDown(){
		Debug.Log ("Called with lookdown = " + lookDown);
		if(lookDown)
		{
			transform.Rotate(20, 0,0);
			lookDown = false;

		} else {

			lookDown = true;
			transform.Rotate(-20, 0 ,0);

		}
		//StartCoroutine (TimerPause ());



	}

	void LookLeftRight()
	{

		if (goToMiddle)
		{
			if (lookLeft) {
				//char just lokked left - so now next time must look to right
				transform.Rotate (0, -20, 0);
				lookLeft = false;
				goToMiddle = false;
			} else
			{
				//char just looked to right
				transform.Rotate(0, 20,0);
				lookLeft = true;
				goToMiddle = false;

			}
		}else if (lookLeft) {
			transform.Rotate (0, 20, 0);
			goToMiddle = true;


		}
		else
		{

			goToMiddle = true;
			transform.Rotate (0, -20, 0);

		}

	}


	IEnumerator TimerPause()
	{
		Debug.Log ("In Pause");
		Debug.Log (Time.time);
		yield return new WaitForSecondsRealtime (1000000000);
		Debug.Log (Time.time);
	}



	// Update is called once per frame
	void Update () {




		//transform.position.x = Mathf.PingPong(Time.time,2.0f) - 1.0f;
		//side to side eye movements
		//sideToSide();
		//upDown ();
		//followMouse ();








		//Get the mouse space in world space
		//var mouseWorldCoordinates = mainCamera.ScreenPointToRay(Input.mousePosition).origin;
		//get Vector pointing from initial position to the target. Vector cant be longer than
		//max distance
		//var originToMouse = mouseWorldCoordinates - _origin;
		//originToMouse = Vector3.ClampMagnitude (originToMouse, maxDistance);


		//Linearly interpolate from current to mouse's position
		//transform.position = Vector3.Lerp(transform.position, _origin + originToMouse, speed * Time.deltaTime);
		//Debug.Log (transform.position);

		//Link to move it for fixed tim
		//http://answers.unity3d.com/questions/34884/move-a-gameobject-for-certain-amount-of-time-and-t.html



	}
}

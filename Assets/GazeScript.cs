using System;
using System.Runtime.InteropServices;
using UnityEngine;
using Random=System.Random;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tobii.XR;
using Valve.VR;
namespace ViveSR.anipal.Eye
{
    public class GazeScript : MonoBehaviour
    {
        public GameObject Sphere;
        public Boolean pressed_flag;
        // at index i+1, the number of trials of goal i+1 that we've completed.
        private int[] trials_done;
        // list of goals, 5 copies each
        private ArrayList trials_remaining;
        private FocusInfo FocusInfo;
        private readonly float MaxDistance = 20;
        private readonly GazeIndex[] GazePriority = new GazeIndex[] { GazeIndex.COMBINE, GazeIndex.LEFT, GazeIndex.RIGHT };
        private static EyeData_v2 eyeData = new EyeData_v2();
        private GameObject right;
        private GameObject gaze_point;
        private GameObject[] goal_objects;
        private GameObject left;
        private GameObject waist;
        // public GameObject Sphere;
        private String csv_fname = "manas_final_waist2.csv";
        private Boolean in_target;
        private Boolean in_start;
        private bool eye_callback_registered = false;
        private int current_state = 0; // 0 = not in trial. 1 = in trial. -1 = done
        private int curr_goal;
        private int curr_trial_index;
        private GameObject start_obj;
        private void Start()
        {
            Debug.Log(gameObject.name);
            // time, headset pose, headset orientation, gaze origin, gaze direction, tag of obj being looked at, focusinfo.point, tracker pose, tracker orientation, curr_goal, trials_done_on_goal, in_target, in_start, current_state, eye_good
            String attrs_str = "time,headset_pose,headset_orientation,gaze_origin,gaze_direction,tag_of_obj_being_looked_at,focusinfo.point,tracker_pose,tracker_orientation,curr_goal,trials_done_on_goal,in_target,in_start,current_state,eye_good\n";
            String header = "";
            foreach (String attr in attrs_str.Split(',')) {
                header += attr + ";";
            }
            header = header.Substring(0, header.Count()-1);
            File.AppendAllText(csv_fname, header);
            trials_remaining = new ArrayList();
            trials_done = new int[5];
            in_target = false;
            in_start = false;
            // right = GameObject.FindGameObjectsWithTag("Right")[0];
            // left = GameObject.FindGameObjectsWithTag("Left")[0];

            for (int i=0; i<5; i++) {
                for (int times_to_add=1; times_to_add<=5; times_to_add++) {
                    trials_remaining.Add(i);
                }
            }
            start_obj = GameObject.FindGameObjectsWithTag("start")[0];
            Sphere = GameObject.FindGameObjectsWithTag("sphere")[0];
            goal_objects = new GameObject[5];
            for (int i = 0; i < 5; i++) {
                goal_objects[i] = GameObject.FindGameObjectsWithTag("g"+(i+1))[0];
            }
            waist = GameObject.FindGameObjectsWithTag("waist")[0];
            // gaze_point = GameObject.FindGameObjectsWithTag("gaze_point")[0];
            //text = GameObject.FindGameObjectsWithTag("text")[0].GetComponent<Text>();
            //TextReader.text = "P"
            if (!SRanipal_Eye_Framework.Instance.EnableEye)
            {
                enabled = false;
                return;
            }
        }

        private void play_sound(String tag) {
            GameObject sound_obj = GameObject.FindGameObjectsWithTag(tag)[0];
            AudioSource sound = sound_obj.GetComponent<AudioSource>();
            sound.Play();
        }

        private void update_trial_state(Boolean in_target, Boolean in_start) {
            // starting next trial
            if (current_state == 0 && in_start) {
                // in next trial
                Debug.Log("we are starting the trial");
                current_state = 1;
            } 
            // trying to start, but they amigo isn't in the right place
            else if (current_state == 0 && !in_start && !in_target) {
                // do the sounds that's like "go back to the start"
                Debug.Log("You're not in the start position");
                play_sound("start");
            }
            // trying to finish current trial, and we are good
            else if (current_state == 1 && in_target) {
                current_state = 0;
                Debug.Log("We're finishing the trial");
                                            
                if (trials_remaining.Count == 0) {
                    // we're done
                    // play the complete sound
                    current_state = -1;
                    play_sound("completed");
                } else{
                play_sound("finished_go_back");
                }
                // play sounds "reached goal ... go bacck... "
            } else {
            Debug.Log("you clicked this at a time where it won't do anything");

            }
            // else do nothing bc we don't wanna mess them up
            // perhaps cooldown if we need
            
        }

        private float dist_2d(Vector3 a, Vector3 b) {
            Vector3 an = new Vector3(0f,0f,0f);
            Vector3 bn = new Vector3(0f,0f,0f);
            an[0] = a[0];
            an[1] = 0;
            an[2] = a[2];
            bn[0] = b[0];
            bn[1] = 0;
            bn[2] = b[2];
            return Vector3.Distance(an,bn);
        }
        private void Update()
        {
            int prev_state = current_state; 
            if (Sphere.GetComponent<MeshRenderer>().enabled) {
                update_trial_state(in_target,in_start);
            }

            Random rand = new Random();
            //TODO: is Next inclusive or exclusive??
            // print("trials remaining" + trials_remaining.Count);


            // otherwise, we still have trials to do
            // get random trial
            // get the correct goal
            if (prev_state==0 && current_state==1){
                prev_state=current_state;
                curr_trial_index = rand.Next(0,trials_remaining.Count);
                curr_goal = (int) trials_remaining[curr_trial_index];
                trials_remaining.RemoveAt(curr_trial_index);
                Debug.Log("curr_goal:" + (curr_goal+1));
                Debug.Log("Trials_done" + trials_done[0] + " " + trials_done[1] + " " + trials_done[2] + " "+ trials_done[3] + " "+ trials_done[4]);
                trials_done[curr_goal]++;
                play_sound("g" + (curr_goal+1));
            }
            
            // remove so we don't do it again

            //TODO change PLEASE
            // in_target = (target1.transform.position[2]-0.5 <= waist.transform.position[2]);
            in_target = dist_2d(goal_objects[curr_goal].transform.position,Camera.main.transform.position) <= .5;

            // in_start = (.5>= waist.transform.position[2]);
            in_start = dist_2d(start_obj.transform.position, Camera.main.transform.position) <= .5;  
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
            bool saved = false;
            var rayOrigin = new Vector3(0f,0f,0f);
            var rayDirection = new Vector3(0f,0f,0f);
            foreach (GazeIndex index in GazePriority)
            {
                Ray GazeRay;
                int dart_board_layer_id = LayerMask.NameToLayer("NoReflection");
                bool eye_focus;
                if (eye_callback_registered)
                    eye_focus = SRanipal_Eye_v2.Focus(index, out GazeRay, out FocusInfo, 0, MaxDistance,5, eyeData);
                else
                    eye_focus = SRanipal_Eye_v2.Focus(index, out GazeRay, out FocusInfo, 0, MaxDistance, 5);

                if (eye_focus && !saved)
                {
                    // gaze_point.transform.position = FocusInfo.point;
                    var eyeTrackingData = TobiiXR.GetEyeTrackingData(TobiiXR_TrackingSpace.World);
                    // The origin of the gaze ray is a 3D point
                    rayOrigin = eyeTrackingData.GazeRay.Origin;

                    // The direction of the gaze ray is a normalized direction vector
                    rayDirection = eyeTrackingData.GazeRay.Direction;

                    // time, headset pose, headset orientation, gaze origin, gaze direction, tag of obj being looked at, focusinfo.point, tracker pose, tracker orientation, curr_goal, trials_done_on_goal, in_target, in_start, current_state
                    
                    saved=true;
         
                    break;
                }

                
            }         
            if (saved) {
            File.AppendAllText(csv_fname, Time.time.ToString("f6") + ";" + Camera.main.transform.position.ToString("f6") + ";" + Camera.main.transform.rotation.ToString("f6") + ";" + rayOrigin.ToString("f6") +  ";" + rayDirection.ToString("f6") 
            + ";" + FocusInfo.transform.tag + ";" + FocusInfo.point.ToString("f6") + ";" + waist.transform.position.ToString("f6") + ";" + waist.transform.rotation.ToString("f6")+";"+
            curr_goal + ";" + trials_done[curr_goal] + ";" + in_target + ";" + in_start + ";" + current_state + ";" + saved + Environment.NewLine);        
            } else{
                File.AppendAllText(csv_fname, Time.time.ToString("f6") + ";" + Camera.main.transform.position.ToString("f6") + ";" + Camera.main.transform.rotation.ToString("f6") + ";" + rayOrigin.ToString("f6") +  ";" + rayDirection.ToString("f6") 
            + ";" + "error"+ ";" + "(-1,-1,-1)" + ";" + waist.transform.position.ToString("f6") + ";" + waist.transform.rotation.ToString("f6")+";"+
            curr_goal + ";" + trials_done[curr_goal] + ";" + in_target + ";" + in_start + ";" + current_state + ";" + saved + Environment.NewLine);     
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
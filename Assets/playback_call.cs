using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;
using System.Linq;


public class GazeData {
    public GazeData(LineRenderer line_render, GameObject sphr, GameObject position){
        lr = line_render;
        sphere = sphr;
        pose = position;
    }

    public LineRenderer lr {get;}
    public GameObject sphere {get;}
    public GameObject pose {get;}
}

public class playback_call : MonoBehaviour
{
   List<List<string>> alllines = new List<List<string>>();

    int num_lines = 0;
    int curr_line=0;
    GameObject person;
    GameObject gaze;
    List<List<GazeData>> renderers;

    Vector3 stringToVec(string s) {
        string[] temp = s.Substring (1, s.Length-2).Split (',');
        return new Vector3 (float.Parse(temp[0]), float.Parse(temp[1]), float.Parse(temp[2]));
    }
    
    // Start is called before the first frame update
    void Start()
    {
        renderers = new List<List<GazeData>>();

        // person = GameObject.FindGameObjectsWithTag("personexample")[0];
        // gaze = GameObject.FindGameObjectsWithTag("gazeexample")[0];
    using(var reader = new StreamReader(@"C:\akash_final_waist.csv"))
    {
        //int count = 0;
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine(); // read each entire line in csv
            var values = line.Split(';'); // splits with ;
            List<string> delimited = values.ToList();
            alllines.Add(delimited); // store delimited words in each corresponding line
            num_lines++;
        }

    }



        // String curr_state = alllines[curr_line][11]; // 0 or 1
        
        // time;headset_pose;headset_orientation;gaze_origin;gaze_direction;tag_of_obj_being_looked_at;focusinfo.point;tracker_pose;
        // tracker_orientation;curr_goal;trials_done_on_goal;in_target;in_start;current_state;eye_good
        int prev_goal = -1;
        int prev_trial = -1;
        for(int i=0; i<alllines.Count; i++) {
            curr_line = i;
            int curr_goal = int.Parse(alllines[i][9]);
            int curr_trial = int.Parse(alllines[i][10]);
            int curr_state = int.Parse(alllines[i][13]);
            
            print(prev_goal + " " + curr_goal + " " + prev_trial + " " + curr_trial);

            if (prev_goal != curr_goal || curr_trial!= prev_trial) {
                prev_goal = curr_goal;
                prev_trial = curr_trial;

                renderers.Add(new List<GazeData>());

            
             }

            if (curr_state == 1){
                print("current state");
                print(curr_state);
                Quaternion quat;
                String squat = alllines[curr_line][2];
                squat = squat.Substring(1, squat.Count()-2);
                String[] quat_str = squat.Split(',');
                print(squat);

                quat.x = float.Parse(quat_str[0]);
                quat.y = float.Parse(quat_str[1]);
                quat.z = float.Parse(quat_str[2]);
                quat.w = float.Parse(quat_str[3]);


                String spose = alllines[curr_line][1];
                spose =  spose.Substring(1, spose.Count()-2);
                String[] pose_str = spose.Split(',');

                Vector3 pose = new Vector3();
                pose[0] = float.Parse(pose_str[0]);
                pose[1] = float.Parse(pose_str[1]);
                pose[2] = float.Parse(pose_str[2]);

                // focus point 
                Vector3 focus_point = stringToVec(alllines[curr_line][6]);
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.transform.position = focus_point;
                sphere.SetActive(false);
                sphere.transform.localScale = new Vector3(.1f, .1f, .1f); 
                // person.transform.SetPositionAndRotation(pose, quat);
                
                // String gort = alllines[curr_line][4];
                // gort =  gort.Substring(1, gort.Count()-2);
                // String[] gaze_orient_str = gort.Split(',');

                // Quaternion gaze_orient = new Quaternion();
                // gaze_orient.x = float.Parse(gaze_orient_str[0]);
                // gaze_orient.y = -float.Parse(gaze_orient_str[1]);
                // gaze_orient.z = float.Parse(gaze_orient_str[2]);
                // gaze_orient.w = float.Parse(gaze_orient_str[3]);
                String gpose = alllines[curr_line][3];
                gpose =  gpose.Substring(1, gpose.Count()-2);
                String[] gaze_pose_str = gpose.Split(',');

                Vector3 gaze_pose = new Vector3();
                gaze_pose[0] = float.Parse(gaze_pose_str[0]);
                gaze_pose[1] = float.Parse(gaze_pose_str[1]);
                gaze_pose[2] = float.Parse(gaze_pose_str[2]);


                LineRenderer lr = new GameObject().AddComponent<LineRenderer>();
                // all_lrs.Add(lr);
                Vector3 headset_pose = stringToVec(alllines[curr_line][1]);
                GameObject hpose = GameObject.CreatePrimitive(PrimitiveType.Cube);
                headset_pose[1] = gaze_pose[1]/2;
                hpose.transform.position = headset_pose;
                hpose.SetActive(false);
                hpose.transform.localScale = new Vector3(.01f, gaze_pose[1], .01f); 
                renderers[renderers.Count-1].Add(new GazeData(lr, sphere, hpose));
                lr.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
 
                
        
                lr.SetColors(Color.blue, Color.green);
                lr.SetWidth(0.025f, 0.025f);
                lr.SetPosition(0, gaze_pose);
                lr.SetPosition(1, focus_point);
                lr.enabled = false;
            
                // String curr_state = alllines[curr_line][11]; // 0 or 1
            } 
        }  
        print("all size" + renderers.Count);
        for (int i = 0; i < renderers.Count; i++) {
            print("size " + renderers[i].Count);
        }
    }

    int current_val = 0;
    int current_index = 0;
    // Update is called once per frame
    void Update()
    {

        // lr.gameObject.SetParent(transform, false); // iffy about this line
        // lr.gameObject.SetPositionAndRotation(gaze_pose, Quaternion.LookRotation(gaze_orient));
        
        
        if (current_index == renderers[current_val].Count) {
            current_val++;
            current_index = 0;
        }
        print("doing it");
        LineRenderer lr = renderers[current_val][current_index].lr;
        float len = (float) renderers[current_val].Count;
        GameObject sphere = renderers[current_val][current_index].sphere;
        Color curr = new Color((len-current_index)/len, 0, current_index/len, 1);
        print("curr "+curr + " curr_index and length" + len + " / "+ current_index);
        lr.SetColors(curr, curr);
        lr.enabled = true;
        var sphere_renderer = sphere.GetComponent<Renderer>();
        sphere_renderer.material.SetColor("_Color", curr);
        sphere.SetActive(true);
        renderers[current_val][current_index].pose.SetActive(true);
        var pose_renderer = renderers[current_val][current_index].pose.GetComponent<Renderer>();
        pose_renderer.material.SetColor("_Color", curr);
        current_index++;
        Destroy(lr.gameObject, 2);
        Destroy(sphere, 2);
        Destroy(renderers[current_val][current_index].pose, 2);
    }
}


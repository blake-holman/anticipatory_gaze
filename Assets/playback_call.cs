using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;
using System.Linq;



public class playback_call : MonoBehaviour
{
    List<List<string>> alllines = new List<List<string>>();
    int num_lines = 0;
    int curr_line=0;
    GameObject person;
    GameObject gaze;
    // Start is called before the first frame update
    void Start()
    {
        person = GameObject.FindGameObjectsWithTag("target")[0];
        gaze = GameObject.FindGameObjectsWithTag("gaze")[0];
    using(var reader = new StreamReader(@"C:\1del.csv"))
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
    
    }

    // Update is called once per frame
    void Update()
    {
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


        person.transform.SetPositionAndRotation(pose, quat);
        
        String gort = alllines[curr_line][4];
        gort =  gort.Substring(1, gort.Count()-2);
        String[] gaze_orient_str = gort.Split(',');

        Vector3 gaze_orient = new Vector3();
        gaze_orient[0] = float.Parse(gaze_orient_str[0]);
        gaze_orient[1] = float.Parse(gaze_orient_str[1]);
        gaze_orient[2] = float.Parse(gaze_orient_str[2]);
        
        String gpose = alllines[curr_line][3];
        gpose =  gpose.Substring(1, gpose.Count()-2);
        String[] gaze_pose_str = gpose.Split(',');

        Vector3 gaze_pose = new Vector3();
        gaze_pose[0] = float.Parse(gaze_pose_str[0]);
        gaze_pose[1] = float.Parse(gaze_pose_str[1]);
        gaze_pose[2] = float.Parse(gaze_pose_str[2]);

        gaze.transform.rotation = Quaternion.LookRotation(gaze_orient);
        gaze.transform.position = gaze_pose;
        curr_line++;

    }
}

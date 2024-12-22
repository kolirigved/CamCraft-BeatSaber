using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword1Controller : MonoBehaviour
{
    public MyListener listener;
    float[] data = new float[3];

    //get self position
    public Transform selfTransform;
    public Vector3 selfPosition;
    public Vector3 selfRotation;
    void Start(){
        selfTransform = GetComponent<Transform>();
    }
    void FixedUpdate()
{
    data = listener.RightData;
    
    // Use local position and local rotation
    selfPosition = selfTransform.localPosition;
    selfRotation = selfTransform.localEulerAngles;
    // Debug.Log(data[0]);
    if(data[0] == 0 && data[1] == 0 && data[2] == 0){
        return;
    }

    // Update local position
    selfPosition.x = data[0];
    selfPosition.y = data[1];
    selfTransform.localPosition = new Vector3(selfPosition.x, selfPosition.y, selfTransform.localPosition.z);

    // Update local rotation
    selfRotation.z = data[2];
    selfTransform.localEulerAngles = new Vector3(selfTransform.localEulerAngles.x, selfTransform.localEulerAngles.y, selfRotation.z + 90);
}
}

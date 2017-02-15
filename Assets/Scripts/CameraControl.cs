using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

    static CameraControl s_instance;

    public static CameraControl Instance
    {
        get { return s_instance; }
    }

    public Transform TargetTransform;

    Vector3 _targetPosition;

    public Vector3 TargetPos
    {
        get { return _targetPosition; }
    }

    // Use this for initialization
    void Start ()
    {
        s_instance = this;
	}
	
	// Update is called once per frame
	void Update () {

        if(Input.GetMouseButtonDown(0))
        {
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPos.y = 0.2f;
            _targetPosition = targetPos;
            TargetTransform.position = targetPos;
        }	
	}   
}

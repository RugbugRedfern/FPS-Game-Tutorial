using UnityEngine;
using System.Collections;

public class FPSRandomRotateAngle : MonoBehaviour
{
    public bool RotateX;
    public bool RotateY;
    public bool RotateZ = true;

    private Transform t;

	// Use this for initialization
	void Awake ()
	{
	    t = transform;
	}
	
	// Update is called once per frame
	void OnEnable ()
	{
	    var rotateVector = Vector3.zero;
	    if (RotateX)
	        rotateVector.x = Random.Range(0, 360);
        if (RotateY)
            rotateVector.y = Random.Range(0, 360);
        if (RotateZ)
            rotateVector.z = Random.Range(0, 360);
        t.Rotate(rotateVector);
	}
}

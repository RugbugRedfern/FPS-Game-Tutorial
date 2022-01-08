using UnityEngine;
using System.Collections;

public class LightBehaviorAuto : MonoBehaviour {
	public AnimationCurve LightCurve;
	public float GraphScaleX, GraphScaleY;
	private float startTime;
	private Light lightSource;

	void Start(){
		lightSource = GetComponent<Light> ();
	}

	void OnEnable () {
		startTime = Time.time;

	}
	
	void Update () {
		var time = Time.time - startTime;
		if (time > GraphScaleX)
			startTime = Time.time;
		var eval = LightCurve.Evaluate (time / GraphScaleX) / GraphScaleY;
		lightSource.intensity = eval;
	}
}

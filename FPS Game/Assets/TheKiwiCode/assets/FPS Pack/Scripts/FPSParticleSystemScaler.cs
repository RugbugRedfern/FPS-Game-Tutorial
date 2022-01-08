using UnityEngine;
using System.Collections;

#if UNITY_EDITOR 
using UnityEditor;
#endif

[ExecuteInEditMode]
public class FPSParticleSystemScaler : MonoBehaviour 
{
	public float particlesScale = 1.0f;
	
	float oldScale;
	
	void Start()
	{
		oldScale = particlesScale;
	}
	
	void Update () 
	{
		#if UNITY_EDITOR 
		if (Mathf.Abs(oldScale - particlesScale) > 0.0001f && particlesScale > 0)
		{
			transform.localScale = new Vector3(particlesScale, particlesScale, particlesScale);
			float scale = particlesScale / oldScale;
			var ps = this.GetComponentsInChildren<ParticleSystem>();
			
			foreach (ParticleSystem particles in ps)
			{
				particles.startSize *= scale;
				particles.startSpeed *= scale;
				particles.gravityModifier *= scale;
				
				SerializedObject serializedObject = new SerializedObject(particles);
				serializedObject.FindProperty("ClampVelocityModule.magnitude.scalar").floatValue *= scale;
				serializedObject.FindProperty("ClampVelocityModule.x.scalar").floatValue *= scale;
				serializedObject.FindProperty("ClampVelocityModule.y.scalar").floatValue *= scale;
				serializedObject.FindProperty("ClampVelocityModule.z.scalar").floatValue *= scale;
				serializedObject.FindProperty("VelocityModule.x.scalar").floatValue *= scale;
				serializedObject.FindProperty("VelocityModule.y.scalar").floatValue *= scale;
				serializedObject.FindProperty("VelocityModule.z.scalar").floatValue *= scale;
				serializedObject.FindProperty("ColorBySpeedModule.range").vector2Value *= scale;
				serializedObject.FindProperty("RotationBySpeedModule.range").vector2Value *= scale;
				serializedObject.FindProperty("ForceModule.x.scalar").floatValue *= scale;
				serializedObject.FindProperty("ForceModule.y.scalar").floatValue *= scale;
				serializedObject.FindProperty("ForceModule.z.scalar").floatValue *= scale;
				serializedObject.FindProperty("SizeBySpeedModule.range").vector2Value *= scale;
				
				serializedObject.ApplyModifiedProperties();
			}

			var trails = this.GetComponentsInChildren<TrailRenderer>();
			foreach (TrailRenderer trail in trails)
			{
				trail.startWidth *= scale;
				trail.endWidth *= scale;
			}
			oldScale = particlesScale;
		}
		#endif
	}
}
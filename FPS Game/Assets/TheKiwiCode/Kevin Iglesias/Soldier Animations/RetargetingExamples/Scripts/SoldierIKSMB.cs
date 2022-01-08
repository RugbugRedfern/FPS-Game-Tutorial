///////////////////////////////////////////////////////////////////////////
//  SoldierIK SMB - StateMachineBehaviour Script				         //
//  Kevin Iglesias - https://www.keviniglesias.com/     			     //
//  Contact Support: support@keviniglesias.com                           //
///////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//For custom inspector only
#if UNITY_EDITOR
	using UnityEditor;
#endif

namespace KevinIglesias {
	public class SoldierIKSMB : StateMachineBehaviour
	{
		private SoldierIK soldierIK;

		public bool applyIK = true;

		public bool onEnter = false;
		public bool customTimePoint = false;
		public bool onExit = false;

		public float timePoint;

		public float speed;
		public bool iKDone;
		public float iKDonePoint;
		public int interpolation;
		
		
		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if(soldierIK == null)
			{
				soldierIK = animator.GetComponent<SoldierIK>();
			}
			
			iKDone = false;
			
			if(soldierIK != null)
			{
				if(onEnter)
				{
					soldierIK.PerformIK(applyIK, speed, interpolation);
					iKDone = true;
				}
			}
			
		}

		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
		{
			
			
			if(soldierIK != null)
			{
				if(customTimePoint)
				{
					float timeElapsed = stateInfo.normalizedTime - Mathf.Floor(stateInfo.normalizedTime);
					if(!iKDone)
					{
						if(timeElapsed >= timePoint)
						{
							soldierIK.PerformIK(applyIK, speed, interpolation);
							iKDone = true;
							iKDonePoint = timeElapsed;
						}
					}else{
						if(timeElapsed < iKDonePoint)
						{
							iKDone = false;
						}
					}
				}
			}
		}
		
		override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if(soldierIK != null)
			{
				if(onExit)
				{
					soldierIK.PerformIK(applyIK, speed, interpolation);
					iKDone = true;
				}
			}
		}
	}
	
	#if UNITY_EDITOR
	//Custom Inspector
	[CustomEditor(typeof(SoldierIKSMB))]
    public class SoldierIKSMBCustomInspector : Editor
    {
		public override void OnInspectorGUI()
		{
			
			var SMBScript = target as SoldierIKSMB;
			
			GUILayout.Space(5);
			
			
			
			string applyIKText = "  Apply IK  ";
			string clearIKText = "  Clear IK  ";
			
			GUIStyle applyStyle = new GUIStyle(GUI.skin.button);
			GUIStyle clearStyle = new GUIStyle(GUI.skin.button);

			if(SMBScript.applyIK)
			{
				applyIKText = "[Apply IK]";
				applyStyle.fontStyle = FontStyle.Bold;
			}else{
				clearIKText = "[Clear IK]";
				clearStyle.fontStyle = FontStyle.Bold;
			}
			
			EditorGUI.BeginChangeCheck();
			bool iApply = false;

			GUILayout.BeginHorizontal();
			if(GUILayout.Button(applyIKText, applyStyle))
			{
				iApply = true;
			}

			if(GUILayout.Button(clearIKText, clearStyle))
			{
				iApply = false;
			}
			GUILayout.EndHorizontal();
			
			if(EditorGUI.EndChangeCheck()) {
					Undo.RegisterCompleteObjectUndo(target, "Change IK Type Option");
					SMBScript.applyIK = iApply;
			}
			
			string resultDescription = "Existing IK will be removed.";
			
			if(SMBScript.applyIK)
			{
				resultDescription = "IK will be applied.";
			}
			
			GUILayout.Space(5);
			
			GUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(resultDescription);
			GUILayout.EndHorizontal();
			
			GUILayout.Space(5);
			DrawUILine(Color.black, 1, 5);
			GUILayout.Space(5);
			
			string onEnterText = " On Enter ";
			string customPointText = " Custom Point ";
			string onExitText = " On Exit ";
			
			GUIStyle onEnterStyle = new GUIStyle(GUI.skin.button);
			GUIStyle customPointStyle = new GUIStyle(GUI.skin.button);
			GUIStyle onExitStyle = new GUIStyle(GUI.skin.button);
			
			if(SMBScript.onEnter)
			{
				onEnterText = "[On Enter]";
				onEnterStyle.fontStyle = FontStyle.Bold;
			}
			if(SMBScript.customTimePoint)
			{
				customPointText = "[Custom Point]";
				customPointStyle.fontStyle = FontStyle.Bold;
			}
			if(SMBScript.onExit)
			{
				onExitText = "[On Exit]";
				onExitStyle.fontStyle = FontStyle.Bold;
			}
			
			EditorGUI.BeginChangeCheck();
			bool iOnEnter = false;
			bool iCustomPoint = false;
			bool iOnExit = false;
			
			GUILayout.BeginHorizontal();
			if(GUILayout.Button(onEnterText, onEnterStyle))
			{
				iOnEnter = true;
				iCustomPoint = false;
				iOnExit = false;
			}
			if(GUILayout.Button(customPointText, customPointStyle))
			{
				iOnEnter = false;
				iCustomPoint = true;
				iOnExit = false;
			}
			if(GUILayout.Button(onExitText, onExitStyle))
			{
				iOnEnter = false;
				iCustomPoint = false;
				iOnExit = true;
			}
			GUILayout.EndHorizontal();
			
			if(EditorGUI.EndChangeCheck()) {
					Undo.RegisterCompleteObjectUndo(target, "Change IK Point Option");
					SMBScript.onEnter = iOnEnter;
					SMBScript.customTimePoint = iCustomPoint; 
					SMBScript.onExit = iOnExit;
			}
			
			if(SMBScript.customTimePoint)
			{
				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Custom time point:");
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("(0.5 means middle of animation)");
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				EditorGUI.BeginChangeCheck();
				float iTime = EditorGUILayout.Slider(SMBScript.timePoint, 0f, 1f);
				if(EditorGUI.EndChangeCheck()) {
					Undo.RegisterCompleteObjectUndo(target, "Change IK Time point");
					SMBScript.timePoint = iTime;
				}
				GUILayout.EndHorizontal();

			}
			
			if(!SMBScript.onEnter & !SMBScript.customTimePoint & !SMBScript.onExit)
			{
				GUI.enabled = false;
			}else{
				GUI.enabled = true;
			}
			
			GUILayout.Space(5);
			DrawUILine(Color.black, 1, 5);
			GUILayout.Space(5);
			
			GUILayout.BeginHorizontal();
			EditorGUI.BeginChangeCheck();
			float iSpeed = EditorGUILayout.FloatField("Speed:", SMBScript.speed);
			if(EditorGUI.EndChangeCheck()) {
				Undo.RegisterCompleteObjectUndo(target, "Change IK Speed");
				SMBScript.speed = iSpeed;
			}
			GUILayout.EndHorizontal();
			
			GUILayout.Space(5);
			
			
			
			if(SMBScript.speed <= 0)
			{
				GUI.enabled = false;
			}else{
				GUI.enabled = true;
			}
			
			string linearText = " Linear ";
			string easeOutText = " Ease Out ";
			string easeInText = " Ease In ";
			string smoothText = " Smooth ";
			
			GUIStyle linearStyle = new GUIStyle(GUI.skin.button);
			GUIStyle easeOutPointStyle = new GUIStyle(GUI.skin.button);
			GUIStyle easeInStyle = new GUIStyle(GUI.skin.button);
			GUIStyle smoothStyle = new GUIStyle(GUI.skin.button);
			
			switch(SMBScript.interpolation)
			{
				case 0:
					linearText = "[Linear]";
					linearStyle.fontStyle = FontStyle.Bold;
				break;
				
				case 1:
					easeOutText = "[Ease Out]";
					easeOutPointStyle.fontStyle = FontStyle.Bold;
				break;
				
				case 2:
					easeInText = "[Ease In]";
					easeInStyle.fontStyle = FontStyle.Bold;
				break;
				
				case 3:
					smoothText = "[Smooth]";
					smoothStyle.fontStyle = FontStyle.Bold;
				break;
				
				default:
					SMBScript.interpolation = 0;
				break;
			}
			

			EditorGUI.BeginChangeCheck();
			int iInterpolation = 0;
			
			GUILayout.BeginHorizontal();
			if(GUILayout.Button(linearText, linearStyle))
			{
				iInterpolation = 0;
			}
			if(GUILayout.Button(easeOutText, easeOutPointStyle))
			{
				iInterpolation = 1;
			}
			if(GUILayout.Button(easeInText, easeInStyle))
			{
				iInterpolation = 2;
			}
			if(GUILayout.Button(smoothText, smoothStyle))
			{
				iInterpolation = 3;
			}
			GUILayout.EndHorizontal();
			
			if(EditorGUI.EndChangeCheck()) {
					Undo.RegisterCompleteObjectUndo(target, "Change IK Interpolation");
					SMBScript.interpolation = iInterpolation;
			}
			
			GUILayout.Space(20);
			
			GUI.enabled = true;
			
		}
		
		//FUNCTION FOR DRAWING A SEPARATOR
		public static void DrawUILine(Color color, int thickness = 1, int padding = 2)
		{
			Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding+thickness));
			r.height = thickness;
			r.y+=padding/2;
			EditorGUI.DrawRect(r, color);
		}
	}
	
	
#endif	
}


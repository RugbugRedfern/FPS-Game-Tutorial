using UnityEngine;
using System.Collections;

public class FPSDemoGUI : MonoBehaviour
{
	public GameObject[] Prefabs;
    public Transform muzzleFlashPoint;
    public GameObject Gun;
    public float reactivateTime = 4;
    public Light Sun;

	private int currentNomber;
	private GameObject currentInstance;
	private GUIStyle guiStyleHeader = new GUIStyle();
    private float sunIntensity;
    float dpiScale;

	// Use this for initialization
	void Start () {
        if (Screen.dpi < 1) dpiScale = 1;
        if (Screen.dpi < 200) dpiScale = 1;
        else dpiScale = Screen.dpi / 200f;
        guiStyleHeader.fontSize = (int)(15f * dpiScale);
		guiStyleHeader.normal.textColor = new Color(0.15f,0.15f,0.15f);
		currentInstance = Instantiate(Prefabs[currentNomber], transform.position, transform.rotation) as GameObject;
        var reactivator = currentInstance.AddComponent<FPSDemoReactivator>();
		reactivator.TimeDelayToReactivate = reactivateTime ;
	    sunIntensity = Sun.intensity;
	}

	private void OnGUI()
	{
        if (GUI.Button(new Rect(10 * dpiScale, 15 * dpiScale, 135 * dpiScale, 37 * dpiScale), "PREVIOUS EFFECT"))
        {
			ChangeCurrent(-1);
		}
        if (GUI.Button(new Rect(160 * dpiScale, 15 * dpiScale, 135 * dpiScale, 37 * dpiScale), "NEXT EFFECT"))
		{
			ChangeCurrent(+1);
		}
        sunIntensity = GUI.HorizontalSlider(new Rect(10 * dpiScale, 70 * dpiScale, 285 * dpiScale, 15 * dpiScale), sunIntensity, 0, 0.6f);
	    Sun.intensity = sunIntensity;
        GUI.Label(new Rect(300 * dpiScale, 70 * dpiScale, 30 * dpiScale, 30 * dpiScale), "SUN INTENSITY", guiStyleHeader);
        GUI.Label(new Rect(400 * dpiScale, 15 * dpiScale, 100 * dpiScale, 20 * dpiScale), "Prefab name is \"" + Prefabs[currentNomber].name + "\"  \r\nHold any mouse button that would move the camera", guiStyleHeader);
	}
	// Update is called once per frame
	void ChangeCurrent(int delta) {
		currentNomber+=delta;
		if (currentNomber> Prefabs.Length - 1)
			currentNomber = 0;
		else if (currentNomber < 0)
			currentNomber = Prefabs.Length - 1;
		if(currentInstance!=null) Destroy(currentInstance);
        if (currentNomber < 10)
        {
            currentInstance = Instantiate(Prefabs[currentNomber], transform.position, transform.rotation) as GameObject;
            Gun.SetActive(false);
        }
        else
        {
            currentInstance = Instantiate(Prefabs[currentNomber], muzzleFlashPoint.position, muzzleFlashPoint.rotation) as GameObject;
            Gun.SetActive(true);
        }
        var reactivator = currentInstance.AddComponent<FPSDemoReactivator>();
		reactivator.TimeDelayToReactivate = reactivateTime ;
	}
}

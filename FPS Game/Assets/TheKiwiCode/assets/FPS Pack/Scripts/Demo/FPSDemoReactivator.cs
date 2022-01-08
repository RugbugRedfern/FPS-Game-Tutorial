using UnityEngine;

public class FPSDemoReactivator : MonoBehaviour
{

    public float StartDelay = 0;
	public float TimeDelayToReactivate = 3;
	
	void Start () {
        InvokeRepeating("Reactivate", StartDelay, TimeDelayToReactivate);
	}

	void Reactivate ()
	{
		gameObject.SetActive(false);
		gameObject.SetActive(true);
	}
}

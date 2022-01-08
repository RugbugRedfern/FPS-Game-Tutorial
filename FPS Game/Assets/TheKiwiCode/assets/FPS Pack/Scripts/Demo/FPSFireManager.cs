using UnityEngine;
using System.Collections;

public class FPSFireManager : MonoBehaviour
{
    public ImpactInfo[] ImpactElemets = new ImpactInfo[0];
    public float BulletDistance = 100;
    public GameObject ImpactEffect;

	void Update () {
	    if (Input.GetMouseButtonDown(0)) {
	        RaycastHit hit;
            var ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out hit, BulletDistance)) {
                var effect = GetImpactEffect(hit.transform.gameObject);
                if (effect==null)
                    return;
                var effectIstance = Instantiate(effect, hit.point, new Quaternion()) as GameObject;
                ImpactEffect.SetActive(false);
                ImpactEffect.SetActive(true);
                effectIstance.transform.LookAt(hit.point + hit.normal);
                Destroy(effectIstance, 4);
            }
           
	    }
	}
    
    [System.Serializable]
    public class ImpactInfo
    {
        public MaterialType.MaterialTypeEnum MaterialType;
        public GameObject ImpactEffect;
    }

    GameObject GetImpactEffect(GameObject impactedGameObject)
    {
        var materialType = impactedGameObject.GetComponent<MaterialType>();
        if (materialType==null)
            return null;
        foreach (var impactInfo in ImpactElemets)
        {
            if (impactInfo.MaterialType==materialType.TypeOfMaterial)
                return impactInfo.ImpactEffect;
        }
        return null;
    }
}

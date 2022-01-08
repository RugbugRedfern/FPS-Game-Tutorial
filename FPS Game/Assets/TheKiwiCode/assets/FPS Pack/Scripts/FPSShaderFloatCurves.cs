using UnityEngine;
using System.Collections;

public class FPSShaderFloatCurves : MonoBehaviour
{
    public string ShaderProperty = "_BumpAmt";
    public int MaterialID = 0;
    public AnimationCurve FloatPropertyCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public float GraphTimeMultiplier = 1, GraphScaleMultiplier = 1;

    private bool canUpdate;
    private Material matInstance;
    private int propertyID;
    private float startTime;

    private void Start()
    {
        var rend = GetComponent<Renderer>();
        if (rend != null)
        {
            var mats = rend.materials;
            if (MaterialID >= mats.Length)
                Debug.Log("ShaderColorGradient: Material ID more than shader materials count.");
            matInstance = mats[MaterialID];
        }
        else
        {
            var proj = GetComponent<Projector>();
            var projMat = proj.material;
            if (!projMat.name.EndsWith("(Instance)"))
                matInstance = new Material(projMat) { name = projMat.name + " (Instance)" };
            else
                matInstance = projMat;
            proj.material = matInstance;
        }
        if (!matInstance.HasProperty(ShaderProperty))
            Debug.Log("ShaderColorGradient: Shader not have \"" + ShaderProperty + "\" property");
        propertyID = Shader.PropertyToID(ShaderProperty);
    }

    private void OnEnable()
    {
        startTime = Time.time;
        canUpdate = true;
    }

    private void Update()
    {
        var time = Time.time - startTime;
        if (canUpdate) {
            var eval = FloatPropertyCurve.Evaluate(time / GraphTimeMultiplier) * GraphScaleMultiplier;
            matInstance.SetFloat(propertyID, eval);
        }
        if (time >= GraphTimeMultiplier)
            canUpdate = false;
    }
}
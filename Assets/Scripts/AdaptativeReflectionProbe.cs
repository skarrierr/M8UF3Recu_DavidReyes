using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(ReflectionProbe))]
public class AdaptativeReflectionProbe : MonoBehaviour
{
    ReflectionProbe probe;
    Camera cam;
    public Vector3 boxCenter;
    public float renderRate = 0.25f;
    void Reset()
    {
        Setup();
        boxCenter = transform.TransformPoint(probe.center);
    }
    void Start()
    {
        Setup();
        StartCoroutine(Render_Corutine());
    }
    void Setup()
    {
        probe = GetComponent<ReflectionProbe>();
        cam = Camera.main;
    }
    IEnumerator Render_Corutine()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(renderRate);
            yield return new WaitForEndOfFrame();
            Render();
        }
    }
    public void Render()
    {
        transform.position = cam.transform.position;
        probe.center = transform.InverseTransformPoint(boxCenter);
        if (probe.refreshMode == UnityEngine.Rendering.ReflectionProbeRefreshMode.ViaScripting)
        {
            probe.RenderProbe();
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1,1,0,.25f);
        Gizmos.DrawCube(boxCenter, probe.size);
    }
#endif
}

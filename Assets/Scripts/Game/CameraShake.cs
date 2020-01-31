using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    private Vector3 originPosition;
    private Quaternion originRotation;
    public float shake_decay;
    public float shake_intensity;

    
    void Update()
    {
        if (shake_intensity > 0)
        {
            transform.position = originPosition + Random.insideUnitSphere * shake_intensity;
            transform.rotation = new Quaternion(
            originRotation.x,
            originRotation.y,
            originRotation.z + Random.Range(-shake_intensity, shake_intensity) * .2f,
            originRotation.w);
            shake_intensity -= shake_decay;
        }
		else
		{
			transform.position = originPosition;
			transform.rotation = originRotation;
		}
    }

    public void Shake(float intensity, float decay)
    {
        originPosition = transform.position;
        originRotation = transform.rotation;
        shake_intensity = intensity;
        shake_decay = decay;
    }
}

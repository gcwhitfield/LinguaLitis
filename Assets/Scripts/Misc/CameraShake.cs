using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
	public float shakeDuration = 0f;
	public float shakeAmount = 0.1f;
	public float decreaseFactor = 1.0f;
    private Transform camTransform;
	
	Vector3 originalPos;
	
	void Awake()
	{
		camTransform = GetComponent(typeof(Transform)) as Transform;
	}
	
	void OnEnable()
	{
		originalPos = camTransform.localPosition;
	}

	void Update()
	{
		if (shakeDuration > 0)
		{
			camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
			
			shakeDuration -= Time.deltaTime * decreaseFactor;
		}
		else
		{
			shakeDuration = 0f;
			camTransform.localPosition = originalPos;
		}
	}
}
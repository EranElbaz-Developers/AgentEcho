using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Pickable : MonoBehaviour
{
    public PickableTypes type;
    public float pickupTime;
    private float startTime;

    public delegate void Pickup(PickableTypes type);

    public event Pickup OnPickup;
    public Light2D pickupEffect;

    private void Awake()
    {
        OnPickup += GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerShooting>().OnPickup;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        startTime = Time.time;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        pickupEffect.pointLightOuterAngle = 0;
        pickupEffect.pointLightInnerAngle = 0;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        var currentTime = (Time.time - startTime);
        var angle = Mathf.Lerp(pickupEffect.transform.rotation.z, 360,
            currentTime / pickupTime);
        pickupEffect.pointLightInnerAngle = angle;
        pickupEffect.pointLightOuterAngle = angle;
        pickupEffect.transform.rotation = Quaternion.Euler(0, 0, -angle / 2);
        if (currentTime > pickupTime)
        {
            if (null != OnPickup)
            {
                OnPickup(type);
            }

            Destroy(gameObject);
        }
    }
}
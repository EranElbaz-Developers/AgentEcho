using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerView))]
[RequireComponent(typeof(ParticleSystem))]
public class PlayerShooting : MonoBehaviour
{
    public Light2D shootingLight;
    public float movementAnglePenalty = 1;
    [Range(1, 10)] public float penaltyLerpSpeed;
    [Range(1, 100)] public float fireFps = 1;
    [Range(1, 100)] public int magSize = 1;
    [Range(1, 10)] public int reloadTime = 1;
    [Range(1, 100)] public float damage = 10;
    public TMP_Text ammoText;
    public Light2D reloadEffect;
    private int currentMag;
    private int allAmmo;
    private float lastShootTime;
    private int sensorCount;
    private PlayerMovement pm;
    private float defaultLightAngle;
    private ParticleSystem shootingEffect;
    private ParticleSystem.ShapeModule shootingEffectShape;
    private bool isReload;
    private float reloadStartTime;

    private void Awake()
    {
        reloadStartTime = Time.time;
        isReload = false;
        currentMag = 0;
        lastShootTime = Time.time;
        defaultLightAngle = shootingLight.pointLightOuterAngle;
        pm = GetComponent<PlayerMovement>();
        sensorCount = GetComponent<PlayerView>().sensorCount;
        shootingEffect = GetComponent<ParticleSystem>();
        shootingEffectShape = shootingEffect.shape;
    }

    public void OnPickup(PickableTypes type)
    {
        if (type == PickableTypes.Ammo)
        {
            allAmmo += magSize;
        }
    }

    private void Update()
    {
        ShootingLightPenalty();
        if (SensorsUtils.FrontView(transform, shootingLight, sensorCount).Any())
        {
            Shoot();
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            if (allAmmo > 0)
            {
                isReload = true;
                reloadStartTime = Time.time;
                
            }
        }

        if (isReload)
        {
            ReloadAnimation();
        }

        ammoText.text = $"{currentMag}/{allAmmo}";
    }

    private void ReloadAnimation()
    {
        var currentTime = (Time.time - reloadStartTime);
        var angle = Mathf.Lerp(reloadEffect.transform.rotation.z, 360,
            currentTime / reloadTime);
        reloadEffect.pointLightInnerAngle = angle;
        reloadEffect.pointLightOuterAngle = angle;
        reloadEffect.transform.rotation = Quaternion.Euler(0, 0, -angle / 2);
        if (currentTime > reloadTime)
        {
            allAmmo -= magSize - currentMag;
            currentMag = magSize;
            isReload = false;
            reloadEffect.pointLightInnerAngle = 0;
            reloadEffect.pointLightOuterAngle = 0;
        }
    }

    public void Shoot()
    {
        if (Time.time - lastShootTime > 1 / fireFps && currentMag > 0)
        {
            lastShootTime = Time.time;
            shootingEffect.Emit(1);
            currentMag -= 1;
        }
    }

    private void ShootingLightPenalty()
    {
        var angle = Mathf.Lerp(shootingLight.pointLightOuterAngle,
            defaultLightAngle + pm.SpeedPercent * movementAnglePenalty,
            Time.deltaTime * penaltyLerpSpeed);
        shootingLight.pointLightOuterAngle = angle;
        shootingEffectShape.angle = angle / 2;
    }
}
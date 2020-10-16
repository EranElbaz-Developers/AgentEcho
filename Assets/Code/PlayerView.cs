using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerView : MonoBehaviour
{
    public Light2D frontLight;
    public Light2D playerLight;
    [Range(2, 100)] public int sensorCount = 3;
    private HashSet<Renderer> inSights;

    private void Awake()
    {
        inSights = new HashSet<Renderer>();
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.GetComponent<Enemy>().OnDeath += EnemyDie;
        }
    }

    private void EnemyDie(GameObject go)
    {
        inSights.Remove(go.GetComponent<Renderer>());
    }

    private void Update()
    {
        var currentSaw = new HashSet<Renderer>();
        foreach (var saw in SensorsUtils.InSights(transform, playerLight, frontLight, sensorCount))
        {
            currentSaw.Add(saw);
            inSights.Add(saw);
            saw.enabled = true;
        }

        var remaining = new HashSet<Renderer>(inSights);
        remaining.ExceptWith(currentSaw);
        foreach (var remained in remaining)
        {
            remained.enabled = false;
            inSights.Remove(remained);
        }
    }
}
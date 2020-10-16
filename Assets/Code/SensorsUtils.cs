using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class SensorsUtils
{
    public static HashSet<Renderer> InSights(Transform transform, Light2D lightSide, Light2D lightFront, int sensorCount)
    {
        var side = SideView(transform, lightSide);
        side.UnionWith(FrontView(transform, lightFront, sensorCount));
        return side;
    }

    public static HashSet<Renderer> SideView(Transform transform, Light2D light)
    {
        var startPos = transform.position.AsVector2();
        var hits = Physics2D.CircleCastAll(startPos, light.pointLightOuterRadius, Vector2.zero,
            light.pointLightOuterRadius);

        var sights = new HashSet<Renderer>();

        foreach (var hit in hits)
        {
            if (hit.collider != null)
            {
                var secondHit = Physics2D.Linecast(startPos,
                    hit.transform.GetComponent<Collider2D>().bounds.ClosestPoint(startPos));
                if (secondHit.collider != null && secondHit.transform.CompareTag("Enemy"))
                {
                    var rendrer = secondHit.transform.GetComponent<Renderer>();
                    sights.Add(rendrer);
                }
            }
        }

        return sights;
    }

    public static HashSet<Renderer> FrontView(Transform transform, Light2D light, int sensorCount)
    {
        var startPos = transform.position.AsVector2();
        var sensors = GetSensorsPositions(transform, light, sensorCount);

        var sights = new HashSet<Renderer>();
        foreach (var position in sensors)
        {
            var hit = Physics2D.Linecast(startPos, position);
            if (hit.collider != null && hit.transform.CompareTag("Enemy"))
            {
                var rendrer = hit.transform.GetComponent<Renderer>();
                sights.Add(rendrer);
            }
        }

        return sights;
    }

    public static List<Vector2> GetSensorsPositions(Transform transform, Light2D light, int sensorCount)
    {
        var playerPos = transform.position;
        var positions = new List<Vector2>();

        var basicSensor = playerPos + transform.up * light.pointLightOuterRadius;
        for (int i = 0; i < sensorCount; i++)
        {
            var sensorPosTop =
                RotatePointAroundPivot(basicSensor, playerPos,
                    new Vector3(0, 0, light.pointLightOuterAngle / (2 * (sensorCount - 1)) * i));
            var sensorPosLow =
                RotatePointAroundPivot(basicSensor, playerPos,
                    new Vector3(0, 0, light.pointLightOuterAngle / (2 * (sensorCount - 1)) * -i));
            positions.Add(sensorPosLow.AsVector2());
            positions.Add(sensorPosTop.AsVector2());
        }

        return positions;
    }

    private static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        var dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles) * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }
}
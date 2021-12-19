using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivineInterventionScript : AttackBase
{
    SpriteRenderer sr;
    public bool isDealingDamage;

    public LineRenderer lineRenderer;
    float laserDistance = 100f;
    public float laserWidthAtTheStart = 1f;
    public float minWidth = 0.1f;
    public float maxWidth = 3f;

    public float laserStartDelay = 0f;
    public float laserDuration = 0f;
    public float laserEndDelay = 1f;

    // laser info
    public Vector2 laserPosition;
    public Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        laserPosition = new Vector2(transform.position.x, transform.position.y);
        direction = laserPosition - (laserPosition - Vector2.up);

        Draw2dRay(-direction * laserDistance, direction * laserDistance);
        StartCoroutine(LaserProgression());
    }

    private void Draw2dRay(Vector2 point1, Vector2 point2)
    {
        lineRenderer.startWidth = laserWidthAtTheStart;
        lineRenderer.endWidth = laserWidthAtTheStart;
        lineRenderer.SetPosition(0, point1);
        lineRenderer.SetPosition(1, point2);
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator LaserProgression()
    {
        // charging laser
        yield return StartCoroutine(LaserInterpolation(laserWidthAtTheStart, minWidth, laserStartDelay, easeOutCubic));

        // laser fire
        yield return StartCoroutine(LaserInterpolation(minWidth, maxWidth, laserDuration, easeInOutCubic));

        // laser disappear
        yield return StartCoroutine(LaserInterpolation(maxWidth, 0f, laserEndDelay, easeOutCubic));

        Destroy(gameObject);
    }

    IEnumerator LaserInterpolation(float fromWidth, float toWidth, float duration, Func<float, float> easeFunction)
    {
        // charging laser
        float timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            float interpolationVal = timeElapsed / duration;
            float width = Mathf.Lerp(fromWidth, toWidth, easeFunction(interpolationVal));


            lineRenderer.startWidth = width;
            lineRenderer.endWidth = width;

            timeElapsed += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    float easeOutCubic(float x)
    {
        return 1f - (float)Math.Pow(1 - x, 3);
    }

    float easeInOutCubic(float x)
    {
        return x < 0.5 ? 4 * x * x * x : 1 - (float)Math.Pow(-2 * x + 2, 3) / 2;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperNovaScript : MonoBehaviour
{
    public GameObject playerWhoShotTheSupernova;
    SpriteRenderer sr;
    public LineRenderer lineRenderer;
    float laserDistance = 10000f;
    float laserWidthAtTheStart = 1f;
    float minWidth = 0.1f;
    float maxWidth = 3f;

    float laserStartDelay = 0.5f;
    float laserDuration = 1.5f;
    float laserEndDelay = 0.3f;

    // laser info
    Vector2 laserPosition;
    Vector2 direction;
    // Start is called before the first frame update
    void Start()
    {
        sr = playerWhoShotTheSupernova.GetComponent<SpriteRenderer>();

        laserPosition = new Vector2(playerWhoShotTheSupernova.transform.position.x + sr.size.x / 2, playerWhoShotTheSupernova.transform.position.y + sr.size.y / 2);
        Vector2 mouseToWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = mouseToWorldPos - laserPosition;

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
        yield return StartCoroutine(LaserInterpolation(maxWidth, 0f, laserEndDelay, easeInOutCubic));

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

            Debug.DrawLine(laserPosition, laserPosition + direction, Color.green);
            Debug.DrawLine(laserPosition, laserPosition - direction, Color.red);

            timeElapsed += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    float calculateIntensity(float intensity)
    {
        // for some reason, the desired intensity value (set in the UI slider) needs to be modified slightly for proper internal consumption
        float adjustedIntensity = intensity - (0.4169F);

        // redefine the color with intensity factored in - this should result in the UI slider matching the desired value
        return Mathf.Pow(2.0F, adjustedIntensity);
    }

    float easeOutCubic(float x) {
        return 1f - (float)Math.Pow(1 - x, 3);
    }
    float easeOutQuad(float x)
    {
        return 1 - (1 - x) * (1 - x);
    }

    float easeInOutCubic(float x)
    {
        return x < 0.5 ? 4 * x * x * x : 1 - (float)Math.Pow(-2 * x + 2, 3) / 2;
    }
}

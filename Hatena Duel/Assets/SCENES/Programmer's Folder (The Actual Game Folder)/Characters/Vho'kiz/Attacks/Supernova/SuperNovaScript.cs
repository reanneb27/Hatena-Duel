using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperNovaScript : AttackBase
{
    SpriteRenderer sr;
    public LineRenderer lineRenderer;
    float laserDistance = 100f;
    float laserWidthAtTheStart = 1f;
    float minWidth = 0.1f;
    float maxWidth = 3f;

    float laserStartDelay = 0.5f;
    float laserDuration = 1.5f;
    float laserEndDelay = 0.3f;

    // laser info
    Vector2 laserPosition;
    Vector2 direction;

    // damage/rage info
    float damageRate = .25f;
    float nextDmgTime;

    float rageRate = .2f;
    float nextRageTime;

    // players info
    CharacterBase player;
    CharacterBase enemy;
    string enemyLayer = "Player1";
    GameObject enemyGameObj;

    public void IncreaseLaserWidth(float width)
    {
        laserWidthAtTheStart *= width;
        maxWidth *= width;
    }

    private void Awake()
    {
        damage = 26f;
        rage = 2f;
    }

    // Start is called before the first frame update
    void Start()
    {
        AudioManager = FindObjectOfType<ArenaAudioManagerScript>();

        player = PlayerWhoCastedTheSkill.gameObject.GetComponent<CharacterBase>();
        if (PlayerWhoCastedTheSkill.gameObject.tag == "Player1")
            enemyLayer = "Player2";
        enemyGameObj = GameObject.FindGameObjectWithTag(enemyLayer);
        enemy = enemyGameObj.GetComponent<CharacterBase>();

        sr = PlayerWhoCastedTheSkill.GetComponent<SpriteRenderer>();
        SpriteRenderer enemySR = enemyGameObj.GetComponent<SpriteRenderer>();

        // set position
        transform.position = player.transform.position + Vector3.up * (sr.size.y / 2);

        laserPosition = new Vector2(PlayerWhoCastedTheSkill.transform.position.x + sr.size.x / 2, PlayerWhoCastedTheSkill.transform.position.y + sr.size.y / 2);
        //Vector2 mouseToWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 enemyPosition = new Vector2(enemyGameObj.transform.position.x + enemySR.size.x / 2, enemyGameObj.transform.position.y + enemySR.size.y / 2); 
        direction = (enemyPosition - laserPosition).normalized;

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
        AudioManager.Play("vhokiz_s3_laser_start");
        AudioManager.Play("vhokiz_s3_laser_beam");
        yield return StartCoroutine(LaserInterpolation(laserWidthAtTheStart, minWidth, laserStartDelay, easeOutCubic, false));

        // laser fire
        yield return StartCoroutine(LaserInterpolation(minWidth, maxWidth, laserDuration, easeInOutCubic, true));

        // laser disappear
        AudioManager.Stop("vhokiz_s3_laser_beam");
        yield return StartCoroutine(LaserInterpolation(maxWidth, 0f, laserEndDelay, easeInOutCubic, false));

        Destroy(gameObject);
    }

    IEnumerator LaserInterpolation(float fromWidth, float toWidth, float duration, Func<float, float> easeFunction, bool addCollider)
    {
        // charging laser
        float timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            float interpolationVal = timeElapsed / duration;
            float width = Mathf.Lerp(fromWidth, toWidth, easeFunction(interpolationVal));


            lineRenderer.startWidth = width;
            lineRenderer.endWidth = width;

            if (addCollider)
            {
                Vector2 startP = laserPosition - direction * laserDistance;
                Vector2 endP = laserPosition + direction * laserDistance;
                bool enemyWasHit = false;

                // multiple ray casts by distance away from each other
                float rayCastDistance = 0.5f;
                float distFactor = 1;
                float laserWidth = width;
                while (laserWidth > rayCastDistance)
                {
                    Vector2 startUpVec = RotateVector(startP, Mathf.PI / 2).normalized * distFactor * rayCastDistance + startP;
                    Vector2 startDownVec = RotateVector(startP, -Mathf.PI / 2).normalized * distFactor * rayCastDistance + startP;
                    Vector2 endUpVec = RotateVector(startP, Mathf.PI / 2).normalized * distFactor * rayCastDistance + endP;
                    Vector2 endDownVec = RotateVector(startP, -Mathf.PI / 2).normalized * distFactor * rayCastDistance + endP;

                    Debug.DrawRay(startUpVec, (endUpVec - startUpVec).normalized * laserDistance * 2, Color.red);
                    Debug.DrawRay(startDownVec, (endDownVec - startDownVec).normalized * laserDistance * 2, Color.cyan);

                    // raycasts
                    enemyWasHit |= Physics2D.Raycast(startUpVec, (endUpVec - startUpVec).normalized, laserDistance * 2, LayerMask.GetMask(enemyLayer));
                    enemyWasHit |= Physics2D.Raycast(startDownVec, (endDownVec - startDownVec).normalized, laserDistance * 2, LayerMask.GetMask(enemyLayer));

                    laserWidth -= rayCastDistance * 2;
                    distFactor++;
                }

                Debug.DrawRay(startP, (endP - startP).normalized * laserDistance * 2, Color.green);
                enemyWasHit |= Physics2D.Raycast(startP, (endP - startP).normalized, laserDistance * 2, LayerMask.GetMask(enemyLayer));

                // now check if enemy was hit by the laser AND it's time to take damage AND add rage
                if (enemyWasHit && Time.time > nextDmgTime)
                {
                    nextDmgTime = Time.time + damageRate;
                    enemy.OnHit(damage, 0);
                }
                if (enemyWasHit && Time.time > nextRageTime)
                {
                    nextRageTime = Time.time + rageRate;
                    enemy.OnHit(0, rage * 2.5f);
                    player.OnHit(0, rage);
                }

            }

            timeElapsed += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    Vector2 RotateVector(Vector2 vec, float angle)
    {
        return new Vector2(Mathf.Cos(angle) * vec.x - Mathf.Sin(angle) * vec.y, Mathf.Sin(angle) * vec.x + Mathf.Cos(angle) * vec.y);
    }

    float easeOutCubic(float x) {
        return 1f - (float)Math.Pow(1 - x, 3);
    }

    float easeInOutCubic(float x)
    {
        return x < 0.5 ? 4 * x * x * x : 1 - (float)Math.Pow(-2 * x + 2, 3) / 2;
    }
}

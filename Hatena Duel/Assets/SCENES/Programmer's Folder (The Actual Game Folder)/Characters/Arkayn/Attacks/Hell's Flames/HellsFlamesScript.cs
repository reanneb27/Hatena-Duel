using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellsFlamesScript : AttackBase
{
    Rigidbody2D RB;
    ParticleSystem StartingPS;
    ParticleSystem HellsFlamesPS;

    float flameDuration = 4f;
    float flameFinishTime;

    // damage/rage info
    float damageRate = .3f;
    float nextDmgTime;

    float rageRate = .2f;
    float nextRageTime;

    // players info
    CharacterBase player;
    SpriteRenderer SR1;
    string enemyLayer = "Player1"; // player tag is the same as its layer
    GameObject enemyGameObj;
    CharacterBase enemy;

    private void Awake()
    {
        damage = 6f;
        rage = 1f;
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
        SR1 = player.GetComponent<SpriteRenderer>();

        // set position
        transform.position = player.transform.position + Vector3.up * (SR1.size.y / 2);

        RB = GetComponent<Rigidbody2D>();
        RB.AddForce(new Vector2(150 * (player.GetComponent<SpriteRenderer>().flipX ? -1 : 1), 400));
        StartingPS = transform.GetChild(0).GetComponent<ParticleSystem>();
        HellsFlamesPS = transform.GetChild(1).GetComponent<ParticleSystem>();

        AudioManager.Play("arkayn_s1_fire_start");
    }

    bool floorFlameState = false;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!floorFlameState && collision.tag == "Ground")
        {
            floorFlameState = true;

            StartingPS.Stop();
            Destroy(StartingPS, 0.7f);

            RB.gravityScale = 0;
            RB.velocity = new Vector2();
            RB.angularVelocity = 0;
            HellsFlamesPS.Play();

            // destroy flames after 4 secs
            flameFinishTime = Time.time + flameDuration;
            StartCoroutine(HellsFlamesProgression());
        }

        if (floorFlameState && collision.tag == enemyLayer) // enemy is on fire
        {
            if (Time.time > nextDmgTime)
            {
                nextDmgTime = Time.time + damageRate;
                enemy.OnHit(damage, 0);
            }
            if (Time.time > nextRageTime)
            {
                nextRageTime = Time.time + rageRate;
                player.OnHit(0, rage);
                enemy.OnHit(0, rage / 2);
            }
        }
    }

    IEnumerator HellsFlamesProgression()
    {
        AudioManager.Play("arkayn_s1_fire");

        while (Time.time < flameFinishTime)
            yield return null;

        HellsFlamesPS.Stop();
        Destroy(gameObject, 0.7f);
    }
}

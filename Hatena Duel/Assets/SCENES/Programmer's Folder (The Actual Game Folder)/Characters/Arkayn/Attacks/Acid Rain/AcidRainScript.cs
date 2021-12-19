using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidRainScript : AttackBase
{
    ParticleSystem AcidRainPS;
    float AcidDuration = 4f;

    // collider attribs
    Vector2 startOffset = new Vector2(0, 0.4009054f);
    Vector2 startSize = new Vector2(10.22628f, 0.1981902f);
    Vector2 endOffset = new Vector2(0, -7.228626f);
    Vector2 endSize = new Vector2(10.22628f, 15.45725f);

    // damage/rage info
    float damageRate = .1f;
    float nextDmgTime;

    float rageRate = .3f;
    float nextRageTime;

    // players info
    CharacterBase player;
    SpriteRenderer SR1;
    CharacterBase enemy;
    string enemyLayer = "Player1";
    GameObject enemyGameObj;

    void Awake()
    {
        damage = 5f;
        rage = 0.8f;
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
        transform.position = new Vector3(player.transform.position.x, transform.position.y) + Vector3.right * 8f * (SR1.flipX ? -1 : 1);

        AcidRainPS = transform.GetChild(0).GetComponent<ParticleSystem>();

        StartCoroutine(AcidRainProgression());
    }

    
    IEnumerator AcidRainProgression()
    {
        AcidRainPS.Play();
        AudioManager.Play("arkayn_s3_rain");
        // acid rain duration
        float acidDuration = Time.time + AcidDuration;
        while (Time.time < acidDuration)
            yield return null;

        AcidRainPS.Stop();
        Destroy(gameObject, 1f);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == enemyLayer)
        {
            if (Time.time > nextDmgTime)
            {
                nextDmgTime = Time.time + damageRate;
                enemy.OnHit(damage, 0);
                player.OnHit(-damage / 2, 0);
            }
            if (Time.time > nextRageTime)
            {
                nextRageTime = Time.time + rageRate;
                player.OnHit(0, rage / 2);
                enemy.OnHit(0, rage);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

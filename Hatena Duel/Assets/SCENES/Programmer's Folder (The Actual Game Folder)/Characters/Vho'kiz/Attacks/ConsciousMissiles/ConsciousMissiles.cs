using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsciousMissiles : AttackBase
{

    public GameObject rocketExplosion;
    public Transform explosionPoint;
    Rigidbody2D rb;
    SpriteRenderer sr;
    BoxCollider2D bc2d;
    float speed = 3;
    float yFollowSpeed = 2;
    float speedCap = 30;
    bool isFlipped = false;

    // players info
    CharacterBase player;
    CharacterBase enemy;
    SpriteRenderer SR1;
    string enemyLayer = "Player1"; // player tag is the same as its layer
    GameObject enemyGameObj;

    private void Awake()
    {
        damage = 1.8f;
        rage = 5f;
    }

    // Start is called before the first frame update
    void Start()
    {
        AudioManager = FindObjectOfType<ArenaAudioManagerScript>();

        bc2d = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();

        player = PlayerWhoCastedTheSkill.gameObject.GetComponent<CharacterBase>();
        if (PlayerWhoCastedTheSkill.gameObject.tag == "Player1")
            enemyLayer = "Player2";
        enemyGameObj = GameObject.FindGameObjectWithTag(enemyLayer);
        enemy = enemyGameObj.GetComponent<CharacterBase>();

        SR1 = player.GetComponent<SpriteRenderer>();

        // set position
        transform.position = player.transform.position + Vector3.up * (SR1.size.y / 2);

        isFlipped = PlayerWhoCastedTheSkill.GetComponent<SpriteRenderer>().flipX;
        transform.Rotate(0f, isFlipped ? 180f : 0f, 0f);

        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(1, 0);

        AudioManager.Play("vhokiz_s1_incoming");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newVelocity = new Vector2(Mathf.Abs(rb.velocity.x), 0) + Mathf.Abs(rb.velocity.x) * new Vector2(speed * Time.deltaTime, 0);

        float targetYPos = enemyGameObj.transform.position.y + enemyGameObj.GetComponent<SpriteRenderer>().size.y / 2;

        float xDir = isFlipped ? -1 : 1;
        float yDir = transform.position.y > targetYPos ? -1 : 1;

        rb.velocity = new Vector2(Mathf.Clamp(newVelocity.x, 0, speedCap) * xDir, yFollowSpeed * yDir);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == enemyLayer || collision.gameObject.tag == "Wall")
        {
            AudioManager.Stop("vhokiz_s1_incoming");
            AudioManager.Play("vhokiz_s1_explosion");

            // check if hit the enemy, decrease CD by 80% and damage/enrage
            if (collision.gameObject.tag == enemyLayer)
            {
                var character = PlayerWhoCastedTheSkill.GetComponent<CharacterBase>();
                if (character is VhoKizScript)
                    character.NextSkill1Time += (1 - (character.Skill1Cooldown - (1 - Mathf.Clamp(character.NextSkill1Time - Time.time, 0, character.Skill1Cooldown)))) * 0.8f;

                enemy.OnHit(damage * Mathf.Abs(rb.velocity.x), rage / 2);
                player.OnHit(0, rage);
            }

            bc2d.enabled = false;
            rb.velocity = new Vector2(0, 0);


            var expl = Instantiate(rocketExplosion, transform.position, Quaternion.identity);
            expl.transform.localScale = transform.localScale;
            expl.transform.Rotate(0f, isFlipped ? 180f : 0f, 0f);

            gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
            sr.enabled = false;
            Destroy(gameObject, 1); // destroy the grenade
            Destroy(expl, 0.833333f); // delete the explosion after 3 seconds
        }

    }
}

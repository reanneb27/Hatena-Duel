using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticStarScript : AttackBase
{
    public GameObject orbExplosion;
    Rigidbody2D rb;
    SpriteRenderer sr;

    // players info
    CharacterBase player;
    CharacterBase enemy;
    SpriteRenderer SR1;
    string enemyLayer = "Player1";
    GameObject enemyGameObj;

    // skill attrib
    public float selfDestructDuration = 3f;
    float selfDestructFinishTime;

    private void Awake()
    {
        damage = 82f;
        rage = 7f;
    }

    // Start is called before the first frame update
    void Start()
    {
        AudioManager = FindObjectOfType<ArenaAudioManagerScript>();

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        player = PlayerWhoCastedTheSkill.gameObject.GetComponent<CharacterBase>();
        if (PlayerWhoCastedTheSkill.gameObject.tag == "Player1")
            enemyLayer = "Player2";
        enemyGameObj = GameObject.FindGameObjectWithTag(enemyLayer);
        enemy = enemyGameObj.GetComponent<CharacterBase>();

        SR1 = player.GetComponent<SpriteRenderer>();

        // set position
        transform.position = player.transform.position + Vector3.up * (SR1.size.y / 2);

        selfDestructFinishTime = Time.time + selfDestructDuration;
        StartCoroutine(SelfDestructProgression());

        AudioManager.Play("vhokiz_s2_incoming");
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = enemyGameObj.transform.position - (gameObject.transform.position - Vector3.up * sr.size.y / 2);
    }

    IEnumerator SelfDestructProgression()
    {
        // wait for self destruct time
        while (Time.time < selfDestructFinishTime)
            yield return null;

        Destroy();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == enemyLayer)
        {
            enemy.OnHit(damage, rage / 2);
            player.OnHit(0, rage);
            Destroy();
        }
    }

    private void Destroy()
    {
        AudioManager.Stop("vhokiz_s2_incoming");
        AudioManager.Play("vhokiz_s2_explosion");

        var expl = Instantiate(orbExplosion, transform.position, Quaternion.identity);
        expl.transform.localScale = transform.localScale;

        Destroy(gameObject);
        Destroy(expl, 0.75f); // delete the explosion after .75 seconds (because explosion animation lasts is 9 frames long at 12 fps, so animation runs for .75 seconds)
    }
}

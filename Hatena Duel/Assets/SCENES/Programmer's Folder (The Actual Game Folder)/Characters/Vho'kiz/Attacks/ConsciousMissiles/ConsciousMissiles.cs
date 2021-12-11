using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsciousMissiles : MonoBehaviour
{
    public GameObject playerWhoLaunchedTheRocket;

    public GameObject enemyPlayer;
    public GameObject rocketExplosion;
    public Transform explosionPoint;
    Rigidbody2D rb;
    SpriteRenderer sr;
    BoxCollider2D bc2d;
    float speed = 3;
    float yFollowSpeed = 2;
    float speedCap = 30;
    bool isFlipped = false;
    // Start is called before the first frame update
    void Start()
    {
        bc2d = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        string enemyTag = playerWhoLaunchedTheRocket.tag == "Player1" ? "Player2" : "Player1";
        enemyPlayer = GameObject.FindGameObjectWithTag(enemyTag);

        isFlipped = playerWhoLaunchedTheRocket.GetComponent<SpriteRenderer>().flipX;
        transform.Rotate(0f, isFlipped ? 180f : 0f, 0f);

        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(1, 0);

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newVelocity = new Vector2(Mathf.Abs(rb.velocity.x), 0) + Mathf.Abs(rb.velocity.x) * new Vector2(speed * Time.deltaTime, 0);


        float targetYPos = playerWhoLaunchedTheRocket.transform.position.y + playerWhoLaunchedTheRocket.GetComponent<SpriteRenderer>().size.y / 2;

        float xDir = isFlipped ? -1 : 1;
        float yDir = transform.position.y > targetYPos ? -1 : 1;

        rb.velocity = new Vector2(Mathf.Clamp(newVelocity.x, 0, speedCap) * xDir, yFollowSpeed * yDir);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == enemyPlayer.tag || collision.gameObject.tag == "Wall")
        {
            // check if hit the enemy, decrease CD by 70%
            if (collision.gameObject.tag == enemyPlayer.tag)
            {
                var character = playerWhoLaunchedTheRocket.GetComponent<CharacterBase>();
                character.NextSkill1Time += (1 - (character.Skill1Cooldown - (1 - Mathf.Clamp(character.NextSkill1Time - Time.time, 0, character.Skill1Cooldown)))) * 0.7f;
            }

            bc2d.enabled = false;
            rb.velocity = new Vector2(0, 0);

            var expl = Instantiate(rocketExplosion, transform.position, Quaternion.identity);
            expl.transform.Rotate(0f, isFlipped ? 180f : 0f, 0f);

            gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
            sr.enabled = false;
            Destroy(gameObject, 1); // destroy the grenade
            Destroy(expl, 0.833333f); // delete the explosion after 3 seconds
        }

    }
}

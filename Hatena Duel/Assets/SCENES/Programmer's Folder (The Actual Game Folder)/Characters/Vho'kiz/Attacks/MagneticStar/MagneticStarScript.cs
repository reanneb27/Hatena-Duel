using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticStarScript : MonoBehaviour
{
    public GameObject playerWhoCreatedTheOrb;
    public GameObject orbExplosion;
    Rigidbody2D rb;
    SpriteRenderer sr;
    GameObject targetPlayer;
    float speed = 5;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        string enemyTag = playerWhoCreatedTheOrb.tag == "Player1" ? "Player2" : "Player1";
        targetPlayer = GameObject.FindGameObjectWithTag(enemyTag);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = targetPlayer.transform.position - (gameObject.transform.position - Vector3.up * sr.size.y / 2);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == targetPlayer.tag)
        {
            Debug.Log("orb time");
            var expl = Instantiate(orbExplosion, transform.position, Quaternion.identity);

            Destroy(gameObject);
            Destroy(expl, 0.75f); // delete the explosion after 3 seconds
        }
        else
        {
            Debug.Log("orb no hit?");
        }
    }
}

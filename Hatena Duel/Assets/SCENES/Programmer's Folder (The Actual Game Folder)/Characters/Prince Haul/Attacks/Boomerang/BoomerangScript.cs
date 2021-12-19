using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangScript : AttackBase
{
    // boomerang attribs
    float ThrowDuration = .5f;
    float ComeBackDuration = .5f;
    float boomerangDistance = 11f;
    bool canDamage = true; // we're using OnTriggerStay2d, so this is needed so that it doesn't deal damage every frame update

    // players info
    CharacterBase player;
    string enemyLayer = "Player1"; // player tag is the same as its layer
    GameObject enemyGameObj;
    CharacterBase enemy;

    private void Awake()
    {
        damage = 22f;
        rage = 5f;
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

        Vector3 endPos = transform.position + Vector3.right * boomerangDistance;
        if (PlayerWhoCastedTheSkill.GetComponent<SpriteRenderer>().flipX)
            endPos = transform.position - Vector3.right * boomerangDistance;

        StartCoroutine(Boomerang(transform.position, endPos, ThrowDuration, ComeBackDuration));
    }

    // resources used: https://forum.unity.com/threads/a-smooth-ease-in-out-version-of-lerp.28312/
    //                 https://answers.unity.com/questions/1833470/how-to-use-easing-inside-coroutine-to-change-a-flo.html
    IEnumerator Boomerang(Vector3 startPos, Vector3 endPos, float throwDuration, float comeBackDuration)
    {
        // boomerang throw
        AudioManager.Play("princehaul_s1");
        float t = 0.0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / throwDuration;
            transform.position = Vector3.Lerp(startPos, endPos, easeOutCubic(t));
            yield return null;
        }

        canDamage = true;

        // boomerang coming back
        t = 0.0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / comeBackDuration;
            transform.position = Vector3.Lerp(endPos, PlayerWhoCastedTheSkill.transform.position + Vector3.up * (PlayerWhoCastedTheSkill.GetComponent<SpriteRenderer>().size.y / 2), easeInCubic(t));
            yield return null;
        }
        


        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == enemyLayer && canDamage)
        {
            enemy.OnHit(damage, rage / 1.5f);
            player.OnHit(0, rage);
            canDamage = false;
        }
    }

    // https://easings.net/
    private float easeOutCubic(float x)
    {
        return 1 - Mathf.Pow(1 - x, 2);
    }
    private float easeInCubic(float x)
    {
        return x * x;
    }
}

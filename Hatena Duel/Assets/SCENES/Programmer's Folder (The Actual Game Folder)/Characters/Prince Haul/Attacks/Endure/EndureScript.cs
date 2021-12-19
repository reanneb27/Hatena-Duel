using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndureScript : AttackBase
{
    SpriteRenderer sr;

    public float duration = 4f;
    float finishTime;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager = FindObjectOfType<ArenaAudioManagerScript>();

        sr = PlayerWhoCastedTheSkill.GetComponent<SpriteRenderer>();

        finishTime = Time.time + duration;
        StartCoroutine(EndureProgression());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = PlayerWhoCastedTheSkill.transform.position + new Vector3(0, sr.size.y - .25f);
    }

    IEnumerator EndureProgression()
    {
        AudioManager.Play("princehaul_s2_charge");

        // endure
        while (Time.time < finishTime)
            yield return null;

        PlayerWhoCastedTheSkill.GetComponent<PrinceHaulScript>().enduring = false;

        gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
        Destroy(gameObject, .5f); // give a .5 second delay before destroying, to wait for particle system particles to disappear
    }
}

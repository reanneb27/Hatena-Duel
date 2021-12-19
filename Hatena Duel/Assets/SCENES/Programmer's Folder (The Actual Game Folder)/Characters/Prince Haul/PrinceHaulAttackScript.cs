using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class PrinceHaulScript : CharacterBase
{
    // skills
    public GameObject Boomerang;
    public GameObject Endure;
    public bool enduring = false;
    public GameObject CopyCat;
    public GameObject StolenSkill;

    // ult extra field variables
    public float CDDecreaseFactor = .5f; // 50% decrease; .4f = 60% cd decrease
    public float tempSkill1CDVar;
    public float tempSkill2CDVar;
    public float tempSkill3CDVar;

    public override void OnHit(float dmg, float _rage)
    {
        if (enduring)
            health = Mathf.Clamp(health - dmg * .2f, 0, maxHealth);
        else
            health = Mathf.Clamp(health - dmg, 0, maxHealth);

        if (enduring)
            rage = Mathf.Clamp(rage + _rage * 2f, 0, maxRage);
        else
            rage = Mathf.Clamp(rage + _rage, 0, maxRage);
    }

    protected override void DoSkill1()
    {
        GameObject boomerang = Instantiate(Boomerang, transform.position + Vector3.up * (SR.size.y / 2), Quaternion.identity);
        boomerang.GetComponent<AttackBase>().PlayerWhoCastedTheSkill = gameObject;

        NextSkill1Time = Time.time + Skill1Cooldown;
    }
    protected override void DoSkill2()
    {
        enduring = true;
        GameObject endure = Instantiate(Endure, transform.position + Vector3.up * (SR.size.y / 2), Quaternion.identity);
        endure.GetComponent<AttackBase>().PlayerWhoCastedTheSkill = gameObject;

        NextSkill2Time = Time.time + Skill2Cooldown;
    }
    protected override void DoSkill3()
    {
        string enemyLayer = "Player1";
        if (gameObject.tag == enemyLayer)
            enemyLayer = "Player2";
        GameObject enemyGameObj = GameObject.FindGameObjectWithTag(enemyLayer);

        StolenSkill = enemyGameObj.GetComponent<CharacterBase>().SkillList.PickRandom();

        GameObject stolenSkill = Instantiate(StolenSkill, StolenSkill.transform.position, Quaternion.identity);
        stolenSkill.GetComponent<AttackBase>().PlayerWhoCastedTheSkill = gameObject;
        stolenSkill.GetComponent<AttackBase>().damage *= .5f; // decrease dmg by 50%

        NextSkill3Time = Time.time + Skill3Cooldown;
    }
    protected override void DoUltimate()
    {
        
        int index = UnityEngine.Random.Range(1, 2);
        AudioManager.Play("princehaul_ult_" + index);

        UltimateSkillIsRunning = true;

        tempSkill1CDVar = Skill1Cooldown;
        tempSkill2CDVar = Skill2Cooldown;
        tempSkill3CDVar = Skill3Cooldown;

        Skill1Cooldown *= CDDecreaseFactor;
        Skill2Cooldown *= CDDecreaseFactor;
        Skill3Cooldown *= CDDecreaseFactor;

        UltimateFinishTime = Time.time + UltimateSkillDuration;
    }
    protected override void DoingUltimate()
    {
        if (Time.time > UltimateFinishTime) // ult duration finished
        {
            // reset to actual CD
            Skill1Cooldown = tempSkill1CDVar;
            Skill2Cooldown = tempSkill2CDVar;
            Skill3Cooldown = tempSkill3CDVar;

            rage = 0;
            UltimateSkillIsRunning = false;
        }
        else // ult still running, decrease rage here
            rage = maxRage * ((UltimateFinishTime - Time.time) / UltimateSkillDuration);
    }
}

public static class EnumerableExtension
{
    public static T PickRandom<T>(this IEnumerable<T> source)
    {
        return source.PickRandom(1).Single();
    }

    public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
    {
        return source.Shuffle().Take(count);
    }

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
        return source.OrderBy(x => Guid.NewGuid());
    }
}

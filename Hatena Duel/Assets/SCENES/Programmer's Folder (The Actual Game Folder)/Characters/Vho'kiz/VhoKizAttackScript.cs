using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class VhoKizScript : CharacterBase
{
    // skills
    public GameObject Rocket;
    public GameObject Orb;
    public GameObject Laser;

    public override void OnHit(float dmg, float _rage)
    {
        health = Mathf.Clamp(health - dmg, 0, maxHealth);

        if (!UltimateSkillIsRunning)
            rage = Mathf.Clamp(rage + _rage, 0, maxRage);
    }

    protected override void DoSkill1()
    {
        GameObject rocket = Instantiate(Rocket, Rocket.transform.position, Quaternion.identity);
        rocket.GetComponent<AttackBase>().PlayerWhoCastedTheSkill = gameObject;

        if (UltimateSkillIsRunning)
        {
            rocket.transform.localScale *= new Vector2(2, 2);
            rocket.transform.GetChild(0).localScale = new Vector3(2, 2, 2);
        }

        NextSkill1Time = Time.time + Skill1Cooldown;
    }

    protected override void DoSkill2()
    {
        GameObject orb = Instantiate(Orb, Orb.transform.position, Quaternion.identity);
        orb.GetComponent<AttackBase>().PlayerWhoCastedTheSkill = gameObject;

        if (UltimateSkillIsRunning)
            orb.transform.localScale *= new Vector2(2, 2);

        NextSkill2Time = Time.time + Skill2Cooldown;
    }

    protected override void DoSkill3()
    {
        GameObject laser = Instantiate(Laser, Laser.transform.position, Quaternion.identity);
        laser.GetComponent<AttackBase>().PlayerWhoCastedTheSkill = gameObject;

        if (UltimateSkillIsRunning)
            laser.GetComponent<SuperNovaScript>().IncreaseLaserWidth(2);

        NextSkill3Time = Time.time + Skill3Cooldown;
    }

    protected override void DoUltimate()
    {
        AudioManager.Play("vhokiz_ult_charge");

        UltimateSkillIsRunning = true;
        UltimateFinishTime = Time.time + UltimateSkillDuration;
    }

    protected override void DoingUltimate()
    {
        if (Time.time > UltimateFinishTime) // ult duration finished
        {
            rage = 0;
            UltimateSkillIsRunning = false;
        }
        else // ult still running, decrease rage here
            rage = maxRage * ((UltimateFinishTime - Time.time) / UltimateSkillDuration);
    }
}

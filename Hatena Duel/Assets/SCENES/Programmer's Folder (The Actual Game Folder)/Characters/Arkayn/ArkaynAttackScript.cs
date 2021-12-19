using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ArkaynScript : CharacterBase
{
    public GameObject HellsFlames;
    public GameObject HeavensGate;
    public GameObject AcidRain;
    public GameObject DivineIntervention;

    // ult extra variables
    float healingRate = 0.3f;
    float nextHealTime;
    float healAmount = 9f;

    public override void OnHit(float dmg, float _rage)
    {
        health = Mathf.Clamp(health - dmg, 0, maxHealth);
        rage = Mathf.Clamp(rage + _rage, 0, maxRage);

        
    }

    protected override void DoSkill1()
    {
        GameObject hellsFlames = Instantiate(HellsFlames, HellsFlames.transform.position, Quaternion.identity);
        hellsFlames.GetComponent<AttackBase>().PlayerWhoCastedTheSkill = gameObject;

        NextSkill1Time = Time.time + Skill1Cooldown;
    }

    protected override void DoSkill2()
    {
        GameObject heavensGate = Instantiate(HeavensGate, HeavensGate.transform.position, Quaternion.identity);
        heavensGate.GetComponent<AttackBase>().PlayerWhoCastedTheSkill = gameObject;

        NextSkill2Time = Time.time + Skill2Cooldown;
    }

    protected override void DoSkill3()
    {
        GameObject acidRain = Instantiate(AcidRain, AcidRain.transform.position, Quaternion.identity);
        acidRain.GetComponent<AttackBase>().PlayerWhoCastedTheSkill = gameObject;

        NextSkill3Time = Time.time + Skill3Cooldown;
    }

    protected override void DoUltimate()
    {
        AudioManager.Play("arkayn_ult_charge");

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
        else // ult still running, decrease rage here, and heal here as well
        {
            rage = maxRage * ((UltimateFinishTime - Time.time) / UltimateSkillDuration);

            // if arkayn dies while on ult, revive at half health and 0 rage
            if (UltimateSkillIsRunning && health == 0)
            {
                GameObject divineIntervention = Instantiate(DivineIntervention, transform.position + Vector3.up * (SR.size.y / 2), Quaternion.identity);
                rage = 0;
                health = maxHealth / 2;
                UltimateSkillIsRunning = false;
            }
            else if (Time.time > nextHealTime)
            {
                nextHealTime = Time.time + healingRate;
                health += healAmount;
            }
        }
    }
}

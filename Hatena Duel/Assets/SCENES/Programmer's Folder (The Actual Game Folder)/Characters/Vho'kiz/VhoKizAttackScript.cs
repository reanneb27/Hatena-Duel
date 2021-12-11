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

    protected override void Attack()
    {
        // ROCKET
        if (Input.GetKey(PlayerSettings.Skill1Key) && Time.time > NextSkill1Time)
        {
            GameObject rocket = Instantiate(Rocket, transform.position + Vector3.up * (SR.size.y / 2), Quaternion.identity);
            rocket.GetComponent<ConsciousMissiles>().playerWhoLaunchedTheRocket = gameObject;

            NextSkill1Time = Time.time + Skill1Cooldown;
        }
        // ORB
        if (Input.GetKey(PlayerSettings.Skill2Key) && Time.time > NextSkill2Time)
        {
            GameObject orb = Instantiate(Orb, transform.position + Vector3.up * (SR.size.y / 2), Quaternion.identity);
            orb.GetComponent<MagneticStarScript>().playerWhoCreatedTheOrb = gameObject;

            NextSkill2Time = Time.time + Skill2Cooldown;
        }
        // LASER
        if (Input.GetKey(PlayerSettings.Skill3Key) && Time.time > NextSkill3Time)
        {
            GameObject laser = Instantiate(Laser, transform.position + Vector3.up * (SR.size.y / 2), Quaternion.identity);
            laser.GetComponent<SuperNovaScript>().playerWhoShotTheSupernova = gameObject;

            NextSkill3Time = Time.time + Skill3Cooldown;
        }
        if (Input.GetKey(PlayerSettings.UltimateAttKey) && !UltimateSkillIsRunning)
        {

        }
    }
}

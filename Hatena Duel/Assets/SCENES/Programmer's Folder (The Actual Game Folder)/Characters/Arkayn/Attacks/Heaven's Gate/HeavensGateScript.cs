using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HeavensGateScript : AttackBase
{
    public SpriteRenderer SR;
    public Material GlowMaterial;
    public Volume Volume;

    public float GlowEntranceDuration = 2f;
    public Color MaterialColor = new Color(191, 159, 64, 255);
    public Color SpriteColor = new Color(118, 117, 98, 255);
    public float StartingMaterialIntensity = 20;
    public float EndingMaterialIntensity = 5f;

    public float StartingVolumeIntensity = 5f;
    public float EndingVolumeIntensity = 0.1f;

    public float DisappearDuration = 4f;

    // players info
    CharacterBase player;
    string enemyLayer = "Player1";
    GameObject enemyGameObj;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager = FindObjectOfType<ArenaAudioManagerScript>();

        player = PlayerWhoCastedTheSkill.gameObject.GetComponent<CharacterBase>();
        if (PlayerWhoCastedTheSkill.gameObject.tag == "Player1")
            enemyLayer = "Player2";
        enemyGameObj = GameObject.FindGameObjectWithTag(enemyLayer);
        SR = gameObject.GetComponent<SpriteRenderer>();
        SR.flipX = player.GetComponent<SpriteRenderer>().flipX;

        // set position
        transform.position = new Vector3(player.transform.position.x, transform.position.y) + Vector3.right * 3f * (SR.flipX ? -1 : 1);

        // set starting intensities here
        GlowMaterial.SetColor("_Color", MaterialColor * StartingMaterialIntensity * 5);

        StartCoroutine(HeavensGateProgression());
    }

    IEnumerator HeavensGateProgression()
    {
        AudioManager.Play("arkayn_s2_gate");

        float glowDuration = Time.time + GlowEntranceDuration;
        while (Time.time < glowDuration)
        {
            float interpolator = 1 - ((glowDuration - Time.time) / GlowEntranceDuration);
            float lerpIntensity = Mathf.Lerp(StartingMaterialIntensity, EndingMaterialIntensity, easeOutCubic(interpolator));
            GlowMaterial.SetColor("_Color", SetCustomMaterialEmissionIntensity(MaterialColor, lerpIntensity));
            yield return null;
        }

        float disappearDuration = Time.time + DisappearDuration;
        while (Time.time < disappearDuration)
        {
            float interpolator = (disappearDuration - Time.time) / DisappearDuration;
            float lerpAlpha = Mathf.Lerp(255, 0, interpolator);
            SR.color = new Color(SR.color.r, SR.color.g, SR.color.b, interpolator);
            yield return null;
        }

        Destroy(gameObject);
    }

    private Color SetCustomMaterialEmissionIntensity(Color color, float intensity)
    {
        // for some reason, the desired intensity value (set in the UI slider) needs to be modified slightly for proper internal consumption
        float adjustedIntensity = intensity - (0.4169F);

        // redefine the color with intensity factored in - this should result in the UI slider matching the desired value
        color *= Mathf.Pow(2.0F, adjustedIntensity);
        return color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    float easeOutCubic(float x)
    {
        return 1f - (float)Mathf.Pow(1 - x, 3);
    }
}

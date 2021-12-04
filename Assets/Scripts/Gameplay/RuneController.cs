using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RuneController : MonoBehaviour
{

    private List<float> P1DelayedDamage;
    private List<float> P2DelayedDamage;
    private List<float>[] DelayedDamageArray;

    public List<int> P1RuneSequence;
    public List<int> P2RuneSequence;

    private List<int> StatusIndicatorPoison;
    private List<int> StatusIndicatorShock;
    private List<int> StatusIndicatorHeal;

    public GameObject[] RuneIcon1;
    public GameObject[] RuneIcon2;

    public Texture slash;
    public Texture fist;
    public Texture heal;
    public Texture quick;
    public Texture poison;
    private Texture[] texturelist;


    // public GameManager.Player currPlayer { get; private set; }
    GameManager.Player P1 = GameManager.Player.P1;
    GameManager.Player P2 = GameManager.Player.P2;
    // for player checking with Caster

    public GameObject player1GameObject;
    public GameObject player2GameObject;
    // for dealing effect damage

    private Health health1;
    private Health health2;

    private void Start()
    {
        health1 = this.player1GameObject.GetComponent<Health>();
        health2 = this.player2GameObject.GetComponent<Health>();

        this.P1DelayedDamage = new List<float>() {0, 0, 0, 0, 0, 0};
        this.P2DelayedDamage = new List<float>() {0, 0, 0, 0, 0, 0};
        // list of damage amounts for next 6 turns
        DelayedDamageArray = new List<float>[2];
        DelayedDamageArray[0] = P1DelayedDamage;
        DelayedDamageArray[1] = P2DelayedDamage;
        
        StatusIndicatorPoison = new List<int>() {0, 0};
        StatusIndicatorShock = new List<int>() {0, 0};
        StatusIndicatorHeal = new List<int>() {0, 0};

        this.P1RuneSequence = new List<int>();
        this.P2RuneSequence = new List<int>();
        P1RuneSequence.Add(0);
        P2RuneSequence.Add(0);

        texturelist = new Texture[6] {fist, poison, quick, heal, fist, fist};

        // generate random rune sequence
        for(int i = 0; i < 5; i++) {
            P1RuneSequence.Add(Random.Range(0, 5));
            P2RuneSequence.Add(Random.Range(0, 5));
        }

        RuneIcon1[0].GetComponent<RawImage>().texture = texturelist[P1RuneSequence[0]];
        RuneIcon1[1].GetComponent<RawImage>().texture = texturelist[P1RuneSequence[1]];
        RuneIcon1[2].GetComponent<RawImage>().texture = texturelist[P1RuneSequence[2]];
        RuneIcon2[0].GetComponent<RawImage>().texture = texturelist[P2RuneSequence[0]];
        RuneIcon2[1].GetComponent<RawImage>().texture = texturelist[P2RuneSequence[1]];
        RuneIcon2[2].GetComponent<RawImage>().texture = texturelist[P2RuneSequence[2]];
    }

    // for updating visual rune indicator icons
    public IEnumerator UpdateRuneIcons(int player) {
        yield return new WaitForSeconds(0.2F);
        if (player == 0) {
            for (float pixels = 50; pixels > 25; pixels -= 5) {
                RuneIcon1[0].GetComponent<RectTransform>().sizeDelta = new Vector2(pixels, pixels);
                yield return new WaitForSeconds(0.005F);
            }
            for (float pixels = 26; pixels < 75; pixels += 5) {
                RuneIcon1[0].GetComponent<RectTransform>().sizeDelta = new Vector2(pixels, pixels);
                yield return new WaitForSeconds(0.005F);
            }
            RuneIcon1[0].GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
            RuneIcon1[0].GetComponent<RawImage>().texture = texturelist[P1RuneSequence[0]];
            RuneIcon1[1].GetComponent<RawImage>().texture = texturelist[P1RuneSequence[1]];
            RuneIcon1[2].GetComponent<RawImage>().texture = texturelist[P1RuneSequence[2]];
        }
        if (player == 1) {
            for (float pixels = 50; pixels > 25; pixels -= 5) {
                RuneIcon2[0].GetComponent<RectTransform>().sizeDelta = new Vector2(pixels, pixels);
                yield return new WaitForSeconds(0.005F);
            }
            for (float pixels = 26; pixels < 75; pixels += 5) {
                RuneIcon2[0].GetComponent<RectTransform>().sizeDelta = new Vector2(pixels, pixels);
                yield return new WaitForSeconds(0.005F);
            }
            RuneIcon2[0].GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
            RuneIcon2[0].GetComponent<RawImage>().texture = texturelist[P2RuneSequence[0]];
            RuneIcon2[1].GetComponent<RawImage>().texture = texturelist[P2RuneSequence[1]];
            RuneIcon2[2].GetComponent<RawImage>().texture = texturelist[P2RuneSequence[2]];
        }

        if (StatusIndicatorPoison[0] > 0) {
            RuneIcon1[3].SetActive(true);
            StatusIndicatorPoison[0] -= 1;
        }
        else {
            RuneIcon1[3].SetActive(false);
        }
        if (StatusIndicatorPoison[1] > 0) {
            RuneIcon2[3].SetActive(true);
            StatusIndicatorPoison[0] -= 1;
        }
        else {
            RuneIcon2[3].SetActive(false);
        }

        if (StatusIndicatorShock[0] > 0) {
            RuneIcon1[4].SetActive(true);
            StatusIndicatorPoison[0] -= 1;
        }
        else {
            RuneIcon1[4].SetActive(false);
        }
        if (StatusIndicatorShock[1] > 0) {
            RuneIcon2[4].SetActive(true);
            StatusIndicatorPoison[1] -= 1;
        }
        else {
            RuneIcon2[4].SetActive(false);
        }

        if (StatusIndicatorHeal[0] > 0) {
            RuneIcon1[5].SetActive(true);
            StatusIndicatorHeal[0] -= 1;
        }
        else {
            RuneIcon1[5].SetActive(false);
        }
        if (StatusIndicatorHeal[1] > 0) {
            RuneIcon2[5].SetActive(true);
            StatusIndicatorHeal[1] -= 1;
        }
        else {
            RuneIcon2[5].SetActive(false);
        }


    }

    public void SoundEffectController(int player, float healthDelta, int effect) {

        FMOD.Studio.EventInstance HealthChange;
        HealthChange = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/Battle/HealthChange");
        //HealthChange.setParameterByName("healthDelta", healthDelta);
        HealthChange.setParameterByName("healthDelta", -1);
        HealthChange.setParameterByName("player", player + 1);
        HealthChange.setParameterByName("effect", effect);
        HealthChange.start();
        HealthChange.release();
    }

    // for dealing damage in future turns
    // Note: DelayedDamage[0] will be called immediately this turn
    public IEnumerator inflictDelayedDamage(int caster, int effect1)
    {
        StartCoroutine(UpdateRuneIcons(caster));
        if (caster == 0) {

            float damage1 = P2DelayedDamage[0];
            health2.BumpHp(damage1);
            P2DelayedDamage.RemoveAt(0);
            P2DelayedDamage.Add(0);
            if (damage1 != 0) {
                SoundEffectController(caster, damage1, effect1);
                yield return new WaitForSeconds(0.6F);
            }
            float damage2 = P1DelayedDamage[0];
            health1.BumpHp(damage2);
            P1DelayedDamage.RemoveAt(0);
            P1DelayedDamage.Add(0);
            if (damage2 != 0) {
                int effect2 = 0;
                if (damage2 < 0) 
                    effect2 = 1;
                else
                    effect2 = 3;
                
                SoundEffectController(caster, damage2, effect2);
                yield return new WaitForSeconds(0.2F);
            }
        }
        else if (caster == 1) {

            float damage1 = P1DelayedDamage[0];
            health1.BumpHp(damage1);
            P1DelayedDamage.RemoveAt(0);
            P1DelayedDamage.Add(0);
            if (damage1 != 0) {
                SoundEffectController(caster, damage1, effect1);
                yield return new WaitForSeconds(0.6F);
            }
            float damage2 = P2DelayedDamage[0];
            health2.BumpHp(damage2);
            P2DelayedDamage.RemoveAt(0);
            P2DelayedDamage.Add(0);
            if (damage2 != 0) {
                int effect2 = 0;
                if (damage2 < 0) 
                    effect2 = 1;
                else
                    effect2 = 3;
                
                SoundEffectController(caster, damage2, effect2);
                yield return new WaitForSeconds(0.2F);
            }
        }

    }

    // call every turn
    public int Turn(GameManager.Player Caster, int wordDmgAmt)
    {
        int player = -1;
        if (Caster == P1) {
            player = 0;
        }
        else if (Caster == P2) {
            player = 1;
        } else {
            Debug.Log("Error in player number");
            return -1;
        }

        int attackType = Effect(player, wordDmgAmt);
        StartCoroutine(inflictDelayedDamage(player, attackType));

        return attackType;
    }

    public int Effect(int caster, int wordDmgAmt) {
        int opponent = (caster + 1) % 2;
        int effect = 0;
        if (caster == 0) {
            effect = P1RuneSequence[0];
            P1RuneSequence.RemoveAt(0);
            P1RuneSequence.Add(Random.Range(0, 4));
        }
        else if (caster == 1) {
            effect = P2RuneSequence[0];
            P2RuneSequence.RemoveAt(0);
            P2RuneSequence.Add(Random.Range(0, 4));
        }
        Debug.Log("Effect number:" + effect.ToString());
        if (effect == 1) {
            // damage over time attack: configurable
            float multiplier = 1.5F;
            int turns = 4;

            for (int i = 0; i < turns; i++) {
                DelayedDamageArray[opponent][i] -= wordDmgAmt * multiplier / turns;
            }
            StatusIndicatorPoison[opponent] = 6;
        }
        else if (effect == 2) {
            // fast attack: configurable
            float multiplier = 1.75F;
            float penalty = 0.0F; // from original damage amt
            
            DelayedDamageArray[opponent][0] -= wordDmgAmt * multiplier;
            DelayedDamageArray[opponent][1] += wordDmgAmt * (multiplier - 1 + penalty);

            StatusIndicatorShock[opponent] = 1;
        }
        else if (effect == 3) {
            // heal: configurable
            float multiplier = 1.0F;

            DelayedDamageArray[caster][0] += wordDmgAmt * multiplier;

            StatusIndicatorHeal[caster] = 1;

        } else {
            // normal attack
            float multiplier = 1.0F;
            DelayedDamageArray[opponent][0] -= wordDmgAmt * multiplier;
        }
        return effect;
    }
}

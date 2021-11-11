using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneController : MonoBehaviour
{
    private List<int> P1DelayedDamage = new List<int>() {0, 0, 0, 0, 0, 0};
    private List<int> P2DelayedDamage = new List<int>() {0, 0, 0, 0, 0, 0};
    // list of damage amounts for next 6 turns

    GameObject P1 = GameManager.Player.P1;
    GameObject P2 = GameManager.Player.P2;
    // for player checking with Caster

    public GameObject player1GameObject;
    public GameObject player2GameObject;
    Health health1 = player1GameObject.GetComponent<Health>();
    Health health2 = player2GameObject.GetComponent<Health>();
    // for dealing effect damage

    public List<int> P1RuneSequence = new List<int>();
    public List<int> P2RuneSequence = new List<int>();

    private Start()
    {
        // generate random rune sequence
        for(int i = 0; i < 6; i++) {
            P1RuneSequence.Add(Random.Range(0, 4));
            P2RuneSequence.Add(Random.Range(0, 4));
        }

    }


    // for dealing damage in future turns
    // Note: DelayedDamage[0] will be called immediately this turn
    public void inflictDelayedDamage()
    {

        health1.BumpHp(P1DelayedDamage[0]);
        P1DelayedDamage.RemoveAt(0);
        P1DelayedDamage.Add(0);

        health2.BumpHp(P2DelayedDamage[0]);
        P2DelayedDamage.RemoveAt(0);
        P2DelayedDamage.Add(0);

    }

    // call every turn
    public void Turn(GameObject Caster)
    {
        Effect(Caster);
        inflictDelayedDamage();
    }

    public int Effect(GameObject Caster) {
        int effect = 0;
        if (Caster == P1) {
            effect = P1RuneSequence[0];
            P1RuneSequence.RemoveAt(0);
            P1RuneSequence.Add(Random.Range(0, 4));
        }
        else if (Caster == P2) {
            effect = P1RuneSequence[0];
            P2RuneSequence.RemoveAt(0);
            P2RuneSequence.Add(Random.Range(0, 4));
        }

        if (effect == 1) {
            // damage over time attack

        }
        else if (effect == 2) {
            // fast attack
            
        }
        else if (effect == 3) {
            // heal

        } else {
            // normal attack
            Debug.Log("Nothing Happened");
        }
        return effect;
    }
}

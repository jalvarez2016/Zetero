using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    public AudioSource Attack;
    public AudioSource Hit;
    public AudioSource Complete;
    public AudioSource Pickup;
    public AudioSource Jump;

    public void AttackFX(){
        Attack.Play();
    }

    public void HitFX(){
        Hit.Play();
    }

    public void CompleteFX(){
        Complete.Play();
    }

    public void PickupFX(){
        Pickup.Play();
    }

    public void JumpFX(){
        Jump.Play();
    }
}
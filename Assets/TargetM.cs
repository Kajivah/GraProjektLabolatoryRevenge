using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetM : MonoBehaviour
{
    public float health = 50f;
    public AudioSource oof;
    /*    public ParticleSystem bloodS;*/
    public GameObject blood;

    void Awake()
    {
        blood.SetActive(false);


    }
    public void TakeDamage(float amount)
    {
        blood.SetActive(false);
        health -= amount;
        if(health <= 0f)
        {
            StartCoroutine(Die());
        }
    }
    IEnumerator Die()
    {
        blood.SetActive(true);
        /*        bloodS.Play();*/
        oof.Play();


        while(oof.isPlaying)
            yield return null;

        Destroy(this.gameObject);
    }

}

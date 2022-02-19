using UnityEngine;
using System.Collections;

public class TargetC : MonoBehaviour
{
    public float healthC = 50f;
    public AudioSource oofC;
    public GameObject bloodC;
    /*    public GameObject Ciulik;*/

    void Awake()
    {
        bloodC.SetActive(false);


    }

/*    void Update()
    {
        
    }*/
    public void TakeDamage(float amount)
    {
        bloodC.SetActive(false);
        healthC -= amount;
        if(healthC <= 0f)
        {
            Die();
        }
    }
    IEnumerator Die()
    {
/*        blood.SetActive(true);
*//*        bloodS.Play();*/
        oofC.Play();
        

        while(oofC.isPlaying)
            yield return null;

        /*        Destroy(Ciulik);*/
        Destroy(this.gameObject);
    }

}

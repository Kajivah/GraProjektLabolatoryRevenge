
using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;
    public float impactForce = 30f;

    public int maxAmmo = 10;
    private int currentAmmo;
    public float reloadTime = 1f;
    private bool isReloading = false;

    public Camera fpscamera;
    public ParticleSystem muzzleflash;
    public GameObject impactEffect;

    public GameObject reloadinSound;

    private float nextTimeToFire = 0f;

    public Animator animator;

    public AudioSource shootingSound;




    void Start()
    {
        shootingSound = GetComponent<AudioSource>();
        

        if(currentAmmo == -1)
        {
            currentAmmo = maxAmmo;
        }
            
    }

    void OnEnable()
    {
        isReloading = false;
        animator.SetBool("Reloading" , false);
    }


    // Update is called once per frame
    void Update()
    {

        if(isReloading)
        {
            return;
        }

        if(currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if(Input.GetKey(KeyCode.Mouse0) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }

    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        reloadinSound.GetComponent<AudioSource>().Play();
        animator.SetBool("Reloading" , true);

        yield return new WaitForSeconds(reloadTime - .25f);
        animator.SetBool("Reloading" , false);
        yield return new WaitForSeconds(.25f);

        currentAmmo = maxAmmo;
        isReloading = false;
    }


    void Shoot()
    {
        muzzleflash.Play();

        currentAmmo--;

        RaycastHit hit;
        if(Physics.Raycast(fpscamera.transform.position , fpscamera.transform.forward , out hit , range))
        {
            UnityEngine.Debug.Log(hit.transform.name);

            shootingSound.Play();

            Target target = hit.transform.GetComponent<Target>();
            if(target != null)
            {
                target.TakeDamage(damage);
            }

            if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            GameObject impactGO = Instantiate(impactEffect , hit.point , Quaternion.LookRotation(hit.normal));
            Destroy(impactGO , 2f);
        }

    }
}




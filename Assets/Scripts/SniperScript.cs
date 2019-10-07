using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SniperScript : MonoBehaviour
{
    public GameObject projectile;
    public Transform startPos;
    public float bulletForce;
    public int bulletCount;
    public Text Ammo;

    private void Start()
    {
        Ammo.text = bulletCount.ToString();
    }

    private void FixedUpdate()
    {
        Ammo.text = bulletCount.ToString();
        if (bulletCount > 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                bulletCount--;
                Ammo.text = bulletCount.ToString();
                GameObject bullet = Instantiate(projectile, startPos.position, transform.rotation);
                bullet.GetComponent<Rigidbody>().AddForce(transform.parent.transform.forward * bulletForce, ForceMode.Impulse);
            }
        }

        if(bulletCount <= 0)
        {
            PlayerScript.hasSniper = false;
        }
    }
}

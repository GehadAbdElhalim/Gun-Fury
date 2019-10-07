using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public AudioClip hurt;
    public AudioClip explosionAudio;
    private AudioSource a1;

    public float moveSpeed;

    public float dashAmount;
    public float immunityAfterDash;
    public float cooldown;
    public float currentCooldown = 0;

    public float Health = 100;

    public Image HealthBar;

    public GameObject Ammo;

    public GameObject AmmoText;

    public Text Score;

    public static int score;

    public GameObject nothing;

    public GameObject Sniper;

    public GameObject MiniGun;

    private Rigidbody rb;

    private TrailRenderer trail;

    private Animator anim;

    private bool attacking;

    private bool dashing;

    private Vector3 attackDirection;

    public float attackSpeed;

    public float attackTime;

    private float currentAttackTime;

    public GameObject particleSystem;

    [SerializeField]
    public static bool hasSniper = false;
    [SerializeField]
    public static bool hasMiniGun = false;

    private void Start()
    {
        score = 0;
        Score.text = score.ToString();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        trail = GetComponent<TrailRenderer>();
        trail.emitting = false;
        hasMiniGun = false;
        hasSniper = false;
        attacking = false;
        dashing = false;
        a1 = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Health <= 0)
        {
            UIScript.PlayerDead = true;
            UIScript.GameIsPaused = true;
        }
        else
        {

            HealthBar.fillAmount = Health / 100;

            if (currentCooldown > 0)
            {
                currentCooldown -= Time.deltaTime;
            }
            else
            {
                currentCooldown = 0;
            }

            if (Input.GetKeyDown(KeyCode.Space) && !attacking)
            {
                Dash();
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                trail.emitting = false;
            }

            if (Input.GetMouseButtonDown(1) && !hasSniper && !hasMiniGun && !attacking)
            {
                Attack();
            }

            Ammo.SetActive(hasSniper || hasMiniGun);
            AmmoText.SetActive(hasSniper || hasMiniGun);

            nothing.SetActive(!hasSniper && !hasMiniGun);
            Sniper.SetActive(hasSniper);
            MiniGun.SetActive(hasMiniGun);

            if (attacking)
            {
                if (currentAttackTime > 0)
                {
                    currentAttackTime -= Time.deltaTime;
                    transform.Translate(attackDirection * attackSpeed * Time.deltaTime, Space.World);
                    transform.RotateAroundLocal(transform.up, 10);
                }

                if (currentAttackTime <= 0)
                {
                    attacking = false;
                    trail.emitting = false;
                }
            }

            if (!attacking)
            {
                transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) * moveSpeed * Time.deltaTime, Space.World);

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    Vector3 distance_vector = this.transform.position - hit.point;
                    if (distance_vector.magnitude > 1)
                    {
                        this.transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
                    }
                }
            }
        }
    }

    void Dash()
    {
        trail.emitting = true;

        RaycastHit hit;
        if(!Physics.Raycast(transform.position + new Vector3(0,0.5f,0) , new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")), (dashAmount+20) * Time.deltaTime))
        {
            transform.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) * dashAmount * Time.deltaTime;
        }
        dashing = true;
        Invoke("FalsifyDash", immunityAfterDash);
    }

    void Attack()
    {
        trail.emitting = true;
        attacking = true;
        currentAttackTime = attackTime;
        attackDirection = transform.forward;
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.velocity = Vector3.zero;
        if(collision.gameObject.tag == "EnemySniper" && attacking)
        {
            if (!hasSniper && !hasMiniGun)
            {
                collision.gameObject.GetComponent<Collider>().enabled = false;
                Sniper.GetComponent<SniperScript>().bulletCount = 5;
                hasSniper = true;
            }
            Destroy(collision.gameObject);
            Instantiate(particleSystem, transform.position, Quaternion.identity);
            a1.PlayOneShot(explosionAudio);
            score += 1;
            Score.text = score.ToString();
            if (EnemySpawnSystem.currentNumberOfEnemies > 0)
            {
                EnemySpawnSystem.currentNumberOfEnemies -= 1;
            }
        }

        if (collision.gameObject.tag == "EnemyMiniGun" && attacking)
        {
            if (!hasSniper && !hasMiniGun)
            {
                collision.gameObject.GetComponent<Collider>().enabled = false;
                MiniGun.GetComponent<MiniGunScript>().bulletCount = 30;
                hasMiniGun = true;
            }
            Destroy(collision.gameObject);
            Instantiate(particleSystem, transform.position, Quaternion.identity);
            a1.PlayOneShot(explosionAudio);
            score += 1;
            Score.text = score.ToString();
            if (EnemySpawnSystem.currentNumberOfEnemies > 0)
            {
                EnemySpawnSystem.currentNumberOfEnemies -= 1;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "EnemyBullet" && !dashing && !attacking)
        {
            Health = Health - 10;
            a1.PlayOneShot(hurt);
            Destroy(other.gameObject);
        }
    }

    void FalsifyDash()
    {
        dashing = false;
    }
}

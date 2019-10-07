using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySniperScript : MonoBehaviour
{
    public AudioClip explosionAudio;
    private AudioSource a1;
    public GameObject Player;
    public GameObject projectile;
    public Transform startPos;
    public float bulletForce;
    public float moveSpeed;
    private int state = 0;
    public float DelayBeforeLaser;
    public float DelayWithLaser;
    private float currentDelay;
    private LineRenderer line;
    public GameObject particleSystem;
    private bool alreadyDead = false;

    // Start is called before the first frame update
    void Start()
    {
        a1 = GetComponent<AudioSource>();
        Player = GameObject.FindGameObjectWithTag("Player");
        currentDelay = DelayBeforeLaser;
        line = GetComponent<LineRenderer>();
        line.SetVertexCount(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(state == 0)
        {
            if(currentDelay > 0)
            {
                transform.LookAt(new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z));
                transform.Translate(transform.forward * moveSpeed * Time.deltaTime,Space.World);
                currentDelay -= Time.deltaTime;
            }

            if(currentDelay <= 0)
            {
                currentDelay = DelayWithLaser;
                state = 1;
            }
        }

        if(state == 1)
        {
            if(currentDelay > 0)
            {
                transform.LookAt(new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z));
                currentDelay -= Time.deltaTime;
                Vector3[] positions = new Vector3[2];
                positions[0] = transform.position;
                positions[1] = Player.transform.position;
                line.SetVertexCount(2);
                line.SetPositions(positions);
                line.widthMultiplier = 0.1f;
            }

            if(currentDelay <= 0)
            {
                line.SetVertexCount(0);
                currentDelay = DelayBeforeLaser;
                state = 2;
            }
        }

        if(state == 2)
        {
            GameObject bullet = Instantiate(projectile, startPos.position, transform.rotation);
            bullet.GetComponent<Rigidbody>().AddForce(transform.forward * bulletForce, ForceMode.Impulse);
            state = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!alreadyDead)
        {
            if (other.tag == "bullet")
            {
                GetComponent<Collider>().enabled = false;
                alreadyDead = true;
                PlayerScript.score += 1;
                Player.GetComponent<PlayerScript>().Score.text = PlayerScript.score.ToString();
                if (EnemySpawnSystem.currentNumberOfEnemies > 0)
                {
                    EnemySpawnSystem.currentNumberOfEnemies--;
                }
                Instantiate(particleSystem, transform.position, Quaternion.identity);
                a1.PlayOneShot(explosionAudio);
                Destroy(gameObject, 0.1f);
            }
        }
    }
}

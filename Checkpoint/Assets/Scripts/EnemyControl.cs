using System.Collections;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{

    [SerializeField] private GameObject player;
    [SerializeField] private float distancewake;
    [SerializeField] private float distanceattack;
    [SerializeField] private float speed;
    [SerializeField] private int life;
    [SerializeField] private Vector3 initial;


    private Animator anim;
    private SpriteRenderer sr;

    bool attacking;
    bool wake;

    public enum State
    {
        Idle,
        Wake,
        Attack,
        Move,
        Death
        
    }

    public State current;

    void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     current = State.Idle;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

        if (current != State.Wake)
        {
            if (Vector2.Distance(transform.position, player.transform.position) < distancewake)
            {
               current = State.Wake;
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, player.transform.position) < distanceattack)
            {
                current = State.Attack;
            }
            else if (Vector2.Distance(transform.position, player.transform.position) > distancewake)
            {
                current = State.Move;
            }
        }
        Animation();
        States();
    }

    void States()
    {
        if (current == State.Wake)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
          

            if (player.transform.position.x > transform.position.x)
            {
                sr.flipX = false;
            }
            else if (player.transform.position.x < transform.position.x)
            {
                sr.flipX = true;
            }
        }

        if (current == State.Move)
        {
            if (transform.position != initial && current != State.Wake && current != State.Idle)
            {
                sr.flipX = true;
                transform.position = Vector3.MoveTowards(transform.position, initial, speed * Time.deltaTime);
            }
            else
            {
                current = State.Idle;
                sr.flipX = false;
            }
        }

        if (current == State.Attack)
        {
            StartCoroutine(Attacking());
        }

        if (current == State.Move || current == State.Wake)
        {
            wake = true;
        }
        else if (current == State.Idle)
        {
            wake = false;
        }
    }

    IEnumerator Attacking()
    {
        attacking = true;
        yield return new WaitForSeconds(0.5f);
        attacking = false;
    }

    void Animation()
    {
        anim.SetBool("pwake", wake);
        anim.SetBool("pattack", attacking);

    }

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.tag == "damage")
        {
           
                StartCoroutine(Damage());
        }
    }

    IEnumerator Damage()
    {
        life--;

        if (life > 0)
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(0.25f);
            sr.color = Color.white;
            yield return new WaitForSeconds(0.25f);
        }
        else
        {
            yield return new WaitForSeconds(0.4f);
            Destroy(gameObject);
        }
    }
}

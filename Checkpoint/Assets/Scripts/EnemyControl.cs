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

    public enum State
    {
        Idle,
        Wake,
        Attack,
        Move
        
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
    }

    IEnumerator Attacking()
    {
        attacking = true;
        yield return new WaitForSeconds(1.5f);
        current = State.Idle;
        attacking = false;
        yield return new WaitForSeconds(3.0f);
    }

    void Animation()
    {

    }
}

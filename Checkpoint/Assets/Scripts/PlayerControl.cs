using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject areadano;
    private Collider2D colliderdano;

    private Rigidbody2D rb;
    private Vector2 direction;
    private bool attack;

  

    void Awake()
    { 
        rb = GetComponent<Rigidbody2D>();
       colliderdano = areadano.GetComponent<Collider2D>();
        colliderdano.enabled = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * speed;

        
    }

    void FixedUpdate()
    {
        rb.linearVelocity = direction;
    }


    void Attack()
    {
        attack = Input.GetButtonDown("Fire1");

        if (attack)
        {
            StartCoroutine(OnAttack());
        }
    }


    IEnumerator OnAttack()
    {
        yield return new WaitForSeconds(0.3f);
        colliderdano.enabled = true;
        yield return new WaitForSeconds(1f);
        colliderdano.enabled = false;
    }
}

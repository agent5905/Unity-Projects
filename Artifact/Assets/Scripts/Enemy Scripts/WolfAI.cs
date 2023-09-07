using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAI : MonoBehaviour
{
    [SerializeField]
    private bool isEater;
    [SerializeField]
    private float moveSpeed = 1f;
    [SerializeField]
    private int attackDamage = 5;
    [SerializeField]
    private float attackTimeThreshold = 1f;
    [SerializeField]
    private float eatTimeThreshold = 2f;
    [SerializeField]
    private LayerMask bushMask;
    [HideInInspector]
    public bool isMoving, left;
    private Artifact artifact;
    private BushFruits bushFruitsTarget;
    private float attackTimer;
    private float eatTimer;
    private bool killingBush;
    private bool isAttacking;

    // Start is called before the first frame update
    void Start()
    {
        if(isEater)
        {
            killingBush = false;
        }
        else
        {
            isAttacking = false;
        }

        artifact = GameObject.FindWithTag("Artifact").GetComponent<Artifact>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!artifact)
            return;

        if(isEater)
        {
            if(bushFruitsTarget && bushFruitsTarget.enabled && bushFruitsTarget.HasFruits() && !killingBush)
            {
                //If not close to the bush walk towards it.
                if(Vector2.Distance(transform.position, bushFruitsTarget.transform.position) > 0.5f)
                {
                    float step = moveSpeed * Time.deltaTime;
                    transform.position = Vector2.MoveTowards(transform.position, bushFruitsTarget.transform.position, step);
                    isMoving = true;
                }
                else
                {
                    isMoving = false;
                    bushFruitsTarget.HarvestFruits();
                    eatTimer = Time.time + eatTimeThreshold;
                    killingBush = true;
                }
            }
            else if (killingBush)
            {
                if(Time.time > eatTimer)
                {
                    bushFruitsTarget.EatBushFruits();
                    killingBush = false;

                    SearchForTarget();
                }
            }
            else
            {
                SearchForTarget();
            }

            if(bushFruitsTarget)
            {
                if(bushFruitsTarget.transform.position.x < transform.position.x)
                    left = true;
                else
                    left = false;
            }
            

            if(!bushFruitsTarget)
                SearchForTarget();
        }
        else
        {
            //Wolf that destroys artifact

            if(Vector2.Distance(transform.position, artifact.transform.position) > 1.5f)
            {
                float step = moveSpeed * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, artifact.transform.position, step);
                isMoving = true;
            }
            else if (!isAttacking)
            {
                isAttacking = true;
                attackTimer = Time.time + attackTimeThreshold;
                isMoving = false;
            }

            if(isAttacking)
            {
                if(Time.time > attackTimer)
                {
                    Attack();
                    attackTimer = Time.time + attackTimeThreshold;
                }
            }

            if(artifact.transform.position.x < transform.position.x)
                left = true;
            else
                left = false;
        }
    }

    //Search for the target
    void SearchForTarget()
    {
        bushFruitsTarget = null;
        Collider2D[] hits;

        for(int i = 1; i < 50; i++)
        {
            hits = Physics2D.OverlapCircleAll(transform.position, Mathf.Exp(i), bushMask);

            foreach(Collider2D hit in hits)
            {
                BushFruits hitBush = hit.GetComponent<BushFruits>();

                if(hit && (hitBush.HasFruits() && hitBush.enabled))
                {
                    bushFruitsTarget = hitBush;
                    break;
                }
            }

            if(bushFruitsTarget)
                break;
        }
    }

    void Attack()
    {
        artifact.TakeDamage(attackDamage);
    }
}

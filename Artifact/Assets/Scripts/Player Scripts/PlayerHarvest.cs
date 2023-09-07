using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHarvest : MonoBehaviour
{
    [SerializeField]
    private float harvestTime = 0.4f;

    private PlayerMovement playerMovement;
    private PlayerBackpack playerBackpack;
    private AudioSource audioSource;
    private Collider2D collideBush;
    private BushFruits hitBush;
    private bool canHarvestFruits;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerBackpack = GetComponent<PlayerBackpack>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
            TryHarvestFruit();
    }

    void TryHarvestFruit()
    {
        if(!canHarvestFruits)
            return;

        if(collideBush != null)
        {
            hitBush = collideBush.GetComponent<BushFruits>();

            if(hitBush)
            {
                audioSource.Play();
                playerMovement.HarvestStopMovement(harvestTime);
                playerBackpack.AddFruits(hitBush.HarvestFruits());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Bush"))
        {
            canHarvestFruits = true;
            collideBush = collider;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.CompareTag("Bush"))
        {
            canHarvestFruits = false;
            collideBush = null;
        }
    }
}

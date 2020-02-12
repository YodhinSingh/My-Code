using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keys : MonoBehaviour
{
    private PlayerController player;
    private GameObject slot1;
    private GameObject slot2;
    private GameObject slot3;
    public float keySlot;


    // Start is called before the first frame update
    void Start()
    {
        slot1 = GameObject.Find("Key_Slot_1");  // find the 3 slot images for the keys
        slot2 = GameObject.Find("Key_Slot_2");
        slot3 = GameObject.Find("Key_Slot_3");
        slot1.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); // set their scale
        slot2.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        slot3.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        slot1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Key_Empty");  // At the start of the game, make sure the image is that where the key is missing
        slot2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Key_Empty");
        slot3.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Key_Empty");
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // When the player touches a key, it will automatically change the image of the key slot associated with this key to an image where its been inserted

        if (other.CompareTag("Player"))     
        {
            
            if (keySlot == 1) 
            {
                slot1.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Key_Filled");
                slot1.transform.localScale = new Vector3(2f, 2f, 2f);   // change the scale of the key slot image to bigger (for UI design)
                player.keyNum++;                                        // increase the player's count of how many keys they collected

            }
            if (keySlot == 2)
            {
                slot2.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Key_Filled");
                slot2.transform.localScale = new Vector3(2f, 2f, 2f);
                player.keyNum++;

            }
            if (keySlot == 3)
            {
                slot3.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Key_Filled");
                slot3.transform.localScale = new Vector3(2f, 2f, 2f);
                player.keyNum++;    

            }
            GameSound.PlaySound("KeyCollect");


            Destroy(gameObject);    // destroy the key
        }
    }
}

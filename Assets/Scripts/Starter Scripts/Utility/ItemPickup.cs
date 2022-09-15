using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ItemPickup : MonoBehaviour
{
    //This component is placed on any object that is a keyItem pick up and to be placed in your "inventory"
    [Header("Inventory System: Item Details")]
    public string itemName = "Item";
    public int itemID = 0;
    public bool destroyOnUse = false;
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.TryGetComponent<PlayerInventory>(out PlayerInventory inv);
            inv.AddItemToInventory(new PlayerInventory.Item(itemName, itemID, spriteRenderer.sprite, spriteRenderer.color, destroyOnUse));

            this.gameObject.SetActive(false);
        }
    }
}

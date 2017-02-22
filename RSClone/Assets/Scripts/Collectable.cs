using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {
  public string Item;

    private void Awake()
    {
        Item thisItem = Inventory.inv.lookupItem(Item);
        gameObject.name = thisItem.name;
        gameObject.GetComponent<ExamineObject>().ExamineText = thisItem.examine;
    }

  private void Collect()
  {
        StartCoroutine(CollectItem());
  }

    private IEnumerator CollectItem()
    {
        Player.character.SendMessage("SetDestination", transform.position);
        while(CollisionMap.Map.Distance(Player.character.transform.position, transform.position) > 0)
        {
            yield return null;
        }

        Inventory.inv.addItem(Item);
        gameObject.SendMessageUpwards("Collected", SendMessageOptions.DontRequireReceiver);
        Destroy(gameObject);
    }
}

using LobbyScripts;
using UnityEngine;

public class KeyHintScript : MonoBehaviour
{
    private bool playerInRange;

    public Interactable linkedObject;
    public GameObject floatingText;

    // Start is called before the first frame update
    void Start()
    {
        floatingText.SetActive(false);
        floatingText.GetComponent<Renderer>().sortingOrder = 17;

        if (!linkedObject)
        {
            linkedObject = GetComponentInParent<Interactable>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Collider2D[] results = Physics2D.OverlapCircleAll(transform.position, 10f);
        //
        // foreach (var col in results)
        // {
        //     Debug.Log(col.name);
        //
        // }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (!linkedObject.isUsable) {return;}
            
        playerInRange = true;
        floatingText.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        floatingText.SetActive(false);
    }
    
}

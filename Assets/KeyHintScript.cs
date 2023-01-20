using UnityEngine;

public class KeyHintScript : MonoBehaviour
{
    private bool playerInRange;

    public GameObject floatingText;

    // Start is called before the first frame update
    void Start()
    {
        floatingText.SetActive(false);
        floatingText.GetComponent<Renderer>().sortingOrder = 9;
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
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            floatingText.SetActive(true);
            
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            floatingText.SetActive(false);
        }
    }
    
}

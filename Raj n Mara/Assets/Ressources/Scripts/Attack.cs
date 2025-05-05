using UnityEngine;

public class Attack : MonoBehaviour
{
    BoxCollider2D hitBoxSlash;
    void Start()
    {
        hitBoxSlash = GetComponent<BoxCollider2D>();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Slash()
    {
        gameObject.SetActive(true);
        // Temporarily activate the hitbox for the slash attack
        if (hitBoxSlash != null)
        {
            hitBoxSlash.enabled = true;

            // Deactivate the hitbox after a short delay
            Invoke(nameof(DeactivateHitbox), 0.1f);
        }
    }
    void DeactivateHitbox()
    {
        if (hitBoxSlash != null)
        {
            hitBoxSlash.enabled = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object has the tag "Enemy"
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject); // Destroy the enemy GameObject
        }
    }
}

using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 10;
    public float currentHealth;

    bool isInvincible = false;
    float invincibleTimer;
    public float invincibleTime = 1f;                   //Time when the player will be untargetable  

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            {
                isInvincible = false;
            }
        }
    }

    public void IncraseHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    }

    public void IncraseMaxHealth()
    {
        maxHealth++;
    }

    public void TakeDamage(float damage)
    {
        //Check if the player is in invinvibility state...
        if (isInvincible)
        {
            //If he is, he cannot take damage.
            return;
        }

        //If not, he happens to be and state of invincibility for a few seconds...
        isInvincible = true;
        invincibleTimer = invincibleTime;
        currentHealth = Mathf.Clamp(currentHealth + damage, 0, maxHealth);
    }
}

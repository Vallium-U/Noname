using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMANAGERscript : MonoBehaviour
{
    public float health = 100f;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "EnemyBullet")
        {
            health -= 25f;
            if (health <= 0f)
            {
                Die();
            }
        }if (other.gameObject.tag == "EnemyMissle")
        {
            health -= 55f;
            if (health <= 0f)
            {
                Die();
            }
        }
    }

    void Die()
    {
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }
}

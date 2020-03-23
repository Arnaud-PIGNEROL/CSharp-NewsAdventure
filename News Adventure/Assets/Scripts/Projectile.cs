using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Projectile : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public float distance;
    public int projectileDamage;
    public LayerMask whatIsSolid;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyProjectile", lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right, distance, whatIsSolid);
        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Enemy"))
            {
                hitInfo.collider.GetComponent<Enemy>().TakeDamage(projectileDamage);
            }
            DestroyProjectile();
        }
        transform.Translate(transform.right * speed * Time.deltaTime);
    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}

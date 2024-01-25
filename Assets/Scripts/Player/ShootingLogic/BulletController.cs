using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletSpeed = 10f;
    public float bulletLifetime = 5f;
    private bool isRicocheting = false;

    void Start()
    {
        // Задаем скорость движения пули
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * bulletSpeed;

        // Запускаем таймер для уничтожения пули через bulletLifetime секунд
        Destroy(gameObject, bulletLifetime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // При столкновении включаем режим рикошета
        if (!isRicocheting)
        {
            isRicocheting = true;

            // Изменяем направление движения на противоположное
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.velocity = -rb.velocity;

            // Уменьшаем время жизни пули, чтобы она исчезла раньше
            bulletLifetime = Mathf.Min(bulletLifetime, 1f);
        }
    }
}
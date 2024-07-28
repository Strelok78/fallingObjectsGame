using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    [SerializeField]
    private GameObject targetObject;

    private GameObject circle;
    private bool isShiftPressed = false;

    void Update()
    {
        if (targetObject == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            isShiftPressed = true;
            CreateCircle();
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            isShiftPressed = false;
            DestroyCircle();
        }

        if (isShiftPressed && circle != null)
        {
            circle.transform.position = targetObject.transform.position;
        }
    }

    void CreateCircle()
    {
        if (circle == null)
        {
            circle = new GameObject("Circle");
            circle.transform.position = targetObject.transform.position;

            // Создание визуального представления круга
            SpriteRenderer renderer = circle.AddComponent<SpriteRenderer>();
            renderer.sprite = CreateCircleSprite();
            renderer.color = Color.blue;

            // Увеличение размера круга на 1 единицу по каждой оси относительно размера передаваемого объекта
            Vector3 targetScale = targetObject.transform.localScale;
            circle.transform.localScale = targetScale + Vector3.one;

            Rigidbody2D rb = circle.AddComponent<Rigidbody2D>();
            rb.isKinematic = true;

            CircleCollider2D collider = circle.AddComponent<CircleCollider2D>();
            collider.isTrigger = false; // Отключаем триггер, чтобы коллайдер учитывал столкновения
        }
    }

    void DestroyCircle()
    {
        if (circle != null)
        {
            Destroy(circle);
        }
    }

    // Вспомогательная функция для создания спрайта круга
    private Sprite CreateCircleSprite()
    {
        Texture2D texture = new Texture2D(128, 128);
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                float xCentered = x - texture.width / 2f;
                float yCentered = y - texture.height / 2f;
                float distance = Mathf.Sqrt(xCentered * xCentered + yCentered * yCentered);
                float radius = texture.width / 2f;
                texture.SetPixel(x, y, distance < radius ? Color.white : Color.clear);
            }
        }
        texture.Apply();
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
}

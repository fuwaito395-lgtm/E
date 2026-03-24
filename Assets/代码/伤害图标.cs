using System.Collections;
using TMPro;
using UnityEngine;
using static class_damage;
public class DamagePopup : MonoBehaviour
{
    public SpriteRenderer iconRenderer;
    public TextMeshPro text;

    public float lifeTime = 0.8f;
    public float floatSpeed = 1.5f;
    public float shakeStrength = 0.2f;

    private Vector3 velocity;

    public void Init(Sprite icon, int damage)
    {
        if (iconRenderer != null)
        {
            iconRenderer.sprite = icon;
            iconRenderer.sortingOrder = 100;
        }

        if (text != null)
        {
            text.text = damage.ToString();
            text.sortingOrder = 100;
        }

        velocity = new Vector3(0, floatSpeed, 0);

        StartCoroutine(Play());
    }

    private IEnumerator Play()
    {
        float t = 0;

        while (t < lifeTime)
        {
            t += Time.deltaTime;

            // 向上移动
            transform.position += velocity * Time.deltaTime;

            // 2D抖动
            Vector2 shake = Random.insideUnitCircle * shakeStrength;
            transform.position += new Vector3(shake.x, shake.y, 0) * Time.deltaTime;

            yield return null;
        }

        Destroy(gameObject);
    }
}
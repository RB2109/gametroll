using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 2f;
    public Transform[] points;

    private int i;

    void Start()
    {
        transform.position = points[0].position;
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, points[i].position) < 0.01f)
        {
            i++;
            if (i == points.Length)
            {
                i = 0;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Player"))
    {
        if (gameObject.activeInHierarchy) // only detach if still active
        {
            collision.transform.SetParent(null);
        }
    }
}


    private IEnumerator DetachPlayerNextFrame(Transform player)
    {
        yield return null; // wait one frame
        if (player != null)
        {
            player.SetParent(null);
        }
    }
}


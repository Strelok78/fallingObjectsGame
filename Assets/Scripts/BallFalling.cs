using UnityEngine;

public class BallFalling : MonoBehaviour
{
    private float _targetX;
    private float _fallSpeed = 5f;
    private float _moveSpeed = 2f;

    public void SetBounds(float xLeft, float xRight)
    {
        _targetX = Random.Range(xLeft, xRight);
    }

    private void Update()
    {
        float newX = Mathf.MoveTowards(transform.position.x, _targetX, _moveSpeed * Time.deltaTime);
        transform.position = new Vector3(newX, transform.position.y - _fallSpeed * Time.deltaTime, transform.position.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<BallFalling>())
            return;

        Destroy(gameObject);
    }
}
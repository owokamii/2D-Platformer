using UnityEngine;

public class ParallaxFactor : MonoBehaviour
{
    [SerializeField] private float parallaxFactor;

    public GameObject cam;

    private float length, startPos;

    private void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        float temp = (cam.transform.position.x * (1 - parallaxFactor));
        float distance = (cam.transform.position.x * parallaxFactor);

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        if (temp > startPos + length) startPos += length;
        else if (temp < startPos - length) startPos -= length;
    }
}

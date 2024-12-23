using UnityEditor.PackageManager;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public Vector3 mousePos;
    public float lookingAngle;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;
    }

    void FixedUpdate()
    {
        lookingAngle = CalculateAngle(mousePos);
    }

    public float CalculateAngle(Vector2 ownPos)
    {
        Vector2 _direction = ownPos - (Vector2)transform.position;
        float _calculatedAngle = Vector2.Angle(Vector2.up, _direction);
        return _calculatedAngle;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(transform.position, mousePos);
    }
}

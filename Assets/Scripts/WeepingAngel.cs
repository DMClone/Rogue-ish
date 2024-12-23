using UnityEngine;

public class WeepingAngel : MonoBehaviour
{
    [SerializeField] private float requiredAngel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float _angle = PlayerController.instance.CalculateAngle(transform.position);
        if (PlayerController.instance.lookingAngle - _angle <= requiredAngel / 2f && PlayerController.instance.lookingAngle - _angle >= -requiredAngel / 2f)
        {
            Debug.Log("I can smell you");
        }
    }
}

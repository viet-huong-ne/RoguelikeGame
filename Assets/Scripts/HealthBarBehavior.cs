using UnityEngine;

public class HealthBarBehavior : MonoBehaviour
{
    [SerializeField] private Transform target;  // Vị trí nhân vật
    [SerializeField] private Vector3 offset;   // Độ lệch từ vị trí nhân vật đến thanh máu

    public Transform Target
    {
        get { return target; }
        set { target = value; }
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            // Chuyển vị trí từ World Space sang Screen Space và cộng thêm offset
            transform.position = Camera.main.WorldToScreenPoint(target.position + offset);
        }
    }
}


using UnityEngine;

public class MousePosition : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float minHeight;
    [SerializeField] private float maxHeight;

    // Update is called once per frame
    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask))
        {
            if (raycastHit.point.y >= minHeight && raycastHit.point.y <= maxHeight)
                transform.position = raycastHit.point;
        }

        if (Input.GetMouseButtonDown(0))
            OnMouseDown();
    }

    void OnMouseDown()
    {
        EntitySummoner.Instance.SummonTurret(0, transform.position);
    }
}

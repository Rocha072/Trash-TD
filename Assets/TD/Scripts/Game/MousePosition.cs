using UnityEngine;
using UnityEngine.EventSystems;

public class MousePosition : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float minHeight;
    [SerializeField] private float maxHeight;
    public Tower towerBeingPlaced;

    void Update()
    {
        if (!PlayerMovement.moveOn)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask))
            {
                if (raycastHit.point.y >= minHeight && raycastHit.point.y <= maxHeight)
                {
                    transform.position = raycastHit.point;
                    towerBeingPlaced.invalidMask.SetActive(false);
                }
                else
                {
                    towerBeingPlaced.invalidMask.SetActive(true);
                }
            }

            if (towerBeingPlaced != null)
            {
                towerBeingPlaced.transform.position = transform.position;

           
                if (Input.GetMouseButtonDown(0))
                {
                    if (EventSystem.current.IsPointerOverGameObject())
                    {
                        return;
                    }
                    towerBeingPlaced = null; 
                }
                
                
                if (Input.GetMouseButtonDown(1))
                {
                    EntitySummoner.Instance.RemoveTower(towerBeingPlaced); 
                    towerBeingPlaced = null;
                }
            }

        }
    }

    public void setTowerToPlaceByID(int towerID)
    {
        if (towerBeingPlaced != null)
            return;

        towerBeingPlaced = EntitySummoner.Instance.SummonTower(towerID, transform.position);
        
    }
}

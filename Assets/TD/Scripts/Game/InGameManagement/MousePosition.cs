using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class MousePosition : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float minHeight;
    [SerializeField] private float maxHeight;
    public Tower towerBeingPlaced;

    async Task Update()
    {
        if (!PlayerMovement.moveOn)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask))
            {
                transform.position = raycastHit.point;
            }

            if (towerBeingPlaced != null)
            {
                towerBeingPlaced.transform.position = transform.position;

                if (Input.GetMouseButtonDown(0) && towerBeingPlaced.validPosition)
                {
                    if (EventSystem.current.IsPointerOverGameObject())
                    {
                        return;
                    }
                    towerBeingPlaced.isBeingPlaced = false;
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

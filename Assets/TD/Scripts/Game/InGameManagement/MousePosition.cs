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

    void Update()
    {

        if (towerBeingPlaced != null)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask))
            {
                transform.position = raycastHit.point;
            }
            towerBeingPlaced.transform.position = transform.position;

            if (Input.GetMouseButtonDown(0) && towerBeingPlaced.validPosition)
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }
                PlayerEconomy.Instance.Buy(towerBeingPlaced);
                towerBeingPlaced.isBeingPlaced = false;
                towerBeingPlaced = null;
            }


            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape))
            {
                EntitySummoner.Instance.RemoveTower(towerBeingPlaced);
                towerBeingPlaced = null;
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

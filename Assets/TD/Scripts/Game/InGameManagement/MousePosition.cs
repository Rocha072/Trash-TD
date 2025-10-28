using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class MousePosition : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private LayerMask towersLayerMask;
    [SerializeField] private float minHeight;
    [SerializeField] private float maxHeight;
    public Tower towerBeingPlaced;
    private Tower hoveredTower;
    private Tower selectedTower;

    void Update()
    {
        if (PlayerMovement.moveOn)
        {
            ResetAllStates();
            return;
        }

        if (towerBeingPlaced != null)
        {
            HandleTowerPlacement();
        }
        else
        {
            HandleTowerInteraction();
        }
    }

    private void ResetAllStates()
    {
        if (hoveredTower != null)
        {
            hoveredTower.UnhoverTower();
            hoveredTower = null;
        }

        if (selectedTower != null)
        {
            selectedTower.UnselectTower();
            selectedTower = null;
        }

        if (towerBeingPlaced != null)
        {
            EntitySummoner.Instance.RemoveTower(towerBeingPlaced);
            towerBeingPlaced = null;
        }
    }

    private void HandleTowerPlacement()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, groundLayerMask))
        {
            transform.position = raycastHit.point;
            towerBeingPlaced.transform.position = transform.position;
        }

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

    private void HandleTowerInteraction()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Tower towerHitThisFrame = null;

        if (Physics.Raycast(ray, out hit, float.MaxValue, towersLayerMask))
        {
            towerHitThisFrame = hit.collider.GetComponent<Tower>();
        }

        HandleHoverLogic(towerHitThisFrame);

        HandleSelectionLogic(towerHitThisFrame);
    }

    private void HandleHoverLogic(Tower towerHitThisFrame)
    {
        if (hoveredTower != null && towerHitThisFrame != hoveredTower)
        {
            hoveredTower.UnhoverTower();
            hoveredTower = null;
        }

        if (towerHitThisFrame != null && towerHitThisFrame != hoveredTower)
        {
            if (towerHitThisFrame != selectedTower)
            {
                hoveredTower = towerHitThisFrame;
                hoveredTower.HoverTower();
            }
        }
    }

    private void HandleSelectionLogic(Tower towerHitThisFrame)
    {
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (towerHitThisFrame != null)
        {
            if (towerHitThisFrame != selectedTower)
            {
                if (selectedTower != null)
                {
                    selectedTower.UnselectTower();
                }

                selectedTower = towerHitThisFrame;
                selectedTower.SelectTower();

                hoveredTower = null;
            }
            // (Se clicou na mesma torre que já estava selecionada, não faz nada.
            //  Aqui você poderia abrir um menu de Upgrade, por exemplo)
        }
        else
        {
            if (selectedTower != null)
            {
                selectedTower.UnselectTower();
                selectedTower = null;
            }
        }
    }

    public void setTowerToPlaceByID(int towerID)
    {
        // Não deixa pegar outra torre se já estiver posicionando
        if (towerBeingPlaced != null)
        {
            EntitySummoner.Instance.RemoveTower(towerBeingPlaced);
        }

        if (selectedTower != null)
        {
            selectedTower.UnselectTower();
            selectedTower = null;
        }

        towerBeingPlaced = EntitySummoner.Instance.SummonTower(towerID, transform.position);
    }

    //void Update()
    //{
    //    if (selectedTower != null)
    //        selectedTower.UnhoverTower();

    //    if (PlayerMovement.moveOn)
    //    {
    //        if (hoveredTower != null)
    //        {
    //            hoveredTower.UnhoverTower();
    //            hoveredTower = null;
    //        }
    //        towerBeingPlaced = null;
    //        return;
    //    }

    //    if (towerBeingPlaced != null)
    //    {
    //        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
    //        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask))
    //        {
    //            transform.position = raycastHit.point;
    //        }
    //        towerBeingPlaced.transform.position = transform.position;
            
    //        if (Input.GetMouseButtonDown(0) && towerBeingPlaced.validPosition)
    //        {
    //            if (EventSystem.current.IsPointerOverGameObject())
    //            {
    //                return;
    //            }
    //            PlayerEconomy.Instance.Buy(towerBeingPlaced);
    //            towerBeingPlaced.isBeingPlaced = false;
    //            towerBeingPlaced = null;
    //        }


    //        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape))
    //        {
    //            EntitySummoner.Instance.RemoveTower(towerBeingPlaced);
    //            towerBeingPlaced = null;
    //        }

    //    }

    //    if (towerBeingPlaced == null)
    //    {
    //        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
    //        RaycastHit hit;

    //        if (Physics.Raycast(ray, out hit, float.MaxValue, towersLayer))
    //        {
    //            Tower towerHitThisFrame = hit.collider.GetComponent<Tower>();

    //            if (towerHitThisFrame != hoveredTower)
    //            {
    //                if (hoveredTower != null)
    //                {
    //                    hoveredTower.UnhoverTower();
    //                }
    //                if (!(selectedTower != null && selectedTower == towerHitThisFrame))
    //                {
    //                    hoveredTower = towerHitThisFrame;
    //                    hoveredTower.HoverTower();
    //                }
    //            }

    //        }
    //        else
    //        {
    //            if (hoveredTower != null)
    //            {
    //                hoveredTower.UnhoverTower();
    //                hoveredTower = null;
    //            }
    //        }
    //    }

    //    if (Input.GetMouseButtonDown(0) && towerBeingPlaced == null)
    //    {
    //        if (selectedTower == null && hoveredTower != null)
    //        {
    //            selectedTower = hoveredTower;
    //            selectedTower.SelectTower();
    //        }

    //        if (selectedTower != null && hoveredTower == null)
    //        {
    //            selectedTower.UnselectTower();
    //            selectedTower = null;
    //        }

    //        if (selectedTower != null && hoveredTower != null)
    //        {
    //            if (selectedTower != hoveredTower)
    //            {
    //                selectedTower.UnselectTower();
    //                selectedTower = hoveredTower;
    //                selectedTower.SelectTower();    
    //            }
    //        }
    //    }



    //}

    //public void setTowerToPlaceByID(int towerID)
    //{
    //    if (towerBeingPlaced != null)
    //        return;

    //    towerBeingPlaced = EntitySummoner.Instance.SummonTower(towerID, transform.position);
        
    //}
}

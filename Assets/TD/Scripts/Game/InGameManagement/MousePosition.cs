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

    public static MousePosition Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

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

        DeselectTower();
        
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

            SelectedTowerCardManager.Instance.ShowSelectCard(selectedTower);
        }
        else
        {
            DeselectTower();
        }
    }

    public void DeselectTower()
    {
        if (selectedTower != null)
        {
            selectedTower.UnselectTower();
            selectedTower = null;
            SelectedTowerCardManager.Instance.HideSelectCard();
        }
    }

    public void setTowerToPlaceByID(int towerID)
    {
        
        if (towerBeingPlaced != null)
        {
            EntitySummoner.Instance.RemoveTower(towerBeingPlaced);
        }

        DeselectTower();

        towerBeingPlaced = EntitySummoner.Instance.SummonTower(towerID, transform.position);
    }


}

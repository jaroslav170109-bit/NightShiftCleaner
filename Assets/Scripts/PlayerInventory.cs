using UnityEngine;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    [Header("Настройки подбора")]
    public Transform handPosition;
    public float pickupRange = 3f;

    [Header("Интерфейс: Подсказка (при наведении)")]
    public GameObject hintObject;
    public TextMeshProUGUI itemText;

    public PickupableItem currentItem = null;
    private Transform playerCamera;

    void Start()
    {
        playerCamera = Camera.main.transform;

        if (hintObject != null) hintObject.SetActive(false);
    }

    void Update()
    {
        CheckForItems();

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPickUp();
        }

        if (Input.GetKeyDown(KeyCode.Q) && currentItem != null)
        {
            DropItem();
        }
    }

    private void CheckForItems()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupRange) && hit.collider.TryGetComponent(out PickupableItem item))
        {
            if (hintObject != null)
            {
                hintObject.SetActive(true);
                if (itemText != null) itemText.text = item.itemName;
            }
        }
        else
        {
            if (hintObject != null && hintObject.activeSelf)
            {
                hintObject.SetActive(false);
            }
        }
    }

    private void TryPickUp()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupRange))
        {
            if (hit.collider.TryGetComponent(out PickupableItem newItem))
            {
                if (currentItem != null) DropItem();

                currentItem = newItem;
                currentItem.PickUp(handPosition);


                if (hintObject != null) hintObject.SetActive(false);
            }
        }
    }

    private void DropItem()
    {
        if (currentItem != null)
        {
            
            // ---------------------

            currentItem.Drop(playerCamera);
            currentItem = null;
        }
    }
}
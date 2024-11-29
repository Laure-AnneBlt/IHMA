using System.Collections.Generic;
using UnityEngine;

public class DistributeInLine : MonoBehaviour
{
    public GameObject[] prefabs;     
    public float spacing = 0.3f;     // Distance between each object
    public Transform table;
    public QRCodeReader qrReader;
    public GameObject qrCodeObject;

    private List<GameObject> objects = new List<GameObject>();   // To hold the 12 objects instantiated from the prefab


    void Awake()
    {
        // Ajout du composant QRCodeReader au GameObject courant
        qrReader = gameObject.AddComponent<QRCodeReader>();
    }

    void Start()
    {
        for (int i = 0; i < prefabs.Length; i++)
        {
            // First set of objects
            Vector3 newPosition = table.position + new Vector3(-table.localScale.x / 2, 0.4f, table.localScale.z / 2) + new Vector3(i * spacing, 0, 0);
            GameObject newObject = Instantiate(prefabs[i], newPosition, Quaternion.identity);
            newObject.AddComponent<Draggable>();
            objects.Add(newObject);
            // Second set of objects
            Vector3 newPosition2 = table.position + new Vector3(-table.localScale.x / 2, 0.4f, -table.localScale.z / 2) + new Vector3(i * spacing, 0, 0);
            GameObject newObject2 = Instantiate(prefabs[i], newPosition2, Quaternion.Euler(0, 90, 0));
            newObject2.AddComponent<Draggable>();
            objects.Add(newObject2);

        }
    }

    void Update()
    {
        foreach (GameObject obj in objects)
        {
            if (IsCloseToTable(obj))
            {
                SnapToSurface(obj);
            }
        }

        // Detect click on the computer and change block color
        DetectAndColorBlock();

        // Stop QR code tracking when it is detected by the Hololens
        if (qrReader.init)
        {
            qrCodeObject.SetActive(false);
        }
        
    }

    private void SnapToSurface(GameObject obj)
    {
        // Align the objects's position and rotation with the table's surface
        Vector3 tablePosition = table.position;
        obj.transform.position = new Vector3(obj.transform.position.x, tablePosition.y + obj.transform.localScale.x / 2, obj.transform.position.z);

        obj.transform.rotation = Quaternion.Euler(0, obj.transform.eulerAngles.y, 0);
    }

    private bool IsCloseToTable(GameObject obj)
    {
        // Project the object's position onto the table's vertical (Y-axis) plane
        Vector3 projectedPosition = new Vector3(obj.transform.position.x, table.position.y, obj.transform.position.z);

        // Calculate the distance between the object and its projection on the table
        float distanceToTable = Vector3.Distance(obj.transform.position, projectedPosition);

        return distanceToTable <= 0.07;
    }

    private void DetectAndColorBlock()
    {
        // Check for mouse click
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Detect the object being clicked
            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedObject = hit.collider.gameObject;

                // Check if the clicked object is part of the objects list
                if (objects.Contains(clickedObject))
                {
                    ChangeObjectColor(clickedObject, Color.red);
                }
            }
        }
    }

/*    private void ChangeBlockColor(GameObject obj, Color color)
    {
        // Change the color of the object's material
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = color;
        }
    }*/

    private void ChangeObjectColor(GameObject rootObject, Color color)
    {
        // Change the color of the root object itself
        Renderer rootRenderer = rootObject.GetComponent<Renderer>();
        if (rootRenderer != null)
        {
            rootRenderer.material.color = color;
        }

        // Iterate over all children of the root object and change their colors
        foreach (Transform child in rootObject.transform)
        {
            Renderer childRenderer = child.GetComponent<Renderer>();
            if (childRenderer != null)
            {
                childRenderer.material.color = color;
            }
        }
    }
}

using UnityEngine;

public class DragObject : MonoBehaviour
{
    private Vector3 offset;
    private float zCoord;

    void OnMouseDown()
    {
        zCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        offset = gameObject.transform.position - GetMouseWorldPos();
    }

    void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() + offset;
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    void Update()
    {
        if (Input.GetMouseButton(1)) // Right mouse button for rotation
        {
            float rotationSpeed = 10.0f;
            float rotationX = Input.GetAxis("Mouse X") * rotationSpeed;
            float rotationY = Input.GetAxis("Mouse Y") * rotationSpeed;

            transform.Rotate(Vector3.up, -rotationX, Space.World);
            transform.Rotate(Vector3.right, rotationY, Space.World);
        }
    }
}
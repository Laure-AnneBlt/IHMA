using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

enum BlockColor { NEUTRAL, RED, BLUE, DONE };

public class DistributeInLine : MonoBehaviour
{
    public GameObject[] prefabs;
    public GameObject casePrefab;
    public float spacing = 0.3f;     // Distance between each object
    public Transform table;
    public QRCodeReader qrReader;
    public GameObject qrCodeObject;
   

    private List<GameObject> objects = new List<GameObject>();   // To hold the 12 objects instantiated from the prefab
    private Dictionary<GameObject, BlockColor> objectStates = new Dictionary<GameObject, BlockColor>(); // Track selection state for each object

    private int n = 10;
    private int m = 10;
    private GameObject[,] caseObject;

    

    void Awake()
    {
        // Ajout du composant QRCodeReader au GameObject courant
        qrReader = gameObject.AddComponent<QRCodeReader>();
    }

    private void CheckColliders(GameObject obj)
    {
        // Vérifie si le parent a un Collider
        if (obj.GetComponent<Collider>() == null)
        {
            Debug.LogWarning($"Collider is missing on {obj.name}");
        }

        // Vérifie les enfants
        foreach (Transform child in obj.transform)
        {
            CheckColliders(child.gameObject);
        }
    }



    void Start()
    {
        caseObject = new GameObject[n, m];



        /*float CubeLength = 0.05f;*/


        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                /*Vector3 bottomLeftCorner = table.position - new Vector3(table.localScale.x / 2, 0, table.localScale.z / 2);*/

                Vector3 newPos = new Vector3(0, 0, 0);
                GameObject caseElement = Instantiate(casePrefab, newPos, Quaternion.identity);
                caseObject[i, j] = caseElement;

                objects.Add(caseElement);
                objectStates[caseElement] = BlockColor.NEUTRAL;
                
            }
        }

        



        for (int i = 0; i < prefabs.Length; i++)
        {
            // First set of objects
            Vector3 newPosition = table.position + new Vector3(-table.localScale.x / 2, 0.4f, table.localScale.z / 2) + new Vector3(i * spacing, 0, 0);
            GameObject newObject = Instantiate(prefabs[i], newPosition, Quaternion.identity);
            newObject.AddComponent<DragObject>();
            objects.Add(newObject);
            objectStates[newObject] = BlockColor.NEUTRAL; // Initially not selected

            CheckColliders(newObject);

            // Second set of objects
            Vector3 newPosition2 = table.position + new Vector3(-table.localScale.x / 2, 0.4f, -table.localScale.z / 2) + new Vector3(i * spacing, 0, 0);
            GameObject newObject2 = Instantiate(prefabs[i], newPosition2, Quaternion.Euler(0, 90, 0));
            newObject2.AddComponent<DragObject>();
            objects.Add(newObject2);
            objectStates[newObject2] = BlockColor.NEUTRAL; // Initially not selected

            CheckColliders(newObject2);
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

        DetectAndColorBlock();
        // DetectAndColorCell(); // Gérer la sélection des cases

        if (qrReader.init)
        {
            qrCodeObject.SetActive(false);
        }


        /*for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                //caseObject[i, j].transform.position = new Vector3(i*0.1f , table.position.y + 0.01f, j * 0.1f);
                Vector3 bottomLeftCorner = table.position - new Vector3(table.localScale.x / 2, 0, table.localScale.z / 2);
                Vector3 newPos = new Vector3(bottomLeftCorner.x + i * CubeLength, table.position.y + 0.01f, (-table.position.z / 2) + j * CubeLength);
                caseObject[i, j].transform.position = newPos;
                    // new Vector3( +i*0.1f, table.position.y + 0.01f, (-table.position.z/2) + j * 0.1f);
            }
        }*/

        Vector3 bottomLeftCorner = table.position - new Vector3(table.localScale.x / 2, 0, table.localScale.z / 2);

        for (int i = 1; i < n; i++)
        {
            for (int j = 1; j < m; j++)
            {
                caseObject[i-1, j-1].transform.position = new Vector3(
                    bottomLeftCorner.x + i * 0.1f,
                    table.position.y + 0.01f,
                    bottomLeftCorner.z + j * 0.1f
                );
            }
        }


    }



    private void SnapToSurface(GameObject obj)
    {
        // Align the object's position and rotation with the table's surface
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
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedObject = hit.collider.gameObject;
                GameObject rootObject = clickedObject.transform.root.gameObject;

                if (objects.Contains(rootObject))
                {
                    BlockColor isSelected = objectStates[rootObject];

                    switch (isSelected)
                    {
                        case BlockColor.NEUTRAL:
                            ChangeObjectColor(rootObject, Color.red);
                            objectStates[rootObject] = BlockColor.RED;
                            break;
                        case BlockColor.RED:
                            ChangeObjectColor(rootObject, Color.blue);
                            objectStates[rootObject] = BlockColor.BLUE;
                            break;
                        case BlockColor.BLUE:
                            ChangeObjectColor(rootObject, Color.green);
                            objectStates[rootObject] = BlockColor.DONE;
                            break;
                        case BlockColor.DONE:
                            ChangeObjectColor(rootObject, Color.white);
                            objectStates[rootObject] = BlockColor.NEUTRAL;
                            break;

                    }
                   

                    
                }
            }
        }
    }

    private void ChangeObjectColor(GameObject rootObject, Color color)
    {
        Renderer rootRenderer = rootObject.GetComponent<Renderer>();
        if (rootRenderer != null)
        {
            rootRenderer.material.color = color;
        }

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



public class DragObject : MonoBehaviour
{
    private Vector3 offset;
    private float zCoord;
    private GameObject rootObject;

    void OnMouseDown()
    {
        rootObject = transform.root.gameObject;

        zCoord = Camera.main.WorldToScreenPoint(rootObject.transform.position).z;
        offset = rootObject.transform.position - GetMouseWorldPos();
    }

    void OnMouseDrag()
    {
        if (rootObject != null)
        {
            rootObject.transform.position = GetMouseWorldPos() + offset;
        }
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
            if (rootObject != null)
            {
                float rotationSpeed = 10.0f;
                float rotationX = Input.GetAxis("Mouse X") * rotationSpeed;
                float rotationY = Input.GetAxis("Mouse Y") * rotationSpeed;

                rootObject.transform.Rotate(Vector3.up, -rotationX, Space.World);
                rootObject.transform.Rotate(Vector3.right, rotationY, Space.World);
            }
        }
    }
}

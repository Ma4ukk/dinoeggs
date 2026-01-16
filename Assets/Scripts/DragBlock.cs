using UnityEngine;
using System.Collections.Generic;

public class DragBlock : MonoBehaviour
{
    [SerializeField] private int eggs;
    [SerializeField] private string color;
    private int i = 0;
    private bool isDragging = false;
    private bool x = true;
    private Vector3 offset;
    private Vector3 OgPosition; 
    private Camera mainCamera;
    public List<GameObject> targets = new List<GameObject>();
    private Vector2 checkSize = Vector2.one * 0.9f;
    void Start()
    {
        mainCamera = Camera.main;
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            targets.Add(child.gameObject);
        }
    }
    void OnMouseDown()
    {
        x = true;
        isDragging = true;
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.position - new Vector3(mouseWorldPos.x, mouseWorldPos.y, 0);
        OgPosition = transform.position;
        Debug.Log("Down");
    }
    void OnMouseDrag()
    {
        if (isDragging)
        {
            if (eggs <= 0) 
            {
                Destroy(this.gameObject);
            }
            Debug.Log("Drag");
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 targetPos = new Vector3(mouseWorldPos.x, mouseWorldPos.y, 0) + offset;
            for (i = 0; i < targets.Count; i++)
            {
                Collider2D[] colliders = Physics2D.OverlapBoxAll(targets[i].transform.position, checkSize, 0f);
                foreach (Collider2D col in colliders)
                {
                    if (col.gameObject != this.gameObject)
                    {
                        if (!col.gameObject.name.Contains(color + "Egg")) 
                        {
                            Debug.Log("2");
                            x = false;
                            break;
                        }
                        Destroy(col.gameObject);
                        eggs -= 1;
                    }
                }
                if (x)
                {
                    Debug.Log("3");
                    transform.position = targetPos;
                }
            }
        }
    }
    void OnMouseUp()
    {
        Debug.Log("Release");
        isDragging = false;
        SnapToGrid();
        if (!x)
        {
            Debug.Log("1");
            transform.position = OgPosition;
            SnapToGrid();
        }   
    }
    void SnapToGrid()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Round(pos.x);
        pos.y = Mathf.Round(pos.y);
        transform.position = pos;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DrawMover : MonoBehaviour
{
    public static List<GameObject> unit;
    public static List<GameObject> unitSelected;

    public GUISkin skin;
    Rect rect;
    bool draw;
    Vector2 startPos;
    Vector2 endPos;
    bool isSelected = false;
    public Texture texture;
    
    void Awake()
    {
        unit = new List<GameObject>();
        unitSelected = new List<GameObject>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(1) && isSelected == true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (unitSelected.Count > 0)
                {
                    for (int j = 0; j < unitSelected.Count; j++)
                    {
                        // делаем что-либо с выделенными объектами
                        unitSelected[j].GetComponent<NavMeshAgent>().SetDestination(hit.point);
                    }
                }
            }
        }
        for (int i = 0; i < unitSelected.Count; i++)
        {
            Debug.Log(i+1);
        }
    }

    bool CheckUnit(GameObject unit)
    {
        bool result = false;
        foreach (GameObject u in unitSelected)
        {
            if (u == unit)
            {
                result = true;
            }
        }
        return result;
        
    }

    void Select()
    {
        if (unitSelected.Count > 0)
        {
            for (int j = 0; j < unitSelected.Count; j++)
            {
                // делаем что-либо с выделенными объектами
                unitSelected[j].GetComponent<MeshRenderer>().material.color = Color.red;
                isSelected = true;
            }
        }
    }

    void Deselect()
    {
        if (unitSelected.Count > 0)
        {
            for (int j = 0; j < unitSelected.Count; j++)
            {
                // отменяем то, что делали с объектоми
                unitSelected[j].GetComponent<MeshRenderer>().material.color = Color.white;
                isSelected = false;
            }
        }
    }

    private void OnGUI()
    {
        GUI.skin = skin;
        GUI.depth = 99;

        if (Input.GetMouseButtonDown(0))
        {
            Deselect();
            startPos = Input.mousePosition;
            draw = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            draw = false;
            Select();
        }
        if (draw)
        {
            unitSelected.Clear();
            endPos = Input.mousePosition;
            if (startPos == endPos) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if(Physics.Raycast(ray,out hit))
                {
                    if(hit.collider.tag != "Ground")
                    {
                        unitSelected.Add(hit.collider.gameObject);
                    }
                }
            }
            else
            {
                Deselect();
            }

            rect = new Rect(Mathf.Min(endPos.x, startPos.x),
                            Screen.height - Mathf.Max(endPos.y, startPos.y),
                            Mathf.Max(endPos.x, startPos.x) - Mathf.Min(endPos.x, startPos.x),
                            Mathf.Max(endPos.y, startPos.y) - Mathf.Min(endPos.y, startPos.y)
                            );
            GUI.DrawTexture(rect,texture);
            for (int j = 0; j < unit.Count; j++)
            {
                // трансформируем позицию объекта из мирового пространства, в пространство экрана
                Vector2 tmp = new Vector2(Camera.main.WorldToScreenPoint(unit[j].transform.position).x, Screen.height - Camera.main.WorldToScreenPoint(unit[j].transform.position).y);

                if (rect.Contains(tmp)) // проверка, находится-ли текущий объект в рамке
                {
                    if (unitSelected.Count == 0)
                    {
                        unitSelected.Add(unit[j]);
                    }
                    else if (!CheckUnit(unit[j]))
                    {
                        unitSelected.Add(unit[j]);
                    }
                }
            }
        }
    }
}

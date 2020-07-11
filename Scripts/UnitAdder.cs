using UnityEngine;

public class UnitAdder : MonoBehaviour
{
    private void Start()
    {
        DrawMover.unit.Add(gameObject);
    }
}

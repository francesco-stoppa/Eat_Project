using UnityEngine;

public class PressureTile : Obstacle
{
    [SerializeField] GameObject objectToHide;
    [SerializeField] E_StepStatus whenHide;

    void Start()
    {
        RaycastHit hit;
        if (!Physics.Raycast(transform.position, transform.up, out hit, 1f)) return;
        if (!hit.collider.gameObject.TryGetComponent<Obstacle>(out Obstacle obstacle)) return;
        if (obstacle.weight == E_ObjectWeight.Levitate) return;
        SetpOn(null);
    }

    public override void SetpOn(Bob bob)
    {
        if (objectToHide == null) return;

        objectToHide.SetActive(whenHide != E_StepStatus.OnStepOn);
    }

    public override void SetpOut(Bob bob)
    {
        if (objectToHide == null) return;

        objectToHide.SetActive(whenHide == E_StepStatus.OnStepOn);
    }
}

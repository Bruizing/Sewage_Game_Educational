using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateUI : CollidableObject
{
    ActionSystem inputActions;
    [SerializeField] private Canvas UIInfoCanvas;
    [SerializeField] private Button BackButton;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

    }

    protected override void OnCollide(GameObject other)
    {
        base.OnCollide(other);
        if (Input.GetButtonDown("Inter"))
        {
            Activate();
        }
    }

    void Activate()
    {
        UIInfoCanvas.enabled = true;
    }

    public void Deactivate()
    {
        BackButton.enabled = false;
        UIInfoCanvas.enabled = false;
    }



}

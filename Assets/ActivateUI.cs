using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateUI : CollidableObject
{
    ActionSystem inputActions;
    [SerializeField] private GameObject UIInfoCanvas;
    [SerializeField] private Button BackButton;

    [SerializeField] private GameObject PopUp;

    private bool on;
    private bool off;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        inputActions = new ActionSystem();
        inputActions.Enable();

        inputActions.Player.Interact.performed += ctx => {};

    }

    protected override void OnCollide(GameObject other)
    {
        base.OnCollide(other);
        if (Input.GetButtonDown("Inter") || inputActions.Player.Interact.triggered)
        {
            Activate();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PopUp.SetActive(true);
        }
    }

    void OTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PopUp.SetActive(true);
            if(UIInfoCanvas.activeInHierarchy == true)
            {
                UIInfoCanvas.SetActive(false);
            }
        }
    }

    void Activate()
    {
        on = true;
        off = false;
        UIInfoCanvas.SetActive(on);
        BackButton.enabled = true;
    }

    public void Deactivate()
    {
        off = true;
        on = false;
        BackButton.enabled = false;
        UIInfoCanvas.SetActive(false);
    }
     
    private void OnDisable()
    {
        inputActions.Disable();
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Interact : MonoBehaviour
{
    public PickUp PickUp;

    // Start is called before the first frame update
    void Start()
    {
        PickUp.GetComponent<PickUp>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;

        if (Input.GetKeyDown(KeyCode.G))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit,PickUp.pickUpRange,
                     ~PickUp.ignoreLayer))
            {
                hit.transform.GetComponent<PlacementPointCheck>().item = PickUp.heldObj.GetComponent<RefillBox>().item;
                hit.transform.GetComponent<IInteraction>().Interact();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit,
                PickUp.pickUpRange, ~PickUp.ignoreLayer))
            {
                hit.transform.GetComponent<IInteraction>().Interact();
            }
        }

    }

}



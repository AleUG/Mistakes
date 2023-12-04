using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 2f;
    public LayerMask interactableLayer;
    public Transform playerCamera;
    public Material interactableMaterial;
    private GameObject textInteract;

    private List<Interactable> interactablesInRange = new List<Interactable>();
    private Dictionary<Renderer, List<Material>> originalMaterials = new Dictionary<Renderer, List<Material>>();
    bool isInArmario = false;
    Interactable lastArmario = null;


    private void Start()
    {
        textInteract = GameObject.Find("TextInteract");
        textInteract.SetActive(false);
    }

    private void Update()
    {
        if (isInArmario)
        {
            textInteract.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E)) { 
                lastArmario.InteractArmario();
                isInArmario = lastArmario.ArmarioState();
                lastArmario = null;
            }
        }

        else if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, interactionRange, interactableLayer))
        {
            textInteract.SetActive(true);

            Interactable interactableObject = hit.collider.GetComponent<Interactable>();

            if (interactableObject != null && !interactablesInRange.Contains(interactableObject))
            {
                interactablesInRange.Add(interactableObject);
                UpdateMaterials(interactableObject, true);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                
                 
                foreach (Interactable interactable in interactablesInRange)
                {
                    if (interactable.CompareTag("Pila"))
                    {
                        interactable.InteractPila();
                    }

                    if (interactable.CompareTag("Door") && interactable.isDoor)
                    {
                        interactable.InteractDoor();
                    }

                    if (interactable.CompareTag("Door") && interactable.isObstacle)
                    {
                        interactable.InteractObstacle();
                    }

                    if(interactable.CompareTag("Cajón"))
                    {
                        interactable.InteractCajon();
                    }

                    if (interactable.CompareTag("Key"))
                    {
                        interactable.InteractKey();
                    }

                    if (interactable.gameObject.name == "LinternaInteract")
                    {
                        interactable.InteractLinterna();
                    }

                    if (interactable.gameObject.name == "MóvilInteract")
                    {
                        interactable.InteractMovil();
                    }

                    if (interactable.CompareTag("Note"))
                    {
                        interactable.InteractNote();
                    }
                    else
                    {
                        lastArmario = interactable;
                        interactable.InteractArmario();
                        isInArmario = interactable.ArmarioState();
                    }
                }
                

            }
        }
        else
        {
            textInteract.SetActive(false);

            foreach (Interactable interactable in interactablesInRange)
            {
                UpdateMaterials(interactable, false);
            }

            interactablesInRange.Clear();
        }
    }

    private void UpdateMaterials(Interactable interactable, bool applyMaterial)
    {
        Renderer[] renderers = interactable.GetComponentsInChildren<Renderer>(true);

        foreach (Renderer renderer in renderers)
        {
            if (!originalMaterials.ContainsKey(renderer))
            {
                originalMaterials.Add(renderer, new List<Material>(renderer.materials));
            }

            List<Material> materials = originalMaterials[renderer];

            if (applyMaterial)
            {
                Material clonedMaterial = new Material(interactableMaterial);
                clonedMaterial.SetFloat("_Scale", 1.05f);
                materials.Add(clonedMaterial);
            }
            else
            {
                // Remove the last material (the one added during interaction)
                if (materials.Count > 0)
                {
                    Material lastMaterial = materials[materials.Count - 1];
                    materials.Remove(lastMaterial);
                    Destroy(lastMaterial); // Destroy the cloned material
                }
            }

            renderer.materials = materials.ToArray();
        }
    }


    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.blue;
        //Gizmos.DrawLine(transform.position, transform.position + playerCamera.forward * interactionRange);
    }
}

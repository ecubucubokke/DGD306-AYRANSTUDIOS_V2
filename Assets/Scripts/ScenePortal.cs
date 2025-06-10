using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Add for canvas utilities
using System.Collections;

/// <summary>
/// Attach this script to a trigger collider (2D or 3D) that represents the portal.
/// When the player enters the trigger, it will request the GameManager to load the designated scene.
/// </summary>
public class ScenePortal : MonoBehaviour
{
    [Tooltip("Name of the target scene that will be loaded when the player enters the portal.")]
    [SerializeField] private string targetSceneName;

    [Tooltip("Optional transition panel prefab (UI) that will be brought on screen before scene loads.")]
    [SerializeField] private GameObject transitionPanelPrefab;

    [Tooltip("Delay (in seconds) to wait after showing the panel before the scene is loaded.")]
    [SerializeField] private float transitionDelay = 1f;

    private GameObject transitionPanelInstance;
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        HandlePortalEnter();
    }

    // Overload for 3D physics in case user uses 3D colliders.
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        HandlePortalEnter();
    }

    private void HandlePortalEnter()
    {
        if (hasTriggered) return;
        hasTriggered = true;

        if (string.IsNullOrEmpty(targetSceneName))
        {
            Debug.LogError($"ScenePortal on {name}: Target scene name is not set!");
            return;
        }

        // Show transition panel if provided
        if (transitionPanelPrefab != null)
        {
            if (transitionPanelInstance == null)
            {
                Canvas canvas = FindObjectOfType<Canvas>();
                if (canvas == null)
                {
                    // Create a simple overlay canvas if none exists
                    GameObject canvasObj = new GameObject("Transition Canvas");
                    canvas = canvasObj.AddComponent<Canvas>();
                    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    canvasObj.AddComponent<CanvasScaler>();
                    canvasObj.AddComponent<GraphicRaycaster>();
                }
                transitionPanelInstance = Instantiate(transitionPanelPrefab, canvas.transform);
            }
            else
            {
                transitionPanelInstance.SetActive(true);
            }
        }

        StartCoroutine(DelayedTransition());
    }

    private IEnumerator DelayedTransition()
    {
        yield return new WaitForSecondsRealtime(transitionDelay);
        GameManager.Instance.TransitionToScene(targetSceneName);
    }
} 
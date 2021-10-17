using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TouchableCharacters : MonoBehaviour
{
    public LayerMask citizenLayerMask;
    public RectTransform marker;
    public GameObject submitButton;
    public GameObject winStateCanvas;
    public GameObject loseStateCanvas;
    public int maxLives = 3;

    private bool selectedIsImpostor;
    private NPCScript npc;
    public void SelectedIsImpostor() {
        if (selectedIsImpostor == true) {
            winStateCanvas.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            maxLives--;
            if (maxLives < 1) {
                loseStateCanvas.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }

    public void ChangeScene() {
        SceneManager.LoadScene("WerlcomePlayerScene");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x - 48, Input.mousePosition.y - 96, 10));
            RaycastHit2D hit = Physics2D.CircleCast(point, .75f, Vector2.one, Mathf.Infinity, citizenLayerMask);
            Debug.Log("Click");
            if (hit)
            {
                Debug.Log("Hit");
                npc = hit.rigidbody.gameObject.GetComponent<NPCScript>();
                selectedIsImpostor = npc.isImpostor;
                marker.gameObject.SetActive(true);
                submitButton.SetActive(true);
            }
        }
        if (npc)
        {
            marker.position = Camera.main.WorldToScreenPoint(npc.gameObject.transform.position);
            Debug.Log("Moving to: " + marker.position);
        }
    }
}

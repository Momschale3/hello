using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private Transform rightHand;
    [SerializeField]
    private Transform rightHandDirection;

    [Space(10)]

    [SerializeField]
    private float rayCastDistance;
    [SerializeField]
    private float rayWidth;

    [Space(10)]

    [SerializeField]
    private LayerMask buttonLM;


    //Privates
    private LineRenderer lineRenderer;

    private void Start()
    {
        SetupRay();
    }

    void SetupRay()
    {
        this.gameObject.GetComponent<LineRenderer>();

        lineRenderer.useWorldSpace = false;
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = rayWidth;
        lineRenderer.endWidth = rayWidth;
      
       
    }

    private void Update()
    {
        Vector3 dir = rightHandDirection.position - rightHand.position;
        Debug.DrawRay(rightHand.position, dir* rayCastDistance, Color.yellow);
        lineRenderer.SetPosition(0, rightHand.position);
        lineRenderer.SetPosition(1, dir * rayCastDistance);  
    }

    public void castRay()
    {
        Debug.LogWarning("Menu Ray hit");
        RaycastHit hit;
        Vector3 dir = rightHandDirection.position - rightHand.position;

        if(Physics.Raycast(transform.position, dir, out hit, rayCastDistance, buttonLM))
        {
            Debug.LogWarning("IHHHHHHHH");
            hit.collider.gameObject.GetComponent<MenuButton>().OnActivated();
        }
    }  
    
    public void OpenMenu(GameObject closeThis, GameObject openThis)
    {
        closeThis.SetActive(false);
        openThis.SetActive(true);
    }

    public void LoadLevel(int levelToLoad)
    {
        SceneManager.LoadScene(levelToLoad);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ONE()
    {
        Debug.LogWarning("HELLO ME BUTTON");
    }

    public void TWO()
    {
        Debug.LogWarning("NONONO SUNNSE");
    }

}

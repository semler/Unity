using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeCamera : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public bool Change = false;
    GameObject mainCamera;

    public void OnPointerDown(PointerEventData eventData)
    {
        //mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z * -1);
        //mainCamera.transform.rotation = new Quaternion(mainCamera.transform.rotation.x, 180, mainCamera.transform.rotation.z, mainCamera.transform.rotation.w);
        Change = !Change;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z * -1);
        //mainCamera.transform.rotation = new Quaternion(mainCamera.transform.rotation.x, 0, mainCamera.transform.rotation.z, mainCamera.transform.rotation.w);
    }

    // Use this for initialization
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


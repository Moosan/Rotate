using UnityEngine;
using UnityEngine.EventSystems;
public class RotateUI : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    [SerializeField]
    private GameObject target;
    private bool isRotating;
    private bool isMoving;
    private Vector3 hitPoint;
    private Vector3 axis;
    private Vector3 lastMousePosition;
    private Vector3 cross;
    private Quaternion startAngle;
    private Vector3 startPosition;
    [SerializeField]
    private Material[] colors;
    private string objname;
    private int colorIndex;
    private new char[] name;
    private MeshRenderer[] renderers;
    void Awake() {
        isRotating = false;
        renderers = new MeshRenderer[0];
    }

    void Update() {
        if (isRotating) {
            Vector3 deltaMousePosition = Input.mousePosition - lastMousePosition;
            Vector3 mr = deltaMousePosition.normalized;
            float size = new Vector2(deltaMousePosition.x,deltaMousePosition.y).magnitude;
            float dot = Vector2.Dot(new Vector2(cross.x, cross.y).normalized, new Vector2(mr.x,mr.y).normalized);
            Quaternion rotateAngle = Quaternion.AngleAxis(size * dot, axis);
;           transform.rotation = rotateAngle * startAngle;
            target.transform.rotation =  rotateAngle * startAngle;
        }
        if (isMoving) {
            Vector3 deltaMousePosition = Input.mousePosition - lastMousePosition;
            Vector3 mr = deltaMousePosition.normalized;
            float size = new Vector2(deltaMousePosition.x, deltaMousePosition.y).magnitude/100;
            float dot = Vector2.Dot(new Vector2(axis.x,axis.y).normalized, new Vector2(mr.x, mr.y).normalized);
            Vector3 movePosition = axis.normalized * size * dot;
            transform.position = startPosition + movePosition;
            target.transform.position = startPosition + movePosition;
        }
    }
    public void OnPointerDown(PointerEventData data)
    {
        n = data.pointerPressRaycast.gameObject;
        name = n.name.ToCharArray();
        lastMousePosition = Input.mousePosition;
        hitPoint = data.pointerPressRaycast.worldPosition;
        
        axis = GetAxis(name[0]);
        if (name[1] == 'r')
        {
            cross = Vector3.Cross(axis, hitPoint);
            startAngle = target.transform.rotation;
            renderers = new MeshRenderer[] { n.GetComponent<MeshRenderer>()};
            isRotating = true;
        }
        else if (name[1] == 'a') {
            startPosition = target.transform.position;
            isMoving = true;
            renderers = n.transform.GetComponentsInChildren<MeshRenderer>();
        }
        foreach (var m in renderers)
        {
            m.material = colors[3];
        }
    }
    private GameObject n;
    public void OnPointerUp(PointerEventData eventData) {
        if (!isRotating&&!isMoving) return;
        foreach (var m in renderers)
        {
            m.material = colors[ColorIndex(name[0])];
        }
        isRotating = false;
        isMoving = false;
    }
    public Vector3 GetAxis(char n){
        if (n == 'x') return transform.right;
        if (n == 'y') return transform.up;
        if (n == 'z') return transform.forward;
        return Vector3.zero;
    }
    public int ColorIndex(char n)
    {
        if (n == 'x') return 0;
        if (n == 'y') return 1;
        if (n == 'z') return 2;
        else return 3;
    }
}
using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    public bool IsActive = false;
    public Canvas canvas;

    public TMP_Text NameField;
    public TMP_Text DescriptionField;
    public TMP_Text CostField;

    private Camera _cam;
    private Vector3 _min, _max;
    private RectTransform _rect;
    public float _offset = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        canvas.enabled = false;
        _cam = Camera.main;
        _rect = GetComponent<RectTransform>();
        _min = new Vector3(0, 0, 0);
        _max = new Vector3(_cam.pixelWidth, _cam.pixelHeight, 0);
    }

    public void SetActive(bool active)
    {
        canvas.enabled = active;
        IsActive = active;
    }

    public void PassData(string title, string description, string lineInfo)
    {
        NameField.text = title;
        DescriptionField.text = description;
        CostField.text = lineInfo;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsActive)
        {
            Vector3 mousePos = Input.mousePosition;
            bool flipX = (_max.x - mousePos.x) < _rect.rect.width;

            //get the tooltip position with offset
            Vector3 position = new Vector3(mousePos.x + ((flipX ? -1 : 1) * _rect.rect.width) / 1.5f, mousePos.y - (_rect.rect.height / 4 + _offset), 0f);
            //clamp it to the screen size so it doesn't go outside
            transform.position = new Vector3(Mathf.Clamp(position.x, _min.x + _rect.rect.width/2, _max.x - _rect.rect.width/2), Mathf.Clamp(position.y, _min.y + _rect.rect.height / 2, _max.y - _rect.rect.height / 2), transform.position.z);
        }
             
    }
}

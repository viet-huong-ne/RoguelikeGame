using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpDamage : MonoBehaviour
{    
    public float lifetime = 1f;
    public float floatSpeed = 300f;
    public TextMeshProUGUI textMesh;
    float timeElapsed = 0.0f;
    RectTransform rTransform;
    public Vector3 floatDirection = new Vector3(0, 1, 0);
    Color startingColor;

    // Start is called before the first frame update
    void Start()
    {
        rTransform = GetComponent<RectTransform>();
        startingColor = textMesh.color;
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        rTransform.position += floatSpeed * floatDirection * Time.deltaTime;

        textMesh.color = new Color(startingColor.r, startingColor.g, startingColor.b, 1 - (timeElapsed / lifetime));

        if(timeElapsed > lifetime){
            Destroy(gameObject);
        }
    }
}

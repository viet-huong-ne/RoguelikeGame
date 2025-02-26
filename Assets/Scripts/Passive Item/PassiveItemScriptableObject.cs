using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PassiveItemScriptableObject", menuName = "ScriptableObjects/")]
public class PassiveItemScriptableObject : ScriptableObject
{
    [SerializeField]
    float multipler;
    public float Multipler { get => multipler; set => multipler = value;  }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

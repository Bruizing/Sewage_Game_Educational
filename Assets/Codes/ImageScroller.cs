using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Scrolls an image to the right or left
//works with 3D objs and only one 
public class ImageScroller : MonoBehaviour
{
    [Range(-5f, 5f)] //Left or Right
    public float ScrollSpeed = 0f;

    private float offset;//how far Material moves or shifts to the left
    private Material Mat;

    // Start is called before the first frame update
    void Start()
    {
        Mat = GetComponent<Renderer>().material;
    }
    // Update is called once per frame
    void Update()
    {
        offset += (Time.deltaTime * ScrollSpeed) / 10f;//(time * value) / amount of slowing down
        Mat.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}
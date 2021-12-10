using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ProgressBar : MonoBehaviour
{
    public int I_Min;

    public int I_Max;
    public int I_Current;
    public Image Barr;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetCurrentFill();
    }

    void GetCurrentFill()
	{
        float currentOffset = I_Current - I_Min;
        float maxiOffset = I_Max - I_Min;
        float fillAmount = currentOffset / maxiOffset;
        Barr.fillAmount = fillAmount;

	}
}

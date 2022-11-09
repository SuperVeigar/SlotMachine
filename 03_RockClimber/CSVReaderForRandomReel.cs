using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSVReaderForRandomReel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void LoadRandomReels(string fileName, out int[] reel_0, out int[] reel_1, out int[] reel_2, out int[] reel_3, out int[] reel_4)
    {
        StreamReader reader = new StreamReader(Application.dataPath + "/Resources/" + fileName + ".csv");

        string lineData = reader.ReadLine();
        string[] arrayData = lineData.Split(',');

        reel_0 = new int[arrayData.Length];
        for(int i = 0; i < arrayData.Length; i++)
        {
            reel_0[i] = int.Parse(arrayData[i]);
        }

        lineData = reader.ReadLine();
        arrayData = lineData.Split(',');

        reel_1 = new int[arrayData.Length];
        for (int i = 0; i < arrayData.Length; i++)
        {
            reel_1[i] = int.Parse(arrayData[i]);
        }

        lineData = reader.ReadLine();
        arrayData = lineData.Split(',');

        reel_2 = new int[arrayData.Length];
        for (int i = 0; i < arrayData.Length; i++)
        {
            reel_2[i] = int.Parse(arrayData[i]);
        }

        lineData = reader.ReadLine();
        arrayData = lineData.Split(',');

        reel_3 = new int[arrayData.Length];
        for (int i = 0; i < arrayData.Length; i++)
        {
            reel_3[i] = int.Parse(arrayData[i]);
        }

        lineData = reader.ReadLine();
        arrayData = lineData.Split(',');

        reel_4 = new int[arrayData.Length];
        for (int i = 0; i < arrayData.Length; i++)
        {
            reel_4[i] = int.Parse(arrayData[i]);
        }
    }
}

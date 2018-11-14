using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class ObjectNode : MonoBehaviour {
    public static HashSet<int> indexList = new HashSet<int>();
    private static Color[] baseColorList = new Color[] {Color.cyan, Color.green, Color.red, Color.black, Color.yellow, Color.blue, Color.magenta, Color.gray, Color.white, Color.grey };
    private static List<Color> colorList = new List<Color>();
    public int index;
    public List<ObjectNode> Connects;
    private Color color = Color.black;

    private void Awake()
    {
        SetColor();
        string pattern = @"(\d+)";
        Match m = Regex.Match(name, pattern, RegexOptions.IgnoreCase);
        if (m.Success)
        {
            index = int.Parse(m.Value);
            if(!indexList.Add(index)) throw new System.Exception("Duplicate index");
        }
        else {
            throw new System.Exception("No index");
        }
    }

    private void SetColor()
    {
        if (colorList.Count == 0) {
            foreach (var c in baseColorList) colorList.Add(c);
        }
        int i = Random.Range(0, colorList.Count);
        color = colorList[i];
        colorList.RemoveAt(i);
    }

    private void OnDrawGizmos()
    {
        Handles.Label(transform.position, index.ToString());
        Gizmos.color = color;
        for (int i = 0; i < Connects.Count; i++)
        {
            Gizmos.DrawLine(transform.position, Connects[i].transform.position);
        }
    }
}

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class DataList
{
    public List<weather> weather =  new List<weather>();
    public main main;
    public coord coord;
}

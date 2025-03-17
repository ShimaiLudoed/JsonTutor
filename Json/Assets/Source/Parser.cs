using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Newtonsoft.Json;

public class CellData
{
    public int CellId { get; private set; }
    public int CellType { get; private set; }

    public CellData(int cellID, int cellType)
    {
        CellId = cellID;
        CellType = cellType;
    }
}

public class CellFieldData
{
    public List<CellData> Data { get; private set; } = new();
}

public static class Parser
{
    private const string _csvFileName = "FieldConfig.csv";
    private const string _jsonFileName = "FieldConfig_JSON.txt";
    private const string _resourcesFolder = "Pres/Res";

    [MenuItem("Parse Tools/Parse CSV to Json")]

    public static void ParseCSV()
    {
        string csvPath = Path.Combine(Application.dataPath,_resourcesFolder ,_csvFileName);
        if(!File.Exists(csvPath))
        {
            Debug.Log(Application.dataPath);
            Debug.LogError($"No {_csvFileName} file in Res");
            return;
        }

        CellFieldData data = new CellFieldData();
        string[] lines = File.ReadAllLines(csvPath);

        for(int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');

            //Хардкод при условии, что мы знаем количество колонок и их название:

            string cellid = values[0].Trim(); //1 column from sheet
            string cellType = values[1].Trim(); //2 column from sheet

            CellData cellData= new(int.Parse(cellid), int.Parse(cellType));
            data.Data.Add(cellData);
        }
        foreach(CellData cellData in data.Data)
        {
            Debug.Log($"{cellData.CellId}: {cellData.CellType}");
        }

        string jsonPath= Path.Combine(Application.dataPath, _resourcesFolder, _jsonFileName);
        string json = JsonConvert.SerializeObject(data);
        File.WriteAllText(jsonPath, json);
    }

    [MenuItem("Parse Tools/Parse CSV to Json")]

    public static bool GetDataFromJSON(out CellFieldData cellFieldData)
    {
        cellFieldData = new();
        string jsonPath = Path.Combine(Application.dataPath, _resourcesFolder, _jsonFileName);
        if (!File.Exists(jsonPath))
        {
            Debug.LogError($"{_jsonFileName} file in Res");
            return false;
        }

        string lines = File.ReadAllText(jsonPath);
        cellFieldData=JsonConvert.DeserializeObject<CellFieldData>(lines);
        foreach (CellData cellData in cellFieldData.Data)
        {
            Debug.Log("all god");
        }
        return true;
    }
}

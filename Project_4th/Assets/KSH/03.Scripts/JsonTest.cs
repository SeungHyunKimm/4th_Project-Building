using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

[SerializeField]
public struct UserInfo
{
    public int age;
    public int height;
    public string name;
    public int weight;
    public string[] family;

    public int ID;
    public int Level;
    public string Class;
}


public class JsonTest : MonoBehaviour
{
    public enum PlayerInformation
    {
        ID,
        Level,
        Class,

    }
    public PlayerInformation PI;

    public UserInfo info = new UserInfo();

    int[] age = new int[10];
    int[] height = new int[10];
    string[] name = new string[10];
    int[] weight = new int[10];
    string[] family = new string[10];


    void Start()
    {



    }

    void inputdata()

    {
        info.age = 10;
        info.height = 175;
        info.name = "김승현";
        info.weight = 65;
        info.family[0] = "아버지";
        info.family[1] = "어머니";
        info.family[2] = "나";


    }

    void SaveData()
    {

        string JsonData = JsonUtility.ToJson(info, true);
        print(JsonData);

        FileStream FS = new FileStream(Application.dataPath + "/myinfo.txt", FileMode.Create);
        byte[] byteData = Encoding.UTF8.GetBytes(JsonData);
        FS.Write(byteData, 0, byteData.Length);
        FS.Close();

    }


    void LoadData()
    {
        FileStream FS = new FileStream(Application.dataPath + "/myinfo.txt", FileMode.Open);
        byte[] byteData = new byte[FS.Length];
        FS.Read(byteData, 0, byteData.Length);
        FS.Close();

        string jsonData = Encoding.UTF8.GetString(byteData);

        info = JsonUtility.FromJson<UserInfo>(jsonData);

        print(info);


    }







    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SaveData();
            print("저장 완료");

        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            LoadData();
            print("읽기 완료");
        }

    }
}

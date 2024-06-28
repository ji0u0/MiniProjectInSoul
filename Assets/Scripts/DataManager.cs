using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private static DataManager _instance;
    public static DataManager I
    {
        get
        {
            if(_instance == null)
            {
                GameObject dataManager = new GameObject();
                dataManager.name = "DataManager";
                _instance = dataManager.AddComponent<DataManager>();
            }
            return _instance;
        }
    }

    // 테이블 묶음을 관리할 DataSet 변수
    private DataSet _database;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

        public void InitDataManager()
    {
        
    }
}

public class DataSet
{
    public string Name;
    public string PhoneNumber;

    public DataSet(string name, string phoneNumber)
    {
        Name = name;
        PhoneNumber = phoneNumber;
    }
}

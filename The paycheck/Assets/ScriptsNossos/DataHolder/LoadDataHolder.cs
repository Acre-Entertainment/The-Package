using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class LoadDataHolder : MonoBehaviour
{
    private DataHolder gameDataHolder;
    private DataHolderSavefile savedDataHolder;
    void Start()
    {
        string savePath = Application.persistentDataPath + "/ThePackage.fit";
        if(File.Exists(savePath))
        {
            gameDataHolder = GameObject.FindGameObjectWithTag("DataHolder").GetComponent<DataHolder>();
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(savePath, FileMode.Open);
            savedDataHolder = binaryFormatter.Deserialize(fileStream) as DataHolderSavefile;
            transferSaveData();
            fileStream.Close();
        }
    }
    private void transferSaveData()
    {
            gameDataHolder.completedSecondLevel = savedDataHolder.completedSecondLevel;
            gameDataHolder.completedThirdLevel = savedDataHolder.completedThirdLevel;
    }
}

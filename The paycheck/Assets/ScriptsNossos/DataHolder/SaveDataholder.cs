using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveDataholder : MonoBehaviour
{
    private DataHolder dataHolder;
    private DataHolderSavefile dataHolderSavefile;
    void Start()
    {
        dataHolder = GameObject.FindGameObjectWithTag("DataHolder").GetComponent<DataHolder>();
        dataHolderSavefile = new DataHolderSavefile();
        transferData();
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        string savePath = Application.persistentDataPath + "/ThePackage.fit";
        FileStream fileStream = new FileStream(savePath, FileMode.Create);
        binaryFormatter.Serialize(fileStream, dataHolderSavefile);
        fileStream.Close();
    }
    private void transferData()
    {
            dataHolderSavefile.completedSecondLevel = dataHolder.completedSecondLevel;
            dataHolderSavefile.completedThirdLevel = dataHolder.completedThirdLevel;
    }
}

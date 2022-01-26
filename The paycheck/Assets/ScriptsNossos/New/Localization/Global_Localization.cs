using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class Global_Localization
{
    public static readonly string[] languages = new string[] 
    {
        "pt_Br",
        "en_Us",
        "es"
    };

    static string current_Language = "pt_Br";
    public static string Current_Language {get => current_Language;}
    static int current_Language_Index = 0;

    public static void Change_Current_Language(string new_Language)
    {
        for(int i = 0; i < languages.Length; i++)
        {
            if(languages[i] == new_Language)
            {
                current_Language = new_Language;
                current_Language_Index = i;
                return;
            }
        }

        Debug.Log(new_Language + " is not a valid language");
    }

    public static string Translated_Text(Simple_Localization simple_Localization)
    {
        //Debug.Log(current_Language_Index);
        //Debug.Log(current_Language);
        return simple_Localization.text_Localized[current_Language_Index];
    }

    public static void Text_To_Dialogue(Dialogue_Localization dialogue_Localization, out string[] names, out string[] lines)
    {
        // Pegar o idioma certo
        string dialogueText = dialogue_Localization.localizations[current_Language_Index].dialogue;
     
        if(dialogueText.Length <= 1)
        {
            Debug.LogError(("The " + dialogue_Localization.name + " don't have a valid information"));
            names = null;
            lines = null;
            return;
        }

        // Tirando o excesso
        dialogueText = dialogueText.Substring(1);
        // Divindo cada linha
        string[] split_Text = dialogueText.Split('-');
        int number_Of_Lines = split_Text.Length;

        lines = new string[number_Of_Lines];
        names = new string[number_Of_Lines];

        int string_Separation_Pos = 0;

        // Separando "Falas" de "Nomes&Emoções"
        for(int i = 0; i < number_Of_Lines; i++)
        {
            string_Separation_Pos = split_Text[i].IndexOf("\n");
            // Personagem e emoção
            names[i] = split_Text[i].Substring(0, string_Separation_Pos);
            // Texto do dialog
            lines[i] = split_Text[i].Substring(string_Separation_Pos + 1);
        }
    }

    #region Save Localization Settings

    public static void Save(Localization_Data data)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/localization_Data.save");
        binaryFormatter.Serialize(file, data);
        file.Close();
    }

    public static Localization_Data Load()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        if (!File.Exists(Application.persistentDataPath + "/localization_Data.save"))
        {
            Debug.LogError("File path does not exist.");
            return null;
        }

        FileStream file = File.Open(Application.persistentDataPath + "/localization_Data.save", FileMode.Open);
        Localization_Data data = (Localization_Data)binaryFormatter.Deserialize(file);
        file.Close();

        return data;
    }

    #endregion

}

[System.Serializable]
public class Localization_Data
{
    public string current_Language;
}
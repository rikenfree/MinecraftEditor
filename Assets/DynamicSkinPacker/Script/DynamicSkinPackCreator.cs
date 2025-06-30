using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using Main.View;
using System.Collections;
using Assets.SimpleZip;
using Newtonsoft.Json;
using Main.Controller;
using SuperStarSdk;

public class DynamicSkinPackCreator : MonoBehaviour
{

    public TextMeshProUGUI SkinSelectedText;
    public GameObject DaynamicPanel;
    public string SkinPackDataString
    {
        get
        {
            return (PlayerPrefs.GetString("SkinPackDataString", ""));
        }
        set
        {
            PlayerPrefs.SetString("SkinPackDataString", value);
        }
    }

    public int SkinVersioningManifest
    {
        get
        {
            return (PlayerPrefs.GetInt("SkinVersioningManifest", 1));
        }
        set
        {
            PlayerPrefs.SetInt("SkinVersioningManifest", value);
        }
    }

    public int SkinPackIndexInt
    {
        get
        {
            return (PlayerPrefs.GetInt("SkinPackIndexInt", 0));
        }
        set
        {
            PlayerPrefs.SetInt("SkinPackIndexInt", value);
        }
    }

    public static DynamicSkinPackCreator Instance;


    string DIRECTPACKFOLDER = "SkinEditorMCPE/DirectPack";
    string EXPORTPACKFOLDER = "SkinEditorMCPE/ExportPack";
    string MAINDOWNLOADPATHDIRECT;
    string MAINDOWNLOADPATHEXPORT;

    string ANDROIDPERSISTANTDOWNLOADPATHDIRECT;
    string ANDROIDPERSISTANTDOWNLOADPATHEXPORT;


    public ManifestRoot manifestRootDirect;
    public ManifestRoot manifestRootExport;
    // public string MAINPERSISTANTPATH;


    private void OnEnable()
    {

        ANDROIDPERSISTANTDOWNLOADPATHDIRECT = Path.Combine(Application.persistentDataPath, DIRECTPACKFOLDER);
        ANDROIDPERSISTANTDOWNLOADPATHEXPORT = Path.Combine(Application.persistentDataPath, EXPORTPACKFOLDER);

#if UNITY_EDITOR || UNITY_IOS
        MAINDOWNLOADPATHDIRECT = Path.Combine(Application.persistentDataPath, DIRECTPACKFOLDER);
#elif UNITY_ANDROID
        
        MAINDOWNLOADPATHDIRECT ="/storage/emulated/0/Download/"+DIRECTPACKFOLDER;

#endif

#if UNITY_EDITOR || UNITY_IOS
        MAINDOWNLOADPATHEXPORT = Path.Combine(Application.persistentDataPath, EXPORTPACKFOLDER);
#elif UNITY_ANDROID
        MAINDOWNLOADPATHEXPORT ="/storage/emulated/0/Download/"+EXPORTPACKFOLDER;

#endif

        //import or share zip path
        if (!Directory.Exists(MAINDOWNLOADPATHDIRECT))
        {
            Debug.Log("CREATE PATH : " + MAINDOWNLOADPATHDIRECT);
            Directory.CreateDirectory(MAINDOWNLOADPATHDIRECT);
        }

        if (!Directory.Exists(MAINDOWNLOADPATHEXPORT))
        {
            Debug.Log("CREATE PATH : " + MAINDOWNLOADPATHEXPORT);

            Directory.CreateDirectory(MAINDOWNLOADPATHEXPORT);
        }
        if (!Directory.Exists(ANDROIDPERSISTANTDOWNLOADPATHDIRECT))
        {
            Debug.Log("CREATE PATH : " + ANDROIDPERSISTANTDOWNLOADPATHDIRECT);

            Directory.CreateDirectory(ANDROIDPERSISTANTDOWNLOADPATHDIRECT);
        }

        if (!Directory.Exists(ANDROIDPERSISTANTDOWNLOADPATHEXPORT))
        {
            Debug.Log("CREATE PATH : " + ANDROIDPERSISTANTDOWNLOADPATHEXPORT);

            Directory.CreateDirectory(ANDROIDPERSISTANTDOWNLOADPATHEXPORT);
        }

        ResetData();
    }

    private void Awake()
    {
        Instance = this;
    }

    public TMP_InputField if_skinPackName;
    public TMP_InputField if_skinName;
    public TMP_InputField if_versionNumber;

    public TextMeshProUGUI txt_imageName;


    public GameObject go_verionField;
    public GameObject go_skinPack;
    public GameObject renamePanel;




    private string packDirectory;
    private string finalPackDirectory;
    private string textPackDirectory;
    public int currentSelectedIndex = 0;
    public SkinDataRoot skinOldDataRoot;
    public SkinDataRoot skinDataRoot;
    public List<TextureReference> allSelectedImages = new List<TextureReference>();

    // public string manifestString = "";
    public string textdatastring = "";


    public TMP_InputField if_skinPackNameRename;
    public TMP_InputField if_skinNameRename;
    public TMP_InputField if_versionNumberRename;
    public Character character;




    public void DirectImportToMCPE()
    {

        Debug.Log("DirectImportToMCPE");
        ResetData();
        if_skinPackName.text = "SkinEditorMCPEDirect";
        if_skinName.text = "SkinMCPE_" + SkinPackIndexInt;
        if_versionNumber.text = "1";
        DirectPickImage(character.skin);
        AddAnotherSkin();
       DownloadPack(1, 0);
    }

    public void ShareDirect()
    {
        Debug.Log("DirectImportToMCPE");
        ResetData();
        if_skinPackName.text = "SkinEditorMCPE";
        if_skinName.text = "SkinMCPE_" + SkinPackIndexInt;
        if_versionNumber.text = "1";
        DirectPickImage(character.skin);
        AddAnotherSkin();
        DownloadPack(2);
    }

    public void OnCloseSaveCharacterRename()
    {
        SoundController.instance.PlayClickSound();
        renamePanel.SetActive(false);
    }

    public void OpenSaveCharacterRename()
    {
        SoundController.instance.PlayClickSound();
        renamePanel.SetActive(true);
    }

    public void RenameAndDirectImportToMCPE()
    {
        Debug.Log("RenameAndDirectImportToMCPE");
        SoundController.instance.PlayClickSound();
        ResetData();
        if (!string.IsNullOrEmpty(if_skinPackNameRename.text))
        {

        }
        else
        {
            Debug.LogError("Please Add Skin Pack Name");
            return;
        }

        if (!string.IsNullOrEmpty(if_skinNameRename.text))
        {

        }
        else
        {
            Debug.LogError("Please Add Skin Name");
            return;
        }

        if (!string.IsNullOrEmpty(if_versionNumberRename.text))
        {
            manifestRootExport.header.version = new List<int>
        {
            1,
            0,
            0
        };
            //DebugManifestFile(int.Parse(if_versionNumberRename.text), if_skinNameRename.text);
        }
        else
        {
            Debug.LogError("Please Add Version of MCPE");
            return;
        }

        if_skinPackName.text = if_skinNameRename.text;
        if_skinName.text = if_skinNameRename.text;
        if_versionNumber.text = if_versionNumberRename.text;
        DirectPickImage(character.skin);
        AddAnotherSkin();
       DownloadPack(1);
    }

    public void ResetData()
    {

        SkinSelectedText.text = "File Added : ";
        if (!string.IsNullOrEmpty(SkinPackDataString))
        {
            skinOldDataRoot = JsonConvert.DeserializeObject<SkinDataRoot>(SkinPackDataString);
        }

        string headerUUIDDirect = Guid.NewGuid().ToString();
        string moduleUUIDDirect = Guid.NewGuid().ToString();

        manifestRootDirect.header.uuid = headerUUIDDirect;
        manifestRootDirect.modules[0].uuid = moduleUUIDDirect;

        string headerUUIDExport = Guid.NewGuid().ToString();
        string moduleUUIDExport = Guid.NewGuid().ToString();

        manifestRootExport.header.uuid = headerUUIDExport;
        manifestRootExport.modules[0].uuid = moduleUUIDExport;



        if_skinPackName.text = string.Empty;
        if_skinName.text = string.Empty;
        if_versionNumber.text = string.Empty;
        txt_imageName.text = "No file location";
        currentSelectedIndex = 0;
        allSelectedImages.Clear();
        skinDataRoot = new SkinDataRoot();
        skinDataRoot.skins = new List<Skin>();
        go_verionField.SetActive(true);
        go_skinPack.SetActive(true);
    }

    public void DaynamicCancelButtonClick()
    {
        SoundController.instance.PlayClickSound();
        DaynamicPanel.SetActive(false);
    }

    public void DownloadPack()
    {
        SoundController.instance.PlayClickSound();
        DownloadPack(1, 1);
    }

    public void SharePack()
    {
        SoundController.instance.PlayClickSound();
       DownloadPack(2, 1);
    }


    public void DownloadPack(int needtoOpenIndex = 0, int exportIndex = 0)
    {
        SuperStarAd.Instance.ShowForceInterstitialWithLoader((result) =>
        {
            Debug.Log("Show Intrestitial  => " + result);
            if (skinDataRoot.skins.Count <= 0)
            {
                if (allSelectedImages.Count <= 0)
                {


                    Debug.LogError("No skin Available ");

                }
                else
                {
                    AddAnotherSkin();
                    DownloadPack(needtoOpenIndex, exportIndex);
                    character.SkinIndex++;
                }


            }
            else
            {

                if (exportIndex == 0)
                {
                    packDirectory = ANDROIDPERSISTANTDOWNLOADPATHDIRECT;
                }
                else
                {
                    packDirectory = ANDROIDPERSISTANTDOWNLOADPATHEXPORT;
                }
                //skin pack name directory created
                finalPackDirectory = packDirectory + "/" + skinDataRoot.serialize_name;

                if (!Directory.Exists(finalPackDirectory))
                {
                    Directory.CreateDirectory(finalPackDirectory);

                }
                else
                {
                    if (exportIndex == 0)
                    {
                        //combine data
                    }
                    else
                    {
                        Directory.Delete(finalPackDirectory , true);
                        Directory.CreateDirectory(finalPackDirectory);
                        //remove old data
                    }
                }

                Directory.CreateDirectory(Path.Combine(finalPackDirectory, "texts"));

                textPackDirectory = finalPackDirectory + "/texts";


                if (File.Exists(Path.Combine(finalPackDirectory, "manifest.json")))
                {
                    File.Delete(Path.Combine(finalPackDirectory, "manifest.json"));
                }

                if (File.Exists(Path.Combine(finalPackDirectory, "skin.json")))
                {
                    File.Delete(Path.Combine(finalPackDirectory, "skin.json"));
                }

                if (exportIndex == 0)
                {
                    SkinVersioningManifest++;
                    manifestRootDirect.header.version[1] = SkinVersioningManifest;
                    manifestRootDirect.modules[0].version[1] = SkinVersioningManifest;
                    File.WriteAllText(Path.Combine(finalPackDirectory, "manifest.json"), JsonUtility.ToJson(manifestRootDirect));
                }
                else
                {
                    SkinVersioningManifest++;
                    manifestRootDirect.header.version[1] = SkinVersioningManifest;
                    manifestRootDirect.modules[0].version[1] = SkinVersioningManifest;
                    manifestRootExport.header.name = skinDataRoot.localization_name;
                    File.WriteAllText(Path.Combine(finalPackDirectory, "manifest.json"), JsonUtility.ToJson(manifestRootExport));

                }


                string packname = "skinpack." + skinDataRoot.localization_name;
                textdatastring = packname + "=" + skinDataRoot.localization_name + "\n";
                if (exportIndex == 0)
                {
                    for (int i = 0; i < skinOldDataRoot.skins.Count; i++)
                    {
                        skinDataRoot.skins.Add(skinOldDataRoot.skins[i]);
                    }
                }

                for (int i = 0; i < skinDataRoot.skins.Count; i++)
                {
                    textdatastring += "\t\t skin." + skinDataRoot.localization_name + "." + skinDataRoot.skins[i].localization_name + "=" + skinDataRoot.skins[i].localization_name + "\n \n";
                }


                if (!string.IsNullOrEmpty(textdatastring))
                {
                    File.WriteAllText(Path.Combine(textPackDirectory, "en_US.lang"), textdatastring);
                }
                else
                {
                    Debug.LogError("Manifest Data not found");
                }

                SkinPackDataString = JsonUtility.ToJson(skinDataRoot);
                //Skin Data Managed
                File.WriteAllText(Path.Combine(finalPackDirectory, "skins.json"), JsonUtility.ToJson(skinDataRoot));

                //image need to be copy at this position
                for (int i = 0; i < allSelectedImages.Count; i++)
                {
                    SaveTextureAsPNG(allSelectedImages[i].texture2D, allSelectedImages[i].FileName, finalPackDirectory);
                }
                string ZipPath = "";


                if (File.Exists(MAINDOWNLOADPATHDIRECT + "/" + skinDataRoot.localization_name + ".mcpack"))
                {
                    Debug.LogError("File Exxist and delete");
                    File.Delete(MAINDOWNLOADPATHDIRECT + "/" + skinDataRoot.localization_name + ".mcpack");
                  
                }
                else { 
                    Debug.LogError("File not exist and not delete");

                }

                if (exportIndex == 0)
                {
                    ZipPath = MAINDOWNLOADPATHDIRECT + "/" + skinDataRoot.localization_name + ".mcpack";
                    Zip.CompressDirectory(finalPackDirectory, ZipPath, true);

                }
                else
                {
                    ZipPath = MAINDOWNLOADPATHEXPORT + "/" + skinDataRoot.localization_name + ".mcpack";

                    Zip.CompressDirectory(finalPackDirectory, ZipPath, true);
                }


                //     Zip.CompressDirectory(finalPackDirectory, ZipPath, true);
                //  lzip.compressDir(finalPackDirectory , 0, packMCpath, false);
                // yield return new WaitForSeconds(1);
                if (needtoOpenIndex == 1)
                {
#if UNITY_EDITOR
                Debug.LogError("Unity Editor will not open it");
#elif UNITY_ANDROID
            AndroidContentOpenerWrapper.OpenContent(ZipPath);
#elif UNITY_IOS

                    new NativeShare().AddFile(ZipPath)
            .SetSubject("3DSkinEditorSkinPAck")
            .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
            .Share();
#endif
                }
                else if (needtoOpenIndex == 2)
                {

#if UNITY_ANDROID

ZipPath = MAINDOWNLOADPATHEXPORT + "/" + skinDataRoot.localization_name + ".zip";

                    Zip.CompressDirectory(finalPackDirectory, ZipPath, true);

                    new NativeShare().AddFile(ZipPath)
            .SetSubject("3DSkinEditorSkinPAck").SetText("3D Skin Editor MCPE")
            .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
            .Share();
#elif UNITY_IOS

                    new NativeShare().AddFile(ZipPath)
            .SetSubject("3DSkinEditorSkinPAck").SetText("3D Skin Editor MCPE")
            .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
            .Share();
#endif
                }
            }

        }, 3);


        
    }
    public void SaveTextureAsPNG(Texture2D texture, string filename, string fpath)
    {
        // Convert the texture to a byte array (PNG)
        byte[] imageData = texture.EncodeToPNG();


        if (filename.Contains(".png"))
        {
            string path = Path.Combine(fpath, filename);

            // Write the data to that path
            File.WriteAllBytes(path, imageData);

            Debug.Log("Saved Image to: " + path);
            // Get the path of the Persistent Data Folder
        }
        else
        {
            string path = Path.Combine(fpath, filename + ".png");

            // Write the data to that path
            File.WriteAllBytes(path, imageData);

            Debug.Log("Saved Image to: " + path);
        }

    }
    public void AddAnotherSkin()
    {

        //go_verionField.SetActive(false);
        //go_skinPack.SetActive(false);
        Debug.Log("AddAnotherSkin ");
        SoundController.instance.PlayClickSound();


        if (!string.IsNullOrEmpty(if_skinPackName.text))
        {
            skinDataRoot.serialize_name = if_skinPackName.text;
            skinDataRoot.localization_name = if_skinPackName.text;
          
        }
        else
        {
            Debug.LogError("Please Add Skin Pack Name");
            ToastManager.Instance.ShowTost("Please Add Skin Pack Name");
            return;
        }
        if (!string.IsNullOrEmpty(if_skinName.text))
        {

        }
        else
        {
            Debug.LogError("Please Add Skin Name");
            ToastManager.Instance.ShowTost("Please Add Skin Name");
            return;
        }

        if (!string.IsNullOrEmpty(if_versionNumber.text))
        {
            manifestRootExport.header.version = new List<int>
        {
            1,
            0,
            0
        };

        }
        else
        {
            Debug.LogError("Please Add Version of MCPE");
            ToastManager.Instance.ShowTost("Please Add Version of MCPE");
            return;
        }




        if (allSelectedImages.Count == currentSelectedIndex)
        {
            Debug.LogError("Please Select Skin Image");
            return;

            // skinDataRoot.skins.Add();
        }
        else if (allSelectedImages.Count == (currentSelectedIndex + 1))
        {
            Skin s = new Skin();
            s.texture = allSelectedImages[currentSelectedIndex].FileName;
            s.localization_name = if_skinName.text;
            s.geometry = "geometry." + if_skinPackName.text + "." + if_skinName.text;
            s.type = "free";
            skinDataRoot.skins.Add(s);
            Debug.LogWarning("One Skin Added Sucessfully !");
            SkinSelectedText.text = SkinSelectedText.text + "\t " + if_skinPackName.text;
            currentSelectedIndex++;

            //

            //go_verionField.SetActive(false);
            //go_skinPack.SetActive(false);
            txt_imageName.text = "No file location";
            if_skinName.text = string.Empty;
        }

    }


    public void DirectPickImage(Texture2D texture)
    {
        if (allSelectedImages.Count == currentSelectedIndex)
        {
            TextureReference textureReference = new TextureReference();
            textureReference.texture2D = texture;
            textureReference.FileName = "Skin_" + SkinPackIndexInt + ".png";
            allSelectedImages.Add(textureReference);
            SkinPackIndexInt++;
            Debug.Log("IMPORT_MCPE");
            RatingController.instance.succesfullPopupMCPE.SetActive(true);
        }
        else
        {
            TextureReference textureReference = new TextureReference();
            textureReference.texture2D = texture;
            textureReference.FileName = "Skin_" + SkinPackIndexInt + ".png";
            allSelectedImages[currentSelectedIndex] = textureReference;
            Debug.Log("ImportUnsccessfully");
        }
    }

    public void PickImage()
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("Image path: " + path);
            if (path != null)
            {
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path, 64, false);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }
                if (allSelectedImages.Count == currentSelectedIndex)
                {
                    TextureReference textureReference = new TextureReference();
                    textureReference.texture2D = texture;
                    textureReference.FileName = "Skin_" + SkinPackIndexInt + ".png";
                    allSelectedImages.Add(textureReference);
                    SkinPackIndexInt++;
                }
                else
                {
                    TextureReference textureReference = new TextureReference();
                    textureReference.texture2D = texture;
                    textureReference.FileName = "Skin_" + SkinPackIndexInt + ".png";
                    allSelectedImages[currentSelectedIndex] = textureReference;
                }

                txt_imageName.text = "Skin_" + SkinPackIndexInt;
                Debug.Log("Filename  " + "Skin_" + SkinPackIndexInt);

                //   HandleTexture(texture);

            }
            else {
                Debug.LogError("Path is null");
            }
        }, "Select a PNG image", "image/png");
    }


    [Serializable]
    public class SkinData
    {
        public string skinName;
        public string texturePath;
        public string type = "free"; // Default to 'free', could be 'paid' or other types
    }



    //[Button]
    //public void DebugManifestFile(int version,string skinpackname) 
    //{


    //    ManifestRoot manifestRoot = new ManifestRoot();
    //    manifestRoot.format_version = 1;


    //    //header handling
    //    ManifestHeader manifestHeader = new ManifestHeader();
    //    manifestHeader.version = new List<int>
    //    {
    //        1,
    //        0,
    //        0
    //    };
    //    manifestHeader.name = skinpackname;
    //   // manifestHeader.uuid = headerUUID;

    //    manifestRoot.header = manifestHeader;

    //    ManifestModule manifestModule = new ManifestModule();
    //    manifestModule.version = new List<int>
    //    {
    //        1,
    //        0,
    //        0
    //    };

    //    manifestModule.type = "skin_pack";
    //    manifestModule.uuid = moduleUUID;
    //    manifestRoot.modules.Add(manifestModule);
    //    manifestString = JsonUtility.ToJson(manifestRoot);
    //    Debug.Log(JsonUtility.ToJson(manifestRoot));
    //}

    [Button]
    public void DebugSkinDataFile()
    {

        Debug.Log(JsonUtility.ToJson(skinDataRoot));
    }
    //void Start()
    //{
    //    packDirectory = Path.Combine(Application.dataPath, "MySkinPack");
    //    CreateSkinPack();
    //}



    public void CreateSkinPack()
    {
        // Ensure the directory is clean
        if (Directory.Exists(packDirectory))
        {
            Directory.Delete(packDirectory, true);
        }
        Directory.CreateDirectory(packDirectory);
        Directory.CreateDirectory(Path.Combine(packDirectory, "skins"));

        // Generate UUIDs






        // Create skins.json dynamically based on provided skins
        //string skinsJsonContent = "{\n  \"skins\": [\n";
        //foreach (SkinData skin in skins)
        //{
        //    skinsJsonContent += $"    {{\n      \"localization_name\": \"{skin.skinName}\",\n      \"texture\": \"skins/{Path.GetFileName(skin.texturePath)}\",\n      \"type\": \"{skin.type}\"\n    }},\n";
        //}
        //skinsJsonContent = skinsJsonContent.TrimEnd(',', '\n') + "\n  ],\n  \"serialize_name\": \"" + packName + "\",\n  \"localization_name\": \"" + packDescription + "\"\n}";


        //File.WriteAllText(Path.Combine(packDirectory, "skins.json"), skinsJsonContent);

        //// Copy skin files to the correct directory
        //foreach (SkinData skin in skins)
        //{
        //    string destPath = Path.Combine(packDirectory, "skins", Path.GetFileName(skin.texturePath));
        //    File.Copy(skin.texturePath, destPath, true);
        //}

        // Create a ZIP from the directory
        // ZipFile.CreateFromDirectory(packDirectory, Path.Combine(Application.dataPath, $"{packName.Replace(" ", "")}.zip"));

        Debug.Log("Skin pack created successfully.");
    }
}




[Serializable]
public class ManifestHeader
{
    public string name;
    public string uuid;
    public List<int> version = new List<int>();
}
[Serializable]
public class ManifestModule
{
    public string type;
    public string uuid;
    public List<int> version = new List<int>();
}
[Serializable]
public class ManifestRoot
{
    public int format_version;
    public ManifestHeader header;
    public List<ManifestModule> modules = new List<ManifestModule>();
}

[Serializable]
public class SkinDataRoot
{
    public List<Skin> skins;
    public string serialize_name;
    public string localization_name;
}
[Serializable]
public class Skin
{
    public string localization_name;
    public string geometry;
    public string texture;
    public string type;
}
[Serializable]
public class TextureReference
{
    public Texture2D texture2D;
    public string FileName;
}
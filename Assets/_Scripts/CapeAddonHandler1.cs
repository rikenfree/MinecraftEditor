using Assets.SimpleZip;
using Main.Controller;
using SuperStarSdk;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static ImageCropper;

public class CapeAddonHandler1 : MonoBehaviour
{
    public static CapeAddonHandler1 Instance;
    [Header("Screen Reference")]
    public GameObject PickImageScreen;
    public GameObject ViewImageScreen;
    public GameObject HDTextureScreen;

    public GameObject[] TutorialScreen;
    public GameObject TutorialParent;

    [Header("Create Texture")]
    public RawImage rawImageViewCroppedImage;
    public RawImage rawImageForMakeTexture;

    public Animator CharacterAnim;

    public GameObject MainCharacter;

    public GameObject backImage;

    public bool IsCustomCapeAddon = false;

    private void Start()
    {
        IsCustomCapeAddon = true;

        if (NativeGallery.CheckPermission(NativeGallery.PermissionType.Write, NativeGallery.MediaType.Image) == NativeGallery.Permission.Denied)
        {
            NativeGallery.RequestPermission(NativeGallery.PermissionType.Write, NativeGallery.MediaType.Image);
        }
        else if (NativeGallery.CheckPermission(NativeGallery.PermissionType.Write, NativeGallery.MediaType.Image) == NativeGallery.Permission.ShouldAsk)
        {
            NativeGallery.RequestPermission(NativeGallery.PermissionType.Write, NativeGallery.MediaType.Image);
        }

        CharacterAnim.SetBool("Anim2", false);

    }
    private void Update()
    {
        if (HDTextureScreen.activeSelf)
        {
            backImage.SetActive(false);
        }
        else
        {
            backImage.SetActive(true);
        }
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public Texture2D pikedImageTexture;
    public Texture2D coppedImageTexture;
    public Texture2D coppedImageTextureForDummyModel;
    public Material bodyMaterial;


    public ScreenshotHandler ScreenshotHandler;

    public void PickImage()
    {
        SoundController1.Instance.PlayClickSound();
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path, 1024, false);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }
                else
                {

                    pikedImageTexture = texture;

                    //   ImageCropper.Instance.Show(PikedImageTexture,,);
                    //open image cropper

                    ImageCropper.Instance.Show(pikedImageTexture, (bool result, Texture originalImage, Texture2D croppedImage) =>
                    {

                        // If screenshot was cropped successfully
                        if (result)
                        {
                            // Assign cropped texture to the RawImage
                            rawImageViewCroppedImage.texture = croppedImage;
                            rawImageForMakeTexture.texture = croppedImage;
                            coppedImageTextureForDummyModel = croppedImage;
                            ViewImageScreen.SetActive(true);
                        }
                        else
                        {
                            Debug.LogError("Some issue happen so not possible to crop close cropper");
                        }
                    });
                }
            }

        }, "Select a PNG image", "image/*");
    }


    public void MakeTexture()
    {
        SuperStarAd.Instance.ShowForceInterstitialWithLoader((o) =>
        {
            SoundController1.Instance.PlayClickSound();
            ScreenshotHandler.takePics();
            //bodyMaterial.GetTexture(coppedImageTexture.ToString());
            bodyMaterial.mainTexture = coppedImageTextureForDummyModel;
            CharacterAnim.SetBool("Anim2", true);
            HDTextureScreen.SetActive(true);
            PickImageScreen.SetActive(false);
            ViewImageScreen.SetActive(false);
            MainCharacter.transform.rotation = Quaternion.identity;

        }, 3);
    }

    [Header("Create Addon")]
    public string ProductName = "CapeMCPE";
    string MAINDOWNLOADPATHDIRECT;
    //string MAINDOWNLOADPATHEXPORT;
    string DIRECTPACKFOLDER;
    //string EXPORTPACKFOLDER;

    string ANDROIDPERSISTANTDOWNLOADPATHDIRECT;
    // string ANDROIDPERSISTANTDOWNLOADPATHEXPORT;


    public ManifestRoot manifestRootDirect;
    public ManifestRoot manifestRootExport;

    public Texture2D packIconPng;
    public string playerentitydata;
    public string playerentitydata1;
    public string capeentitydata;
    public string customecapejson;


    private void OnEnable()
    {
        HandlePath();
    }

    private void HandlePath()
    {
        string DIRECTPACKFOLDER = ProductName;
        string EXPORTPACKFOLDER = ProductName;


        ANDROIDPERSISTANTDOWNLOADPATHDIRECT = Path.Combine(Application.persistentDataPath, DIRECTPACKFOLDER);
        //ANDROIDPERSISTANTDOWNLOADPATHEXPORT = Path.Combine(Application.persistentDataPath, EXPORTPACKFOLDER);

#if UNITY_EDITOR || UNITY_IOS
        MAINDOWNLOADPATHDIRECT = Path.Combine(Application.persistentDataPath, DIRECTPACKFOLDER);
#elif UNITY_ANDROID
        MAINDOWNLOADPATHDIRECT ="/storage/emulated/0/Download/"+DIRECTPACKFOLDER;

#endif

        //#if UNITY_EDITOR || UNITY_IOS
        //        MAINDOWNLOADPATHEXPORT = Path.Combine(Application.persistentDataPath, EXPORTPACKFOLDER);
        //#elif UNITY_ANDROID
        //        MAINDOWNLOADPATHEXPORT ="/storage/emulated/0/Download/"+EXPORTPACKFOLDER;

        //#endif

        Debug.Log("MAINDOWNLOADPATHDIRECT_PATH ::> " + MAINDOWNLOADPATHDIRECT);
        // Debug.Log("MAINDOWNLOADPATHEXPORT_PATH ::> " + MAINDOWNLOADPATHEXPORT);
        Debug.Log("ANDROIDPERSISTANTDOWNLOADPATHDIRECT_PATH ::> " + ANDROIDPERSISTANTDOWNLOADPATHDIRECT);
        //Debug.Log("ANDROIDPERSISTANTDOWNLOADPATHEXPORT_PATH ::> " + ANDROIDPERSISTANTDOWNLOADPATHEXPORT);

        Directory.Delete(MAINDOWNLOADPATHDIRECT, true);
        DeleteDirectory(MAINDOWNLOADPATHDIRECT);
        //DeleteDirectory(MAINDOWNLOADPATHEXPORT);
        //DeleteDirectory(ANDROIDPERSISTANTDOWNLOADPATHDIRECT);
        //DeleteDirectory(ANDROIDPERSISTANTDOWNLOADPATHEXPORT);


        string headerUUIDDirect = Guid.NewGuid().ToString();
        string moduleUUIDDirect = Guid.NewGuid().ToString();

        Debug.Log("moduleUUIDDirect  ::> " + moduleUUIDDirect);
        manifestRootDirect.header.uuid = headerUUIDDirect;

        Debug.Log("moduleUUIDDirect1 ::> " + manifestRootDirect.header.uuid);
        manifestRootDirect.modules[0].uuid = moduleUUIDDirect;

        //string headerUUIDExport = Guid.NewGuid().ToString();
        //string moduleUUIDExport = Guid.NewGuid().ToString();

        //manifestRootExport.header.uuid = headerUUIDExport;
        //manifestRootExport.modules[0].uuid = moduleUUIDExport;

        //old folder remove new folder creation

    }

    public void CreateDirectImmportPack()
    {
        SoundController1.Instance.PlayClickSound();

        SuperStarAd.Instance.ShowRewardVideo((o) =>
        {
            if (o)
            {
                createAddonForCape(0);
            }
            else
            {
                ToastManager.Instance.ShowTost("Watch Video To Import This Addon");
            }
        });

    }

    public void createAddonForCape(int exporttype)
    {
        if (string.IsNullOrEmpty(ANDROIDPERSISTANTDOWNLOADPATHDIRECT))
        {
            Debug.LogError("ANDROIDPERSISTANTDOWNLOADPATHDIRECT is null or empty");
            return;
        }

        if (string.IsNullOrEmpty(MAINDOWNLOADPATHDIRECT))
        {
            Debug.LogError("MAINDOWNLOADPATHDIRECT is null or empty");
            return;
        }

        Debug.Log("ANDROIDPERSISTANTDOWNLOADPATHDIRECT ::> " + ANDROIDPERSISTANTDOWNLOADPATHDIRECT);
        Debug.Log("MAINDOWNLOADPATHDIRECT ::> " + MAINDOWNLOADPATHDIRECT);

        if (exporttype == 0)
        {
            //string MainCapeFolder = Path.Combine(ANDROIDPERSISTANTDOWNLOADPATHDIRECT, "VelocityCapeAddon");
            string MainCapeFolder = Path.Combine(MAINDOWNLOADPATHDIRECT, "VelocityCapeAddon");
            Debug.Log("MainCapeFolder ::> " + MainCapeFolder);

            if (!Directory.Exists(MainCapeFolder))
            {
                Directory.CreateDirectory(MainCapeFolder);
            }
            else
            {
                Debug.Log("DeletePath_");
                Directory.Delete(MainCapeFolder, true);
                Directory.CreateDirectory(MainCapeFolder);
            }

            string textureFolder = Path.Combine(MainCapeFolder, "textures");
            string entityFolder = Path.Combine(MainCapeFolder, "entity");
            string modelsFolder = Path.Combine(MainCapeFolder, "models");
            string modelsentityFolder = Path.Combine(modelsFolder, "entity");
            string render_controllersFolder = Path.Combine(MainCapeFolder, "render_controllers");

            //create 4 folders
            Directory.CreateDirectory(textureFolder);
            Directory.CreateDirectory(entityFolder);
            Directory.CreateDirectory(modelsFolder);
            Directory.CreateDirectory(modelsentityFolder);
            Directory.CreateDirectory(render_controllersFolder);


            string S = JsonUtility.ToJson(manifestRootDirect);
            Debug.Log("sss ::. " + S);

            File.WriteAllText(Path.Combine(MainCapeFolder, "manifest.json"), JsonUtility.ToJson(manifestRootDirect));
            SaveTextureAsPNG(packIconPng, "pack_icon.png", MainCapeFolder);
            SaveTextureAsPNG(coppedImageTexture, "custom.cape.png", textureFolder);


            Debug.Log("NEW_PATH ::> " + entityFolder + "/player.entity.json");
            Debug.Log("S  ::> " + playerentitydata);

            File.WriteAllText(Path.Combine(entityFolder, "player.entity.json"), playerentitydata);
            File.WriteAllText(Path.Combine(entityFolder, "player.entity(1).json"), playerentitydata1);
            File.WriteAllText(Path.Combine(modelsentityFolder, "Cape.json"), capeentitydata);
            File.WriteAllText(Path.Combine(render_controllersFolder, "custom.cape.json"), customecapejson);


            Debug.Log("PAck Path => " + MainCapeFolder);
            string ZipPath = MAINDOWNLOADPATHDIRECT + "/VelocityCustome" + ".mcaddon";
            Debug.Log("ZipPath ::>" + ZipPath);
            //Zip.CompressDirectory(MainCapeFolder, ZipPath, true);
            Main(MAINDOWNLOADPATHDIRECT);

            // string ZipPath = MainCapeFolder;
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
    }

    public void Main(string MAINDOWNLOADPATHDIRECT)
    {
        string mainCapeFolder = Path.Combine(MAINDOWNLOADPATHDIRECT, "VelocityCapeAddon"); // Replace with your folder path
        string zipPath = MAINDOWNLOADPATHDIRECT + "/VelocityCustome" + ".mcaddon"; // Replace with your desired zip file path

        try
        {
            CompressDirectory(mainCapeFolder, zipPath);
            Debug.Log("Directory compressed successfully.");
        }
        catch (Exception ex)
        {
            Debug.Log("Directory compressed Unsuccessfully.");
            Debug.Log($"An error occurred: {ex.Message}");
        }
    }

    public void CompressDirectory(string sourceDir, string destinationZip)
    {
        if (Directory.Exists(sourceDir))
        {
            // Check if destination zip file already exists and delete it if true
            if (File.Exists(destinationZip))
            {
                Debug.Log("FileWasDeleted");
                File.Delete(destinationZip);
            }

            // Create the zip file
            ZipFile.CreateFromDirectory(sourceDir, destinationZip, System.IO.Compression.CompressionLevel.Optimal, true);
        }
        else
        {
            throw new DirectoryNotFoundException($"Source directory '{sourceDir}' does not exist.");
        }
    }



    public void SaveTextureAsPNG(Texture2D texture, string filename, string fpath)
    {
        // Convert the texture to a byte array (PNG)
        byte[] imageData = texture.DeCompress().EncodeToPNG();


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

    public void ChangeScreen(int ScreenNo)
    {
        SuperStarAd.Instance.ShowForceInterstitialWithLoader((o) =>
        {
            SoundController1.Instance.PlayClickSound();
            CloseTutorial();
            switch (ScreenNo)
            {
                case 0:
                    HDTextureScreen.SetActive(false);
                    TutorialParent.SetActive(true);
                    TutorialScreen[0].SetActive(true);
                    break;
                case 1:
                    TutorialScreen[1].SetActive(true);
                    break;
                case 2:
                    TutorialScreen[2].SetActive(true);
                    break;
                case 3:
                    TutorialScreen[3].SetActive(true);
                    break;
                case 4:
                    TutorialScreen[4].SetActive(true);
                    break;
                case 5:
                    TutorialParent.SetActive(false);
                    PickImageScreen.SetActive(true);
                    break;
            }
        }, 1);
    }

    public void BackScreen(int ScreenNo)
    {
        SuperStarAd.Instance.ShowInterstitialTimer((o) =>
        {
            SoundController1.Instance.PlayClickSound();
            CloseTutorial();

            switch (ScreenNo)
            {
                case 0:
                    TutorialParent.SetActive(false);
                    HDTextureScreen.SetActive(true);
                    MainCharacter.transform.rotation = Quaternion.identity;
                    break;
                case 1:
                    TutorialScreen[0].SetActive(true);
                    break;
                case 2:
                    TutorialScreen[1].SetActive(true);
                    break;
                case 3:
                    TutorialScreen[2].SetActive(true);
                    break;
                case 4:
                    TutorialScreen[3].SetActive(true);
                    break;
            }
        });
    }

    public void CloseTutorial()
    {
        foreach (var screen in TutorialScreen)
        {
            screen.SetActive(false);
        }
    }

    public void MainScreenCloseButton()
    {
        SuperStarAd.Instance.ShowInterstitialTimer((o) =>
        {
            SoundController1.Instance.PlayClickSound();
            IsCustomCapeAddon = false;
            SceneManager.LoadSceneAsync(3);
        });
    }

    public void TextureCloseButton()
    {
        SuperStarAd.Instance.ShowInterstitialTimer((o) =>
        {
            SoundController1.Instance.PlayClickSound();
            ViewImageScreen.SetActive(false);
            PickImageScreen.SetActive(true);
        });
    }

    public void PreviewCloseButton()
    {
        SuperStarAd.Instance.ShowInterstitialTimer((o) =>
        {
            SoundController1.Instance.PlayClickSound();
            HDTextureScreen.SetActive(false);
            PickImageScreen.SetActive(true);
        });
    }

    public void RateUs()
    {
        SoundController1.Instance.PlayClickSound();
        SuperStarSdkManager.Instance.Rate();
    }

    public static void DeleteDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            // Delete all files in the directory
            foreach (string file in Directory.GetFiles(path))
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            // Delete all subdirectories in the directory
            foreach (string subdirectory in Directory.GetDirectories(path))
            {
                DeleteDirectory(subdirectory);
            }

            // Delete the empty directory itself
            Directory.Delete(path);
            Debug.Log($"Directory deleted: {path}");
        }
        else
        {
            Debug.LogWarning($"Directory not found: {path}");
            Directory.CreateDirectory(path);
        }

        Debug.Log("path ::> " + path);
        Directory.CreateDirectory(path);
    }
}

[Serializable]
public class ManifestHeader1
{
    public string description;
    public string name;
    public string uuid;
    public List<int> version = new List<int>();
}
[Serializable]
public class ManifestModule1
{
    public string description;
    public string type;
    public string uuid;
    public List<int> version = new List<int>();
}
[Serializable]
public class ManifestRoot1
{
    public int format_version;
    public ManifestHeader header;
    public List<ManifestModule> modules = new List<ManifestModule>();
}

public static class ExtensionMethod
{
    public static Texture2D DeCompress(this Texture2D source)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }
}


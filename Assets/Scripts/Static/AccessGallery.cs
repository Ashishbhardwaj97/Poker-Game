using UnityEngine;
using UnityEngine.UI;

public class AccessGallery : MonoBehaviour
{
	public static AccessGallery instance;
	internal Texture2D profileImage;
	public Image profilePicture;
	public Image[] profileTex;
	public Text[] profileName;
	public Text[] profileId;

	private void Awake()
    {
		instance = this;
    }

    public void ClickOnUploadImage()
    {
		PickImage(256);
    }
    bool ovalSelection = false;
    bool autoZoom = false;
    float minAspectRatio, maxAspectRatio;
    private void PickImage(int maxSize)
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("Image path: " + path);
            if (path != null)
            {
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }



                //.............Crop image.......//


                ovalSelection = false;
                autoZoom = false;
                if (!float.TryParse("1", out minAspectRatio))
                    minAspectRatio = 0f;
                if (!float.TryParse("1", out maxAspectRatio))
                    maxAspectRatio = 0f;

                //if (Admin.instance.adminPanel.activeInHierarchy)
                //{
                //    ovalSelection = false;
                //    autoZoom = false;
                //    if (!float.TryParse(".7", out minAspectRatio))
                //        minAspectRatio = 0f;
                //    if (!float.TryParse(".9", out maxAspectRatio))
                //        maxAspectRatio = 0f;
                //}
                //else if (!CreateClubMain.instance.homePage.activeInHierarchy)
                //{
                //    ovalSelection = true;
                //    autoZoom = true;

                //    if (!float.TryParse("1", out minAspectRatio))
                //        minAspectRatio = 0f;
                //    if (!float.TryParse("1", out maxAspectRatio))
                //        maxAspectRatio = 0f;
                //}
                //else
                //{
                //    ovalSelection = false;
                //    autoZoom = false;
                //    if (!float.TryParse(".7", out minAspectRatio))
                //        minAspectRatio = 0f;
                //    if (!float.TryParse(".9", out maxAspectRatio))
                //        maxAspectRatio = 0f;
                //}
                Debug.Log("openCropImage........");
                ImageCropper.Instance.Show(texture, (bool result, Texture originalImage, Texture2D croppedImage) =>
                {
                    if (result)
                    {
                        // Assign cropped texture to the RawImage
                        profilePicture.enabled = true;
                        profilePicture.sprite = ConvertToSprite(croppedImage);

                        //if(Admin.instance.adminPanel.activeInHierarchy)
                        //{
                        //    Admin.instance.clubImage.sprite = ConvertToSprite(croppedImage);
                        //}
                        //else if (!CreateClubMain.instance.homePage.activeInHierarchy)
                        //{
                        //    profileImage = croppedImage;

                        //    if (Profile._instance.profileEditPanel.activeInHierarchy)
                        //    {
                        //        Profile._instance.editProfilePic.sprite = ConvertToSprite(croppedImage)
                        //    }
                        //}
                        //else
                        //{
                        //    CreateClubMain.instance.uploadClubImage.sprite = ConvertToSprite(croppedImage);
                        //}
                    }
                    else
                    {
                        profilePicture.enabled = false;
                    }
                },
            settings: new ImageCropper.Settings()
            {

                ovalSelection = ovalSelection,
                autoZoomEnabled = autoZoom,
                imageBackground = Color.clear, // transparent background
                selectionMinAspectRatio = minAspectRatio,
                selectionMaxAspectRatio = maxAspectRatio,

            },
            croppedImageResizePolicy: (ref int width, ref int height) =>
            {

            });
                //......................End Crop image.......................................//                
                //Debug.Log(profilePicture.texture.dimension);
                //Debug.Log(profilePicture.texture.width);
                //Debug.Log(profilePicture.texture.height);                
            }
        }, "Select a PNG image", "image/png");
    }

    public Sprite ConvertToSprite(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    }
}

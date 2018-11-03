# NatShare API
NatShare is a lightweight, easy-to-use native sharing API for Unity Engine. NatShare supports images (using a `Texture2D`) and media files (using a `string` path). Currently, you can save media to the camera roll, open the native sharing UI, and retrieve a thumbnail for a video:

## Native Sharing
To share an image, you can use the `Share` function:
```csharp
Texture2D image = ...;
NatShare.Share(image);
```

The `Share` function also has an overload for sharing plain text or a media file:
```csharp
string videoPath = "file://some/path/to/a/media/file.ext";
NatShare.Share(videoPath);
string message = "Happy Birthday!";
NatShare.Share(message);
```

All sharing functions take a callback parameter. This callback can be used to know when the user has finished the sharing activity:
```csharp
string videoPath = ...;
NatShare.ShareMedia(videoPath, callback: OnShare);

void OnShare () {
    Debug.Log("User shared recording!");
}
```

## Saving to the Camera Roll
You can save images or media files to the camera roll:
```csharp
// Save a texture to the camera roll
Texture2D image = ...;
NatShare.SaveToCameraRoll(image);
// Now save a media file to the camera roll
string gifPath = ...;
NatShare.SaveToCameraRoll(gifPath);
```

## Retrieving a Video Thumbnail
NatShare also supports generating thumbnails for videos:
```csharp
string videoPath = ...;
var thumbnail = NatShare.GetThumbnail(videoPath);
// Do stuff with thumbnail...
```

You can also request the thumbnail at a specific time in the video:
```csharp
string videoPath = ...;
// Request the thumbnail at 5 seconds
var thumbnail = NatShare.GetThumbnail(videoPath, 5f);
```

## Requirements
- On Android, NatShare requires API Level 16 and up
- On iOS, NatShare requires iOS 7 and up

## Quick Tips
- To discuss, report an issue, or request a feature, visit [Unity forums](https://forum.unity.com/threads/natshare-free-sharing-api.527074/) or [GitHub](https://github.com/olokobayusuf/NatShare-API)
- Contact me at [olokobayusuf@gmail.com](mailto:olokobayusuf@gmail.com)

Thank you very much!
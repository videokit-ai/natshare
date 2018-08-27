# NatShare API
NatShare is a lightweight, easy-to-use native sharing API for Unity Engine. NatShare supports images (using a `Texture2D`) and media files (using a `string` path). Currently, you can save media to the camera roll, open the native sharing UI, and retrieve a thumbnail for a video:

## Native Sharing
To share an image, you can use the `ShareImage` function:
```csharp
Texture2D image = ...;
// Share by using the `NatShare` function
NatShare.ShareImage(image);
```

To share a media file, use the `ShareMedia` function:
```csharp
string videoPath = ...;
NatShare.ShareMedia(videoPath);
```

Finally, to share plain text, use the `ShareText` function:
```csharp
NatShare.ShareText("Hi there!");
```

## Saving to the Camera Roll
You can save images or media files to the camera roll:
```csharp
// Save to the camera roll by using the `NatShare` function
Texture2D image = ...;
NatShare.SaveToCameraRoll(image);
// Now save a video to the camera roll
string gifPath = ...;
NatShare.SaveToCameraRoll(gifPath);
```

## Retrieving a Video Thumbnail
NatShare also supports generating thumbnails for videos:
```csharp
string videoPath = ...;
NatShare.GetThumbnail(videoPath, OnThumbnail);

void OnThumbnail (Texture2D thumbnail) {
    // Do stuff with thumbnail...
}
```

You can also request the thumbnail at a specific time in the video:
```csharp
string videoPath = ...;
// Request the thumbnail at 5 seconds
NatShare.GetThumbnail(videoPath, thumbnail => preview.texture = thumbnail, 5f);
```

## Requirements
- On Android, NatShare requires API Level 16 and up
- On iOS, NatShare requires iOS 7 and up

## Quick Tips
- To discuss, report an issue, or request a feature, visit [Unity forums](https://forum.unity.com/threads/natshare-free-sharing-api.527074/) or [GitHub](https://github.com/olokobayusuf/NatShare-API)
- Contact me at [olokobayusuf@gmail.com](mailto:olokobayusuf@gmail.com)

Thank you very much!
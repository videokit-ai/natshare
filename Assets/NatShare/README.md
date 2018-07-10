# NatShare API
NatShare is a lightweight, easy-to-use native sharing API for Unity Engine. NatShare supports images (using a `Texture2D`) and videos (using a `string` path). Currently, you can save an image to the camera roll, open the native sharing UI, and retrieve a thumbnail for a video:

## Sharing Callbacks
If you would like to receive callbacks (currently only for native sharing), first use the `EnableCallbacks` function, passing in a game object name that has a component that implements the success and failure methods:
```csharp
GameObject go = ...
// Enable callbacks that will be received on the `go` object.
NatShare.EnableCallbacks(go.name, "NatShareSuccess", "NatShareFailure");
// It MUST have a component that implements the success and failure functions like:
public void NatShareSuccess(string type) { 
    Debug.LogFormat("share success with type: {0}", type) 
};
// will log "share success with type: activity.share.image" when an image is successfully shared
```

## Native Sharing
To share an image, you can use the `Share` function:
```csharp
Texture2D image = ...;
// Share by using the `NatShare` function
NatShare.Share(image);
// Or use it directly on the `Texture2D`
image.Share();
```

The `Share` function has an overload that accepts `string` paths. This can be used to share videos:
```csharp
string videoPath = ...;
NatShare.Share(videoPath);
```

## Saving to the Camera Roll
Saving to the camera roll is very simple:
```csharp
Texture2D image = ...;
// Save to the camera roll by using the `NatShare` function
NatShare.SaveToCameraRoll(image);
// Or use it directly on the `Texture2D`
image.SaveToCameraRoll();
```

The `SaveToCameraRoll` function also has an overload that accepts `string` paths. This can be used for videos:
```csharp
string videoPath = ...;
NatShare.SaveToCameraRoll(videoPath);
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
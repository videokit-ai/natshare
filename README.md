# NatShare
NatShare is a lightweight, easy-to-use native sharing API for Unity Engine. NatShare supports sharing text, images (using a `Texture2D`) and media files (using a `string` path). Currently, you can save media to the camera roll and open the native sharing UI.

## Setup Instructions
Add the following items to your Unity project's `Packages/manifest.json`:
```json
{
  "scopedRegistries": [
    {
      "name": "NatML",
      "url": "https://registry.npmjs.com",
      "scopes": ["api.natsuite"]
    }
  ],
  "dependencies": {
    "api.natsuite.natshare": "1.2.6"
  }
}
```

## Native Sharing
To share an image, you can use the `SharePayload`:
```csharp
// Say we want to share an image
Texture2D image = ...;
// Create a share payload
var payload = new SharePayload();
// Add the image to the payload
payload.AddImage(image);
// Share!
payload.Commit();
```

You can share multiple items at once:
```csharp
// Create a payload
var payload = new SharePayload();
// Add different items to be shared
payload.AddText("Happy Birthday!");
payload.AddImage(image);
payload.AddMedia("/path/to/some/media/file.mp4");
// Share!
payload.Commit();
```

The `ISharePayload.Commit` function returns a task which when completed, returns a `bool` indicating whether the sharing operation was successful:
```csharp
async void ShareVideo () {
    // Share
    var payload = new SharePayload();
    payload.AddMedia("/path/to/some/media/file.mp4");
    // Check if user successfully shared items
    var success = await payload.Commit();
    Debug.Log($"Successfully shared items: {success}");
}    
```

## Saving to the Camera Roll
You can save images or media files to the camera roll with the `SavePayload`:
```csharp
// Say we want to save an image to the camera roll
Texture2D image = ...;
// Create a save payload
var payload = new SavePayload();
// Add different items
payload.AddImage(image);
payload.AddMedia("/path/to/some/media/file.gif");
// Save to the camera roll!
payload.Commit();
```

___

## Platform Notes

### iOS
After building an Xcode project from Unity, add the following keys to the `Info.plist` file with a good description:
- `NSPhotoLibraryUsageDescription`
- `NSPhotoLibraryAddUsageDescription`

### Android
When building an Android project with Proguard stripping, make sure to add a keep rule for NatShare:
```
-keep class api.natsuite.** { *; }
```

## Requirements
- Unity 2019.3+
- Android API level 24+
- iOS 13+

## Resources
- Join the [NatML community on Discord](https://discord.gg/y5vwgXkz2f).
- See the [NatShare documentation](https://docs.natml.ai/natshare).
- See more [NatML projects on GitHub](https://github.com/natsuite).
- Read the [NatML blog](https://blog.natml.ai/).
- Discuss [NatShare on Unity Forums](https://forum.unity.com/threads/natshare-free-sharing-api.527074/).
- Contact us at [hi@natml.ai](mailto:hi@natml.ai).

Thank you very much!
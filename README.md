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
      "scopes": ["ai.natml"]
    }
  ],
  "dependencies": {
    "ai.natml.natshare": "1.3.0"
  }
}
```

## Native Sharing
To share media, you can use the `SharePayload`:
```csharp
// Say we want to share an image
Texture2D image = ...;
// Create a share payload
using var payload = new SharePayload();
// Add the image to the payload
payload.AddImage(image);
// Share!
await payload.Share();
```

## Saving to the Camera Roll
You can save images or media files to the camera roll with the `SavePayload`:
```csharp
// Say we want to save an image to the camera roll
Texture2D image = ...;
// Create a save payload
using var payload = new SavePayload();
// Add different items
payload.AddImage(image);
payload.AddMedia("/path/to/some/media/animated.gif");
// Save to the camera roll!
await payload.Save();
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
-keep class ai.natml.** { *; }
```

## Requirements
- Unity 2021.2+

## Supported Platforms
- Android API level 24+
- iOS 14+
- WebGL:
  - Chrome 91+
  - Firefox 90+
  - Safari 13+

## Resources
- Join the [NatML community on Discord](https://hub.natml.ai/community).
- See the [NatShare documentation](https://docs.natml.ai/natshare).
- See more [NatML projects on GitHub](https://github.com/natmlx).
- Read the [NatML blog](https://blog.natml.ai/).
- Discuss [NatShare on Unity Forums](https://forum.unity.com/threads/natshare-free-sharing-api.527074/).
- Contact us at [hi@natml.ai](mailto:hi@natml.ai).

Thank you very much!
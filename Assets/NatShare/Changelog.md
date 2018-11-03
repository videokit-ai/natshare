## NatShare 1.1f3
+ Added support for saving to an album in the camera roll.
+ Added `copy` parameter to `SaveToCameraRoll`. When `false`, the media file will be moved to the camera roll instead of being copied.
+ Deprecated `message` parameter in `Share*` functions.
+ Refactored all `Share*` functions to overloads of one `NatShare.Share` function.

## NatShare 1.1f2
+ Added callbacks to the `NatShare.Share...` functions. You can use these to know when the user has completed the sharing activity.
+ Added better error logging on iOS.
+ Changed `NatShare.GetThumbnail` to return the thumbnail texture instead of take a callback.
+ Deprecated the WebGL backend since most functions were not supported.

## NatShare 1.1f1
+ Added `ShareText` function for sharing plain text.
+ Added `ShareMedia` function for sharing media files like videos, images, and so on.
+ Refactored `Share` function to `ShareImage`.
+ Correctly set duration of video file being saved to camera roll on Android.
+ Fix creation date on saved video being incorrect on Android.
+ Fix sharing not being available for Instagram and Twitter on iOS.
+ Fix videos not being copied to the top level of the device gallery on Android.

## NatShare 1.0f3
+ Images being shared or saved will not appear in a "NatShare" album.
+ Added null checking in all functions to prevent crashing.
+ Fixed hard crash when thumbnail cannot be retreived on iOS.

## NatShare 1.0f2
+ Added ability to share images and videos with message.
+ Fixed tearing and skew in generated thumbnails on iOS.

## NatShare 1.0f1
+ First release.
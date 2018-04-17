//
//  NatShare.m
//  NatShare
//
//  Created by Yusuf Olokoba on 4/14/18.
//  Copyright © 2018 Yusuf Olokoba. All rights reserved.
//

#import <AVFoundation/AVFoundation.h>
#import <Accelerate/Accelerate.h>
#import <Photos/Photos.h>
#import "UnityInterface.h"

bool NSShareImage (uint8_t* pngData, int dataSize) {
    NSData* data = [NSData dataWithBytes:pngData length:dataSize];
    UIImage* image = [UIImage imageWithData:data];
    UIActivityViewController* controller = [[UIActivityViewController alloc] initWithActivityItems:@[image] applicationActivities:nil];
    UIViewController* vc = UnityGetGLViewController();
    controller.modalPresentationStyle = UIModalPresentationPopover;
    controller.popoverPresentationController.sourceView = vc.view;
    // Request save tp photos permission, if not, crash occurs (Ilya Playunov)
    [PHPhotoLibrary requestAuthorization:^(PHAuthorizationStatus status) {
        [vc presentViewController:controller animated:YES completion:nil];
    }];
    return true;
}

bool NSShareVideo (const char* videoPath) {
    NSString* path = [NSURL URLWithString:[NSString stringWithUTF8String:videoPath]].path;
    if (![NSFileManager.defaultManager fileExistsAtPath:path]) return false;
    UIActivityViewController* controller = [[UIActivityViewController alloc] initWithActivityItems:@[[NSURL fileURLWithPath:path]] applicationActivities:nil];
    UIViewController* vc = UnityGetGLViewController();
    controller.modalPresentationStyle = UIModalPresentationPopover;
    controller.popoverPresentationController.sourceView = vc.view;
    // Request save tp photos permission, if not, crash occurs (Ilya Playunov)
    [PHPhotoLibrary requestAuthorization:^(PHAuthorizationStatus status) {
        [vc presentViewController:controller animated:YES completion:nil];
    }];
    return true;
}

bool NSSaveImageToCamerRoll (uint8_t* pngData, int dataSize) {
    NSData* data = [NSData dataWithBytes:pngData length:dataSize];
    UIImage* image = [UIImage imageWithData:data];
    if (PHPhotoLibrary.authorizationStatus == PHAuthorizationStatusDenied) return false;
    [PHPhotoLibrary requestAuthorization:^(PHAuthorizationStatus status) {
        if (status == PHAuthorizationStatusAuthorized)
            [PHPhotoLibrary.sharedPhotoLibrary performChanges:^{
                [PHAssetChangeRequest creationRequestForAssetFromImage:image];
            } completionHandler:nil];
    }];
    return true;
}

bool NSSaveVideoToCameraRoll (const char* videoPath) {
    NSURL* videoURL = [NSURL URLWithString:[NSString stringWithUTF8String:videoPath]];
    if (![NSFileManager.defaultManager fileExistsAtPath:videoURL.path]) return false;
    if (PHPhotoLibrary.authorizationStatus == PHAuthorizationStatusDenied) return false;
    [PHPhotoLibrary requestAuthorization:^(PHAuthorizationStatus status) {
        if (status == PHAuthorizationStatusAuthorized)
            [PHPhotoLibrary.sharedPhotoLibrary performChanges:^{
                [PHAssetChangeRequest creationRequestForAssetFromVideoAtFileURL:videoURL];
            } completionHandler:nil];
    }];
    return true;
}

bool NCGetThumbnail (const char* videoPath, float time, void** pixelBuffer, int* width, int* height) {
    NSURL* url = [NSURL URLWithString:[NSString stringWithUTF8String:videoPath]];
    AVAssetImageGenerator* generator = [AVAssetImageGenerator assetImageGeneratorWithAsset:[AVURLAsset assetWithURL:url]];
    generator.appliesPreferredTrackTransform = true;
    NSError* error = nil;
    CGImageRef image = [generator copyCGImageAtTime:CMTimeMakeWithSeconds(time, 1) actualTime:NULL error:&error];
    if (error) {
        NSLog(@"NatCorder Error: %@", error);
        return false;
    }
    *width = (int)CGImageGetWidth(image);
    *height = (int)CGImageGetHeight(image);
    CFDataRef rawData = CGDataProviderCopyData(CGImageGetDataProvider(image));
    const uint8_t* dataPtr = CFDataGetBytePtr(rawData);
    const size_t size =  CFDataGetLength(rawData);
    *pixelBuffer = malloc(size);
    vImage_Buffer input = { (void*)dataPtr, (size_t)*height, (size_t)*width, size / *height }, output = { *pixelBuffer, input.height, input.width, input.rowBytes };
    vImageVerticalReflect_ARGB8888(&input, &output, kvImageNoFlags);
    CFRelease(rawData);
    CGImageRelease(image);
    return true;
}

void NCFreeThumbnail (const intptr_t pixelBuffer) {
    free((void*)pixelBuffer);
}

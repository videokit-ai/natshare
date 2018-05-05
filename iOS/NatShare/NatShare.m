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

bool NSShareImage (uint8_t* pngData, int dataSize, const char* message) {
    NSData* data = [NSData dataWithBytes:pngData length:dataSize];
    UIImage* image = [UIImage imageWithData:data];
    UIActivityViewController* controller = [[UIActivityViewController alloc] initWithActivityItems:@[image, [NSString stringWithUTF8String:message]] applicationActivities:nil];
    UIViewController* vc = UnityGetGLViewController();
    controller.modalPresentationStyle = UIModalPresentationPopover;
    controller.popoverPresentationController.sourceView = vc.view;
    // Request save tp photos permission, if not, crash occurs (Ilya Playunov)
    [PHPhotoLibrary requestAuthorization:^(PHAuthorizationStatus status) {
        [vc presentViewController:controller animated:YES completion:nil];
    }];
    return true;
}

bool NSShareVideo (const char* videoPath, const char* message) {
    NSString* path = [NSURL URLWithString:[NSString stringWithUTF8String:videoPath]].path;
    if (![NSFileManager.defaultManager fileExistsAtPath:path]) return false;
    UIActivityViewController* controller = [[UIActivityViewController alloc] initWithActivityItems:@[[NSURL fileURLWithPath:path], [NSString stringWithUTF8String:message]] applicationActivities:nil];
    UIViewController* vc = UnityGetGLViewController();
    controller.modalPresentationStyle = UIModalPresentationPopover;
    controller.popoverPresentationController.sourceView = vc.view;
    // Request save tp photos permission, if not, crash occurs (Ilya Playunov)
    [PHPhotoLibrary requestAuthorization:^(PHAuthorizationStatus status) {
        [vc presentViewController:controller animated:YES completion:nil];
    }];
    return true;
}

bool NSSaveImageToCameraRoll (uint8_t* pngData, int dataSize) {
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

bool NSGetThumbnail (const char* videoPath, float time, void** pixelBuffer, int* width, int* height) {
    NSURL* url = [NSURL URLWithString:[NSString stringWithUTF8String:videoPath]];
    AVAssetImageGenerator* generator = [AVAssetImageGenerator assetImageGeneratorWithAsset:[AVURLAsset assetWithURL:url]];
    generator.appliesPreferredTrackTransform = true;
    NSError* error = nil;
    CGImageRef image = [generator copyCGImageAtTime:CMTimeMakeWithSeconds(time, 1) actualTime:NULL error:&error];
    if (error) {
        NSLog(@"NatShare Error: Unable to retrieve thumbnail with error: %@", error);
        return false;
    }
    *width = (int)CGImageGetWidth(image);
    *height = (int)CGImageGetHeight(image);
    *pixelBuffer = malloc(*width * *height * 4);
    CFDataRef rawData = CGDataProviderCopyData(CGImageGetDataProvider(image));
    vImage_Buffer input = {
        (void*)CFDataGetBytePtr(rawData),
        *height,
        *width,
        CGImageGetBytesPerRow(image)
    }, output = {
        *pixelBuffer,
        *height,
        *width,
        *width * *height * 4
    };
    vImageVerticalReflect_ARGB8888(&input, &output, kvImageNoFlags);
    CFRelease(rawData);
    CGImageRelease(image);
    return true;
}

void NSFreeThumbnail (const intptr_t pixelBuffer) {
    free((void*)pixelBuffer);
}

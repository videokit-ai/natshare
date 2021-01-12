//
//  NSSavePayload.m
//  NatShare
//
//  Created by Yusuf Olokoba on 8/8/19.
//  Copyright © 2021 Yusuf Olokoba. All rights reserved.
//

@import Photos;
@import MobileCoreServices;
#import "NSPayload.h"

@interface NSSavePayload ()
@property NSString* album;
@property NSMutableArray<UIImage*>* images;
@property NSMutableArray<NSURL*>* media;
@end


@implementation NSSavePayload

@synthesize album;
@synthesize images;
@synthesize media;

- (instancetype) initWithAlbum:(NSString*) album {
    self = super.init;
    self.album = album;
    self.images = NSMutableArray.array;
    self.media = NSMutableArray.array;
    return self;
}

- (void) addText:(NSString*) text {}

- (void) addImage:(UIImage*) image {
    [images addObject:image];
}

- (void) addMedia:(NSURL*) uri {
    [media addObject:uri];
}

- (void) commitWithCompletionHandler:(NSShareCompletionBlock) completionHandler {
    // Request write-only permissions on iOS 14 or newer
    if (@available(iOS 14, *)) {
        PHAuthorizationStatus status = [PHPhotoLibrary authorizationStatusForAccessLevel:PHAccessLevelAddOnly];
        if (status == PHAuthorizationStatusNotDetermined)
            [PHPhotoLibrary requestAuthorizationForAccessLevel:PHAccessLevelAddOnly handler:^(PHAuthorizationStatus status) {
                status = status == PHAuthorizationStatusLimited ? PHAuthorizationStatusAuthorized : status; // CHECK
                [self saveItemsWithAuthorizationStatus:status andCompletionHandler:completionHandler];
            }];
        else
            [self saveItemsWithAuthorizationStatus:status andCompletionHandler:completionHandler];
    }
    // Request permissions on iOS 13 or older
    else {
        PHAuthorizationStatus status = PHPhotoLibrary.authorizationStatus;
        if (status == PHAuthorizationStatusNotDetermined)
            [PHPhotoLibrary requestAuthorization:^(PHAuthorizationStatus status) {
                [self saveItemsWithAuthorizationStatus:status andCompletionHandler:completionHandler];
            }];
        else
            [self saveItemsWithAuthorizationStatus:status andCompletionHandler:completionHandler];
    }    
}

- (void) saveItemsWithAuthorizationStatus:(PHAuthorizationStatus) status andCompletionHandler:(NSShareCompletionBlock) completionHandler {
    // Check auth
    if (status != PHAuthorizationStatusAuthorized) { // CHECK // Should this also include limited auth?
        NSLog(@"NatShare Error: Failed to save media because user did not grant authorization: %li", (long)status);
        if (completionHandler)
            completionHandler(false);
        return;
    }
    // Save
    [PHPhotoLibrary.sharedPhotoLibrary performChanges:^{
        // Create addition requests
        NSMutableArray<PHObjectPlaceholder*>* placeholders = NSMutableArray.array;
        for (UIImage* image in images)
            [placeholders addObject:[PHAssetChangeRequest creationRequestForAssetFromImage:image].placeholderForCreatedAsset];
        for (NSURL* uri in media) {
            CFStringRef fileExtension = (__bridge CFStringRef)uri.pathExtension;
            CFStringRef fileUTI = UTTypeCreatePreferredIdentifierForTag(kUTTagClassFilenameExtension, fileExtension, NULL);
            if (UTTypeConformsTo(fileUTI, kUTTypeImage))
                [placeholders addObject:[PHAssetChangeRequest creationRequestForAssetFromImageAtFileURL:uri].placeholderForCreatedAsset];
            else if (UTTypeConformsTo(fileUTI, kUTTypeMovie))
                [placeholders addObject:[PHAssetChangeRequest creationRequestForAssetFromVideoAtFileURL:uri].placeholderForCreatedAsset];
            else
                NSLog(@"NatShare Error: Failed to save media at path '%@' to camera roll because media is neither image nor video", uri);
        }
        // Add to album if applicable
        if (album)
            [[NSSavePayload albumRequestForName:album] addAssets:placeholders];
    } completionHandler:^(BOOL success, NSError* error) {
        if (completionHandler)
            completionHandler(success);
    }];
}

+ (PHAssetCollectionChangeRequest*) albumRequestForName:(NSString*) name { // MUST be called within a PHPhotoLibrary change block
    // Check if there is an existing album with name
    PHFetchOptions* options = PHFetchOptions.new;
    options.predicate = [NSPredicate predicateWithFormat:@"title = %@", name];
    PHFetchResult* results = [PHAssetCollection fetchAssetCollectionsWithType:PHAssetCollectionTypeAlbum subtype:PHAssetCollectionSubtypeAny options:options];
    if (results.count > 0)
        return [PHAssetCollectionChangeRequest changeRequestForAssetCollection:results.firstObject];
    // Create album
    return [PHAssetCollectionChangeRequest creationRequestForAssetCollectionWithTitle:name];
}

@end

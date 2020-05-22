//
//  NSSavePayload.m
//  NatShare
//
//  Created by Yusuf Olokoba on 8/8/19.
//  Copyright © 2020 Yusuf Olokoba. All rights reserved.
//

@import Photos;
@import MobileCoreServices;
#import "NSPayload.h"

@interface NSSavePayload ()
@property NSString* album;
@property CompletionBlock completionHandler;
@property NSMutableArray<UIImage*>* images;
@property NSMutableArray<NSURL*>* media;
@end


@implementation NSSavePayload

@synthesize album;
@synthesize completionHandler;
@synthesize images;
@synthesize media;

- (instancetype) initWithAlbum:(NSString*) album andCompletionHandler:(CompletionBlock) completionHandler {
    self = super.init;
    self.album = album;
    self.completionHandler = completionHandler;
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

- (void) commit {
    // Request permissions if not determined // We need to block until user decides // #69
    if (PHPhotoLibrary.authorizationStatus == PHAuthorizationStatusNotDetermined) {
        dispatch_semaphore_t semaphore = dispatch_semaphore_create(0);
        [PHPhotoLibrary requestAuthorization:^(PHAuthorizationStatus status) { dispatch_semaphore_signal(semaphore); }];
        dispatch_semaphore_wait(semaphore, DISPATCH_TIME_FOREVER);
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
                NSLog(@"NatShare Error: Failed to commit media at path '%@' to camera roll because media is neither image nor video", uri);
        }
        // Add to album if applicable
        if (album)
            [[NSSavePayload albumRequestForName:album] addAssets:placeholders];
    } completionHandler:^(BOOL success, NSError* error) {
        dispatch_async(dispatch_get_main_queue(), ^{ completionHandler(success); });
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

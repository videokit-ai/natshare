//
//  Bridge.m
//  NatShare
//
//  Created by Yusuf Olokoba on 8/8/19.
//  Copyright © 2020 Yusuf Olokoba. All rights reserved.
//

#import "NatShare.h"
#import "NSPayload.h"
#import "UnityInterface.h"

void* NSCreateSharePayload (void) {
    id<NSPayload> payload = [NSSharePayload.alloc initWithSourceViewController:UnityGetGLViewController()];
    return (__bridge_retained void*)payload;
}

void* NSCreateSavePayload (const char* album) {
    NSString* albumStr = album ? [NSString stringWithUTF8String:album] : nil;
    id<NSPayload> payload = [NSSavePayload.alloc initWithAlbum:albumStr];
    return (__bridge_retained void*)payload;
}

void* NSCreatePrintPayload (bool color, bool landscape) {
    id<NSPayload> payload = [NSPrintPayload.alloc initWithColor:color landscape:landscape];
    return (__bridge_retained void*)payload;
}

void NSAddText (void* payloadPtr, const char* text) {
    id<NSPayload> payload = (__bridge id<NSPayload>)payloadPtr;
    [payload addText:[NSString stringWithUTF8String:text]];
}

void NSAddImage (void* payloadPtr, uint8_t* jpegData, int32_t dataSize) {
    id<NSPayload> payload = (__bridge id<NSPayload>)payloadPtr;
    UIImage* image = [UIImage imageWithData:[NSData dataWithBytes:jpegData length:dataSize]];
    [payload addImage:image];
}

void NSAddMedia (void* payloadPtr, const char* path) {
    id<NSPayload> payload = (__bridge id<NSPayload>)payloadPtr;
    [payload addMedia:[NSURL fileURLWithPath:[NSString stringWithUTF8String:path]]];
}

void NSCommit (void* payloadPtr, NSShareHandler completionHandler, void* context) {
    id<NSPayload> payload = (__bridge_transfer id<NSPayload>)payloadPtr;
    [payload commitWithCompletionHandler:^(bool success) {
        if (completionHandler)
            completionHandler(context, success);
    }];
    payload = nil;
}

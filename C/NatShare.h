//
//  NatShare.h
//  NatShare
//
//  Created by Yusuf Olokoba on 5/22/20.
//  Copyright © 2020 Yusuf Olokoba. All rights reserved.
//

#pragma once

#include "stdint.h"
#include "stdbool.h"

// Platform defines
#ifdef __cplusplus
    #define BRIDGE extern "C"
#else
    #define BRIDGE
#endif

/*!
 @abstract Callback invoked with the result of the sharing action.
 
 @param context
 User context provided to payload.
 
 @param success
 Whether the sharing action was successful.
 */
typedef void (*CompletionHandler) (void* context, bool success);

/*!
 @function NSCreateSharePayload
 
 @abstract Create a payload for sharing media.
 
 @discussion Create a payload for sharing media.
 
 @param completionHandler
 Completion handler to be invoked with the result of the sharing action. Can be `NULL`.
 
 @param context
 User context passed to the completion handler. Can be `NULL`.
 
 @returns Opaque pointer to created payload.
 */
BRIDGE void* NSCreateSharePayload (CompletionHandler completionHandler, void* context);

/*!
 @function NSCreateSavePayload
 
 @abstract Create a payload for saving media to the camera roll.
 
 @discussion Create a payload for saving media to the camera roll.
 
 @param album
 Album in which media should be saved. Pass `NULL` to save directly into camera roll.
 
 @param completionHandler
 Completion handler to be invoked with the result of the sharing action. Can be `NULL`.
 
 @param context
 User context passed to the completion handler. Can be `NULL`.
 
 @returns Opaque pointer to created payload.
 */
BRIDGE void* NSCreateSavePayload (const char* album, CompletionHandler completionHandler, void* context);

/*!
 @function NSCreatePrintPayload
 
 @abstract Create a payload for printing images.
 
 @discussion Create a payload for printing images. At this time, only images provided with `NSAddImage` are supported.
 
 @param color
 Whether the images should be printed in color or greyscale.
 
 @param landscape
 Whether the images should be printed in landscape or portrait orientation.
 
 @param completionHandler
 Completion handler to be invoked with the result of the sharing action. Can be `NULL`.
 
 @param context
 User context passed to the completion handler. Can be `NULL`.
 
 @returns Opaque pointer to created payload.
 */
BRIDGE void* NSCreatePrintPayload (bool color, bool landscape, CompletionHandler completionHandler, void* context);

/*!
 @function NSAddText
 
 @abstract Add text to the payload.
 
 @discussion Add text to the payload.
 
 @param payload
 Opaque pointer to a payload.
 
 @param text
 UTF8 text to add.
 */
BRIDGE void NSAddText (void* payload, const char* text);

/*!
 @function NSAddImage
 
 @abstract Add an image to the payload.
 
 @discussion The image must be provided as JPEG encoded data.
 
 @param payload
 Opaque pointer to a payload.
 
 @param jpegData
 Buffer containing JPEG encoded image data.
 
 @param dataSize
 Size of image data buffer.
 */
BRIDGE void NSAddImage (void* payload, uint8_t* jpegData, int32_t dataSize);

/*!
 @function NSAddMedia
 
 @abstract Add media file to the payload.
 
 @discussion Add media to the payload using its file path.
 
 @param payload
 Opaque pointer to a payload.
 
 @param path
 Path to media file as a UTF-8 string.
 */
BRIDGE void NSAddMedia (void* payload, const char* path);

/*!
 @function NSCommit
 
 @abstract Commit the payload for sharing.
 
 @discussion This function finalizes the payload and performs sharing actions.
 After this function is invoked, the payload MUST NOT be used further.
 
 @param payload
 Opaque pointer to a payload.
 */
BRIDGE void NSCommit (void* payload);

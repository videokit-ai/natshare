//
//  NSPayload.h
//  NatShare
//
//  Created by Yusuf Olokoba on 8/8/19.
//  Copyright © 2020 Yusuf Olokoba. All rights reserved.
//

@import Foundation;
@import UIKit;

typedef void (^NSShareCompletionBlock) (bool success);

@protocol NSPayload <NSObject>
- (void) addText:(nonnull NSString*) text;
- (void) addImage:(nonnull UIImage*) image;
- (void) addMedia:(nonnull NSURL*) url;
- (void) commitWithHandler:(NSShareCompletionBlock _Nullable) completionHandler;
@end

@interface NSSharePayload : NSObject <NSPayload>
- (nonnull instancetype) initWithSourceViewController:(nonnull UIViewController*) sourceViewController;
- (void) addText:(nonnull NSString*) text;
- (void) addImage:(nonnull UIImage*) image;
- (void) addMedia:(nonnull NSURL*) url;
- (void) commitWithHandler:(NSShareCompletionBlock _Nullable) completionHandler;
@end


@interface NSSavePayload : NSObject <NSPayload>
- (nonnull instancetype) initWithAlbum:(nullable NSString*) album;
- (void) addImage:(nonnull UIImage*) image;
- (void) addMedia:(nonnull NSURL*) url;
- (void) commitWithHandler:(NSShareCompletionBlock _Nullable) completionHandler;
@end

@interface NSPrintPayload : NSObject <NSPayload>
- (nonnull instancetype) initWithColor:(bool) color landscape:(bool) landscape;
- (void) addText:(nonnull NSString*) text;
- (void) addImage:(nonnull UIImage*) image;
- (void) addMedia:(nonnull NSURL*) url;
- (void) commitWithHandler:(NSShareCompletionBlock _Nullable) completionHandler;
@end

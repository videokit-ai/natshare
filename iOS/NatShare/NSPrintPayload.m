//
//  NSPrintPayload.m
//  NatShare
//
//  Created by Yusuf Olokoba on 8/9/19.
//  Copyright © 2019 Yusuf Olokoba. All rights reserved.
//

#import "NSPayload.h"

@interface NSPrintPayload ()
@property CompletionHandler completionHandler;
@property UIPrintInfo* printInfo;
@property NSMutableArray* items;
@end


@implementation NSPrintPayload

@synthesize completionHandler;
@synthesize printInfo;
@synthesize items;

- (instancetype) initWithGreyscale:(bool) greyscale landscape:(bool) landscape andCompletionHandler:(CompletionHandler) completionHandler {
    self = super.init;
    // Setup state
    self.completionHandler = completionHandler;
    self.printInfo = UIPrintInfo.printInfo;
    self.items = NSMutableArray.array;
    // Set print options
    printInfo.outputType = greyscale ? UIPrintInfoOutputPhotoGrayscale : UIPrintInfoOutputGeneral;
    printInfo.orientation = landscape ? UIPrintInfoOrientationLandscape : UIPrintInfoOrientationPortrait;
    return self;
}

- (void) addText:(NSString*) text { }

- (void) addImage:(UIImage*) image {
    [items addObject:image];
}

- (void) addMedia:(NSURL*) url {
    [items addObject:url];
}

- (void) commit {
    UIPrintInteractionController* printController = UIPrintInteractionController.sharedPrintController;
    printController.printInfo = printInfo;
    printController.printingItems = items;
    [printController presentAnimated:true completionHandler:^(UIPrintInteractionController* _, BOOL completed, NSError* error) {
        dispatch_async(dispatch_get_main_queue(), ^{
            if (completionHandler)
                completionHandler(completed);
        });
    }];
}

@end

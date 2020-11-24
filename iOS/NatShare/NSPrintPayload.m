//
//  NSPrintPayload.m
//  NatShare
//
//  Created by Yusuf Olokoba on 8/9/19.
//  Copyright © 2020 Yusuf Olokoba. All rights reserved.
//

#import "NSPayload.h"

@interface NSPrintPayload ()
@property UIPrintInfo* printInfo;
@property NSMutableArray* items;
@end


@implementation NSPrintPayload

@synthesize printInfo;
@synthesize items;

- (instancetype) initWithColor:(bool) color landscape:(bool) landscape {
    self = super.init;
    // Setup state
    self.printInfo = UIPrintInfo.printInfo;
    self.items = NSMutableArray.array;
    // Set print options
    printInfo.outputType = color ? UIPrintInfoOutputGeneral : UIPrintInfoOutputPhotoGrayscale;
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

- (void) commitWithHandler:(NSShareCompletionBlock) completionHandler {
    UIPrintInteractionController* printController = UIPrintInteractionController.sharedPrintController;
    printController.printInfo = printInfo;
    printController.printingItems = items;
    [printController presentAnimated:true completionHandler:^(UIPrintInteractionController* _, BOOL completed, NSError* error) {
        if (completionHandler)
            completionHandler(completed);
    }];
}

@end

//
//  NSSharePayload.m
//  NatShare
//
//  Created by Yusuf Olokoba on 8/8/19.
//  Copyright © 2020 Yusuf Olokoba. All rights reserved.
//

#import "NSPayload.h"

@interface NSSharePayload ()
@property UIViewController* sourceViewController;
@property NSMutableArray* payload;
@end


@implementation NSSharePayload

@synthesize sourceViewController;
@synthesize payload;

- (instancetype) initWithSourceViewController:(UIViewController*) sourceViewController {
    self = super.init;
    self.sourceViewController = sourceViewController;
    self.payload = NSMutableArray.array;
    return self;
}

- (void) addText:(NSString*) text {
    [payload addObject:text];
}

- (void) addImage:(UIImage*) image {
    [payload addObject:image];
}

- (void) addMedia:(NSURL*) url {
    [payload addObject:url];
}

- (void) commitWithCompletionHandler:(NSShareCompletionBlock) completionHandler {
    // Create view
    UIActivityViewController* shareController = [UIActivityViewController.alloc initWithActivityItems:payload applicationActivities:nil];
    shareController.modalPresentationStyle = UIModalPresentationPopover;
    shareController.completionWithItemsHandler = ^(UIActivityType activityType, BOOL completed, NSArray* returnedItems, NSError* activityError) {
        if (completionHandler)
            completionHandler(completed);
    };
    // Present
    CGSize sourceViewSize = sourceViewController.view.frame.size;
    UIPopoverPresentationController* presentationController = shareController.popoverPresentationController;
    presentationController.sourceView = sourceViewController.view;
    presentationController.sourceRect = CGRectMake(sourceViewSize.width / 2, sourceViewSize.height / 2, 1, 1); // middle of screen
    presentationController.permittedArrowDirections = UIPopoverArrowDirectionAny;
    [sourceViewController presentViewController:shareController animated:YES completion:nil];
}

@end

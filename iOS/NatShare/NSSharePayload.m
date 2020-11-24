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

- (void) commitWithCompletionHandler:(NSShareCompletionBlock) completionHandler { // TEST // iPad
    // Present share view
    UIActivityViewController* shareController = [UIActivityViewController.alloc initWithActivityItems:payload applicationActivities:nil];
    shareController.completionWithItemsHandler = ^(UIActivityType activityType, BOOL completed, NSArray* returnedItems, NSError* activityError) {
        if (completionHandler)
            completionHandler(completed);
    };
    /*
    shareController.modalPresentationStyle = UIModalPresentationPopover;
    shareController.popoverPresentationController.sourceView = sourceViewController.view;
    shareController.view.translatesAutoresizingMaskIntoConstraints = NO;
    shareController.popoverPresentationController.sourceRect = CGRectMake(0, 200, 768, 20); // Workaround for iPads complaining about unsatisfied constraints on iPadOS 13
     */
    [sourceViewController presentViewController:shareController animated:YES completion:nil];
}

@end

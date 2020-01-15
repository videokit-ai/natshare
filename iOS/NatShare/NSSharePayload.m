//
//  NSSharePayload.m
//  NatShare
//
//  Created by Yusuf Olokoba on 8/8/19.
//  Copyright © 2019 Yusuf Olokoba. All rights reserved.
//

#import "NSPayload.h"

@interface NSSharePayload ()
@property UIViewController* sourceViewController;
@property CompletionBlock completionHandler;
@property NSMutableArray* payload;
@end


@implementation NSSharePayload

@synthesize sourceViewController;
@synthesize completionHandler;
@synthesize payload;

- (instancetype) initWithSourceViewController:(UIViewController*) sourceViewController andCompletionHandler:(CompletionBlock) completionHandler {
    self = super.init;
    self.sourceViewController = sourceViewController;
    self.completionHandler = completionHandler;
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

- (void) commit {
    // Present share view
    UIActivityViewController* shareController = [UIActivityViewController.alloc initWithActivityItems:payload applicationActivities:nil];
    shareController.modalPresentationStyle = UIModalPresentationPopover;
    shareController.popoverPresentationController.sourceView = sourceViewController.view;
    shareController.view.translatesAutoresizingMaskIntoConstraints = NO; // DEPLOY // iOS 13
    [shareController setCompletionWithItemsHandler:^(UIActivityType activityType, BOOL completed, NSArray* returnedItems, NSError* activityError) {
        dispatch_async(dispatch_get_main_queue(), ^{
            if (completionHandler)
                completionHandler(completed);
        });
    }];
    [sourceViewController presentViewController:shareController animated:YES completion:nil];
}

@end

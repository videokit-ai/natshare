//
//  NSSharePayload.m
//  NatShare
//
//  Created by Yusuf Olokoba on 8/8/19.
//  Copyright © 2019 Yusuf Olokoba. All rights reserved.
//

#import "NSPayload.h"
#import "UnityInterface.h"

@interface NSSharePayload ()
@property CompletionHandler completionHandler;
@property NSMutableArray* payload;
@end


@implementation NSSharePayload

@synthesize completionHandler;
@synthesize payload;

- (instancetype) initWithCompletionHandler:(CompletionHandler) completionHandler {
    self = super.init;
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
    UIViewController* appController = UnityGetGLViewController();
    shareController.modalPresentationStyle = UIModalPresentationPopover;
    shareController.popoverPresentationController.sourceView = appController.view;
    [shareController setCompletionWithItemsHandler:^(UIActivityType activityType, BOOL completed, NSArray* returnedItems, NSError* activityError) {
        dispatch_async(dispatch_get_main_queue(), ^{
            if (completionHandler)
                completionHandler();
        });
    }];
    [appController presentViewController:shareController animated:YES completion:nil];
}

@end

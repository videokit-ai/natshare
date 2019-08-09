//
//  NSSavePayload.m
//  NatShare
//
//  Created by Yusuf Olokoba on 8/8/19.
//  Copyright © 2019 Yusuf Olokoba. All rights reserved.
//

#import "NSPayload.h"

@interface NSSavePayload ()

@end


@implementation NSSavePayload

- (instancetype) initWithAlbum:(NSString*) album andCompletionHandler:(CompletionHandler) completionHandler {
    self = super.init;
    
    return self;
}

- (void) dispose {
    
}

- (void) addImage:(UIImage*) image {
    
}

- (void) addMedia:(NSURL*) url {
    
}

- (void) commit {
    
}

@end

//
//  ActivityStringItemSource.m
//  NatShare
//
//  Created by Shawn Roske on 7/31/18.
//  Copyright © 2018 Yusuf Olokoba. All rights reserved.
//

#import "ActivityStringItemSource.h"

@interface ActivityStringItemSource()<UIActivityItemSource>

@property(strong, nonatomic) NSMutableAttributedString *stringContent;
@end

@implementation ActivityStringItemSource

- (instancetype)initWithString:(NSMutableAttributedString *) placeholder
{
    self = [super init];
    if (self) {
        _stringContent = placeholder;
    }
    return self;
}

- (id)activityViewControllerPlaceholderItem:(UIActivityViewController *)activityViewController {
    return [NSObject new];
}

- (id)activityViewController:(UIActivityViewController *)activityViewController itemForActivityType:(UIActivityType)activityType {
    if([activityType containsString:@"instagram"]) {
        return nil;
    } else {
        return _stringContent;
    }
}

@end

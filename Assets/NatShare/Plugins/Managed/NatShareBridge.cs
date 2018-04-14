/* 
*   NatShare
*   Copyright (c) 2018 Yusuf Olokoba
*/

namespace NatShareU.Core {

    using System;
    using System.Runtime.InteropServices;

    public static class NatShareBridge {

        private const string Assembly =
        #if (UNITY_IOS || UNITY_WEBGL) && !UNITY_EDITOR
        "__Internal";
        #else
        "NatShare";
        #endif

        
    }
}
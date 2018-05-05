/* 
*   NatShare
*   Copyright (c) 2018 Yusuf Olokoba
*/

const NatShare = {

    NSShareImage : function (pngData, dataSize, message) {
        console.log("NatShare Error: Sharing is not implemented on WebGL");
        return false;
    },
    
    NSShareVideo : function (path, message) {
        console.log("NatShare Error: Sharing is not implemented on WebGL");
        return false;
    },

    NSSaveImageToCameraRoll : function (pngData, dataSize) {
        console.log("NatShare Error: Sharing is not implemented on WebGL");
        return false;
    },

    NSSaveVideoToCameraRoll : function (path) {
        console.log("NatShare Error: Sharing is not implemented on WebGL");
        return false;
    },

    NSGetThumbnail : function (path, time, buffer, width, height) {
        const videoPlayer = document.createElement("video");
        videoPlayer.addEventListener("seeked", function () {
            // Load frame
            const frame = document.createElement("canvas");
            const frameContext = frame.getContext("2d");
            frame.width = videoPlayer.videoWidth;
            frame.height = videoPlayer.videoHeight;
            frameContext.drawImage(videoPlayer, 0, 0);
            const pixelBuffer = frameContext.getImageData(0, 0, frame.width, frame.height);
            const bufferSize = pixelBuffer.width * pixelBuffer.height * 4;
            const destinationBuffer = new Uint8ClampedArray(HEAPU8.buffer, _malloc(bufferSize), bufferSize);
            destinationBuffer.set(pixelBuffer.data);
            // Return to C#
            const thumbnailData = new Int32Array(HEAPU8.buffer, buffer, 3);
            thumbnailData[1] = pixelBuffer.width;
            thumbnailData[2] = pixelBuffer.height;
            thumbnailData[0] = destinationBuffer.byteOffset;
        });
        videoPlayer.addEventListener("loadeddata", function () { this.currentTime = time; });
        videoPlayer.src = Pointer_stringify(path);
        videoPlayer.preload = "auto";
        return true;
    },

    NSFreeThumbnail : function (buffer) {
        _free(new Int32Array(HEAPU8.buffer, buffer, 1)[0]);
    }
};

mergeInto(LibraryManager.library, NatShare);
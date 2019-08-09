package com.yusufolokoba.natshare;

public interface Payload {
    void addText (String text);
    void addImage (byte[] pixelBuffer, int width, int height);
    void addMedia (String uri);
    void commit ();
}

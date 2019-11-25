package com.natsuite.natshare;

public interface Payload {
    void addText (String text);
    void addImage (byte[] pngData);
    void addMedia (String uri);
    void commit ();
}

package api.natsuite.natshare;

import java.nio.ByteBuffer;

public interface Payload {

    interface Callback {
        void onCompletion (boolean success);
    }

    void addText (String text);

    void addImage (ByteBuffer jpegData);

    void addMedia (String uri);

    void commit ();
}

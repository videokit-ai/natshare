package api.natsuite.natshare;

public interface Payload {

    interface CompletionHandler {
        void onCompletion (boolean success);
    }

    void addText (String text);

    void addImage (byte[] pngData);

    void addMedia (String uri);

    void commit ();
}

package api.natsuite.natshare;

import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import androidx.print.PrintHelper;

/**
 * NatShare
 * Created by Yusuf Olokoba on 08/09/19.
 */
public final class PrintPayload implements Payload {

    private final int callback;
    private final PrintHelper printHelper;
    private Bitmap latestImage;

    public PrintPayload (boolean greyscale, boolean landscape, int callback) {
        this.callback = callback;
        this.printHelper = new PrintHelper(Bridge.activity());
        printHelper.setColorMode(greyscale ? PrintHelper.COLOR_MODE_MONOCHROME : PrintHelper.COLOR_MODE_COLOR);
        printHelper.setOrientation(landscape ? PrintHelper.ORIENTATION_LANDSCAPE : PrintHelper.ORIENTATION_PORTRAIT);
    }

    @Override
    public void addText (String text) { } // Not implemented

    @Override
    public void addImage (byte[] pngData) {
        final Bitmap image = BitmapFactory.decodeByteArray(pngData, 0, pngData.length);
        if (latestImage != null)
            latestImage.recycle();
        latestImage = image;
    }

    @Override
    public void addMedia (String uri) { } // Not implemented

    @Override
    public void commit () {
        printHelper.printBitmap("NatShare Print", latestImage, new PrintHelper.OnPrintFinishCallback() {
            @Override
            public void onFinish () {
                if (callback != 0)
                    Bridge.callback(callback);
            }
        });
    }
}

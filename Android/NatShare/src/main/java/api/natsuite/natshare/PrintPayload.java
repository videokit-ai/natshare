package api.natsuite.natshare;

import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.net.Uri;

import androidx.print.PrintHelper;
import com.unity3d.player.UnityPlayer;

import java.io.File;
import java.nio.ByteBuffer;
import java.util.ArrayList;

/**
 * NatShare
 * Created by Yusuf Olokoba on 08/09/19.
 */
public final class PrintPayload implements Payload {

    private final PrintHelper printHelper;
    private final Callback completionHandler;
    private final ArrayList<Bitmap> images;
    private final ArrayList<Uri> files;
    private int remaining;

    public PrintPayload (boolean color, boolean landscape, Callback completionHandler) {
        // Init state
        this.printHelper = new PrintHelper(UnityPlayer.currentActivity);
        this.completionHandler = completionHandler;
        this.images = new ArrayList<>();
        this.files = new ArrayList<>();
        // Configure printing
        printHelper.setColorMode(color ? PrintHelper.COLOR_MODE_COLOR : PrintHelper.COLOR_MODE_MONOCHROME);
        printHelper.setOrientation(landscape ? PrintHelper.ORIENTATION_LANDSCAPE : PrintHelper.ORIENTATION_PORTRAIT);
    }

    @Override
    public void addText (String text) { } // Not implemented

    @Override
    public void addImage (ByteBuffer jpegData) {
        // Read into managed memory
        final byte[] buffer = new byte[jpegData.capacity()];
        jpegData.clear();
        jpegData.get(buffer);
        // Create bitmap
        final Bitmap image = BitmapFactory.decodeByteArray(buffer, 0, buffer.length);
        images.add(image);
    }

    @Override
    public void addMedia (String uri) {
        files.add(Uri.fromFile(new File(uri)));
    }

    @Override
    public void commit () {
        // Check if supported
        if (!printHelper.systemSupportsPrint()) {
            completionHandler.onCompletion(false);
            return;
        }
        // Print
        remaining = images.size() + files.size();
        try {
            for (Bitmap bitmap : images)
                printHelper.printBitmap("NatShare Print", bitmap, () -> {
                    bitmap.recycle();
                    if (--remaining == 0)
                        completionHandler.onCompletion(true);
                });
            for (Uri uri : files)
                printHelper.printBitmap("NatShare Print", uri, () -> {
                    if (--remaining == 0)
                        completionHandler.onCompletion(true);
                });
        } catch (Exception ex) {
            completionHandler.onCompletion(false);
        }
    }
}

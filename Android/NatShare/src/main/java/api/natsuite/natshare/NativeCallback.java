package api.natsuite.natshare;

final class NativeCallback implements Payload.Callback {

    private final long callback, context;

    public NativeCallback (final long callback, final long context) { this.callback = callback; this.context = context; }

    @Override public void onCompletion (boolean success) { onCompletion(callback, context, success); }

    private native void onCompletion (long callback, long context, boolean success);
}

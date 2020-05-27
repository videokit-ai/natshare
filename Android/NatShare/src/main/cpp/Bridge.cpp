//
//  Bridge.cpp
//  NatShare
//
//  Created by Yusuf Olokoba on 5/22/2020.
//  Copyright © 2020 Yusuf Olokoba. All rights reserved.
//

#include "NatShare.h"
#include <jni.h>


static JNIEnv* GetEnv ();
static jobject CreateCallback (JNIEnv* env, NSShareHandler completionHandler, void* context);


#pragma region --NatCorder--

void* NSCreateSharePayload (NSShareHandler completionHandler, void* context) {
    // Get Java environment
    JNIEnv* env = GetEnv();
    if (!env)
        return nullptr;
    // Create callback
    jobject callback = CreateCallback(env, completionHandler, context);
    jclass clazz = env->FindClass("api/natsuite/natshare/SharePayload");
    jmethodID constructor = env->GetMethodID(clazz, "<init>", "(Lapi/natsuite/natshare/Payload$Callback;)V");
    // Create payload
    jobject object = env->NewObject(clazz, constructor, callback);
    jobject payload = env->NewGlobalRef(object);
    // Release locals
    env->DeleteLocalRef(callback);
    env->DeleteLocalRef(clazz);
    env->DeleteLocalRef(object);
    return static_cast<void*>(payload);
}

void* NSCreateSavePayload (const char* album, NSShareHandler completionHandler, void* context) { // INCOMPLETE
    // Get Java environment
    JNIEnv* env = GetEnv();
    if (!env)
        return nullptr;
}

void* NSCreatePrintPayload (bool color, bool landscape, NSShareHandler completionHandler, void* context) {
    // Get Java environment
    JNIEnv* env = GetEnv();
    if (!env)
        return nullptr;
    // Create callback
    jobject callback = CreateCallback(env, completionHandler, context);
    jclass clazz = env->FindClass("api/natsuite/natshare/PrintPayload");
    jmethodID constructor = env->GetMethodID(clazz, "<init>", "(ZZLapi/natsuite/natshare/Payload$Callback;)V");
    // Create payload
    jobject object = env->NewObject(clazz, constructor, color, landscape, callback);
    jobject payload = env->NewGlobalRef(object);
    // Release locals
    env->DeleteLocalRef(callback);
    env->DeleteLocalRef(clazz);
    env->DeleteLocalRef(object);
    return static_cast<void*>(payload);
}

void NSAddText (void* payloadPtr, const char* textPtr) {
    // Get Java environment
    JNIEnv* env = GetEnv();
    if (!env)
        return;
    // Invoke method
    jstring text = env->NewStringUTF(textPtr);
    jobject payload = static_cast<jobject>(payloadPtr);
    jclass clazz = env->GetObjectClass(payload);
    jmethodID method = env->GetMethodID(clazz, "addText", "(Ljava/lang/String;)V");
    env->CallVoidMethod(payload, method, text);
    // Release locals
    env->DeleteLocalRef(text);
    env->DeleteLocalRef(clazz);
}

void NSAddImage (void* payloadPtr, uint8_t* jpegData, int32_t dataSize) {
    // Get Java environment
    JNIEnv* env = GetEnv();
    if (!env)
        return;
    // Invoke method
    jobject buffer = env->NewDirectByteBuffer(jpegData, dataSize);
    jobject payload = static_cast<jobject>(payloadPtr);
    jclass clazz = env->GetObjectClass(payload);
    jmethodID method = env->GetMethodID(clazz, "addImage", "(Ljava/nio/ByteBuffer;)V");
    // Release
    env->DeleteLocalRef(buffer);
    env->DeleteLocalRef(clazz);
}

void NSAddMedia (void* payloadPtr, const char* pathPtr) {
    // Get Java environment
    JNIEnv* env = GetEnv();
    if (!env)
        return;
    // Invoke method
    jstring path = env->NewStringUTF(pathPtr);
    jobject payload = static_cast<jobject>(payloadPtr);
    jclass clazz = env->GetObjectClass(payload);
    jmethodID method = env->GetMethodID(clazz, "addMedia", "(Ljava/lang/String;)V");
    env->CallVoidMethod(payload, method, path);
    // Release locals
    env->DeleteLocalRef(path);
    env->DeleteLocalRef(clazz);
}

void NSCommit (void* payloadPtr) {
    // Get Java environment
    JNIEnv* env = GetEnv();
    if (!env)
        return;
    // Invoke method
    jobject payload = static_cast<jobject>(payloadPtr);
    jclass clazz = env->GetObjectClass(payload);
    jmethodID method = env->GetMethodID(clazz, "commit", "()V");
    env->CallVoidMethod(payload, method);
    // Release locals
    env->DeleteLocalRef(clazz);
}
#pragma endregion


#pragma region --JNI--

static JavaVM* jvm;

BRIDGE JNIEXPORT jint JNICALL JNI_OnLoad (JavaVM* vm, void* reserved) {
    jvm = vm;
    return JNI_VERSION_1_6;
}

JNIEnv* GetEnv () {
    JNIEnv* env = nullptr;
    int status = jvm->GetEnv(reinterpret_cast<void**>(&env), JNI_VERSION_1_6);
    if (status == JNI_EDETACHED)
        jvm->AttachCurrentThread(&env, nullptr);
    return env;
}

jobject CreateCallback (JNIEnv* env, NSShareHandler completionHandler, void* context) {
    jclass clazz = env->FindClass("api/natsuite/natshare/NativeCallback");
    jmethodID constructor = env->GetMethodID(clazz, "<init>", "(JJ)V");
    jobject object = env->NewObject(clazz, constructor, (jlong)(void*)completionHandler, (jlong)context);
    env->DeleteLocalRef(clazz);
    return object;
}

BRIDGE JNIEXPORT void JNICALL Java_api_natsuite_natshare_NativeCallback_onCompletion (JNIEnv* env, jobject object, jlong callbackPtr, jlong contextPtr, jboolean success) {
    ((NSShareHandler)(void*)callbackPtr)((void*)contextPtr, success);
}
#pragma endregion
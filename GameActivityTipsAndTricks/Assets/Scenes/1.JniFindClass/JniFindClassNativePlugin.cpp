#include <jni.h>
#include <string>

// Defined in Il2CppOutputProject\IL2CPP\libil2cpp\os\Android\Initialize.cpp
extern JavaVM* sJavaVM;
static JNIEnv* s_JniEnv;
static std::string s_Result;


JNIEnv* GetJNIEnv() {
    if (s_JniEnv != nullptr)
        return s_JniEnv;
    JNIEnv* env;
    int status = sJavaVM->GetEnv((void**)&s_JniEnv, JNI_VERSION_1_6);
    if (status < 0) {
        status = sJavaVM->AttachCurrentThread(&env, NULL);
        if (status < 0)
            return nullptr;
    }
    return s_JniEnv;
}

extern "C" const char* GetData() {
    auto randomClass = GetJNIEnv()->FindClass("com/example/MyClass");
    if (randomClass == nullptr) {
        s_Result = "\"com/example/MyClass not found\"";
    }
    else {
        auto getStringMethod = GetJNIEnv()->GetStaticMethodID(randomClass, "getString",
            "()Ljava/lang/String;");
        auto result = (jstring)GetJNIEnv()->CallStaticObjectMethod(randomClass, getStringMethod,
            nullptr);
        const char* strReturn = GetJNIEnv()->GetStringUTFChars(result, 0);
        s_Result = strReturn;
        GetJNIEnv()->ReleaseStringUTFChars(result, strReturn);
    }
    return s_Result.c_str();
}
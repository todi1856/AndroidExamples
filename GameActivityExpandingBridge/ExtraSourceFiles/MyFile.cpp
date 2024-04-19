#include "UGAApplication.h"
#include "game-activity/native_app_glue/android_native_app_glue.h"

void UnityGameActivityPluginLoad(Unity::UnityApplication& application)
{
    application.GetEvents().Register<Unity::UnityEventProcessInput>([](const Unity::UnityEventProcessInput& e)
    {
        auto inputBuffer = e.GetInputBuffer();

        if (inputBuffer->motionEventsCount != 0) {
            for (uint64_t i = 0; i < inputBuffer->motionEventsCount; ++i) {
                GameActivityMotionEvent* motionEvent = &inputBuffer->motionEvents[i];
                if (motionEvent->action == AKEY_EVENT_ACTION_DOWN)
                    e.GetApplication().SendMessage("SendMessageReceiver", "SendMessageFromCpp", "HelloFromBridge");
            }
        }
    });
}
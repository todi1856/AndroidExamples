#include "UGAApplication.h"
#include "game-activity/native_app_glue/android_native_app_glue.h"

static bool s_AppWasInitialized = false;

void UnityGameActivityPluginLoad(Unity::UnityApplication& application)
{
    application.GetEvents().Register<Unity::UnityEventProcessInput>([](const Unity::UnityEvent& e){
        auto inputBuffer = static_cast<const Unity::UnityEventProcessInput&>(e).GetInputBuffer();
        if (inputBuffer->motionEventsCount != 0) {
            for (uint64_t i = 0; i < inputBuffer->motionEventsCount; ++i) {
                GameActivityMotionEvent* motionEvent = &inputBuffer->motionEvents[i];
                if (motionEvent->action == AKEY_EVENT_ACTION_DOWN) {
                    char buff[100];
                    snprintf(buff, sizeof(buff), "Tap at %.2f x %.2f",
                        GameActivityPointerAxes_getX(&motionEvent->pointers[0]),
                        GameActivityPointerAxes_getY(&motionEvent->pointers[0]));
                    e.GetApplication().SendMessage("SendMessageReceiver", "SendMessageFromCpp", buff);
                }
            }
        }
    });

    application.GetEvents().Register<Unity::UnityEventProcessApplicationCommandAfter>([](const Unity::UnityEvent& e){
        auto cmd = static_cast<const Unity::UnityEventProcessApplicationCommandAfter&>(e).GetCommand();

        if (cmd == APP_CMD_INIT_WINDOW) {
            // In Unity there was a bug, where calling SendMessage before Unity runtime is initialized would cause crash.
            // Thus skip any SendMessage calls before APP_CMD_INIT_WINDOW
            s_AppWasInitialized = true;
            return;
        }

        if (s_AppWasInitialized && cmd == APP_CMD_RESUME) {
            e.GetApplication().SendMessage("SendMessageReceiver", "SendMessageFromCpp", "Resume caught");
        }
    });
}



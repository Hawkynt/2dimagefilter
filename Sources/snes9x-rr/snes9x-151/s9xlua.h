#ifndef _S9XLUA_H
#define _S9XLUA_H

enum LuaCallID
{
	LUACALL_BEFOREEMULATION,
	LUACALL_AFTEREMULATION,
	LUACALL_BEFOREEXIT,

	LUACALL_COUNT
};
void CallRegisteredLuaFunctions(LuaCallID calltype);

enum LuaMemHookType
{
	LUAMEMHOOK_WRITE,
	LUAMEMHOOK_READ,
	LUAMEMHOOK_EXEC,
	LUAMEMHOOK_WRITE_SUB,
	LUAMEMHOOK_READ_SUB,
	LUAMEMHOOK_EXEC_SUB,

	LUAMEMHOOK_COUNT
};
void CallRegisteredLuaMemHook(unsigned int address, int size, unsigned int value, LuaMemHookType hookType);

// Just forward function declarations 

void S9xLuaFrameBoundary();
int S9xLoadLuaCode(const char *filename);
int S9xReloadLuaCode();
void S9xLuaStop();
int S9xLuaRunning();

int S9xLuaUsingJoypad(int);
int S9xLuaReadJoypad(int);
int S9xLuaSpeed();
bool8 S9xLuaRerecordCountSkip();

void S9xLuaGui(void *, int width, int height, int bpp, int pitch);
void S9xLuaClearGui();
void S9xLuaEnableGui(bool enabled);

struct lua_State* S9xGetLuaState();
char* S9xGetLuaScriptName(); // note: returns a pointer to a writable static buffer of _MAX_PATH length

// And some interesting REVERSE declarations!
char *S9xGetFreezeFilename(int slot);

extern char luascript[128];

#endif

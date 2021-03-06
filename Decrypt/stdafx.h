// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once

#include "targetver.h"

#define WIN32_LEAN_AND_MEAN             // Exclude rarely-used stuff from Windows headers
// Windows Header Files:
#include <windows.h>

#include <cryptlib.h>
#include <files.h>
#include <base64.h>
#include <modes.h>
#include <aes.h>
#include <zlib.h>

// Crypto++ Library
#ifdef _DEBUG
#ifdef _WIN64
#pragma comment(lib, "cryptlibd_x64")
#else
#pragma comment(lib, "cryptlibd")
#endif
#else
#ifdef _WIN64
#pragma comment(lib, "cryptlib_x64")
#else
#pragma comment(lib, "cryptlib")
#endif
#endif

// TODO: reference additional headers your program requires here




using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using JetBrains.Annotations;

#pragma warning disable 0419

namespace GLFW
{


    /// <summary>
    ///     The base class the vast majority of the GLFW functions, excluding only Vulkan and native platform specific
    ///     functions.
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    public static class Glfw
    {
        #region Fields and Constants

        /// <summary>
        ///     The native library name,
        ///     <para>For Unix users using an installed version of GLFW, this needs refactored to <c>glfw</c>.</para>
        /// </summary>
#if Windows
        public const string LIBRARY = "glfw3";
#elif OSX
        public const string LIBRARY = "libglfw.3"; // mac
#else
        public const string LIBRARY = "glfw3";
#endif
        
        private static readonly ErrorCallback errorCallback = GlfwError;

        /*************************************************************************
 * GLFW API tokens
 *************************************************************************/

        /*! @name GLFW version macros
         *  @{ */
        /*! @brief The major version number of the GLFW header.
         *
         *  The major version number of the GLFW header.  This is incremented when the
         *  API is changed in non-compatible ways.
         *  @ingroup init
         */
        public static readonly int GLFW_VERSION_MAJOR = 3;
        /*! @brief The minor version number of the GLFW header.
         *
         *  The minor version number of the GLFW header.  This is incremented when
         *  features are added to the API but it remains backward-compatible.
         *  @ingroup init
         */
        public static readonly int GLFW_VERSION_MINOR = 4;
        /*! @brief The revision number of the GLFW header.
         *
         *  The revision number of the GLFW header.  This is incremented when a bug fix
         *  release is made that does not contain any API changes.
         *  @ingroup init
         */
        public static readonly int GLFW_VERSION_REVISION = 0;
        /*! @} */

        /*! @brief One.
         *
         *  This is only semantic sugar for the number 1.  You can instead use `1` or
         *  `true` or `_True` or `GL_TRUE` or `VK_TRUE` or anything else that is equal
         *  to one.
         *
         *  @ingroup init
         */
        public static readonly int GLFW_TRUE = 1;
        /*! @brief Zero.
         *
         *  This is only semantic sugar for the number 0.  You can instead use `0` or
         *  `false` or `_False` or `GL_FALSE` or `VK_FALSE` or anything else that is
         *  equal to zero.
         *
         *  @ingroup init
         */
        public static readonly int GLFW_FALSE = 0;

        /*! @name Key and button actions
         *  @{ */
        /*! @brief The key or mouse button was released.
         *
         *  The key or mouse button was released.
         *
         *  @ingroup input
         */
        public static readonly int GLFW_RELEASE = 0;
        /*! @brief The key or mouse button was pressed.
         *
         *  The key or mouse button was pressed.
         *
         *  @ingroup input
         */
        public static readonly int GLFW_PRESS = 1;
        /*! @brief The key was held down until it repeated.
         *
         *  The key was held down until it repeated.
         *
         *  @ingroup input
         */
        public static readonly int GLFW_REPEAT = 2;
        /*! @} */

        /*! @defgroup hat_state Joystick hat states
         *  @brief Joystick hat states.
         *
         *  See [joystick hat input](@ref joystick_hat) for how these are used.
         *
         *  @ingroup input
         *  @{ */
        public static readonly int GLFW_HAT_CENTERED         = 0;
        public static readonly int GLFW_HAT_UP               = 1;
        public static readonly int GLFW_HAT_RIGHT            = 2;
        public static readonly int GLFW_HAT_DOWN             = 4;
        public static readonly int GLFW_HAT_LEFT             = 8;
        public static readonly int GLFW_HAT_RIGHT_UP         = (GLFW_HAT_RIGHT | GLFW_HAT_UP);
        public static readonly int GLFW_HAT_RIGHT_DOWN       = (GLFW_HAT_RIGHT | GLFW_HAT_DOWN);
        public static readonly int GLFW_HAT_LEFT_UP          = (GLFW_HAT_LEFT  | GLFW_HAT_UP);
        public static readonly int GLFW_HAT_LEFT_DOWN        = (GLFW_HAT_LEFT  | GLFW_HAT_DOWN);
        /*! @} */

        /*! @defgroup keys Keyboard keys
         *  @brief Keyboard key IDs.
         *
         *  See [key input](@ref input_key) for how these are used.
         *
         *  These key codes are inspired by the _USB HID Usage Tables v1.12_ (p. 53-60),
         *  but re-arranged to map to 7-bit ASCII for printable keys (function keys are
         *  put in the 256+ range).
         *
         *  The naming of the key codes follow these rules:
         *   - The US keyboard layout is used
         *   - Names of printable alphanumeric characters are used (e.g. "A", "R",
         *     "3", etc.)
         *   - For non-alphanumeric characters, Unicode:ish names are used (e.g.
         *     "COMMA", "LEFT_SQUARE_BRACKET", etc.). Note that some names do not
         *     correspond to the Unicode standard (usually for brevity)
         *   - Keys that lack a clear US mapping are named "WORLD_x"
         *   - For non-printable keys, custom names are used (e.g. "F4",
         *     "BACKSPACE", etc.)
         *
         *  @ingroup input
         *  @{
         */

        /* The unknown key */
        public static readonly int GLFW_KEY_UNKNOWN          = -1;
                                                             
        /* Printable keys */                                 
        public static readonly int GLFW_KEY_SPACE            = 32;
        public static readonly int GLFW_KEY_APOSTROPHE       = 39;  /* ' */
        public static readonly int GLFW_KEY_COMMA            = 44;  /* , */
        public static readonly int GLFW_KEY_MINUS            = 45;  /* - */
        public static readonly int GLFW_KEY_PERIOD           = 46;  /* . */
        public static readonly int GLFW_KEY_SLASH            = 47;  /* / */
        public static readonly int GLFW_KEY_0                = 48;
        public static readonly int GLFW_KEY_1                = 49;
        public static readonly int GLFW_KEY_2                = 50;
        public static readonly int GLFW_KEY_3                = 51;
        public static readonly int GLFW_KEY_4                = 52;
        public static readonly int GLFW_KEY_5                = 53;
        public static readonly int GLFW_KEY_6                = 54;
        public static readonly int GLFW_KEY_7                = 55;
        public static readonly int GLFW_KEY_8                = 56;
        public static readonly int GLFW_KEY_9                = 57;
        public static readonly int GLFW_KEY_SEMICOLON        = 59;  /* ; */
        public static readonly int GLFW_KEY_EQUAL            = 61;  /* = */
        public static readonly int GLFW_KEY_A                = 65;
        public static readonly int GLFW_KEY_B                = 66;
        public static readonly int GLFW_KEY_C                = 67;
        public static readonly int GLFW_KEY_D                = 68;
        public static readonly int GLFW_KEY_E                = 69;
        public static readonly int GLFW_KEY_F                = 70;
        public static readonly int GLFW_KEY_G                = 71;
        public static readonly int GLFW_KEY_H                = 72;
        public static readonly int GLFW_KEY_I                = 73;
        public static readonly int GLFW_KEY_J                = 74;
        public static readonly int GLFW_KEY_K                = 75;
        public static readonly int GLFW_KEY_L                = 76;
        public static readonly int GLFW_KEY_M                = 77;
        public static readonly int GLFW_KEY_N                = 78;
        public static readonly int GLFW_KEY_O                = 79;
        public static readonly int GLFW_KEY_P                = 80;
        public static readonly int GLFW_KEY_Q                = 81;
        public static readonly int GLFW_KEY_R                = 82;
        public static readonly int GLFW_KEY_S                = 83;
        public static readonly int GLFW_KEY_T                = 84;
        public static readonly int GLFW_KEY_U                = 85;
        public static readonly int GLFW_KEY_V                = 86;
        public static readonly int GLFW_KEY_W                = 87;
        public static readonly int GLFW_KEY_X                = 88;
        public static readonly int GLFW_KEY_Y                = 89;
        public static readonly int GLFW_KEY_Z                = 90;
        public static readonly int GLFW_KEY_LEFT_BRACKET     = 91;  /* [ */
        public static readonly int GLFW_KEY_BACKSLASH        = 92;  /* \ */
        public static readonly int GLFW_KEY_RIGHT_BRACKET    = 93;  /* ] */
        public static readonly int GLFW_KEY_GRAVE_ACCENT     = 96;  /* ` */
        public static readonly int GLFW_KEY_WORLD_1          = 161; /* non-US #1 */
        public static readonly int GLFW_KEY_WORLD_2          = 162; /* non-US #2 */
                                                             
        /* Function keys */                                  
        public static readonly int GLFW_KEY_ESCAPE           = 256;
        public static readonly int GLFW_KEY_ENTER            = 257;
        public static readonly int GLFW_KEY_TAB              = 258;
        public static readonly int GLFW_KEY_BACKSPACE        = 259;
        public static readonly int GLFW_KEY_INSERT           = 260;
        public static readonly int GLFW_KEY_DELETE           = 261;
        public static readonly int GLFW_KEY_RIGHT            = 262;
        public static readonly int GLFW_KEY_LEFT             = 263;
        public static readonly int GLFW_KEY_DOWN             = 264;
        public static readonly int GLFW_KEY_UP               = 265;
        public static readonly int GLFW_KEY_PAGE_UP          = 266;
        public static readonly int GLFW_KEY_PAGE_DOWN        = 267;
        public static readonly int GLFW_KEY_HOME             = 268;
        public static readonly int GLFW_KEY_END              = 269;
        public static readonly int GLFW_KEY_CAPS_LOCK        = 280;
        public static readonly int GLFW_KEY_SCROLL_LOCK      = 281;
        public static readonly int GLFW_KEY_NUM_LOCK         = 282;
        public static readonly int GLFW_KEY_PRINT_SCREEN     = 283;
        public static readonly int GLFW_KEY_PAUSE            = 284;
        public static readonly int GLFW_KEY_F1               = 290;
        public static readonly int GLFW_KEY_F2               = 291;
        public static readonly int GLFW_KEY_F3               = 292;
        public static readonly int GLFW_KEY_F4               = 293;
        public static readonly int GLFW_KEY_F5               = 294;
        public static readonly int GLFW_KEY_F6               = 295;
        public static readonly int GLFW_KEY_F7               = 296;
        public static readonly int GLFW_KEY_F8               = 297;
        public static readonly int GLFW_KEY_F9               = 298;
        public static readonly int GLFW_KEY_F10              = 299;
        public static readonly int GLFW_KEY_F11              = 300;
        public static readonly int GLFW_KEY_F12              = 301;
        public static readonly int GLFW_KEY_F13              = 302;
        public static readonly int GLFW_KEY_F14              = 303;
        public static readonly int GLFW_KEY_F15              = 304;
        public static readonly int GLFW_KEY_F16              = 305;
        public static readonly int GLFW_KEY_F17              = 306;
        public static readonly int GLFW_KEY_F18              = 307;
        public static readonly int GLFW_KEY_F19              = 308;
        public static readonly int GLFW_KEY_F20              = 309;
        public static readonly int GLFW_KEY_F21              = 310;
        public static readonly int GLFW_KEY_F22              = 311;
        public static readonly int GLFW_KEY_F23              = 312;
        public static readonly int GLFW_KEY_F24              = 313;
        public static readonly int GLFW_KEY_F25              = 314;
        public static readonly int GLFW_KEY_KP_0             = 320;
        public static readonly int GLFW_KEY_KP_1             = 321;
        public static readonly int GLFW_KEY_KP_2             = 322;
        public static readonly int GLFW_KEY_KP_3             = 323;
        public static readonly int GLFW_KEY_KP_4             = 324;
        public static readonly int GLFW_KEY_KP_5             = 325;
        public static readonly int GLFW_KEY_KP_6             = 326;
        public static readonly int GLFW_KEY_KP_7             = 327;
        public static readonly int GLFW_KEY_KP_8             = 328;
        public static readonly int GLFW_KEY_KP_9             = 329;
        public static readonly int GLFW_KEY_KP_DECIMAL       = 330;
        public static readonly int GLFW_KEY_KP_DIVIDE        = 331;
        public static readonly int GLFW_KEY_KP_MULTIPLY      = 332;
        public static readonly int GLFW_KEY_KP_SUBTRACT      = 333;
        public static readonly int GLFW_KEY_KP_ADD           = 334;
        public static readonly int GLFW_KEY_KP_ENTER         = 335;
        public static readonly int GLFW_KEY_KP_EQUAL         = 336;
        public static readonly int GLFW_KEY_LEFT_SHIFT       = 340;
        public static readonly int GLFW_KEY_LEFT_CONTROL     = 341;
        public static readonly int GLFW_KEY_LEFT_ALT         = 342;
        public static readonly int GLFW_KEY_LEFT_SUPER       = 343;
        public static readonly int GLFW_KEY_RIGHT_SHIFT      = 344;
        public static readonly int GLFW_KEY_RIGHT_CONTROL    = 345;
        public static readonly int GLFW_KEY_RIGHT_ALT        = 346;
        public static readonly int GLFW_KEY_RIGHT_SUPER      = 347;
        public static readonly int GLFW_KEY_MENU             = 348;
                                                             
        public static readonly int GLFW_KEY_LAST             = GLFW_KEY_MENU;

        /*! @} */

        /*! @defgroup mods Modifier key flags
         *  @brief Modifier key flags.
         *
         *  See [key input](@ref input_key) for how these are used.
         *
         *  @ingroup input
         *  @{ */

        /*! @brief If this bit is set one or more Shift keys were held down.
         *
         *  If this bit is set one or more Shift keys were held down.
         */
        public static readonly int GLFW_MOD_SHIFT         = 0x0001;
        /*! @brief If this bit is set one or more Control keys were held down.
         *
         *  If this bit is set one or more Control keys were held down.
         */
        public static readonly int GLFW_MOD_CONTROL       = 0x0002;
        /*! @brief If this bit is set one or more Alt keys were held down.
         *
         *  If this bit is set one or more Alt keys were held down.
         */
        public static readonly int GLFW_MOD_ALT           = 0x0004;
        /*! @brief If this bit is set one or more Super keys were held down.
         *
         *  If this bit is set one or more Super keys were held down.
         */
        public static readonly int GLFW_MOD_SUPER         = 0x0008;
        /*! @brief If this bit is set the Caps Lock key is enabled.
         *
         *  If this bit is set the Caps Lock key is enabled and the @ref
         *  GLFW_LOCK_KEY_MODS input mode is set.
         */
        public static readonly int GLFW_MOD_CAPS_LOCK     = 0x0010;
        /*! @brief If this bit is set the Num Lock key is enabled.
         *
         *  If this bit is set the Num Lock key is enabled and the @ref
         *  GLFW_LOCK_KEY_MODS input mode is set.
         */
        public static readonly int GLFW_MOD_NUM_LOCK      = 0x0020;

        /*! @} */

        /*! @defgroup buttons Mouse buttons
         *  @brief Mouse button IDs.
         *
         *  See [mouse button input](@ref input_mouse_button) for how these are used.
         *
         *  @ingroup input
         *  @{ */
        public static readonly int GLFW_MOUSE_BUTTON_1       = 0;
        public static readonly int GLFW_MOUSE_BUTTON_2       = 1;
        public static readonly int GLFW_MOUSE_BUTTON_3       = 2;
        public static readonly int GLFW_MOUSE_BUTTON_4       = 3;
        public static readonly int GLFW_MOUSE_BUTTON_5       = 4;
        public static readonly int GLFW_MOUSE_BUTTON_6       = 5;
        public static readonly int GLFW_MOUSE_BUTTON_7       = 6;
        public static readonly int GLFW_MOUSE_BUTTON_8       = 7;
        public static readonly int GLFW_MOUSE_BUTTON_LAST    = GLFW_MOUSE_BUTTON_8;
        public static readonly int GLFW_MOUSE_BUTTON_LEFT    = GLFW_MOUSE_BUTTON_1;
        public static readonly int GLFW_MOUSE_BUTTON_RIGHT   = GLFW_MOUSE_BUTTON_2;
        public static readonly int GLFW_MOUSE_BUTTON_MIDDLE  = GLFW_MOUSE_BUTTON_3;
        /*! @} */

        /*! @defgroup joysticks Joysticks
         *  @brief Joystick IDs.
         *
         *  See [joystick input](@ref joystick) for how these are used.
         *
         *  @ingroup input
         *  @{ */
        public static readonly int GLFW_JOYSTICK_1           = 0;
        public static readonly int GLFW_JOYSTICK_2           = 1;
        public static readonly int GLFW_JOYSTICK_3           = 2;
        public static readonly int GLFW_JOYSTICK_4           = 3;
        public static readonly int GLFW_JOYSTICK_5           = 4;
        public static readonly int GLFW_JOYSTICK_6           = 5;
        public static readonly int GLFW_JOYSTICK_7           = 6;
        public static readonly int GLFW_JOYSTICK_8           = 7;
        public static readonly int GLFW_JOYSTICK_9           = 8;
        public static readonly int GLFW_JOYSTICK_10          = 9;
        public static readonly int GLFW_JOYSTICK_11          = 10;
        public static readonly int GLFW_JOYSTICK_12          = 11;
        public static readonly int GLFW_JOYSTICK_13          = 12;
        public static readonly int GLFW_JOYSTICK_14          = 13;
        public static readonly int GLFW_JOYSTICK_15          = 14;
        public static readonly int GLFW_JOYSTICK_16          = 15;
        public static readonly int GLFW_JOYSTICK_LAST        = GLFW_JOYSTICK_16;
        /*! @} */

        /*! @defgroup gamepad_buttons Gamepad buttons
         *  @brief Gamepad buttons.
         *
         *  See @ref gamepad for how these are used.
         *
         *  @ingroup input
         *  @{ */
        public static readonly int GLFW_GAMEPAD_BUTTON_A             = 0;
        public static readonly int GLFW_GAMEPAD_BUTTON_B             = 1;
        public static readonly int GLFW_GAMEPAD_BUTTON_X             = 2;
        public static readonly int GLFW_GAMEPAD_BUTTON_Y             = 3;
        public static readonly int GLFW_GAMEPAD_BUTTON_LEFT_BUMPER   = 4;
        public static readonly int GLFW_GAMEPAD_BUTTON_RIGHT_BUMPER  = 5;
        public static readonly int GLFW_GAMEPAD_BUTTON_BACK          = 6;
        public static readonly int GLFW_GAMEPAD_BUTTON_START         = 7;
        public static readonly int GLFW_GAMEPAD_BUTTON_GUIDE         = 8;
        public static readonly int GLFW_GAMEPAD_BUTTON_LEFT_THUMB    = 9;
        public static readonly int GLFW_GAMEPAD_BUTTON_RIGHT_THUMB   = 10;
        public static readonly int GLFW_GAMEPAD_BUTTON_DPAD_UP       = 11;
        public static readonly int GLFW_GAMEPAD_BUTTON_DPAD_RIGHT    = 12;
        public static readonly int GLFW_GAMEPAD_BUTTON_DPAD_DOWN     = 13;
        public static readonly int GLFW_GAMEPAD_BUTTON_DPAD_LEFT     = 14;
        public static readonly int GLFW_GAMEPAD_BUTTON_LAST          = GLFW_GAMEPAD_BUTTON_DPAD_LEFT;

        public static readonly int GLFW_GAMEPAD_BUTTON_CROSS         = GLFW_GAMEPAD_BUTTON_A;
        public static readonly int GLFW_GAMEPAD_BUTTON_CIRCLE        = GLFW_GAMEPAD_BUTTON_B;
        public static readonly int GLFW_GAMEPAD_BUTTON_SQUARE        = GLFW_GAMEPAD_BUTTON_X;
        public static readonly int GLFW_GAMEPAD_BUTTON_TRIANGLE      = GLFW_GAMEPAD_BUTTON_Y;
        /*! @} */

        /*! @defgroup gamepad_axes Gamepad axes
         *  @brief Gamepad axes.
         *
         *  See @ref gamepad for how these are used.
         *
         *  @ingroup input
         *  @{ */
        public static readonly int GLFW_GAMEPAD_AXIS_LEFT_X         = 0;
        public static readonly int GLFW_GAMEPAD_AXIS_LEFT_Y         = 1;
        public static readonly int GLFW_GAMEPAD_AXIS_RIGHT_X        = 2;
        public static readonly int GLFW_GAMEPAD_AXIS_RIGHT_Y        = 3;
        public static readonly int GLFW_GAMEPAD_AXIS_LEFT_TRIGGER   = 4;
        public static readonly int GLFW_GAMEPAD_AXIS_RIGHT_TRIGGER  = 5;
        public static readonly int GLFW_GAMEPAD_AXIS_LAST           = GLFW_GAMEPAD_AXIS_RIGHT_TRIGGER;
        /*! @} */

        /*! @defgroup errors Error codes
         *  @brief Error codes.
         *
         *  See [error handling](@ref error_handling) for how these are used.
         *
         *  @ingroup init
         *  @{ */
        /*! @brief No error has occurred.
         *
         *  No error has occurred.
         *
         *  @analysis Yay.
         */
        public static readonly int GLFW_NO_ERROR             = 0;
        /*! @brief GLFW has not been initialized.
         *
         *  This occurs if a GLFW function was called that must not be called unless the
         *  library is [initialized](@ref intro_init).
         *
         *  @analysis Application programmer error.  Initialize GLFW before calling any
         *  function that requires initialization.
         */
        public static readonly int GLFW_NOT_INITIALIZED      = 0x00010001;
        /*! @brief No context is current for this thread.
         *
         *  This occurs if a GLFW function was called that needs and operates on the
         *  current OpenGL or OpenGL ES context but no context is current on the calling
         *  thread.  One such function is @ref glfwSwapInterval.
         *
         *  @analysis Application programmer error.  Ensure a context is current before
         *  calling functions that require a current context.
         */
        public static readonly int GLFW_NO_CURRENT_CONTEXT    = 0x00010002;
        /*! @brief One of the arguments to the function was an invalid enum value.
         *
         *  One of the arguments to the function was an invalid enum value, for example
         *  requesting @ref GLFW_RED_BITS with @ref glfwGetWindowAttrib.
         *
         *  @analysis Application programmer error.  Fix the offending call.
         */
        public static readonly int GLFW_INVALID_ENUM          = 0x00010003;
        /*! @brief One of the arguments to the function was an invalid value.
         *
         *  One of the arguments to the function was an invalid value, for example
         *  requesting a non-existent OpenGL or OpenGL ES version like 2.7.
         *
         *  Requesting a valid but unavailable OpenGL or OpenGL ES version will instead
         *  result in a @ref GLFW_VERSION_UNAVAILABLE error.
         *
         *  @analysis Application programmer error.  Fix the offending call.
         */
        public static readonly int GLFW_INVALID_VALUE         = 0x00010004;
        /*! @brief A memory allocation failed.
         *
         *  A memory allocation failed.
         *
         *  @analysis A bug in GLFW or the underlying operating system.  Report the bug
         *  to our [issue tracker](https://github.com/glfw/glfw/issues).
         */
        public static readonly int GLFW_OUT_OF_MEMORY         = 0x00010005;
        /*! @brief GLFW could not find support for the requested API on the system.
         *
         *  GLFW could not find support for the requested API on the system.
         *
         *  @analysis The installed graphics driver does not support the requested
         *  API, or does not support it via the chosen context creation API.
         *  Below are a few examples.
         *
         *  @par
         *  Some pre-installed Windows graphics drivers do not support OpenGL.  AMD only
         *  supports OpenGL ES via EGL, while Nvidia and Intel only support it via
         *  a WGL or GLX extension.  macOS does not provide OpenGL ES at all.  The Mesa
         *  EGL, OpenGL and OpenGL ES libraries do not interface with the Nvidia binary
         *  driver.  Older graphics drivers do not support Vulkan.
         */
        public static readonly int GLFW_API_UNAVAILABLE       = 0x00010006;
        /*! @brief The requested OpenGL or OpenGL ES version is not available.
         *
         *  The requested OpenGL or OpenGL ES version (including any requested context
         *  or framebuffer hints) is not available on this machine.
         *
         *  @analysis The machine does not support your requirements.  If your
         *  application is sufficiently flexible, downgrade your requirements and try
         *  again.  Otherwise, inform the user that their machine does not match your
         *  requirements.
         *
         *  @par
         *  Future invalid OpenGL and OpenGL ES versions, for example OpenGL 4.8 if 5.0
         *  comes out before the 4.x series gets that far, also fail with this error and
         *  not @ref GLFW_INVALID_VALUE, because GLFW cannot know what future versions
         *  will exist.
         */
        public static readonly int GLFW_VERSION_UNAVAILABLE   = 0x00010007;
        /*! @brief A platform-specific error occurred that does not match any of the
         *  more specific categories.
         *
         *  A platform-specific error occurred that does not match any of the more
         *  specific categories.
         *
         *  @analysis A bug or configuration error in GLFW, the underlying operating
         *  system or its drivers, or a lack of required resources.  Report the issue to
         *  our [issue tracker](https://github.com/glfw/glfw/issues).
         */
        public static readonly int GLFW_PLATFORM_ERROR        = 0x00010008;
        /*! @brief The requested format is not supported or available.
         *
         *  If emitted during window creation, the requested pixel format is not
         *  supported.
         *
         *  If emitted when querying the clipboard, the contents of the clipboard could
         *  not be converted to the requested format.
         *
         *  @analysis If emitted during window creation, one or more
         *  [hard constraints](@ref window_hints_hard) did not match any of the
         *  available pixel formats.  If your application is sufficiently flexible,
         *  downgrade your requirements and try again.  Otherwise, inform the user that
         *  their machine does not match your requirements.
         *
         *  @par
         *  If emitted when querying the clipboard, ignore the error or report it to
         *  the user, as appropriate.
         */
        public static readonly int GLFW_FORMAT_UNAVAILABLE    = 0x00010009;
        /*! @brief The specified window does not have an OpenGL or OpenGL ES context.
         *
         *  A window that does not have an OpenGL or OpenGL ES context was passed to
         *  a function that requires it to have one.
         *
         *  @analysis Application programmer error.  Fix the offending call.
         */
        public static readonly int GLFW_NO_WINDOW_CONTEXT     = 0x0001000A;
        /*! @brief The specified cursor shape is not available.
         *
         *  The specified standard cursor shape is not available, either because the
         *  current platform cursor theme does not provide it or because it is not
         *  available on the platform.
         *
         *  @analysis Platform or system settings limitation.  Pick another
         *  [standard cursor shape](@ref shapes) or create a
         *  [custom cursor](@ref cursor_custom).
         */
        public static readonly int GLFW_CURSOR_UNAVAILABLE    = 0x0001000B;
        /*! @brief The requested feature is not provided by the platform.
         *
         *  The requested feature is not provided by the platform, so GLFW is unable to
         *  implement it.  The documentation for each function notes if it could emit
         *  this error.
         *
         *  @analysis Platform or platform version limitation.  The error can be ignored
         *  unless the feature is critical to the application.
         *
         *  @par
         *  A function call that emits this error has no effect other than the error and
         *  updating any existing out parameters.
         */
        public static readonly int GLFW_FEATURE_UNAVAILABLE   = 0x0001000C;
        /*! @brief The requested feature is not implemented for the platform.
         *
         *  The requested feature has not yet been implemented in GLFW for this platform.
         *
         *  @analysis An incomplete implementation of GLFW for this platform, hopefully
         *  fixed in a future release.  The error can be ignored unless the feature is
         *  critical to the application.
         *
         *  @par
         *  A function call that emits this error has no effect other than the error and
         *  updating any existing out parameters.
         */
        public static readonly int GLFW_FEATURE_UNIMPLEMENTED = 0x0001000D;
        /*! @brief Platform unavailable or no matching platform was found.
         *
         *  If emitted during initialization, no matching platform was found.  If @ref
         *  GLFW_PLATFORM is set to `GLFW_ANY_PLATFORM`, GLFW could not detect any of the
         *  platforms supported by this library binary, except for the Null platform.  If set to
         *  a specific platform, it is either not supported by this library binary or GLFW was not
         *  able to detect it.
         *
         *  If emitted by a native access function, GLFW was initialized for a different platform
         *  than the function is for.
         *
         *  @analysis Failure to detect any platform usually only happens on non-macOS Unix
         *  systems, either when no window system is running or the program was run from
         *  a terminal that does not have the necessary environment variables.  Fall back to
         *  a different platform if possible or notify the user that no usable platform was
         *  detected.
         *
         *  Failure to detect a specific platform may have the same cause as above or be because
         *  support for that platform was not compiled in.  Call @ref glfwPlatformSupported to
         *  check whether a specific platform is supported by a library binary.
         */
        public static readonly int GLFW_PLATFORM_UNAVAILABLE  = 0x0001000E;
        /*! @} */

        /*! @addtogroup window
         *  @{ */
        /*! @brief Input focus window hint and attribute
         *
         *  Input focus [window hint](@ref GLFW_FOCUSED_hint) or
         *  [window attribute](@ref GLFW_FOCUSED_attrib).
         */
        public static readonly int GLFW_FOCUSED               = 0x00020001;
        /*! @brief Window iconification window attribute
         *
         *  Window iconification [window attribute](@ref GLFW_ICONIFIED_attrib).
         */
        public static readonly int GLFW_ICONIFIED             = 0x00020002;
        /*! @brief Window resize-ability window hint and attribute
         *
         *  Window resize-ability [window hint](@ref GLFW_RESIZABLE_hint) and
         *  [window attribute](@ref GLFW_RESIZABLE_attrib).
         */
        public static readonly int GLFW_RESIZABLE             = 0x00020003;
        /*! @brief Window visibility window hint and attribute
         *
         *  Window visibility [window hint](@ref GLFW_VISIBLE_hint) and
         *  [window attribute](@ref GLFW_VISIBLE_attrib).
         */
        public static readonly int GLFW_VISIBLE               = 0x00020004;
        /*! @brief Window decoration window hint and attribute
         *
         *  Window decoration [window hint](@ref GLFW_DECORATED_hint) and
         *  [window attribute](@ref GLFW_DECORATED_attrib).
         */
        public static readonly int GLFW_DECORATED             = 0x00020005;
        /*! @brief Window auto-iconification window hint and attribute
         *
         *  Window auto-iconification [window hint](@ref GLFW_AUTO_ICONIFY_hint) and
         *  [window attribute](@ref GLFW_AUTO_ICONIFY_attrib).
         */
        public static readonly int GLFW_AUTO_ICONIFY          = 0x00020006;
        /*! @brief Window decoration window hint and attribute
         *
         *  Window decoration [window hint](@ref GLFW_FLOATING_hint) and
         *  [window attribute](@ref GLFW_FLOATING_attrib).
         */
        public static readonly int GLFW_FLOATING              = 0x00020007;
        /*! @brief Window maximization window hint and attribute
         *
         *  Window maximization [window hint](@ref GLFW_MAXIMIZED_hint) and
         *  [window attribute](@ref GLFW_MAXIMIZED_attrib).
         */
        public static readonly int GLFW_MAXIMIZED             = 0x00020008;
        /*! @brief Cursor centering window hint
         *
         *  Cursor centering [window hint](@ref GLFW_CENTER_CURSOR_hint).
         */
        public static readonly int GLFW_CENTER_CURSOR         = 0x00020009;
        /*! @brief Window framebuffer transparency hint and attribute
         *
         *  Window framebuffer transparency
         *  [window hint](@ref GLFW_TRANSPARENT_FRAMEBUFFER_hint) and
         *  [window attribute](@ref GLFW_TRANSPARENT_FRAMEBUFFER_attrib).
         */
        public static readonly int GLFW_TRANSPARENT_FRAMEBUFFER = 0x0002000A;
        /*! @brief Mouse cursor hover window attribute.
         *
         *  Mouse cursor hover [window attribute](@ref GLFW_HOVERED_attrib).
         */
        public static readonly int GLFW_HOVERED               = 0x0002000B;
        /*! @brief Input focus on calling show window hint and attribute
         *
         *  Input focus [window hint](@ref GLFW_FOCUS_ON_SHOW_hint) or
         *  [window attribute](@ref GLFW_FOCUS_ON_SHOW_attrib).
         */
        public static readonly int GLFW_FOCUS_ON_SHOW         = 0x0002000C;

        /*! @brief Mouse input transparency window hint and attribute
         *
         *  Mouse input transparency [window hint](@ref GLFW_MOUSE_PASSTHROUGH_hint) or
         *  [window attribute](@ref GLFW_MOUSE_PASSTHROUGH_attrib).
         */
        public static readonly int GLFW_MOUSE_PASSTHROUGH     = 0x0002000D;

        /*! @brief Framebuffer bit depth hint.
         *
         *  Framebuffer bit depth [hint](@ref GLFW_RED_BITS).
         */
        public static readonly int GLFW_RED_BITS              = 0x00021001;
        /*! @brief Framebuffer bit depth hint.
         *
         *  Framebuffer bit depth [hint](@ref GLFW_GREEN_BITS).
         */
        public static readonly int GLFW_GREEN_BITS            = 0x00021002;
        /*! @brief Framebuffer bit depth hint.
         *
         *  Framebuffer bit depth [hint](@ref GLFW_BLUE_BITS).
         */
        public static readonly int GLFW_BLUE_BITS             = 0x00021003;
        /*! @brief Framebuffer bit depth hint.
         *
         *  Framebuffer bit depth [hint](@ref GLFW_ALPHA_BITS).
         */
        public static readonly int GLFW_ALPHA_BITS            = 0x00021004;
        /*! @brief Framebuffer bit depth hint.
         *
         *  Framebuffer bit depth [hint](@ref GLFW_DEPTH_BITS).
         */
        public static readonly int GLFW_DEPTH_BITS            = 0x00021005;
        /*! @brief Framebuffer bit depth hint.
         *
         *  Framebuffer bit depth [hint](@ref GLFW_STENCIL_BITS).
         */
        public static readonly int GLFW_STENCIL_BITS          = 0x00021006;
        /*! @brief Framebuffer bit depth hint.
         *
         *  Framebuffer bit depth [hint](@ref GLFW_ACCUM_RED_BITS).
         */
        public static readonly int GLFW_ACCUM_RED_BITS        = 0x00021007;
        /*! @brief Framebuffer bit depth hint.
         *
         *  Framebuffer bit depth [hint](@ref GLFW_ACCUM_GREEN_BITS).
         */
        public static readonly int GLFW_ACCUM_GREEN_BITS      = 0x00021008;
        /*! @brief Framebuffer bit depth hint.
         *
         *  Framebuffer bit depth [hint](@ref GLFW_ACCUM_BLUE_BITS).
         */
        public static readonly int GLFW_ACCUM_BLUE_BITS       = 0x00021009;
        /*! @brief Framebuffer bit depth hint.
         *
         *  Framebuffer bit depth [hint](@ref GLFW_ACCUM_ALPHA_BITS).
         */
        public static readonly int GLFW_ACCUM_ALPHA_BITS      = 0x0002100A;
        /*! @brief Framebuffer auxiliary buffer hint.
         *
         *  Framebuffer auxiliary buffer [hint](@ref GLFW_AUX_BUFFERS).
         */
        public static readonly int GLFW_AUX_BUFFERS           = 0x0002100B;
        /*! @brief OpenGL stereoscopic rendering hint.
         *
         *  OpenGL stereoscopic rendering [hint](@ref GLFW_STEREO).
         */
        public static readonly int GLFW_STEREO                = 0x0002100C;
        /*! @brief Framebuffer MSAA samples hint.
         *
         *  Framebuffer MSAA samples [hint](@ref GLFW_SAMPLES).
         */
        public static readonly int GLFW_SAMPLES               = 0x0002100D;
        /*! @brief Framebuffer sRGB hint.
         *
         *  Framebuffer sRGB [hint](@ref GLFW_SRGB_CAPABLE).
         */
        public static readonly int GLFW_SRGB_CAPABLE          = 0x0002100E;
        /*! @brief Monitor refresh rate hint.
         *
         *  Monitor refresh rate [hint](@ref GLFW_REFRESH_RATE).
         */
        public static readonly int GLFW_REFRESH_RATE          = 0x0002100F;
        /*! @brief Framebuffer double buffering hint and attribute.
         *
         *  Framebuffer double buffering [hint](@ref GLFW_DOUBLEBUFFER_hint) and
         *  [attribute](@ref GLFW_DOUBLEBUFFER_attrib).
         */
        public static readonly int GLFW_DOUBLEBUFFER          = 0x00021010;

        /*! @brief Context client API hint and attribute.
         *
         *  Context client API [hint](@ref GLFW_CLIENT_API_hint) and
         *  [attribute](@ref GLFW_CLIENT_API_attrib).
         */
        public static readonly int GLFW_CLIENT_API            = 0x00022001;
        /*! @brief Context client API major version hint and attribute.
         *
         *  Context client API major version [hint](@ref GLFW_CONTEXT_VERSION_MAJOR_hint)
         *  and [attribute](@ref GLFW_CONTEXT_VERSION_MAJOR_attrib).
         */
        public static readonly int GLFW_CONTEXT_VERSION_MAJOR = 0x00022002;
        /*! @brief Context client API minor version hint and attribute.
         *
         *  Context client API minor version [hint](@ref GLFW_CONTEXT_VERSION_MINOR_hint)
         *  and [attribute](@ref GLFW_CONTEXT_VERSION_MINOR_attrib).
         */
        public static readonly int GLFW_CONTEXT_VERSION_MINOR = 0x00022003;
        /*! @brief Context client API revision number hint and attribute.
         *
         *  Context client API revision number
         *  [attribute](@ref GLFW_CONTEXT_REVISION_attrib).
         */
        public static readonly int GLFW_CONTEXT_REVISION      = 0x00022004;
        /*! @brief Context robustness hint and attribute.
         *
         *  Context client API revision number [hint](@ref GLFW_CONTEXT_ROBUSTNESS_hint)
         *  and [attribute](@ref GLFW_CONTEXT_ROBUSTNESS_attrib).
         */
        public static readonly int GLFW_CONTEXT_ROBUSTNESS    = 0x00022005;
        /*! @brief OpenGL forward-compatibility hint and attribute.
         *
         *  OpenGL forward-compatibility [hint](@ref GLFW_OPENGL_FORWARD_COMPAT_hint)
         *  and [attribute](@ref GLFW_OPENGL_FORWARD_COMPAT_attrib).
         */
        public static readonly int GLFW_OPENGL_FORWARD_COMPAT = 0x00022006;
        /*! @brief Debug mode context hint and attribute.
         *
         *  Debug mode context [hint](@ref GLFW_CONTEXT_DEBUG_hint) and
         *  [attribute](@ref GLFW_CONTEXT_DEBUG_attrib).
         */
        public static readonly int GLFW_CONTEXT_DEBUG         = 0x00022007;
        /*! @brief Legacy name for compatibility.
         *
         *  This is an alias for compatibility with earlier versions.
         */
        public static readonly int GLFW_OPENGL_DEBUG_CONTEXT  = GLFW_CONTEXT_DEBUG;
        /*! @brief OpenGL profile hint and attribute.
         *
         *  OpenGL profile [hint](@ref GLFW_OPENGL_PROFILE_hint) and
         *  [attribute](@ref GLFW_OPENGL_PROFILE_attrib).
         */
        public static readonly int GLFW_OPENGL_PROFILE        = 0x00022008;
        /*! @brief Context flush-on-release hint and attribute.
         *
         *  Context flush-on-release [hint](@ref GLFW_CONTEXT_RELEASE_BEHAVIOR_hint) and
         *  [attribute](@ref GLFW_CONTEXT_RELEASE_BEHAVIOR_attrib).
         */
        public static readonly int GLFW_CONTEXT_RELEASE_BEHAVIOR = 0x00022009;
        /*! @brief Context error suppression hint and attribute.
         *
         *  Context error suppression [hint](@ref GLFW_CONTEXT_NO_ERROR_hint) and
         *  [attribute](@ref GLFW_CONTEXT_NO_ERROR_attrib).
         */
        public static readonly int GLFW_CONTEXT_NO_ERROR      = 0x0002200A;
        /*! @brief Context creation API hint and attribute.
         *
         *  Context creation API [hint](@ref GLFW_CONTEXT_CREATION_API_hint) and
         *  [attribute](@ref GLFW_CONTEXT_CREATION_API_attrib).
         */
        public static readonly int GLFW_CONTEXT_CREATION_API  = 0x0002200B;
        /*! @brief Window content area scaling window
         *  [window hint](@ref GLFW_SCALE_TO_MONITOR).
         */
        public static readonly int GLFW_SCALE_TO_MONITOR      = 0x0002200C;
        /*! @brief macOS specific
         *  [window hint](@ref GLFW_COCOA_RETINA_FRAMEBUFFER_hint).
         */
        public static readonly int GLFW_COCOA_RETINA_FRAMEBUFFER = 0x00023001;
        /*! @brief macOS specific
         *  [window hint](@ref GLFW_COCOA_FRAME_NAME_hint).
         */
        public static readonly int GLFW_COCOA_FRAME_NAME        = 0x00023002;
        /*! @brief macOS specific
         *  [window hint](@ref GLFW_COCOA_GRAPHICS_SWITCHING_hint).
         */
        public static readonly int GLFW_COCOA_GRAPHICS_SWITCHING = 0x00023003;
        /*! @brief X11 specific
         *  [window hint](@ref GLFW_X11_CLASS_NAME_hint).
         */
        public static readonly int GLFW_X11_CLASS_NAME        = 0x00024001;
        /*! @brief X11 specific
         *  [window hint](@ref GLFW_X11_CLASS_NAME_hint).
         */
        public static readonly int GLFW_X11_INSTANCE_NAME     = 0x00024002;
        public static readonly int GLFW_WIN32_KEYBOARD_MENU   = 0x00025001;
        /*! @} */

        public static readonly int GLFW_NO_API                         = 0;
        public static readonly int GLFW_OPENGL_API            = 0x00030001;
        public static readonly int GLFW_OPENGL_ES_API         = 0x00030002;

        public static readonly int GLFW_NO_ROBUSTNESS                  = 0;
        public static readonly int GLFW_NO_RESET_NOTIFICATION = 0x00031001;
        public static readonly int GLFW_LOSE_CONTEXT_ON_RESET = 0x00031002;

        public static readonly int GLFW_OPENGL_ANY_PROFILE             = 0;
        public static readonly int GLFW_OPENGL_CORE_PROFILE   = 0x00032001;
        public static readonly int GLFW_OPENGL_COMPAT_PROFILE = 0x00032002;

        public static readonly int GLFW_CURSOR                = 0x00033001;
        public static readonly int GLFW_STICKY_KEYS           = 0x00033002;
        public static readonly int GLFW_STICKY_MOUSE_BUTTONS  = 0x00033003;
        public static readonly int GLFW_LOCK_KEY_MODS         = 0x00033004;
        public static readonly int GLFW_RAW_MOUSE_MOTION      = 0x00033005;

        public static readonly int GLFW_CURSOR_NORMAL         = 0x00034001;
        public static readonly int GLFW_CURSOR_HIDDEN         = 0x00034002;
        public static readonly int GLFW_CURSOR_DISABLED       = 0x00034003;

        public static readonly int GLFW_ANY_RELEASE_BEHAVIOR           = 0;
        public static readonly int GLFW_RELEASE_BEHAVIOR_FLUSH = 0x00035001;
        public static readonly int GLFW_RELEASE_BEHAVIOR_NONE = 0x00035002;

        public static readonly int GLFW_NATIVE_CONTEXT_API    = 0x00036001;
        public static readonly int GLFW_EGL_CONTEXT_API       = 0x00036002;
        public static readonly int GLFW_OSMESA_CONTEXT_API    = 0x00036003;

        public static readonly int GLFW_ANGLE_PLATFORM_TYPE_NONE   = 0x00037001;
        public static readonly int GLFW_ANGLE_PLATFORM_TYPE_OPENGL = 0x00037002;
        public static readonly int GLFW_ANGLE_PLATFORM_TYPE_OPENGLES = 0x00037003;
        public static readonly int GLFW_ANGLE_PLATFORM_TYPE_D3D9   = 0x00037004;
        public static readonly int GLFW_ANGLE_PLATFORM_TYPE_D3D11  = 0x00037005;
        public static readonly int GLFW_ANGLE_PLATFORM_TYPE_VULKAN = 0x00037007;
        public static readonly int GLFW_ANGLE_PLATFORM_TYPE_METAL  = 0x00037008;

        /*! @defgroup shapes Standard cursor shapes
         *  @brief Standard system cursor shapes.
         *
         *  These are the [standard cursor shapes](@ref cursor_standard) that can be
         *  requested from the platform (window system).
         *
         *  @ingroup input
         *  @{ */

        /*! @brief The regular arrow cursor shape.
         *
         *  The regular arrow cursor shape.
         */
        public static readonly int GLFW_ARROW_CURSOR          = 0x00036001;
        /*! @brief The text input I-beam cursor shape.
         *
         *  The text input I-beam cursor shape.
         */
        public static readonly int GLFW_IBEAM_CURSOR          = 0x00036002;
        /*! @brief The crosshair cursor shape.
         *
         *  The crosshair cursor shape.
         */
        public static readonly int GLFW_CROSSHAIR_CURSOR      = 0x00036003;
        /*! @brief The pointing hand cursor shape.
         *
         *  The pointing hand cursor shape.
         */
        public static readonly int GLFW_POINTING_HAND_CURSOR  = 0x00036004;
        /*! @brief The horizontal resize/move arrow shape.
         *
         *  The horizontal resize/move arrow shape.  This is usually a horizontal
         *  double-headed arrow.
         */
        public static readonly int GLFW_RESIZE_EW_CURSOR      = 0x00036005;
        /*! @brief The vertical resize/move arrow shape.
         *
         *  The vertical resize/move shape.  This is usually a vertical double-headed
         *  arrow.
         */
        public static readonly int GLFW_RESIZE_NS_CURSOR      = 0x00036006;
        /*! @brief The top-left to bottom-right diagonal resize/move arrow shape.
         *
         *  The top-left to bottom-right diagonal resize/move shape.  This is usually
         *  a diagonal double-headed arrow.
         *
         *  @note @macos This shape is provided by a private system API and may fail
         *  with @ref GLFW_CURSOR_UNAVAILABLE in the future.
         *
         *  @note @x11 This shape is provided by a newer standard not supported by all
         *  cursor themes.
         *
         *  @note @wayland This shape is provided by a newer standard not supported by
         *  all cursor themes.
         */
        public static readonly int GLFW_RESIZE_NWSE_CURSOR    = 0x00036007;
        /*! @brief The top-right to bottom-left diagonal resize/move arrow shape.
         *
         *  The top-right to bottom-left diagonal resize/move shape.  This is usually
         *  a diagonal double-headed arrow.
         *
         *  @note @macos This shape is provided by a private system API and may fail
         *  with @ref GLFW_CURSOR_UNAVAILABLE in the future.
         *
         *  @note @x11 This shape is provided by a newer standard not supported by all
         *  cursor themes.
         *
         *  @note @wayland This shape is provided by a newer standard not supported by
         *  all cursor themes.
         */
        public static readonly int GLFW_RESIZE_NESW_CURSOR    = 0x00036008;
        /*! @brief The omni-directional resize/move cursor shape.
         *
         *  The omni-directional resize cursor/move shape.  This is usually either
         *  a combined horizontal and vertical double-headed arrow or a grabbing hand.
         */
        public static readonly int GLFW_RESIZE_ALL_CURSOR     = 0x00036009;
        /*! @brief The operation-not-allowed shape.
         *
         *  The operation-not-allowed shape.  This is usually a circle with a diagonal
         *  line through it.
         *
         *  @note @x11 This shape is provided by a newer standard not supported by all
         *  cursor themes.
         *
         *  @note @wayland This shape is provided by a newer standard not supported by
         *  all cursor themes.
         */
        public static readonly int GLFW_NOT_ALLOWED_CURSOR    = 0x0003600A;
        /*! @brief Legacy name for compatibility.
         *
         *  This is an alias for compatibility with earlier versions.
         */
        public static readonly int GLFW_HRESIZE_CURSOR        = GLFW_RESIZE_EW_CURSOR;
        /*! @brief Legacy name for compatibility.
         *
         *  This is an alias for compatibility with earlier versions.
         */
        public static readonly int GLFW_VRESIZE_CURSOR        = GLFW_RESIZE_NS_CURSOR;
        /*! @brief Legacy name for compatibility.
         *
         *  This is an alias for compatibility with earlier versions.
         */
        public static readonly int GLFW_HAND_CURSOR           = GLFW_POINTING_HAND_CURSOR;
        /*! @} */

        public static readonly int GLFW_CONNECTED             = 0x00040001;
        public static readonly int GLFW_DISCONNECTED          = 0x00040002;

        /*! @addtogroup init
         *  @{ */
        /*! @brief Joystick hat buttons init hint.
         *
         *  Joystick hat buttons [init hint](@ref GLFW_JOYSTICK_HAT_BUTTONS).
         */
        public static readonly int GLFW_JOYSTICK_HAT_BUTTONS  = 0x00050001;
        /*! @brief ANGLE rendering backend init hint.
         *
         *  ANGLE rendering backend [init hint](@ref GLFW_ANGLE_PLATFORM_TYPE_hint).
         */
        public static readonly int GLFW_ANGLE_PLATFORM_TYPE   = 0x00050002;
        /*! @brief Platform selection init hint.
         *
         *  Platform selection [init hint](@ref GLFW_PLATFORM).
         */
        public static readonly int GLFW_PLATFORM              = 0x00050003;
        /*! @brief macOS specific init hint.
         *
         *  macOS specific [init hint](@ref GLFW_COCOA_CHDIR_RESOURCES_hint).
         */
        public static readonly int GLFW_COCOA_CHDIR_RESOURCES = 0x00051001;
        /*! @brief macOS specific init hint.
         *
         *  macOS specific [init hint](@ref GLFW_COCOA_MENUBAR_hint).
         */
        public static readonly int GLFW_COCOA_MENUBAR         = 0x00051002;
        /*! @brief X11 specific init hint.
         *
         *  X11 specific [init hint](@ref GLFW_X11_XCB_VULKAN_SURFACE_hint).
         */
        public static readonly int GLFW_X11_XCB_VULKAN_SURFACE = 0x00052001;
        /*! @} */

        /*! @addtogroup init
         *  @{ */
        /*! @brief Hint value that enables automatic platform selection.
         *
         *  Hint value for @ref GLFW_PLATFORM that enables automatic platform selection.
         */
        public static readonly int GLFW_ANY_PLATFORM          = 0x00060000;
        public static readonly int GLFW_PLATFORM_WIN32        = 0x00060001;
        public static readonly int GLFW_PLATFORM_COCOA        = 0x00060002;
        public static readonly int GLFW_PLATFORM_WAYLAND      = 0x00060003;
        public static readonly int GLFW_PLATFORM_X11          = 0x00060004;
        public static readonly int GLFW_PLATFORM_NULL         = 0x00060005;
        /*! @} */

        public static readonly int GLFW_DONT_CARE             = -1;

        #endregion

        #region Constructors

        static Glfw()
        {
            Init();
            SetErrorCallback(errorCallback);
        }

        #endregion

        /// <summary>
        ///     Returns and clears the error code of the last error that occurred on the calling thread, and optionally
        ///     a description of it.
        ///     <para>
        ///         If no error has occurred since the last call, it returns <see cref="ErrorCode.None" /> and the
        ///         description pointer is set to <c>null</c>.
        ///     </para>
        /// </summary>
        /// <param name="description">The description string, or <c>null</c> if there is no error.</param>
        /// <returns>The error code.</returns>
        public static ErrorCode GetError(out string description)
        {
            var code = GetErrorPrivate(out var ptr);
            description = code == ErrorCode.None ? null : Util.PtrToStringUTF8(ptr);
            return code;
        }

        /// <summary>
        ///     Retrieves the content scale for the specified monitor. The content scale is the ratio between the
        ///     current DPI and the platform's default DPI.
        ///     <para>
        ///         This is especially important for text and any UI elements. If the pixel dimensions of your UI scaled by
        ///         this look appropriate on your machine then it should appear at a reasonable size on other machines
        ///         regardless of their DPI and scaling settings. This relies on the system DPI and scaling settings being
        ///         somewhat correct.
        ///     </para>
        /// </summary>
        /// <param name="monitor">The monitor to query.</param>
        /// <param name="xScale">The scale on the x-axis.</param>
        /// <param name="yScale">The scale on the y-axis.</param>
        [DllImport(LIBRARY, EntryPoint = "glfwGetMonitorContentScale", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetMonitorContentScale(IntPtr monitor, out float xScale, out float yScale);

        /// <summary>
        ///     Returns the current value of the user-defined pointer of the specified <paramref name="monitor" />.
        /// </summary>
        /// <param name="monitor">The monitor whose pointer to return.</param>
        /// <returns>The user-pointer, or <see cref="IntPtr.Zero" /> if none is defined.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwGetMonitorUserPointer", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetMonitorUserPointer(IntPtr monitor);

        /// <summary>
        ///     This function sets the user-defined pointer of the specified <paramref name="monitor" />.
        ///     <para>The current value is retained until the monitor is disconnected.</para>
        /// </summary>
        /// <param name="monitor">The monitor whose pointer to set.</param>
        /// <param name="pointer">The user-defined pointer value.</param>
        [DllImport(LIBRARY, EntryPoint = "glfwSetMonitorUserPointer", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetMonitorUserPointer(IntPtr monitor, IntPtr pointer);

        /// <summary>
        ///     Returns the opacity of the window, including any decorations.
        /// </summary>
        /// <param name="window">The window to query.</param>
        /// <returns>The opacity value of the specified window, a value between <c>0.0</c> and <c>1.0</c> inclusive.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwGetWindowOpacity", CallingConvention = CallingConvention.Cdecl)]
        public static extern float GetWindowOpacity(IntPtr window);

        /// <summary>
        ///     Sets the opacity of the window, including any decorations.
        ///     <para>
        ///         The opacity (or alpha) value is a positive finite number between zero and one, where zero is fully
        ///         transparent and one is fully opaque.
        ///     </para>
        /// </summary>
        /// <param name="window">The window to set the opacity for.</param>
        /// <param name="opacity">The desired opacity of the specified window.</param>
        [DllImport(LIBRARY, EntryPoint = "glfwSetWindowOpacity", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetWindowOpacity(IntPtr window, float opacity);

        /// <summary>
        ///     Sets hints for the next call to <see cref="CreateWindow" />. The hints, once set, retain their values until
        ///     changed by a call to this function or <see cref="DefaultWindowHints" />, or until the library is terminated.
        ///     <para>
        ///         Some hints are platform specific. These may be set on any platform but they will only affect their
        ///         specific platform. Other platforms will ignore them. Setting these hints requires no platform specific
        ///         headers or functions.
        ///     </para>
        /// </summary>
        /// <param name="hint">The window hit to set.</param>
        /// <param name="value">The new value of the window hint.</param>
        [DllImport(LIBRARY, EntryPoint = "glfwWindowHintString", CallingConvention = CallingConvention.Cdecl)]
        public static extern void WindowHintString(Hint hint, byte[] value);

        /// <summary>
        ///     Helper function to call <see cref="WindowHintString(Hint, byte[])" /> with UTF-8 encoding.
        /// </summary>
        /// <param name="hint">The window hit to set.</param>
        /// <param name="value">The new value of the window hint.</param>
        // ReSharper disable once InconsistentNaming
        public static void WindowHintStringUTF8(Hint hint, string value)
        {
            WindowHintString(hint, Encoding.UTF8.GetBytes(value));
        }

        /// <summary>
        ///     Helper function to call <see cref="WindowHintString(Hint, byte[])" /> with ASCII encoding.
        /// </summary>
        /// <param name="hint">The window hit to set.</param>
        /// <param name="value">The new value of the window hint.</param>
        // ReSharper disable once InconsistentNaming
        public static void WindowHintStringASCII(Hint hint, string value)
        {
            WindowHintString(hint, Encoding.ASCII.GetBytes(value));
        }

        /// <summary>
        ///     Retrieves the content scale for the specified window. The content scale is the ratio between the current DPI and
        ///     the platform's default DPI. This is especially important for text and any UI elements. If the pixel dimensions of
        ///     your UI scaled by this look appropriate on your machine then it should appear at a reasonable size on other
        ///     machines regardless of their DPI and scaling settings. This relies on the system DPI and scaling settings being
        ///     somewhat correct.
        ///     <para>
        ///         On systems where each monitors can have its own content scale, the window content scale will depend on which
        ///         monitor the system considers the window to be on.
        ///     </para>
        /// </summary>
        /// <param name="window">The window to query.</param>
        /// <param name="xScale">The content scale on the x-axis.</param>
        /// <param name="yScale">The content scale on the y-axis.</param>
        [DllImport(LIBRARY, EntryPoint = "glfwGetWindowContentScale", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetWindowContentScale(IntPtr window, out float xScale, out float yScale);

        /// <summary>
        ///     Requests user attention to the specified <paramref name="window" />. On platforms where this is not supported,
        ///     attention is
        ///     requested to the application as a whole.
        ///     <para>
        ///         Once the user has given attention, usually by focusing the window or application, the system will end the
        ///         request automatically.
        ///     </para>
        /// </summary>
        /// <param name="window">The window to request user attention to.</param>
        [DllImport(LIBRARY, EntryPoint = "glfwRequestWindowAttention", CallingConvention = CallingConvention.Cdecl)]
        public static extern void RequestWindowAttention(IntPtr window);

        /// <summary>
        ///     This function returns whether raw mouse motion is supported on the current system.
        ///     <para>
        ///         This status does not change after GLFW has been initialized so you only need to check this once. If you
        ///         attempt to enable raw motion on a system that does not support it, an error will be emitted.
        ///     </para>
        /// </summary>
        /// <returns><c>true</c> if raw mouse motion is supported on the current machine, or <c>false</c> otherwise.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwRawMouseMotionSupported", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool RawMouseMotionSupported();

        /// <summary>
        ///     Sets the maximization callback of the specified <paramref name="window," /> which is called when the window is
        ///     maximized or restored.
        /// </summary>
        /// <param name="window">The window whose callback to set.</param>
        /// <param name="cb">The new callback, or <c>null</c> to remove the currently set callback.</param>
        /// <returns>The previously set callback, or <c>null</c> if no callback was set or the library had not been initialized.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwSetWindowMaximizeCallback", CallingConvention = CallingConvention.Cdecl)]
        public static extern WindowMaximizedCallback SetWindowMaximizeCallback(IntPtr window,
            WindowMaximizedCallback cb);

        /// <summary>
        ///     Sets the window content scale callback of the specified window, which is called when the content scale of the
        ///     specified window changes.
        /// </summary>
        /// <param name="window">The window whose callback to set.</param>
        /// <param name="cb">The new callback, or <c>null</c> to remove the currently set callback</param>
        /// <returns>The previously set callback, or <c>null</c> if no callback was set or the library had not been initialized.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwSetWindowContentScaleCallback",
            CallingConvention = CallingConvention.Cdecl)]
        public static extern WindowContentsScaleCallback SetWindowContentScaleCallback(IntPtr window,
            WindowContentsScaleCallback cb);

        /// <summary>
        ///     Returns the platform-specific scan-code of the specified key.
        ///     <para>If the key is <see cref="Keys.Unknown" /> or does not exist on the keyboard this method will return -1.</para>
        /// </summary>
        /// <param name="key">The named key to query.</param>
        /// <returns>The platform-specific scan-code for the key, or -1 if an error occurred.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwGetKeyScancode", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetKeyScanCode(Keys key);

        /// <summary>
        ///     Sets the value of an attribute of the specified window.
        /// </summary>
        /// <param name="window">
        ///     The window to set the attribute for
        ///     <para>Valid attributes include:</para>
        ///     <para>
        ///         <see cref="WindowAttribute.Decorated" />
        ///     </para>
        ///     <para>
        ///         <see cref="WindowAttribute.Resizable" />
        ///     </para>
        ///     <para>
        ///         <see cref="WindowAttribute.Floating" />
        ///     </para>
        ///     <para>
        ///         <see cref="WindowAttribute.AutoIconify" />
        ///     </para>
        ///     <para>
        ///         <see cref="WindowAttribute.Focused" />
        ///     </para>
        /// </param>
        /// <param name="attr">A supported window attribute.</param>
        /// <param name="value">The value to set.</param>
        [DllImport(LIBRARY, EntryPoint = "glfwSetWindowAttrib", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetWindowAttribute(IntPtr window, WindowAttribute attr, bool value);

        [DllImport(LIBRARY, EntryPoint = "glfwGetJoystickHats", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GetJoystickHats(int joystickId, out int count);

        /// <summary>
        ///     Returns the state of all hats of the specified joystick as a bitmask.
        /// </summary>
        /// <param name="joystickId">The joystick to query.</param>
        /// <returns>A bitmask enumeration containing the state of the joystick hats.</returns>
        public static Hat GetJoystickHats(int joystickId)
        {
            var hat = Hat.Centered;
            var ptr = GetJoystickHats(joystickId, out var count);
            for (var i = 0; i < count; i++)
            {
                var value = Marshal.ReadByte(ptr, i);
                hat |= (Hat) value;
            }

            return hat;
        }

        [DllImport(LIBRARY, EntryPoint = "glfwGetJoystickGUID", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GetJoystickGuidPrivate(int joystickId);

        /// <summary>
        ///     Returns the SDL compatible GUID, as a hexadecimal string, of the specified joystick.
        ///     <para>
        ///         The GUID is what connects a joystick to a gamepad mapping. A connected joystick will always have a GUID even
        ///         if there is no gamepad mapping assigned to it.
        ///     </para>
        /// </summary>
        /// <param name="joystickId">The joystick to query.</param>
        /// <returns>The GUID of the joystick, or <c>null</c> if the joystick is not present or an error occurred.</returns>
        public static string GetJoystickGuid(int joystickId)
        {
            var ptr = GetJoystickGuidPrivate(joystickId);
            return ptr == IntPtr.Zero ? null : Util.PtrToStringUTF8(ptr);
        }

        /// <summary>
        ///     This function returns the current value of the user-defined pointer of the specified joystick.
        /// </summary>
        /// <param name="joystickId">The joystick whose pointer to return.</param>
        /// <returns>The user-defined pointer, or <see cref="IntPtr.Zero" /> if never defined.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwGetJoystickUserPointer", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetJoystickUserPointer(int joystickId);

        /// <summary>
        ///     This function sets the user-defined pointer of the specified joystick.
        ///     <para>The current value is retained until the joystick is disconnected.</para>
        /// </summary>
        /// <param name="joystickId">The joystick whose pointer to set.</param>
        /// <param name="pointer">The new value.</param>
        [DllImport(LIBRARY, EntryPoint = "glfwSetJoystickUserPointer", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetJoystickUserPointer(int joystickId, IntPtr pointer);

        /// <summary>
        ///     Returns whether the specified joystick is both present and has a gamepad mapping.
        /// </summary>
        /// <param name="joystickId">The joystick to query.</param>
        /// <returns><c>true</c> if a joystick is both present and has a gamepad mapping, or <c>false</c> otherwise.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwJoystickIsGamepad", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool JoystickIsGamepad(int joystickId);

        [DllImport(LIBRARY, EntryPoint = "glfwUpdateGamepadMappings", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool UpdateGamepadMappings([NotNull] byte[] mappings);

        /// <summary>
        ///     Parses the specified string and updates the internal list with any gamepad mappings it finds.
        ///     <para>
        ///         This string may contain either a single gamepad mapping or many mappings separated by newlines. The parser
        ///         supports the full format of the SDL <c>gamecontrollerdb.txt</c> source file including empty lines and comments.
        ///     </para>
        /// </summary>
        /// <param name="mappings">The string containing the gamepad mappings.</param>
        /// <returns><c>true</c> if successful, or <c>false</c> if an error occurred.</returns>
        public static bool UpdateGamepadMappings(string mappings)
        {
            return UpdateGamepadMappings(Encoding.ASCII.GetBytes(mappings));
        }

        [DllImport(LIBRARY, EntryPoint = "glfwGetGamepadName", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GetGamepadNamePrivate(int gamepadId);

        /// <summary>
        ///     Returns the human-readable name of the gamepad from the gamepad mapping assigned to the specified joystick.
        /// </summary>
        /// <param name="gamepadId">The joystick to query.</param>
        /// <returns>
        ///     The name of the gamepad, or <c>null</c> if the joystick is not present, does not have a mapping or an error
        ///     occurred.
        /// </returns>
        public static string GetGamepadName(int gamepadId)
        {
            var ptr = GetGamepadNamePrivate(gamepadId);
            return ptr == IntPtr.Zero ? null : Util.PtrToStringUTF8(ptr);
        }

        /// <summary>
        ///     Retrieves the state of the specified joystick remapped to an Xbox-like gamepad.
        /// </summary>
        /// <param name="id">The joystick to query.</param>
        /// <param name="state">The gamepad input state of the joystick.</param>
        /// <returns>
        ///     <c>true</c> if successful, or <c>false</c> if no joystick is connected, it has no gamepad mapping or an error
        ///     occurred.
        /// </returns>
        [DllImport(LIBRARY, EntryPoint = "glfwGetGamepadState", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GetGamepadState(int id, out GamePadState state);

        #region Properties

        /// <summary>
        ///     Gets the window whose OpenGL or OpenGL ES context is current on the calling thread, or <see cref="Window.None" />
        ///     if no context is current.
        /// </summary>
        /// <value>
        ///     The current context.
        /// </value>
        public static Window CurrentContext => GetCurrentContext();

        /// <summary>
        ///     Gets an array of handles for all currently connected monitors.
        ///     <para>The primary monitor is always first in the array.</para>
        /// </summary>
        /// <value>
        ///     The monitors.
        /// </value>
        public static Monitor[] Monitors
        {
            get
            {
                var ptr = GetMonitors(out var count);
                var monitors = new Monitor[count];
                var offset = 0;
                for (var i = 0; i < count; i++, offset += IntPtr.Size)
                {
                    monitors[i] = Marshal.PtrToStructure<Monitor>(ptr + offset);
                }

                return monitors;
            }
        }

        /// <summary>
        ///     Gets the primary monitor. This is usually the monitor where elements like the task bar or global menu bar are
        ///     located.
        /// </summary>
        /// <value>
        ///     The primary monitor, or <see cref="Monitor.None" /> if no monitors were found or if an error occurred.
        /// </value>
        public static Monitor PrimaryMonitor => GetPrimaryMonitor();

        /// <summary>
        ///     Gets or sets the value of the GLFW timer.
        ///     <para>
        ///         The resolution of the timer is system dependent, but is usually on the order of a few micro- or nanoseconds.
        ///         It uses the highest-resolution monotonic time source on each supported platform.
        ///     </para>
        /// </summary>
        /// <value>
        ///     The time.
        /// </value>
        public static double Time
        {
            get => GetTime();
            set => SetTime(value);
        }

        /// <summary>
        ///     Gets the frequency, in Hz, of the raw timer.
        /// </summary>
        /// <value>
        ///     The frequency of the timer, in Hz, or zero if an error occurred.
        /// </value>
        public static ulong TimerFrequency => GetTimerFrequency();

        /// <summary>
        ///     Gets the current value of the raw timer, measured in 1 / frequency seconds.
        /// </summary>
        /// <value>
        ///     The timer value.
        /// </value>
        public static ulong TimerValue => GetTimerValue();

        /// <summary>
        ///     Gets the version of the native GLFW library.
        /// </summary>
        /// <value>
        ///     The version.
        /// </value>
        public static Version Version
        {
            get
            {
                GetVersion(out var major, out var minor, out var revision);
                return new Version(major, minor, revision);
            }
        }

        /// <summary>
        ///     Gets the compile-time generated version string of the GLFW library binary.
        ///     <para>It describes the version, platform, compiler and any platform-specific compile-time options.</para>
        /// </summary>
        /// <value>
        ///     The version string.
        /// </value>
        public static string VersionString => Util.PtrToStringUTF8(GetVersionString());

        #endregion

        #region External

        /// <summary>
        ///     This function sets hints for the next initialization of GLFW.
        ///     <para>
        ///         The values you set hints to are never reset by GLFW, but they only take effect during initialization.
        ///         Once GLFW has been initialized, any values you set will be ignored until the library is terminated and
        ///         initialized again.>.
        ///     </para>
        /// </summary>
        /// <param name="hint">
        ///     The hint, valid values are <see cref="Hint.JoystickHatButtons" />,
        ///     <see cref="Hint.CocoaMenuBar" />, and <see cref="Hint.CocoaChDirResources" />.
        /// </param>
        /// <param name="value">The value of the hint.</param>
        [DllImport(LIBRARY, EntryPoint = "glfwInitHint", CallingConvention = CallingConvention.Cdecl)]
        public static extern void InitHint(Hint hint, bool value);

        /// <summary>
        ///     This function initializes the GLFW library. Before most GLFW functions can be used, GLFW must be initialized, and
        ///     before an application terminates GLFW should be terminated in order to free any resources allocated during or after
        ///     initialization.
        ///     <para>
        ///         If this function fails, it calls <see cref="Terminate" /> before returning. If it succeeds, you should call
        ///         <see cref="Terminate" /> before the application exits
        ///     </para>
        ///     <para>
        ///         Additional calls to this function after successful initialization but before termination will return
        ///         <c>true</c> immediately.
        ///     </para>
        /// </summary>
        /// <returns><c>true</c> if successful, or <c>false</c> if an error occurred.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwInit", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Init();

        /// <summary>
        ///     This function destroys all remaining windows and cursors, restores any modified gamma ramps and frees any other
        ///     allocated resources. Once this function is called, you must again call <see cref="Init" /> successfully before you
        ///     will be able to use most GLFW functions.
        ///     If GLFW has been successfully initialized, this function should be called before the application exits. If
        ///     initialization fails, there is no need to call this function, as it is called by <see cref="Init" /> before it
        ///     returns failure.
        /// </summary>
        /// <note type="warning">
        ///     The contexts of any remaining windows must not be current on any other thread when this function
        ///     is called.
        /// </note>
        [DllImport(LIBRARY, EntryPoint = "glfwTerminate", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Terminate();

        /// <summary>
        ///     Sets the error callback, which is called with an error code and a human-readable description each
        ///     time a GLFW error occurs.
        /// </summary>
        /// <param name="errorHandler">The callback function, or <c>null</c> to unbind this callback.</param>
        /// <returns>The previously set callback function, or <c>null</c> if no callback was already set.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwSetErrorCallback", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(ErrorCallback))]
        public static extern ErrorCallback SetErrorCallback(ErrorCallback errorHandler);

        [DllImport(LIBRARY, EntryPoint = "glfwCreateWindow", CallingConvention = CallingConvention.Cdecl)]
        private static extern Window CreateWindow(int width, int height, byte[] title, Monitor monitor, Window share);

        /// <summary>
        ///     This function destroys the specified window and its context. On calling this function, no further callbacks will be
        ///     called for that window.
        ///     <para>If the context of the specified window is current on the main thread, it is detached before being destroyed.</para>
        /// </summary>
        /// <param name="window">A window instance.</param>
        [DllImport(LIBRARY, EntryPoint = "glfwDestroyWindow", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DestroyWindow(Window window);

        /// <summary>
        ///     This function makes the specified window visible if it was previously hidden. If the window is already visible or
        ///     is in full screen mode, this function does nothing.
        /// </summary>
        /// <param name="window">A window instance.</param>
        [DllImport(LIBRARY, EntryPoint = "glfwShowWindow", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ShowWindow(Window window);

        /// <summary>
        ///     This function hides the specified window if it was previously visible. If the window is already hidden or is in
        ///     full screen mode, this function does nothing.
        /// </summary>
        /// <param name="window">A window instance.</param>
        [DllImport(LIBRARY, EntryPoint = "glfwHideWindow", CallingConvention = CallingConvention.Cdecl)]
        public static extern void HideWindow(Window window);

        /// <summary>
        ///     This function retrieves the position, in screen coordinates, of the upper-left corner of the client area of the
        ///     specified window.
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the client area.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the client area.</param>
        [DllImport(LIBRARY, EntryPoint = "glfwGetWindowPos", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetWindowPosition(Window window, out int x, out int y);

        /// <summary>
        ///     Sets the position, in screen coordinates, of the upper-left corner of the client area of the
        ///     specified windowed mode window.
        ///     <para>If the window is a full screen window, this function does nothing.</para>
        /// </summary>
        /// <note type="important">
        ///     Do not use this function to move an already visible window unless you have very good reasons for
        ///     doing so, as it will confuse and annoy the user.
        /// </note>
        /// <param name="window">A window instance.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the client area.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the client area.</param>
        /// <remarks>
        ///     The window manager may put limits on what positions are allowed. GLFW cannot and should not override these
        ///     limits.
        /// </remarks>
        [DllImport(LIBRARY, EntryPoint = "glfwSetWindowPos", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetWindowPosition(Window window, int x, int y);

        /// <summary>
        ///     This function retrieves the size, in screen coordinates, of the client area of the specified window.
        ///     <para>
        ///         If you wish to retrieve the size of the framebuffer of the window in pixels, use
        ///         <see cref="GetFramebufferSize" />.
        ///     </para>
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <param name="width">The width, in screen coordinates.</param>
        /// <param name="height">The height, in screen coordinates.</param>
        [DllImport(LIBRARY, EntryPoint = "glfwGetWindowSize", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetWindowSize(Window window, out int width, out int height);

        /// <summary>
        ///     Sets the size, in screen coordinates, of the client area of the specified window.
        ///     <para>
        ///         For full screen windows, this function updates the resolution of its desired video mode and switches to the
        ///         video mode closest to it, without affecting the window's context. As the context is unaffected, the bit depths
        ///         of the framebuffer remain unchanged.
        ///     </para>
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <param name="width">The desired width, in screen coordinates, of the window client area.</param>
        /// <param name="height">The desired height, in screen coordinates, of the window client area.</param>
        /// <remarks>The window manager may put limits on what sizes are allowed. GLFW cannot and should not override these limits.</remarks>
        [DllImport(LIBRARY, EntryPoint = "glfwSetWindowSize", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetWindowSize(Window window, int width, int height);

        /// <summary>
        ///     This function retrieves the size, in pixels, of the framebuffer of the specified window.
        ///     <para>If you wish to retrieve the size of the window in screen coordinates, use <see cref="GetWindowSize" />.</para>
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <param name="width">The width, in pixels, of the framebuffer.</param>
        /// <param name="height">The height, in pixels, of the framebuffer.</param>
        [DllImport(LIBRARY, EntryPoint = "glfwGetFramebufferSize", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetFramebufferSize(Window window, out int width, out int height);

        /// <summary>
        ///     Sets the position callback of the specified window, which is called when the window is moved.
        ///     <para>The callback is provided with the screen position of the upper-left corner of the client area of the window.</para>
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <param name="positionCallback">The position callback to be invoked on position changes.</param>
        /// <returns>The previously set callback, or <c>null</c> if no callback was set or the library had not been initialized.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwSetWindowPosCallback", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(PositionCallback))]
        public static extern PositionCallback SetWindowPositionCallback(Window window,
            PositionCallback positionCallback);

        /// <summary>
        ///     Sets the size callback of the specified window, which is called when the window is resized.
        ///     <para>The callback is provided with the size, in screen coordinates, of the client area of the window.</para>
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <param name="sizeCallback">The size callback to be invoked on size changes.</param>
        /// <returns>The previously set callback, or <c>null</c> if no callback was set or the library had not been initialized.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwSetWindowSizeCallback", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(SizeCallback))]
        public static extern SizeCallback SetWindowSizeCallback(Window window, SizeCallback sizeCallback);

        /// <summary>
        ///     Sets the window title, encoded as UTF-8, of the specified window.
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <param name="title">The title as an array of UTF-8 encoded bytes.</param>
        [DllImport(LIBRARY, EntryPoint = "glfwSetWindowTitle", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SetWindowTitle(Window window, byte[] title);

        /// <summary>
        ///     This function brings the specified window to front and sets input focus. The window should already be visible and
        ///     not iconified.
        /// </summary>
        /// <param name="window">The window.</param>
        [DllImport(LIBRARY, EntryPoint = "glfwFocusWindow", CallingConvention = CallingConvention.Cdecl)]
        public static extern void FocusWindow(Window window);

        /// <summary>
        ///     Sets the focus callback of the specified window, which is called when the window gains or loses input
        ///     focus.
        ///     <para>
        ///         After the focus callback is called for a window that lost input focus, synthetic key and mouse button release
        ///         events will be generated for all such that had been pressed.
        ///     </para>
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <param name="focusCallback">The new callback, or <c>null</c> to remove the currently set callback.</param>
        [DllImport(LIBRARY, EntryPoint = "glfwSetWindowFocusCallback", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(FocusCallback))]
        public static extern FocusCallback SetWindowFocusCallback(Window window, FocusCallback focusCallback);

        /// <summary>
        ///     This function retrieves the major, minor and revision numbers of the GLFW library.
        ///     <para>
        ///         It is intended for when you are using GLFW as a shared library and want to ensure that you are using the
        ///         minimum required version.
        ///     </para>
        /// </summary>
        /// <param name="major">The major.</param>
        /// <param name="minor">The minor.</param>
        /// <param name="revision">The revision.</param>
        /// <seealso cref="Version" />
        [DllImport(LIBRARY, EntryPoint = "glfwGetVersion", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetVersion(out int major, out int minor, out int revision);

        /// <summary>
        ///     Gets the compile-time generated version string of the GLFW library binary.
        ///     <para>It describes the version, platform, compiler and any platform-specific compile-time options.</para>
        /// </summary>
        /// <returns>A pointer to the null-terminated UTF-8 encoded version string.</returns>
        /// <seealso cref="VersionString" />
        [DllImport(LIBRARY, EntryPoint = "glfwGetVersionString", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GetVersionString();

        [DllImport(LIBRARY, EntryPoint = "glfwGetTime", CallingConvention = CallingConvention.Cdecl)]
        private static extern double GetTime();

        [DllImport(LIBRARY, EntryPoint = "glfwSetTime", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SetTime(double time);

        [DllImport(LIBRARY, EntryPoint = "glfwGetTimerFrequency", CallingConvention = CallingConvention.Cdecl)]
        private static extern ulong GetTimerFrequency();

        [DllImport(LIBRARY, EntryPoint = "glfwGetTimerValue", CallingConvention = CallingConvention.Cdecl)]
        private static extern ulong GetTimerValue();

        /// <summary>
        ///     This function retrieves the size, in screen coordinates, of each edge of the frame of the specified window.
        ///     <para>
        ///         This size includes the title bar, if the window has one. The size of the frame may vary depending on the
        ///         window-related hints used to create it.
        ///     </para>
        ///     <para>
        ///         Because this function retrieves the size of each window frame edge and not the offset along a particular
        ///         coordinate axis, the retrieved values will always be zero or positive.
        ///     </para>
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <param name="left">The size, in screen coordinates, of the left edge of the window frame</param>
        /// <param name="top">The size, in screen coordinates, of the top edge of the window frame</param>
        /// <param name="right">The size, in screen coordinates, of the right edge of the window frame.</param>
        /// <param name="bottom">The size, in screen coordinates, of the bottom edge of the window frame</param>
        [DllImport(LIBRARY, EntryPoint = "glfwGetWindowFrameSize", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetWindowFrameSize(Window window, out int left, out int top, out int right,
            out int bottom);

        /// <summary>
        ///     This function maximizes the specified window if it was previously not maximized. If the window is already
        ///     maximized, this function does nothing.
        ///     <para>If the specified window is a full screen window, this function does nothing.</para>
        /// </summary>
        /// <param name="window">A window instance.</param>
        [DllImport(LIBRARY, EntryPoint = "glfwMaximizeWindow", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MaximizeWindow(Window window);

        /// <summary>
        ///     This function iconifies (minimizes) the specified window if it was previously restored.
        ///     <para>If the window is already iconified, this function does nothing.</para>
        /// </summary>
        /// <param name="window">A window instance.</param>
        [DllImport(LIBRARY, EntryPoint = "glfwIconifyWindow", CallingConvention = CallingConvention.Cdecl)]
        public static extern void IconifyWindow(Window window);

        /// <summary>
        ///     This function restores the specified window if it was previously iconified (minimized) or maximized.
        ///     <para>If the window is already restored, this function does nothing.</para>
        ///     <para>
        ///         If the specified window is a full screen window, the resolution chosen for the window is restored on the
        ///         selected monitor.
        ///     </para>
        /// </summary>
        /// <param name="window">A window instance.</param>
        [DllImport(LIBRARY, EntryPoint = "glfwRestoreWindow", CallingConvention = CallingConvention.Cdecl)]
        public static extern void RestoreWindow(Window window);

        /// <summary>
        ///     This function makes the OpenGL or OpenGL ES context of the specified window current on the calling thread.
        ///     <para>
        ///         A context can only be made current on a single thread at a time and each thread can have only a single
        ///         current context at a time.
        ///     </para>
        ///     <para>By default, making a context non-current implicitly forces a pipeline flush.</para>
        /// </summary>
        /// <param name="window">A window instance.</param>
        [DllImport(LIBRARY, EntryPoint = "glfwMakeContextCurrent", CallingConvention = CallingConvention.Cdecl)]
        public static extern void MakeContextCurrent(Window window);

        /// <summary>
        ///     This function swaps the front and back buffers of the specified window when rendering with OpenGL or OpenGL ES.
        ///     <para>
        ///         If the swap interval is greater than zero, the GPU driver waits the specified number of screen updates before
        ///         swapping the buffers.
        ///     </para>
        ///     <para>This function does not apply to Vulkan.</para>
        /// </summary>
        /// <param name="window">A window instance.</param>
        [DllImport(LIBRARY, EntryPoint = "glfwSwapBuffers", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SwapBuffers(Window window);

        /// <summary>
        ///     Sets the swap interval for the current OpenGL or OpenGL ES context, i.e. the number of screen updates
        ///     to wait from the time <see cref="SwapBuffers" /> was called before swapping the buffers and returning.
        ///     <para>This is sometimes called vertical synchronization, vertical retrace synchronization or just vsync.</para>
        ///     <para>
        ///         A context must be current on the calling thread. Calling this function without a current context will cause
        ///         an exception.
        ///     </para>
        ///     <para>
        ///         This function does not apply to Vulkan. If you are rendering with Vulkan, see the present mode of your
        ///         swapchain instead.
        ///     </para>
        /// </summary>
        /// <param name="interval">
        ///     The minimum number of screen updates to wait for until the buffers are swapped by
        ///     <see cref="SwapBuffers" />.
        /// </param>
        [DllImport(LIBRARY, EntryPoint = "glfwSwapInterval", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SwapInterval(int interval);

        /// <summary>
        ///     Gets whether the specified API extension is supported by the current OpenGL or OpenGL ES context.
        ///     <para>It searches both for client API extension and context creation API extensions.</para>
        /// </summary>
        /// <param name="extension">The extension name as an array of ASCII encoded bytes.</param>
        /// <returns><c>true</c> if the extension is supported; otherwise <c>false</c>.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwExtensionSupported", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool GetExtensionSupported(byte[] extension);

        /// <summary>
        ///     This function resets all window hints to their default values.
        /// </summary>
        [DllImport(LIBRARY, EntryPoint = "glfwDefaultWindowHints", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DefaultWindowHints();

        /// <summary>
        ///     Gets the value of the close flag of the specified window.
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <returns><c>true</c> if close flag is present; otherwise <c>false</c>.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwWindowShouldClose", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool WindowShouldClose(Window window);

        /// <summary>
        ///     Sets the value of the close flag of the specified window.
        ///     <para>This can be used to override the user's attempt to close the window, or to signal that it should be closed.</para>
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <param name="close"><c>true</c> to set close flag, or <c>false</c> to cancel flag.</param>
        [DllImport(LIBRARY, EntryPoint = "glfwSetWindowShouldClose", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetWindowShouldClose(Window window, bool close);

        /// <summary>
        ///     Sets the icon of the specified window. If passed an array of candidate images, those of or closest to
        ///     the sizes desired by the system are selected. If no images are specified, the window reverts to its default icon.
        ///     <para>
        ///         The desired image sizes varies depending on platform and system settings. The selected images will be
        ///         rescaled as needed. Good sizes include 16x16, 32x32 and 48x48.
        ///     </para>
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <param name="count">The number of images in <paramref name="images" />.</param>
        /// <param name="images">An array of icon images.</param>
        [DllImport(LIBRARY, EntryPoint = "glfwSetWindowIcon", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetWindowIcon(Window window, int count, Image[] images);

        /// <summary>
        ///     This function puts the calling thread to sleep until at least one event is available in the event queue. Once one
        ///     or more events are available, it behaves exactly like glfwPollEvents, i.e. the events in the queue are processed
        ///     and the function then returns immediately. Processing events will cause the window and input callbacks associated
        ///     with those events to be called.
        ///     <para>
        ///         Since not all events are associated with callbacks, this function may return without a callback having been
        ///         called even if you are monitoring all callbacks.
        ///     </para>
        ///     <para>
        ///         On some platforms, a window move, resize or menu operation will cause event processing to block. This is due
        ///         to how event processing is designed on those platforms. You can use the window refresh callback to redraw the
        ///         contents of your window when necessary during such operations.
        ///     </para>
        /// </summary>
        [DllImport(LIBRARY, EntryPoint = "glfwWaitEvents", CallingConvention = CallingConvention.Cdecl)]
        public static extern void WaitEvents();

        /// <summary>
        ///     This function processes only those events that are already in the event queue and then returns immediately.
        ///     Processing events will cause the window and input callbacks associated with those events to be called.
        ///     <para>
        ///         On some platforms, a window move, resize or menu operation will cause event processing to block. This is due
        ///         to how event processing is designed on those platforms. You can use the window refresh callback to redraw the
        ///         contents of your window when necessary during such operations.
        ///     </para>
        ///     <para>
        ///         On some platforms, certain events are sent directly to the application without going through the event queue,
        ///         causing callbacks to be called outside of a call to one of the event processing functions.
        ///     </para>
        /// </summary>
        [DllImport(LIBRARY, EntryPoint = "glfwPollEvents", CallingConvention = CallingConvention.Cdecl)]
        public static extern void PollEvents();

        /// <summary>
        ///     This function posts an empty event from the current thread to the event queue, causing <see cref="WaitEvents" /> or
        ///     <see cref="WaitEventsTimeout " /> to return.
        /// </summary>
        [DllImport(LIBRARY, EntryPoint = "glfwPostEmptyEvent", CallingConvention = CallingConvention.Cdecl)]
        public static extern void PostEmptyEvent();

        /// <summary>
        ///     This function puts the calling thread to sleep until at least one event is available in the event queue, or until
        ///     the specified timeout is reached. If one or more events are available, it behaves exactly like
        ///     <see cref="PollEvents" />, i.e. the events in the queue are processed and the function then returns immediately.
        ///     Processing events will cause the window and input callbacks associated with those events to be called.
        ///     <para>The timeout value must be a positive finite number.</para>
        ///     <para>
        ///         Since not all events are associated with callbacks, this function may return without a callback having been
        ///         called even if you are monitoring all callbacks.
        ///     </para>
        ///     <para>
        ///         On some platforms, a window move, resize or menu operation will cause event processing to block. This is due
        ///         to how event processing is designed on those platforms. You can use the window refresh callback to redraw the
        ///         contents of your window when necessary during such operations.
        ///     </para>
        /// </summary>
        /// <param name="timeout">The maximum amount of time, in seconds, to wait.</param>
        [DllImport(LIBRARY, EntryPoint = "glfwWaitEventsTimeout", CallingConvention = CallingConvention.Cdecl)]
        public static extern void WaitEventsTimeout(double timeout);

        /// <summary>
        ///     Sets the close callback of the specified window, which is called when the user attempts to close the
        ///     window, for example by clicking the close widget in the title bar.
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <param name="closeCallback">The new callback, or <c>null</c> to remove the currently set callback.</param>
        /// <returns>The previously set callback, or <c>null</c> if no callback was set or the library had not been initialized.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwSetWindowCloseCallback", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(WindowCallback))]
        public static extern WindowCallback SetCloseCallback(Window window, WindowCallback closeCallback);

        [DllImport(LIBRARY, EntryPoint = "glfwGetPrimaryMonitor", CallingConvention = CallingConvention.Cdecl)]
        private static extern Monitor GetPrimaryMonitor();

        [DllImport(LIBRARY, EntryPoint = "glfwGetVideoMode", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GetVideoModeInternal(Monitor monitor);

        [DllImport(LIBRARY, EntryPoint = "glfwGetVideoModes", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GetVideoModes(Monitor monitor, out int count);

        /// <summary>
        ///     Gets the handle of the monitor that the specified window is in full screen on.
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <returns>The monitor, or <see cref="Monitor.None" /> if the window is in windowed mode or an error occurred.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwGetWindowMonitor", CallingConvention = CallingConvention.Cdecl)]
        public static extern Monitor GetWindowMonitor(Window window);

        /// <summary>
        ///     Sets the monitor that the window uses for full screen mode or, if the monitor is
        ///     <see cref="Monitor.None" />, makes it windowed mode.
        ///     <para>
        ///         When setting a monitor, this function updates the width, height and refresh rate of the desired video mode
        ///         and switches to the video mode closest to it. The window position is ignored when setting a monitor.
        ///     </para>
        ///     <para>
        ///         When the monitor is <see cref="Monitor.None" />, the position, width and height are used to place the window
        ///         client area. The refresh rate is ignored when no monitor is specified.
        ///     </para>
        ///     <para>
        ///         If you only wish to update the resolution of a full screen window or the size of a windowed mode window, use
        ///         <see cref="SetWindowSize" />.
        ///     </para>
        ///     <para>
        ///         When a window transitions from full screen to windowed mode, this function restores any previous window
        ///         settings such as whether it is decorated, floating, resizable, has size or aspect ratio limits, etc..
        ///     </para>
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <param name="monitor">The desired monitor, or <see cref="Monitor.None" /> to set windowed mode.</param>
        /// <param name="x">The desired x-coordinate of the upper-left corner of the client area.</param>
        /// <param name="y">The desired y-coordinate of the upper-left corner of the client area.</param>
        /// <param name="width">The desired width, in screen coordinates, of the client area or video mode.</param>
        /// <param name="height">The desired height, in screen coordinates, of the client area or video mode.</param>
        /// <param name="refreshRate">The desired refresh rate, in Hz, of the video mode, or <see cref="Constants.Default" />.</param>
        [DllImport(LIBRARY, EntryPoint = "glfwSetWindowMonitor", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetWindowMonitor(Window window, Monitor monitor, int x, int y, int width, int height,
            int refreshRate);

        [DllImport(LIBRARY, EntryPoint = "glfwGetGammaRamp", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr GetGammaRampInternal(Monitor monitor);

        /// <summary>
        ///     Sets the current gamma ramp for the specified monitor.
        ///     <para>
        ///         The original gamma ramp for that monitor is saved by GLFW the first time this function is called and is
        ///         restored by <see cref="Terminate" />.
        ///     </para>
        ///     <para>WARNING: Gamma ramps with sizes other than 256 are not supported on some platforms (Windows).</para>
        /// </summary>
        /// <param name="monitor">The monitor whose gamma ramp to set.</param>
        /// <param name="gammaRamp">The gamma ramp to use.</param>
        [DllImport(LIBRARY, EntryPoint = "glfwSetGammaRamp", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetGammaRamp(Monitor monitor, GammaRamp gammaRamp);

        /// <summary>
        ///     This function generates a 256-element gamma ramp from the specified exponent and then calls
        ///     <see cref="SetGammaRamp" /> with it.
        ///     <para>The value must be a finite number greater than zero.</para>
        /// </summary>
        /// <param name="monitor">The monitor whose gamma ramp to set.</param>
        /// <param name="gamma">The desired exponent.</param>
        [DllImport(LIBRARY, EntryPoint = "glfwSetGamma", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetGamma(Monitor monitor, float gamma);

        [DllImport(LIBRARY, EntryPoint = "glfwGetClipboardString", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GetClipboardStringInternal(Window window);

        [DllImport(LIBRARY, EntryPoint = "glfwSetClipboardString", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SetClipboardString(Window window, byte[] bytes);

        /// <summary>
        ///     Sets the file drop callback of the specified window, which is called when one or more dragged files
        ///     are dropped on the window.
        ///     <para>
        ///         Because the path array and its strings may have been generated specifically for that event, they are not
        ///         guaranteed to be valid after the callback has returned. If you wish to use them after the callback returns, you
        ///         need to make a deep copy.
        ///     </para>
        /// </summary>
        /// <param name="window">The window whose callback to set.</param>
        /// <param name="dropCallback">The new file drop callback, or <c>null</c> to remove the currently set callback.</param>
        /// <returns>The previously set callback, or <c>null</c> if no callback was set or the library had not been initialized.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwSetDropCallback", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(FileDropCallback))]
        public static extern FileDropCallback SetDropCallback(Window window, FileDropCallback dropCallback);

        [DllImport(LIBRARY, EntryPoint = "glfwGetMonitorName", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GetMonitorNameInternal(Monitor monitor);

        /// <summary>
        ///     Creates a new custom cursor image that can be set for a window with glfwSetCursor.
        ///     <para>
        ///         The cursor can be destroyed with <see cref="DestroyCursor" />. Any remaining cursors are destroyed by
        ///         <see cref="Terminate" />.
        ///     </para>
        ///     <para>
        ///         The pixels are 32-bit, little-endian, non-premultiplied RGBA, i.e. eight bits per channel. They are arranged
        ///         canonically as packed sequential rows, starting from the top-left corner.
        ///     </para>
        ///     <para>
        ///         The cursor hotspot is specified in pixels, relative to the upper-left corner of the cursor image. Like all
        ///         other coordinate systems in GLFW, the X-axis points to the right and the Y-axis points down.
        ///     </para>
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="xHotspot">The x hotspot.</param>
        /// <param name="yHotspot">The y hotspot.</param>
        /// <returns>The created cursor.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwCreateCursor", CallingConvention = CallingConvention.Cdecl)]
        public static extern Cursor CreateCursor(Image image, int xHotspot, int yHotspot);

        /// <summary>
        ///     This function destroys a cursor previously created with <see cref="CreateCursor" />. Any remaining cursors will be
        ///     destroyed by <see cref="Terminate" />.
        /// </summary>
        /// <param name="cursor">The cursor object to destroy.</param>
        [DllImport(LIBRARY, EntryPoint = "glfwDestroyCursor", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DestroyCursor(Cursor cursor);

        /// <summary>
        ///     Sets the cursor image to be used when the cursor is over the client area of the specified window.
        ///     <para>The set cursor will only be visible when the cursor mode of the window is <see cref="CursorMode.Normal" />.</para>
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <param name="cursor">The cursor to set, or <see cref="Cursor.None" /> to switch back to the default arrow cursor.</param>
        [DllImport(LIBRARY, EntryPoint = "glfwSetCursor", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetCursor(Window window, Cursor cursor);

        /// <summary>
        ///     Returns a cursor with a standard shape, that can be set for a window with <see cref="SetCursor" />.
        /// </summary>
        /// <param name="type">The type of cursor to create.</param>
        /// <returns>A new cursor ready to use or <see cref="Cursor.None" /> if an error occurred.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwCreateStandardCursor", CallingConvention = CallingConvention.Cdecl)]
        public static extern Cursor CreateStandardCursor(CursorType type);

        /// <summary>
        ///     Gets the position of the cursor, in screen coordinates, relative to the upper-left corner of the
        ///     client area of the specified window
        ///     <para>
        ///         If the cursor is disabled then the cursor position is unbounded and limited only by the minimum and maximum
        ///         values of a double.
        ///     </para>
        ///     <para>
        ///         The coordinate can be converted to their integer equivalents with the floor function. Casting directly to an
        ///         integer type works for positive coordinates, but fails for negative ones.
        ///     </para>
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <param name="x">The cursor x-coordinate, relative to the left edge of the client area.</param>
        /// <param name="y">The cursor y-coordinate, relative to the left edge of the client area.</param>
        [DllImport(LIBRARY, EntryPoint = "glfwGetCursorPos", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetCursorPosition(Window window, out double x, out double y);

        /// <summary>
        ///     Sets the position, in screen coordinates, of the cursor relative to the upper-left corner of the
        ///     client area of the specified window. The window must have input focus. If the window does not have input focus when
        ///     this function is called, it fails silently.
        ///     <para>
        ///         If the cursor mode is disabled then the cursor position is unconstrained and limited only by the minimum and
        ///         maximum values of a <see cref="double" />.
        ///     </para>
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <param name="x">The desired x-coordinate, relative to the left edge of the client area.</param>
        /// <param name="y">The desired y-coordinate, relative to the left edge of the client area.</param>
        [DllImport(LIBRARY, EntryPoint = "glfwSetCursorPos", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetCursorPosition(Window window, double x, double y);

        /// <summary>
        ///     Sets the cursor position callback of the specified window, which is called when the cursor is moved.
        ///     <para>
        ///         The callback is provided with the position, in screen coordinates, relative to the upper-left corner of the
        ///         client area of the window.
        ///     </para>
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <param name="mouseCallback">The new callback, or <c>null</c> to remove the currently set callback.</param>
        /// <returns>The previously set callback, or<c>null</c> if no callback was set or the library had not been initialized.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwSetCursorPosCallback", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(MouseCallback))]
        public static extern MouseCallback SetCursorPositionCallback(Window window, MouseCallback mouseCallback);

        /// <summary>
        ///     Sets the cursor boundary crossing callback of the specified window, which is called when the cursor
        ///     enters or leaves the client area of the window.
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <param name="mouseCallback">The new callback, or <c>null</c> to remove the currently set callback.</param>
        /// <returns>The previously set callback, or <c>null</c> if no callback was set or the library had not been initialized.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwSetCursorEnterCallback", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(MouseEnterCallback))]
        public static extern MouseEnterCallback SetCursorEnterCallback(Window window, MouseEnterCallback mouseCallback);

        /// <summary>
        ///     Sets the mouse button callback of the specified window, which is called when a mouse button is
        ///     pressed or released.
        ///     <para>
        ///         When a window loses input focus, it will generate synthetic mouse button release events for all pressed mouse
        ///         buttons. You can tell these events from user-generated events by the fact that the synthetic ones are generated
        ///         after the focus loss event has been processed, i.e. after the window focus callback has been called.
        ///     </para>
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <param name="mouseCallback">The new callback, or <c>null</c> to remove the currently set callback.</param>
        /// <returns>The previously set callback, or <c>null</c> if no callback was set or the library had not been initialized.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwSetMouseButtonCallback", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(MouseButtonCallback))]
        public static extern MouseButtonCallback SetMouseButtonCallback(Window window,
            MouseButtonCallback mouseCallback);

        /// <summary>
        ///     Sets the scroll callback of the specified window, which is called when a scrolling device is used,
        ///     such as a mouse wheel or scrolling area of a touchpad.
        ///     <para>The scroll callback receives all scrolling input, like that from a mouse wheel or a touchpad scrolling area.</para>
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <param name="mouseCallback">	The new scroll callback, or <c>null</c> to remove the currently set callback.</param>
        /// <returns>The previously set callback, or <c>null</c> if no callback was set or the library had not been initialized.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwSetScrollCallback", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(MouseCallback))]
        public static extern MouseCallback SetScrollCallback(Window window, MouseCallback mouseCallback);

        /// <summary>
        ///     Gets the last state reported for the specified mouse button to the specified window.
        ///     <para>
        ///         If the <see cref="InputMode.StickyMouseButton" /> input mode is enabled, this function returns
        ///         <see cref="InputState.Press" /> the first time you call it for a mouse button that was pressed, even if that
        ///         mouse button has already been released.
        ///     </para>
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <param name="button">The desired mouse button.</param>
        /// <returns>The input state of the <paramref name="button" />.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwGetMouseButton", CallingConvention = CallingConvention.Cdecl)]
        public static extern InputState GetMouseButton(Window window, MouseButton button);

        /// <summary>
        ///     Sets the user-defined pointer of the specified window. The current value is retained until the window
        ///     is destroyed. The initial value is <see cref="IntPtr.Zero" />.
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <param name="userPointer">The user pointer value.</param>
        [DllImport(LIBRARY, EntryPoint = "glfwSetWindowUserPointer", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetWindowUserPointer(Window window, IntPtr userPointer);

        /// <summary>
        ///     Gets the current value of the user-defined pointer of the specified window. The initial value is
        ///     <see cref="IntPtr.Zero" />.
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <returns>The user-defined pointer.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwGetWindowUserPointer", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetWindowUserPointer(Window window);

        /// <summary>
        ///     Sets the size limits of the client area of the specified window. If the window is full screen, the
        ///     size limits only take effect once it is made windowed. If the window is not resizable, this function does nothing.
        ///     <para>The size limits are applied immediately to a windowed mode window and may cause it to be resized.</para>
        ///     <para>
        ///         The maximum dimensions must be greater than or equal to the minimum dimensions and all must be greater than
        ///         or equal to zero.
        ///     </para>
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <param name="minWidth">The minimum width of the client area.</param>
        /// <param name="minHeight">The minimum height of the client area.</param>
        /// <param name="maxWidth">The maximum width of the client area.</param>
        /// <param name="maxHeight">The maximum height of the client area.</param>
        [DllImport(LIBRARY, EntryPoint = "glfwSetWindowSizeLimits", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetWindowSizeLimits(Window window, int minWidth, int minHeight, int maxWidth,
            int maxHeight);

        /// <summary>
        ///     Sets the required aspect ratio of the client area of the specified window. If the window is full
        ///     screen, the aspect ratio only takes effect once it is made windowed. If the window is not resizable, this function
        ///     does nothing.
        ///     <para>
        ///         The aspect ratio is specified as a numerator and a denominator and both values must be greater than zero. For
        ///         example, the common 16:9 aspect ratio is specified as 16 and 9, respectively.
        ///     </para>
        ///     <para>
        ///         If the numerator and denominator is set to <see cref="Constants.Default" /> then the aspect ratio limit is
        ///         disabled.
        ///     </para>
        ///     <para>The aspect ratio is applied immediately to a windowed mode window and may cause it to be resized.</para>
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <param name="numerator">The numerator of the desired aspect ratio.</param>
        /// <param name="denominator">The denominator of the desired aspect ratio.</param>
        [DllImport(LIBRARY, EntryPoint = "glfwSetWindowAspectRatio", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetWindowAspectRatio(Window window, int numerator, int denominator);

        [DllImport(LIBRARY, EntryPoint = "glfwGetCurrentContext", CallingConvention = CallingConvention.Cdecl)]
        private static extern Window GetCurrentContext();

        /// <summary>
        ///     Gets the size, in millimeters, of the display area of the specified monitor.
        /// </summary>
        /// <param name="monitor">The monitor to query.</param>
        /// <param name="width">The width, in millimeters, of the monitor's display area.</param>
        /// <param name="height">The height, in millimeters, of the monitor's display area.</param>
        [DllImport(LIBRARY, EntryPoint = "glfwGetMonitorPhysicalSize", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetMonitorPhysicalSize(Monitor monitor, out int width, out int height);

        /// <summary>
        ///     Gets the position, in screen coordinates, of the upper-left corner of the specified monitor.
        /// </summary>
        /// <param name="monitor">The monitor to query.</param>
        /// <param name="x">The monitor x-coordinate.</param>
        /// <param name="y">The monitor y-coordinate.</param>
        [DllImport(LIBRARY, EntryPoint = "glfwGetMonitorPos", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetMonitorPosition(Monitor monitor, out int x, out int y);

        [DllImport(LIBRARY, EntryPoint = "glfwGetMonitors", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GetMonitors(out int count);

        /// <summary>
        ///     Sets the character callback of the specified window, which is called when a Unicode character is
        ///     input.
        ///     <para>
        ///         The character callback is intended for Unicode text input. As it deals with characters, it is keyboard layout
        ///         dependent, whereas the key callback is not. Characters do not map 1:1 to physical keys, as a key may produce
        ///         zero, one or more characters. If you want to know whether a specific physical key was pressed or released, see
        ///         the key callback instead.
        ///     </para>
        ///     <para>
        ///         The character callback behaves as system text input normally does and will not be called if modifier keys are
        ///         held down that would prevent normal text input on that platform, for example a Super (Command) key on OS X or
        ///         Alt key on Windows. There is a character with modifiers callback that receives these events.
        ///     </para>
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <param name="charCallback">The new callback, or <c>null</c> to remove the currently set callback.</param>
        /// <returns>The previously set callback, or <c>null</c> if no callback was set or the library had not been initialized.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwSetCharCallback", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(CharCallback))]
        public static extern CharCallback SetCharCallback(Window window, CharCallback charCallback);

        /// <summary>
        ///     Sets the character with modifiers callback of the specified window, which is called when a Unicode
        ///     character is input regardless of what modifier keys are used.
        ///     <para>
        ///         The character with modifiers callback is intended for implementing custom Unicode character input. For
        ///         regular Unicode text input, see the character callback. Like the character callback, the character with
        ///         modifiers callback deals with characters and is keyboard layout dependent. Characters do not map 1:1 to
        ///         physical keys, as a key may produce zero, one or more characters. If you want to know whether a specific
        ///         physical key was pressed or released, see the key callback instead.
        ///     </para>
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <param name="charCallback">The new callback, or <c>null</c> to remove the currently set callback.</param>
        /// <returns>The previously set callback, or <c>null</c> if no callback was set or an error occurred.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwSetCharModsCallback", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(CharModsCallback))]
        public static extern CharModsCallback SetCharModsCallback(Window window, CharModsCallback charCallback);

        /// <summary>
        ///     Gets the last state reported for the specified key to the specified window.
        ///     <para>The higher-level action <see cref="InputState.Repeat" /> is only reported to the key callback.</para>
        ///     <para>
        ///         If the sticky keys input mode is enabled, this function returns <see cref="InputState.Press" /> the first
        ///         time you call it for a key that was pressed, even if that key has already been released.
        ///     </para>
        ///     <para>
        ///         The key functions deal with physical keys, with key tokens named after their use on the standard US keyboard
        ///         layout. If you want to input text, use the Unicode character callback instead.
        ///     </para>
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <param name="key">The key to query.</param>
        /// <returns>Either <see cref="InputState.Press" /> or <see cref="InputState.Release" />.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwGetKey", CallingConvention = CallingConvention.Cdecl)]
        public static extern InputState GetKey(Window window, Keys key);

        [DllImport(LIBRARY, EntryPoint = "glfwGetKeyName", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GetKeyNameInternal(Keys key, int scanCode);

        /// <summary>
        ///     Sets the framebuffer resize callback of the specified window, which is called when the framebuffer of
        ///     the specified window is resized.
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <param name="sizeCallback">The new callback, or <c>null</c> to remove the currently set callback.</param>
        /// <returns>The previously set callback, or <c>null</c> if no callback was set or the library had not been initialized.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwSetFramebufferSizeCallback", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(SizeCallback))]
        public static extern SizeCallback SetFramebufferSizeCallback(Window window, SizeCallback sizeCallback);

        /// <summary>
        ///     Sets the refresh callback of the specified window, which is called when the client area of the window
        ///     needs to be redrawn, for example if the window has been exposed after having been covered by another window.
        ///     <para>
        ///         On compositing window systems such as Aero, Compiz or Aqua, where the window contents are saved off-screen,
        ///         this callback may be called only very infrequently or never at all.
        ///     </para>
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <param name="callback">The new callback, or <c>null</c> to remove the currently set callback.</param>
        /// <returns>The previously set callback, or <c>null</c> if no callback was set or the library had not been initialized.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwSetWindowRefreshCallback", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(WindowCallback))]
        public static extern WindowCallback SetWindowRefreshCallback(Window window, WindowCallback callback);

        /// <summary>
        ///     Sets the key callback of the specified window, which is called when a key is pressed, repeated or
        ///     released.
        ///     <para>
        ///         The key functions deal with physical keys, with layout independent key tokens named after their values in the
        ///         standard US keyboard layout. If you want to input text, use the character callback instead.
        ///     </para>
        ///     <para>
        ///         When a window loses input focus, it will generate synthetic key release events for all pressed keys. You can
        ///         tell these events from user-generated events by the fact that the synthetic ones are generated after the focus
        ///         loss event has been processed, i.e. after the window focus callback has been called.
        ///     </para>
        ///     <para>
        ///         The scancode of a key is specific to that platform or sometimes even to that machine. Scancodes are intended
        ///         to allow users to bind keys that don't have a GLFW key token. Such keys have key set to
        ///         <see cref="Keys.Unknown" />, their state is not saved and so it cannot be queried with <see cref="GetKey" />.
        ///     </para>
        ///     <para>Sometimes GLFW needs to generate synthetic key events, in which case the scancode may be zero.</para>
        /// </summary>
        /// <param name="window">The new key callback, or <c>null</c> to remove the currently set callback.</param>
        /// <param name="keyCallback">The key callback.</param>
        /// <returns>The previously set callback, or <c>null</c> if no callback was set or the library had not been initialized.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwSetKeyCallback", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(KeyCallback))]
        public static extern KeyCallback SetKeyCallback(Window window, KeyCallback keyCallback);

        /// <summary>
        ///     Gets whether the specified joystick is present.
        /// </summary>
        /// <param name="joystick">The joystick to query.</param>
        /// <returns><c>true</c> if the joystick is present, or <c>false</c> otherwise.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwJoystickPresent", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool JoystickPresent(Joystick joystick);

        [DllImport(LIBRARY, EntryPoint = "glfwGetJoystickName", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GetJoystickNameInternal(Joystick joystick);

        [DllImport(LIBRARY, EntryPoint = "glfwGetJoystickAxes", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GetJoystickAxes(Joystick joystic, out int count);

        [DllImport(LIBRARY, EntryPoint = "glfwGetJoystickButtons", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GetJoystickButtons(Joystick joystick, out int count);

        /// <summary>
        ///     Sets the joystick configuration callback, or removes the currently set callback.
        ///     <para>This is called when a joystick is connected to or disconnected from the system.</para>
        /// </summary>
        /// <param name="callback">The new callback, or <c>null</c> to remove the currently set callback.</param>
        /// <returns>The previously set callback, or <c>null</c> if no callback was set or the library had not been initialized.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwSetJoystickCallback", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(JoystickCallback))]
        public static extern JoystickCallback SetJoystickCallback(JoystickCallback callback);

        /// <summary>
        ///     Sets the monitor configuration callback, or removes the currently set callback. This is called when a
        ///     monitor is connected to or disconnected from the system.
        /// </summary>
        /// <param name="monitorCallback">The new callback, or <c>null</c> to remove the currently set callback.</param>
        /// <returns>The previously set callback, or <c>null</c> if no callback was set or the library had not been initialized.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwSetMonitorCallback", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(MonitorCallback))]
        public static extern MonitorCallback SetMonitorCallback(MonitorCallback monitorCallback);

        /// <summary>
        ///     Sets the iconification callback of the specified window, which is called when the window is iconified
        ///     or restored.
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <param name="callback">The new callback, or <c>null</c> to remove the currently set callback.</param>
        /// <returns>The previously set callback, or <c>null</c> if no callback was set or the library had not been initialized.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwSetWindowIconifyCallback", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.FunctionPtr, MarshalTypeRef = typeof(IconifyCallback))]
        public static extern IconifyCallback SetWindowIconifyCallback(Window window, IconifyCallback callback);

        /// <summary>
        ///     Sets an input mode option for the specified window.
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <param name="mode">The mode to set a new value for.</param>
        /// <param name="value">The new value of the specified input mode.</param>
        [DllImport(LIBRARY, EntryPoint = "glfwSetInputMode", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetInputMode(Window window, InputMode mode, int value);

        /// <summary>
        ///     Gets the value of an input option for the specified window.
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <param name="mode">The mode to query.</param>
        /// <returns>Dependent on mode being queried.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwGetInputMode", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetInputMode(Window window, InputMode mode);

        /// <summary>
        ///     Returns the position, in screen coordinates, of the upper-left corner of the work area of the specified
        ///     monitor along with the work area size in screen coordinates.
        ///     <para>
        ///         The work area is defined as the area of the monitor not occluded by the operating system task bar
        ///         where present. If no task bar exists then the work area is the monitor resolution in screen
        ///         coordinates.
        ///     </para>
        /// </summary>
        /// <param name="monitor">The monitor to query.</param>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        /// <param name="width">The monitor width.</param>
        /// <param name="height">The monitor height.</param>
        [DllImport(LIBRARY, EntryPoint = "glfwGetMonitorWorkarea", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetMonitorWorkArea(IntPtr monitor, out int x, out int y, out int width,
            out int height);

        [DllImport(LIBRARY, EntryPoint = "glfwGetProcAddress", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GetProcAddress(byte[] procName);

        /// <summary>
        ///     Sets hints for the next call to <see cref="CreateWindow" />. The hints, once set, retain their values
        ///     until changed by a call to <see cref="WindowHint(Hint, int)" /> or <see cref="DefaultWindowHints" />, or until the
        ///     library is
        ///     terminated.
        ///     <para>
        ///         This function does not check whether the specified hint values are valid. If you set hints to invalid values
        ///         this will instead be reported by the next call to <see cref="CreateWindow" />.
        ///     </para>
        /// </summary>
        /// <param name="hint">The hint.</param>
        /// <param name="value">The value.</param>
        [DllImport(LIBRARY, EntryPoint = "glfwWindowHint", CallingConvention = CallingConvention.Cdecl)]
        public static extern void WindowHint(Hint hint, int value);

        /// <summary>
        ///     Gets the value of the specified window attribute.
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <param name="attribute">The attribute to retrieve.</param>
        /// <returns>The value of the <paramref name="attribute" />.</returns>
        [DllImport(LIBRARY, EntryPoint = "glfwGetWindowAttrib", CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetWindowAttribute(Window window, int attribute);

        [DllImport(LIBRARY, EntryPoint = "glfwGetError", CallingConvention = CallingConvention.Cdecl)]
        private static extern ErrorCode GetErrorPrivate(out IntPtr description);

        #endregion

        #region Methods

        /// <summary>
        ///     This function creates a window and its associated OpenGL or OpenGL ES context. Most of the options controlling how
        ///     the window and its context should be created are specified with window hints.
        /// </summary>
        /// <param name="width">The desired width, in screen coordinates, of the window. This must be greater than zero.</param>
        /// <param name="height">The desired height, in screen coordinates, of the window. This must be greater than zero.</param>
        /// <param name="title">The initial window title.</param>
        /// <param name="monitor">The monitor to use for full screen mode, or <see cref="Monitor.None" /> for windowed mode.</param>
        /// <param name="share">
        ///     A window instance whose context to share resources with, or <see cref="Window.None" /> to not share
        ///     resources..
        /// </param>
        /// <returns>The created window, or <see cref="Window.None" /> if an error occurred.</returns>
        public static Window CreateWindow(int width, int height, [NotNull] string title, Monitor monitor, Window share)
        {
            return CreateWindow(width, height, Encoding.UTF8.GetBytes(title), monitor, share);
        }

        /// <summary>
        ///     Gets the client API.
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <returns>The client API.</returns>
        public static ClientApi GetClientApi(Window window)
        {
            return (ClientApi) GetWindowAttribute(window, (int) ContextAttributes.ClientApi);
        }

        /// <summary>
        ///     Gets the contents of the system clipboard, if it contains or is convertible to a UTF-8 encoded
        ///     string.
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <returns>The contents of the clipboard as a UTF-8 encoded string, or <c>null</c> if an error occurred.</returns>
        [NotNull]
        public static string GetClipboardString(Window window)
        {
            return Util.PtrToStringUTF8(GetClipboardStringInternal(window));
        }

        /// <summary>
        ///     Gets the API used to create the context of the specified window.
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <returns>The API used to create the context.</returns>
        public static ContextApi GetContextCreationApi(Window window)
        {
            return (ContextApi) GetWindowAttribute(window, (int) ContextAttributes.ContextCreationApi);
        }

        /// <summary>
        ///     Gets the context version of the specified window.
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <returns>The context version.</returns>
        public static Version GetContextVersion(Window window)
        {
            GetContextVersion(window, out var major, out var minor, out var revision);
            return new Version(major, minor, revision);
        }

        /// <summary>
        ///     Gets whether the specified API extension is supported by the current OpenGL or OpenGL ES context.
        ///     <para>It searches both for client API extension and context creation API extensions.</para>
        /// </summary>
        /// <param name="extension">The extension name.</param>
        /// <returns><c>true</c> if the extension is supported; otherwise <c>false</c>.</returns>
        public static bool GetExtensionSupported(string extension)
        {
            return GetExtensionSupported(Encoding.ASCII.GetBytes(extension));
        }

        /// <summary>
        ///     Gets the current gamma ramp of the specified monitor.
        /// </summary>
        /// <param name="monitor">The monitor to query.</param>
        /// <returns>The current gamma ramp, or empty structure if an error occurred.</returns>
        public static GammaRamp GetGammaRamp(Monitor monitor)
        {
            return (GammaRamp) Marshal.PtrToStructure<GammaRampInternal>(GetGammaRampInternal(monitor));
        }

        /// <summary>
        ///     Gets value indicating if specified window is using a debug context.
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <returns><c>true</c> if window context is debug context, otherwise <c>false</c>.</returns>
        public static bool GetIsDebugContext(Window window)
        {
            return GetWindowAttribute(window, (int) ContextAttributes.OpenglDebugContext) == (int) Constants.True;
        }

        /// <summary>
        ///     Gets value indicating if specified window is using a forward compatible context.
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <returns><c>true</c> if window context is forward compatible, otherwise <c>false</c>.</returns>
        public static bool GetIsForwardCompatible(Window window)
        {
            return GetWindowAttribute(window, (int) ContextAttributes.OpenglForwardCompat) == (int) Constants.True;
        }

        /// <summary>
        ///     Gets the values of all axes of the specified joystick. Each element in the array is a value
        ///     between -1.0 and 1.0.
        ///     <para>
        ///         Querying a joystick slot with no device present is not an error, but will return an empty array. Call
        ///         <see cref="JoystickPresent" /> to check device presence.
        ///     </para>
        /// </summary>
        /// <param name="joystick">The joystick to query.</param>
        /// <returns>An array of axes values.</returns>
        public static float[] GetJoystickAxes(Joystick joystick)
        {
            var ptr = GetJoystickAxes(joystick, out var count);
            var axes = new float[count];
            if (count > 0)
                Marshal.Copy(ptr, axes, 0, count);
            return axes;
        }

        /// <summary>
        ///     Gets the state of all buttons of the specified joystick.
        /// </summary>
        /// <param name="joystick">The joystick to query.</param>
        /// <returns>An array of values, either <see cref="InputState.Press" /> and <see cref="InputState.Release" />.</returns>
        public static InputState[] GetJoystickButtons(Joystick joystick)
        {
            var ptr = GetJoystickButtons(joystick, out var count);
            var states = new InputState[count];
            for (var i = 0; i < count; i++)
                states[i] = (InputState) Marshal.ReadByte(ptr, i);
            return states;
        }

        /// <summary>
        ///     Gets the name of the specified joystick.
        ///     <para>
        ///         Querying a joystick slot with no device present is not an error. <see cref="JoystickPresent" /> to check
        ///         device presence.
        ///     </para>
        /// </summary>
        /// <param name="joystick">The joystick to query.</param>
        /// <returns>The name of the joystick, or <c>null</c> if the joystick is not present or an error occurred.</returns>
        public static string GetJoystickName(Joystick joystick)
        {
            return Util.PtrToStringUTF8(GetJoystickNameInternal(joystick));
        }

        /// <summary>
        ///     Gets the localized name of the specified printable key. This is intended for displaying key
        ///     bindings to the user.
        ///     <para>
        ///         If the key is <see cref="Keys.Unknown" />, the scancode is used instead, otherwise the scancode is ignored.
        ///         If a non-printable key or (if the key is <see cref="Keys.Unknown" />) a scancode that maps to a non-printable
        ///         key is specified, this function returns NULL.
        ///     </para>
        /// </summary>
        /// <param name="key">The key to query.</param>
        /// <param name="scanCode">The scancode of the key to query.</param>
        /// <returns>The localized name of the key.</returns>
        public static string GetKeyName(Keys key, int scanCode)
        {
            return Util.PtrToStringUTF8(GetKeyNameInternal(key, scanCode));
        }

        /// <summary>
        ///     Gets a human-readable name, encoded as UTF-8, of the specified monitor.
        ///     <para>
        ///         The name typically reflects the make and model of the monitor and is not guaranteed to be unique among the
        ///         connected monitors.
        ///     </para>
        /// </summary>
        /// <param name="monitor">The monitor to query.</param>
        /// <returns>The name of the monitor, or <c>null</c> if an error occurred.</returns>
        public static string GetMonitorName(Monitor monitor)
        {
            return Util.PtrToStringUTF8(GetMonitorNameInternal(monitor));
        }

        /// <summary>
        ///     Gets the address of the specified OpenGL or OpenGL ES core or extension function, if it is
        ///     supported by the current context.
        ///     This function does not apply to Vulkan. If you are rendering with Vulkan, use
        ///     <see cref="Vulkan.GetInstanceProcAddress" /> instead.
        /// </summary>
        /// <param name="procName">Name of the function.</param>
        /// <returns>The address of the function, or <see cref="IntPtr.Zero" /> if an error occurred.</returns>
        public static IntPtr GetProcAddress(string procName)
        {
            return GetProcAddress(Encoding.ASCII.GetBytes(procName));
        }

        /// <summary>
        ///     Gets the profile of the specified window.
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <returns>Profile of the window.</returns>
        public static Profile GetProfile(Window window)
        {
            return (Profile) GetWindowAttribute(window, (int) ContextAttributes.OpenglProfile);
        }

        /// <summary>
        ///     Gets the robustness value of the specified window.
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <returns>Current set value of the robustness.</returns>
        public static Robustness GetRobustness(Window window)
        {
            return (Robustness) GetWindowAttribute(window, (int) ContextAttributes.ContextRobustness);
        }

        /// <summary>
        ///     Gets the current video mode of the specified monitor.
        ///     <para>
        ///         If you have created a full screen window for that monitor, the return value will depend on whether that
        ///         window is iconified.
        ///     </para>
        /// </summary>
        /// <param name="monitor">The monitor to query.</param>
        /// <returns>The current mode of the monitor, or <c>null</c> if an error occurred.</returns>
        public static VideoMode GetVideoMode(Monitor monitor)
        {
            var ptr = GetVideoModeInternal(monitor);
            return Marshal.PtrToStructure<VideoMode>(ptr);
        }

        /// <summary>
        ///     Gets an array of all video modes supported by the specified monitor.
        ///     <para>
        ///         The returned array is sorted in ascending order, first by color bit depth (the sum of all channel depths) and
        ///         then by resolution area (the product of width and height).
        ///     </para>
        /// </summary>
        /// <param name="monitor">The monitor to query.</param>
        /// <returns>The array of video modes.</returns>
        public static VideoMode[] GetVideoModes(Monitor monitor)
        {
            var pointer = GetVideoModes(monitor, out var count);
            var modes = new VideoMode[count];
            for (var i = 0; i < count; i++, pointer += Marshal.SizeOf<VideoMode>())
                modes[i] = Marshal.PtrToStructure<VideoMode>(pointer);
            return modes;
        }

        /// <summary>
        ///     Gets the value of an attribute of the specified window or its OpenGL or OpenGL ES context.
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <param name="attribute">The window attribute whose value to return.</param>
        /// <returns>The value of the attribute, or zero if an error occurred.</returns>
        public static bool GetWindowAttribute(Window window, WindowAttribute attribute)
        {
            return GetWindowAttribute(window, (int) attribute) == (int) Constants.True;
        }

        /// <summary>
        ///     Sets the system clipboard to the specified string.
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <param name="str">The string to set to the clipboard.</param>
        public static void SetClipboardString(Window window, string str)
        {
            SetClipboardString(window, Encoding.UTF8.GetBytes(str));
        }

        /// <summary>
        ///     Sets the window title, encoded as UTF-8, of the specified window.
        /// </summary>
        /// <param name="window">A window instance.</param>
        /// <param name="title">The title to set.</param>
        public static void SetWindowTitle(Window window, string title)
        {
            SetWindowTitle(window, Encoding.UTF8.GetBytes(title));
        }

        /// <summary>
        ///     Sets hints for the next call to <see cref="CreateWindow" />. The hints, once set, retain their values
        ///     until changed by a call to <see cref="WindowHint(Hint, int)" /> or <see cref="DefaultWindowHints" />, or until the
        ///     library is
        ///     terminated.
        ///     <para>
        ///         This function does not check whether the specified hint values are valid. If you set hints to invalid values
        ///         this will instead be reported by the next call to <see cref="CreateWindow" />.
        ///     </para>
        /// </summary>
        /// <param name="hint">The hint.</param>
        /// <param name="value">The value.</param>
        public static void WindowHint(Hint hint, bool value)
        {
            WindowHint(hint, value ? Constants.True : Constants.False);
        }

        /// <summary>
        ///     Sets hints for the next call to <see cref="CreateWindow" />. The hints, once set, retain their values
        ///     until changed by a call to <see cref="WindowHint(Hint, int)" /> or <see cref="DefaultWindowHints" />, or until the
        ///     library is
        ///     terminated.
        ///     <para>
        ///         This function does not check whether the specified hint values are valid. If you set hints to invalid values
        ///         this will instead be reported by the next call to <see cref="CreateWindow" />.
        ///     </para>
        /// </summary>
        /// <param name="hint">The hint.</param>
        /// <param name="value">The value.</param>
        public static void WindowHint(Hint hint, ClientApi value) { WindowHint(hint, (int) value); }

        /// <summary>
        ///     Sets hints for the next call to <see cref="CreateWindow" />. The hints, once set, retain their values
        ///     until changed by a call to <see cref="WindowHint(Hint, int)" /> or <see cref="DefaultWindowHints" />, or until the
        ///     library is
        ///     terminated.
        ///     <para>
        ///         This function does not check whether the specified hint values are valid. If you set hints to invalid values
        ///         this will instead be reported by the next call to <see cref="CreateWindow" />.
        ///     </para>
        /// </summary>
        /// <param name="hint">The hint.</param>
        /// <param name="value">The value.</param>
        public static void WindowHint(Hint hint, Constants value) { WindowHint(hint, (int) value); }

        /// <summary>
        ///     Sets hints for the next call to <see cref="CreateWindow" />. The hints, once set, retain their values
        ///     until changed by a call to <see cref="WindowHint(Hint, int)" /> or <see cref="DefaultWindowHints" />, or until the
        ///     library is
        ///     terminated.
        ///     <para>
        ///         This function does not check whether the specified hint values are valid. If you set hints to invalid values
        ///         this will instead be reported by the next call to <see cref="CreateWindow" />.
        ///     </para>
        /// </summary>
        /// <param name="hint">The hint.</param>
        /// <param name="value">The value.</param>
        public static void WindowHint(Hint hint, ContextApi value) { WindowHint(hint, (int) value); }

        /// <summary>
        ///     Sets hints for the next call to <see cref="CreateWindow" />. The hints, once set, retain their values
        ///     until changed by a call to <see cref="WindowHint(Hint, int)" /> or <see cref="DefaultWindowHints" />, or until the
        ///     library is
        ///     terminated.
        ///     <para>
        ///         This function does not check whether the specified hint values are valid. If you set hints to invalid values
        ///         this will instead be reported by the next call to <see cref="CreateWindow" />.
        ///     </para>
        /// </summary>
        /// <param name="hint">The hint.</param>
        /// <param name="value">The value.</param>
        public static void WindowHint(Hint hint, Robustness value) { WindowHint(hint, (int) value); }

        /// <summary>
        ///     Sets hints for the next call to <see cref="CreateWindow" />. The hints, once set, retain their values
        ///     until changed by a call to <see cref="WindowHint(Hint, int)" /> or <see cref="DefaultWindowHints" />, or until the
        ///     library is
        ///     terminated.
        ///     <para>
        ///         This function does not check whether the specified hint values are valid. If you set hints to invalid values
        ///         this will instead be reported by the next call to <see cref="CreateWindow" />.
        ///     </para>
        /// </summary>
        /// <param name="hint">The hint.</param>
        /// <param name="value">The value.</param>
        public static void WindowHint(Hint hint, Profile value) { WindowHint(hint, (int) value); }

        /// <summary>
        ///     Sets hints for the next call to <see cref="CreateWindow" />. The hints, once set, retain their values
        ///     until changed by a call to <see cref="WindowHint(Hint, int)" /> or <see cref="DefaultWindowHints" />, or until the
        ///     library is
        ///     terminated.
        ///     <para>
        ///         This function does not check whether the specified hint values are valid. If you set hints to invalid values
        ///         this will instead be reported by the next call to <see cref="CreateWindow" />.
        ///     </para>
        /// </summary>
        /// <param name="hint">The hint.</param>
        /// <param name="value">The value.</param>
        public static void WindowHint(Hint hint, ReleaseBehavior value) { WindowHint(hint, (int) value); }

        private static void GetContextVersion(Window window, out int major, out int minor, out int revision)
        {
            major = GetWindowAttribute(window, (int) ContextAttributes.ContextVersionMajor);
            minor = GetWindowAttribute(window, (int) ContextAttributes.ContextVersionMinor);
            revision = GetWindowAttribute(window, (int) ContextAttributes.ContextVersionRevision);
        }

        private static void GlfwError(ErrorCode code, IntPtr message)
        {
            throw new Exception(Util.PtrToStringUTF8(message));
        }

        #endregion
    }
}
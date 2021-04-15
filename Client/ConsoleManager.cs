/**
 * OOP 4200 - Final Project - Durak
 * 
 * ConsoleManager.cs is used for debugging purposes
 * 
 * @author      Raje Singh, Fleur Blanckaert, Gabriel Dietrich, Dalton Young
 * @version     1.0
 * @since       2021-02 
 */

using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

namespace Client
{
    /**
     * Much of this class was not programmed by any of the collaborators of this project. You can find the original code from this
     * stack overflow question: https://stackoverflow.com/questions/160587/no-output-to-console-from-a-wpf-application
     * on this answer : https://stackoverflow.com/a/718505
     * from this user : https://stackoverflow.com/users/58961/john-leidegren
     *
     * All credit goes to John Leidegren.
     */
    /// <summary>
    /// Provides methods to attach and detach a console from an application.
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    public static class ConsoleManager
    {
        /// <summary>
        /// The string for the DLL file containing the methods that we need.
        /// </summary>
        private const string Kernel32_Dll = "kernel32.dll";

        /// <summary>
        /// The external <see cref="AllocConsole"/> function from the ConsoleAPI.
        /// Allocates a new console for the calling process.
        /// </summary>
        /// <returns>True if the function succeeds in allocating the new console, false otherwise.</returns>
        [DllImport(Kernel32_Dll)]
        private static extern bool AllocConsole();

        /// <summary>
        /// The external <see cref="FreeConsole"/> function from the ConsoleAPI. 
        /// Detaches the calling process from its console.
        /// </summary>
        /// <returns>True if the function succeeds in detaching the console, false otherwise.</returns>
        [DllImport(Kernel32_Dll)]
        private static extern bool FreeConsole();

        /// <summary>
        /// The external <see cref="GetConsoleWindow"/> function from the ConsoleAPI.
        /// Retrieves the window handle used by the console associated with the calling process.
        /// </summary>
        /// <returns>The handle to the window used by the console associated with the calling process or NULL if there is
        /// no such associated console.
        /// </returns>
        [DllImport(Kernel32_Dll)]
        private static extern IntPtr GetConsoleWindow();

        /// <summary>
        /// The external <see cref="SetConsoleIcon"/> function from kernel32.
        /// Sets the icon of the console using a handle.
        /// </summary>
        /// <param name="hIcon">The handle of the icon.</param>
        /// <returns>True if the function succeeded in setting the icon, false otherwise.</returns>
        [DllImport(Kernel32_Dll)]
        private static extern bool SetConsoleIcon(IntPtr hIcon);

        /// <summary>
        /// Represents whether the calling process has a console or not.
        /// </summary>
        public static bool HasConsole => GetConsoleWindow() != IntPtr.Zero;

        /// <summary>
        /// The Icon of the console.
        /// </summary>
        public static Icon ConsoleIcon { get; set; }

        private static int WINDOW_HEIGHT = 5;

        //private static int WINDOW_WIDTH = 63;

        /// <summary>
        /// Attaches the console to the calling process.
        /// </summary>
        public static void Show()
        {
            //#if DEBUG
            if (!HasConsole)
            {
                AllocConsole();
                Console.Title = @"Durak Console";
                try
                {
                    // Note that the VS Debugger Output acts as the main Console, so this does not take effect
                    // in the IDE.
                    Console.WindowHeight = WINDOW_HEIGHT;
                    //Console.WindowWidth = WINDOW_WIDTH;
                }
                catch (IOException)
                {
                }
            }
            //#endif
        }

        /// <summary>
        /// Detaches the console from the calling process.
        /// </summary>
        public static void Hide()
        {
            //#if DEBUG
            if (HasConsole)
            {
                SetOutAndErrorNull();
                FreeConsole();
            }
            //#endif
        }

        /// <summary>
        /// Hides or shows the console depending on if the calling process has a console or not.
        /// </summary>
        public static void Toggle()
        {
            if (HasConsole)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

        /// <summary>
        /// Sets the Icon of the console.
        /// </summary>
        /// <param name="ico">The icon to set.</param>
        public static void SetConsoleIcon(Icon ico)
        {
            ConsoleIcon = ico.Clone() as Icon;
            if (ConsoleIcon == null) return;
            SetConsoleIcon(ConsoleIcon.Handle);
        }

        /// <summary>
        /// Sets the <see cref="Console"/> out and error <see cref="TextWriter"/> to Null ones.
        /// </summary>
        private static void SetOutAndErrorNull()
        {
            // Set the Standard Output Stream to a null TextWriter
            // to prevent reading from the console.
            Console.SetOut(TextWriter.Null);

            // Set the Standard Error Stream to a null TextWriter 
            // to prevent reading from the console.
            Console.SetError(TextWriter.Null);
        }
    }
}

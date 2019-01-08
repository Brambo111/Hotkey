using System;
using System.Windows.Forms;
using ConsoleHotKey;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Threading;

namespace Hotkeys
{
    class Program
    {
        private delegate bool ConsoleEventDelegate(int eventType);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);
        public static List<int> RegistredHotkeyID = new List<int>();

        static void Main(string[] args)
        {
            //Register hotkeys
            RegistredHotkeyID.Add(HotKeyManager.RegisterHotKey(Keys.X, KeyModifiers.Alt | KeyModifiers.Control));
            RegistredHotkeyID.Add(HotKeyManager.RegisterHotKey(Keys.F12, KeyModifiers.Alt));

            //Add eventhandler for hotkeys.
            HotKeyManager.HotKeyPressed += new EventHandler<HotKeyEventArgs>(HotKeyManager_HotKeyPressed);

            //Add eventhandler for closing terminal.
            SetConsoleCtrlHandler(new ConsoleEventDelegate(ConsoleEventCallback), true);

            //Prevent console from closing.
            Console.ReadLine();
        }


        /// <summary>
        /// Logs key combination to console when a registered hotkeycombination is pressed
        /// </summary>
        /// <param name="sender">Object that called the function</param>
        /// <param name="e">Hotkey combination that triggered the function</param>
        static void HotKeyManager_HotKeyPressed(object sender, HotKeyEventArgs e)
        {
            Console.WriteLine("The following hotkey was triggered: " + e.Modifiers + "+" + e.Key);
        }


        /// <summary>
        /// Checks if console close button is pressed and unregisters hotkey
        /// </summary>
        /// <param name="eventType"></param>
        /// <returns>false for now</returns>
        static bool ConsoleEventCallback(int eventType)
        {
            if (eventType == 2)
            {
                Console.WriteLine("Shutting down application");
                foreach(var hotkeyId in RegistredHotkeyID)
                {
                    HotKeyManager.UnregisterHotKey(hotkeyId);
                }
                Console.WriteLine("Hotkeys unbound. Closing application");
                Thread.Sleep(1000);
            }
            return false;
        }
          

    }
}

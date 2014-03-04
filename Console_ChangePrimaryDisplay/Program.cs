using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices; // DLL Import
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;

namespace ConsoleApplication1
{
    class Program
    {
        public static WinApi.DEVMODE NewDevMode()
        {
            WinApi.DEVMODE dm = new WinApi.DEVMODE();
            dm.dmDeviceName = new String(new char[31]);
            dm.dmFormName = new String(new char[31]);
            dm.dmSize = (ushort)Marshal.SizeOf(dm);
            return dm;
        }
        static void Main(string[] args)
        {
            if(args.Length ==0){
                Console.Write("invalid");
                return;
            }
            //test
            //args[0] = "1";
            //testend
            Console.Write("引数は{0}\n", args[0]);
            SetPrimary(int.Parse(args[0]));


        }

        public static void SetPrimary(int dn)
        {
            string[] devName = new string[3];
            if (dn == 1 || dn == 2)
            {
                if(dn ==1)
                {  
                    devName[1] = "\\\\.\\DISPLAY" + 1;
                    devName[2] = "\\\\.\\DISPLAY" + 2; 
                }
                if (dn == 2)
                {
                    devName[1] = "\\\\.\\DISPLAY" + 2;
                    devName[2] = "\\\\.\\DISPLAY" + 1; 
                }

                Console.Write("CHANGE PRIMARY DISPLAY TO \\\\.\\DISPLAY" + dn + "\n\n\n");


                int deviceID;
                WinApi.DisplaySetting_Results result = 0;

                //manual gather - NewPrimary name ----------------------------------------------------
                WinApi.DISPLAY_DEVICE ddOne = new WinApi.DISPLAY_DEVICE();

                ddOne.cb = Marshal.SizeOf(ddOne);
                deviceID = 1;
                WinApi.User_32.EnumDisplayDevices(null, deviceID, ref ddOne, 0);
                string NewPrimary = devName[1];//ddOne.DeviceName;

                //manual gather - OldPrimary name ----------------------------------------------------
                WinApi.DISPLAY_DEVICE ddThree = new WinApi.DISPLAY_DEVICE();

                ddThree.cb = Marshal.SizeOf(ddThree);
                deviceID = 2;
                WinApi.User_32.EnumDisplayDevices(null, deviceID, ref ddThree, 0);
                string OldPrimary = devName[2];

                //ACTION 1 start ----------------------------------------------------------------------------
                WinApi.DEVMODE ndm1 = NewDevMode();
                WinApi.User_32.EnumDisplaySettings(NewPrimary, (int)WinApi.DEVMODE_SETTINGS.ENUM_REGISTRY_SETTINGS, ref ndm1);

                WinApi.DEVMODE ndm3 = NewDevMode();
                ndm3.dmFields = WinApi.DEVMODE_Flags.DM_POSITION;
                ndm3.dmPosition.x = (int)ndm1.dmPelsWidth;
                ndm3.dmPosition.y = 0;

                result = (WinApi.DisplaySetting_Results)WinApi.User_32.ChangeDisplaySettingsEx(OldPrimary, ref ndm3, (IntPtr)null, (int)WinApi.DeviceFlags.CDS_UPDATEREGISTRY | (int)WinApi.DeviceFlags.CDS_NORESET, IntPtr.Zero);

                Console.Write("Action 1 result:" + result.ToString());

                //ACTION 1 end ----------------------------------------------------------------------------

                //ACTION 2 start ----------------------------------------------------------------------------
                WinApi.DEVMODE ndm2 = NewDevMode();
                WinApi.User_32.EnumDisplaySettings(NewPrimary, (int)WinApi.DEVMODE_SETTINGS.ENUM_REGISTRY_SETTINGS, ref ndm2);

                WinApi.DEVMODE ndm4 = NewDevMode();
                ndm4.dmFields = WinApi.DEVMODE_Flags.DM_POSITION;
                ndm4.dmPosition.x = 0;
                ndm4.dmPosition.y = 0;

                result = (WinApi.DisplaySetting_Results)WinApi.User_32.ChangeDisplaySettingsEx(NewPrimary, ref ndm4, (IntPtr)null, (int)WinApi.DeviceFlags.CDS_SET_PRIMARY | (int)WinApi.DeviceFlags.CDS_UPDATEREGISTRY | (int)WinApi.DeviceFlags.CDS_NORESET, IntPtr.Zero);
                Console.Write("Action 2 result:" + result.ToString());
                //ACTION 2 end ----------------------------------------------------------------------------

                //ACTION 3 start ----------------------------------------------------------------------------
                WinApi.DEVMODE ndm5 = NewDevMode();
                result = (WinApi.DisplaySetting_Results)WinApi.User_32.ChangeDisplaySettingsEx(OldPrimary, ref ndm5, (IntPtr)null, (int)WinApi.DeviceFlags.CDS_UPDATEREGISTRY, (IntPtr)null);
                Console.Write("Action 3.1 result:" + result.ToString());

                WinApi.DEVMODE ndm6 = NewDevMode();
                result = (WinApi.DisplaySetting_Results)WinApi.User_32.ChangeDisplaySettingsEx(NewPrimary, ref ndm6, (IntPtr)null, (int)WinApi.DeviceFlags.CDS_SET_PRIMARY | (int)WinApi.DeviceFlags.CDS_UPDATEREGISTRY, IntPtr.Zero);
                Console.Write("Action 3.2 result:" + result.ToString());
                //ACTION 3 end ----------------------------------------------------------------------------
            







            }
            else
            {
                Console.Write("Error, Invailed\n");
                return;
            }
            


        }
        
    }

}

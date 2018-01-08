using Microsoft.Win32;
using System;
using System.Windows.Forms;

namespace Register
{
    public class RegistryHelper
    {
        public static bool IsKeyExist(string sKeyName)
        {
            string[] sKeyNameColl;
            RegistryKey hklm = Registry.LocalMachine;
            RegistryKey hkSoftWare = hklm.OpenSubKey(@"SOFTWARE");
            sKeyNameColl = hkSoftWare.GetSubKeyNames(); //获取SOFTWARE下所有的子项
            foreach (string sName in sKeyNameColl)
            {
                if (sName == sKeyName)
                {
                    hklm.Close();
                    hkSoftWare.Close();
                    return true;
                }
            }
            hklm.Close();
            hkSoftWare.Close();
            return false;
        }

        public static void addKey(string keyName)
        {
            RegistryKey hklm = Registry.LocalMachine;
            RegistryKey hkSoftWare = hklm.CreateSubKey(@"SOFTWARE\" + keyName);
            hklm.Close();
            hkSoftWare.Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal; // WindowsImpersonationContext
using System.Security.Permissions; // PermissionSetAttribute

namespace Impersonate
{
    public class Impersonator :IDisposable
    {
        public Impersonator(string domainName, string userName, string password,Action action)
        {
            ImpersonateValidUser(domainName, userName, password,action);
        }

        public void Dispose()
        {
            //UndoImpersonation();
        }


        // obtains user token
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool LogonUser(
                string lpszUserName,
                string lpszDomain,
                string lpszPassword,
                int dwLogonType,
                int dwLogonProvider,
                ref IntPtr phToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool RevertToSelf();

        // closes open handes returned by LogonUser
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public extern static bool CloseHandle(IntPtr handle);

        // creates duplicate token handle
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool DuplicateToken(
                IntPtr hToken,
                int impersonationLevel,
                ref IntPtr hNewToken);


        /// <summary>
        /// Attempts to impersonate a user.  If successful, returns 
        /// a WindowsImpersonationContext of the new users identity.
        /// </summary>
        /// <param name="sUsername">Username you want to impersonate</param>
        /// <param name="sDomain">Logon domain</param>
        /// <param name="sPassword">User's password to logon with</param></param>
        /// <returns></returns>
        private void ImpersonateValidUser(string sUsername, string sDomain, string sPassword,Action action)
        {
            // initialize tokens
            IntPtr pExistingTokenHandle = new IntPtr(0);
            IntPtr pDuplicateTokenHandle = new IntPtr(0);
            pExistingTokenHandle = IntPtr.Zero;
            pDuplicateTokenHandle = IntPtr.Zero;

            // if domain name was blank, assume local machine
            if (sDomain == "")
                sDomain = System.Environment.MachineName;

            try
            {
                string sResult = null;

                const int LOGON32_PROVIDER_DEFAULT = 0;

                // create token
                const int LOGON32_LOGON_INTERACTIVE = 2;
                //const int SecurityImpersonation = 2;
                const int LOGON32_LOGON_NEW_CREDENTIALS = 9;
                if (RevertToSelf())
                {
                    // get handle to token
                    bool bImpersonated = LogonUser(sUsername, sDomain, sPassword,
                                LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, ref pExistingTokenHandle);

                    // did impersonation fail?
                    if (false == bImpersonated)
                    {
                        int nErrorCode = Marshal.GetLastWin32Error();
                        sResult = "LogonUser() failed with error code: " + nErrorCode + "\r\n";

                        // throw the reason why LogonUser failed
                        throw new Exception(sResult);
                    }

                    // Get identity before impersonation
                    sResult += "Before impersonation: " + WindowsIdentity.GetCurrent().Name + "\r\n";

                    bool bRetVal = DuplicateToken(pExistingTokenHandle, (int)SECURITY_IMPERSONATION_LEVEL.SecurityImpersonation, ref pDuplicateTokenHandle);

                    // did DuplicateToken fail?
                    if (false == bRetVal)
                    {
                        int nErrorCode = Marshal.GetLastWin32Error();
                        CloseHandle(pExistingTokenHandle); // close existing handle
                        sResult += "DuplicateToken() failed with error code: " + nErrorCode + "\r\n";

                        // throw the reason why DuplicateToken failed
                        throw new Exception(sResult);
                    }
                    else
                    {
                        // create new identity using new primary token
                        WindowsIdentity newId = new WindowsIdentity(pDuplicateTokenHandle);
                        //impersonationContext = newId.Impersonate();

                        WindowsIdentity.RunImpersonated(newId.AccessToken, () =>
                        {
                            action();
                            //var userName = WindowsIdentity.GetCurrent().Name;
                            //// Check the identity.  
                            //System.IO.File.ReadAllText("E:\\RestrictFile2.txt");
                            //Console.WriteLine("During impersonation: " + userName);
                        });

                        // check the identity after impersonation
                        //sResult += "After impersonation: " + WindowsIdentity.GetCurrent().Name + "\r\n";

                    }
                }
                else
                {
                    int nErrorCode = Marshal.GetLastWin32Error();
                    sResult += "RevertToSelf() failed with error code: " + nErrorCode + "\r\n";
                    throw new Exception(sResult);
                }

              
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                // close handle(s)
                if (pExistingTokenHandle != IntPtr.Zero)
                    CloseHandle(pExistingTokenHandle);
                if (pDuplicateTokenHandle != IntPtr.Zero)
                    CloseHandle(pDuplicateTokenHandle);
            }
        }
    }


    // group type enum
    public enum SECURITY_IMPERSONATION_LEVEL : int
    {
        SecurityAnonymous = 0,
        SecurityIdentification = 1,
        SecurityImpersonation = 2,
        SecurityDelegation = 3
    }


}

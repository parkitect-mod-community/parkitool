﻿﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace Parkitool
{
    static class Util
    {
        public static string GetSteamOS()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return "windows";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return "macos";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return "linux";
            }

            return "unknown";
        }

        public static string GetSteamArch()
        {
            return Environment.Is64BitOperatingSystem ? "64" : "32";
        }

        public static string ReadPassword()
        {
            ConsoleKeyInfo keyInfo;
            StringBuilder password = new StringBuilder();

            do
            {
                keyInfo = Console.ReadKey( true );

                if ( keyInfo.Key == ConsoleKey.Backspace )
                {
                    if ( password.Length > 0 )
                        password.Remove( password.Length - 1, 1 );
                    continue;
                }

                /* Printable ASCII characters only */
                char c = keyInfo.KeyChar;
                if ( c >= ' ' && c <= '~' )
                    password.Append( c );
            } while ( keyInfo.Key != ConsoleKey.Enter );

            return password.ToString();
        }



        public static byte[] AdlerHash(byte[] input)
        {
            uint a = 0, b = 0;
            for (int i = 0; i < input.Length; i++)
            {
                a = (a + input[i]) % 65521;
                b = (b + a) % 65521;
            }
            return BitConverter.GetBytes(a | (b << 16));
        }

        public static byte[] SHAHash( byte[] input )
        {
            using (var sha = SHA1.Create())
            {
                var output = sha.ComputeHash( input );

                return output;
            }
        }

        public static byte[] DecodeHexString( string hex )
        {
            if ( hex == null )
                return null;

            int chars = hex.Length;
            byte[] bytes = new byte[ chars / 2 ];

            for ( int i = 0 ; i < chars ; i += 2 )
                bytes[ i / 2 ] = Convert.ToByte( hex.Substring( i, 2 ), 16 );

            return bytes;
        }

        public static string EncodeHexString( byte[] input )
        {
            return input.Aggregate( new StringBuilder(),
                ( sb, v ) => sb.Append( v.ToString( "x2" ) )
                ).ToString();
        }
    }
}

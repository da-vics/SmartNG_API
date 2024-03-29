﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace apiTest1.Helpers
{
    public class DataEncryptionHelper
    {

        public string Encrypt(string hashSource, string saltVar = null, string extraSalt = null)
        {
            string hash = string.Empty;

            hashSource += (saltVar + extraSalt);

            using (MD5 md5Hash = MD5.Create())
            {
                hash = GetMd5Hash(md5Hash, hashSource);

            }

            return hash;
        }

        public string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        public bool VerifyMd5Hash(string input, string hash, string saltVar = null, string extraSalt = null)
        {

            string hashOfInput = Encrypt(input, saltVar, extraSalt);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}

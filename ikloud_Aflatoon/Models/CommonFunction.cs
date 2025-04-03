using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using ikloud_Aflatoon;

namespace ikloud_Aflatoon.Models
{
    public class CommonFunction
    {
        private static String m_strPassPhrase = "MyPriv@Password!$$";    //'---- any text string is good here    
        //private static String m_strHashAlgorithm = "MD5";   //'--- we are doing MD5 encryption - can be "SHA1"    
        private static Int32 m_strPasswordIterations = 2;   // '--- can be any number    
        private static String m_strInitVector = "@1B2c3D4e5F6g7H8";  // '--- must be 16 bytes    
        private static Int32 m_intKeySize = 256;                     //'--- can be 192 or 128
        //Encrypt Function:     
        public string EncryptPassword(string Password)
        {
            string sEncryptedPassword = "";
            String sEncryptKey = "P@SSW@RD@09"; //Should be minimum 8 characters   
            try
            {
                sEncryptedPassword = EncryptPasswordMD5(Password, sEncryptKey);
            }
            catch (Exception)
            {
                return sEncryptedPassword;
            }
            return sEncryptedPassword;
        }

        public string DecryptPassword(string Password)
        {
             String sDecryptedPassword="";
             String sEncryptKey= "P@SSW@RD@09";// 'Should be minimum 8 characters      
            try
            {
                sDecryptedPassword = DecryptPasswordMD5(Password, sEncryptKey);
            }

        catch(Exception)
        {
            return sDecryptedPassword;
        }

        return sDecryptedPassword;
        }
        protected static string EncryptPasswordMD5(string plainText, string p_strSaltValue)
        {
             String strReturn   = String.Empty;
            try
            {
                 Byte[] initVectorBytes ;
            initVectorBytes = System.Text.Encoding.ASCII.GetBytes(m_strInitVector);
            Byte[] saltValueBytes ;
            saltValueBytes = System.Text.Encoding.ASCII.GetBytes(p_strSaltValue);     //       ' Convert our plaintext into a byte array. Let us assume that plaintext contains UTF8-encoded characters.            
            Byte[] plainTextBytes;
            plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);//            ' First, we must create a password, from which the key will be derived.This password will be generated from the specified passphrase and salt value. The password will be created using the specified hash  algorithm. Password creation can be done in several iterations.            
            Rfc2898DeriveBytes password;
            password = new Rfc2898DeriveBytes(m_strPassPhrase, saltValueBytes, m_strPasswordIterations) ;//           ' Use the password to generate pseudo-random bytes for the encryption  key. Specify the size of the key in bytes (instead of bits).           
            Byte[] keyBytes ;
            Int32 intKeySize = 0;
            intKeySize = (Int32)(m_intKeySize / 8);
            keyBytes = password.GetBytes(intKeySize);//            ' Create uninitialized Rijndael encryption object.            
            System.Security.Cryptography.RijndaelManaged symmetricKey;
            symmetricKey = new System.Security.Cryptography.RijndaelManaged() ;//           ' It is reasonable to set encryption mode to Cipher Block Chaining  (CBC). Use default options for other symmetric key parameters.            
            symmetricKey.Mode = System.Security.Cryptography.CipherMode.CBC ;//           'symmetricKey.Padding = PaddingMode.Zeros Generate encryptor from the existing key bytes and initialization  vector. Key size will be defined based on the number of the key  bytes.            
            System.Security.Cryptography.ICryptoTransform encryptor;
            encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);//            ' Define memory stream which will be used to hold encrypted data.            
            System.IO.MemoryStream memoryStream;
            memoryStream = new System.IO.MemoryStream(); //           ' Define cryptographic stream (always use Write mode for encryption).            
            System.Security.Cryptography.CryptoStream cryptoStream;
            cryptoStream = new System.Security.Cryptography.CryptoStream(memoryStream, encryptor, System.Security.Cryptography.CryptoStreamMode.Write);//            ' Start encrypting.            
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length) ;//           ' Finish encrypting.           
            cryptoStream.FlushFinalBlock() ;//           ' Convert our encrypted data from a memory stream into a byte array.            
            Byte[] cipherTextBytes;
            cipherTextBytes = memoryStream.ToArray()  ;//          ' Close both streams.            
            memoryStream.Close();
            cryptoStream.Close();//          ' Convert encrypted data into a base64-encoded string.            
            String cipherText  ;
            cipherText = Convert.ToBase64String(cipherTextBytes);//            ' Return encrypted string.            
            strReturn = cipherText;
            }
            catch(Exception)
            {
                strReturn = null;
            }
            return strReturn;
        }
        protected static string DecryptPasswordMD5(string cipherText, string p_strSaltValue)
        {
            String strReturn = String.Empty;

            try
            {
                Byte[] initVectorBytes;
                initVectorBytes = System.Text.Encoding.ASCII.GetBytes(m_strInitVector);
                Byte[] saltValueBytes;
                saltValueBytes = System.Text.Encoding.ASCII.GetBytes(p_strSaltValue);//         ' Convert our ciphertext into a byte array.           
                Byte[] cipherTextBytes;
                cipherTextBytes = Convert.FromBase64String(cipherText);//            ' First, we must create a password, from which the key will be derived. This password will be generated from the specified passphrase and salt value. The password will be created using the specified hash algorithm. Password creation can be done in several iterations.            
                Rfc2898DeriveBytes password;
                password = new Rfc2898DeriveBytes(m_strPassPhrase, saltValueBytes, m_strPasswordIterations);//        ' Use the password to generate pseudo-random bytes for the encryption key. Specify the size of the key in bytes (instead of bits).            
                Byte[] keyBytes;
                Int32 intKeySize;
                intKeySize = (Int32)(m_intKeySize / 8);
                keyBytes = password.GetBytes(intKeySize);//          ' Create uninitialized Rijndael encryption object.           
                System.Security.Cryptography.RijndaelManaged symmetricKey;
                symmetricKey = new System.Security.Cryptography.RijndaelManaged(); //' It is reasonable to set encryption mode to Cipher Block Chaining (CBC). Use default options for other symmetric key parameters.      
                symmetricKey.Mode = System.Security.Cryptography.CipherMode.CBC;//      'symmetricKey.Padding = PaddingMode.Zeros Generate decryptor from the existing key bytes and initialization vector. Key size will be defined based on the number of the key bytes.           
                System.Security.Cryptography.ICryptoTransform decryptor;//
                decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);//       ' Define memory stream which will be used to hold encrypted data.       
                System.IO.MemoryStream memoryStream ;
                memoryStream = new System.IO.MemoryStream(cipherTextBytes) ;//           ' Define memory stream which will be used to hold encrypted data.        
                System.Security.Cryptography.CryptoStream cryptoStream;
                cryptoStream = new System.Security.Cryptography.CryptoStream(memoryStream, decryptor, System.Security.Cryptography.CryptoStreamMode.Read);//          ' Since at this point we don't know what the size of decrypted data will be, allocate the buffer long enough to hold ciphertext; plaintext is never longer than ciphertext.            
                Byte[] plainTextBytes;
                plainTextBytes=new Byte[cipherTextBytes.Length];
                // Array.Resize (plainTextBytes[],cipherTextBytes.Length) ;    //       ' Start decrypting.           
                Int32 decryptedByteCount;
                decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);//            ' Close both streams.           
                memoryStream.Close();
                cryptoStream.Close() ;//           ' Convert decrypted data into a string.                 ' Let us assume that the original plaintext string was UTF8-encoded.   
                String plainText; 
                plainText = System.Text.Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount) ;//           ' Return decrypted string.  
                strReturn = plainText;

            }
            catch (Exception)
            {
                return strReturn = null;
            }
            return strReturn;
        }
    }
}
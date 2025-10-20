using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

/// <summary> 
/// Encryption utility class that implements Triple DES algorithm 
/// </summary> 
/// 
public class SharedEncryptor
{
    //Encryption Key 
    private byte[] EncryptionKey { get; set; }
    // The Initialization Vector for the DES encryption routine 
    private byte[] IV { get; set; }

    /// <summary> 
    /// Constructor for TripleDESImplementation class 
    /// </summary> 
    /// <param name="encryptionKey">The 24-byte encryption key (24 character ASCII)</param> 
    /// <param name="IV">The 8-byte DES encryption initialization vector (8 characters ASCII)</param> 
    public SharedEncryptor()
    {
        this.EncryptionKey = Encoding.ASCII.GetBytes("farassan_jobrequest_fars");// must be 24 ascci character
        this.IV = Encoding.ASCII.GetBytes("5Mehd!M0");// must be 8 ascci character
    }
    /// <summary> 
    /// Encrypts a text block 
    /// </summary> 
    public string Encrypt(string textToEncrypt)
    {
        TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
        tdes.Key = EncryptionKey;
        tdes.IV = IV;

        byte[] buffer = Encoding.UTF8.GetBytes(textToEncrypt);
        return Convert.ToBase64String(tdes.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length));
    }

    /// <summary> 
    /// Decrypts an encrypted text block 
    /// </summary> 
    public string Decrypt(string textToDecrypt)
    {
        byte[] buffer = Convert.FromBase64String(textToDecrypt);

        TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
        des.Key = EncryptionKey;
        des.IV = IV;

        return Encoding.UTF8.GetString(des.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length));
    }
}

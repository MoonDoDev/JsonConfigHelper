using System.Security.Cryptography;
using System.Text;

namespace AppConfigTest;

/// <summary>
/// 
/// </summary>
/// <param name="encryptionKey"></param>
public sealed class EncryptionService( byte[] encryptionKey ): IDisposable
{
    private readonly byte[] _encryptionKey = encryptionKey;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="plainText"></param>
    /// <returns></returns>
    public string Encrypt( string plainText )
    {
        using var aes = new AesCcm( _encryptionKey );

        var nonce = new byte[ AesGcm.NonceByteSizes.MaxSize ];
        RandomNumberGenerator.Fill( nonce );

        var plainTextBytes = Encoding.UTF8.GetBytes( plainText );
        var cipherTextBytes = new byte[ plainTextBytes.Length ];
        var tag = new byte[ AesGcm.TagByteSizes.MaxSize ];

        aes.Encrypt( nonce, plainTextBytes, cipherTextBytes, tag );
        return new AesCipherText( nonce, tag, cipherTextBytes ).ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cipherText"></param>
    /// <returns></returns>
    public string Decrypt( string cipherText )
    {
        using var aes = new AesCcm( _encryptionKey );

        var gcmCipherText = AesCipherText.FromBase64String( cipherText );
        var plainTextBytes = new byte[ gcmCipherText.CipherTextBytes.Length ];

        aes.Decrypt( gcmCipherText.Nonce, gcmCipherText.CipherTextBytes, gcmCipherText.Tag, plainTextBytes );
        return Encoding.UTF8.GetString( plainTextBytes );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    void IDisposable.Dispose()
    {
        GC.SuppressFinalize( this );
    }
}

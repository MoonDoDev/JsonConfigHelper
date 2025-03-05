using System.Security.Cryptography;

namespace AppConfigTest;

/// <summary>
/// 
/// </summary>
/// <param name="nonce"></param>
/// <param name="tag"></param>
/// <param name="cipherTextBytes"></param>
public sealed class AesCipherText( byte[] nonce, byte[] tag, byte[] cipherTextBytes )
{
    public byte[] Nonce { get; } = nonce;
    public byte[] Tag { get; } = tag;
    public byte[] CipherTextBytes { get; } = cipherTextBytes;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="textData"></param>
    /// <returns></returns>
    public static AesCipherText FromBase64String( string textData )
    {
        var dataBytes = Convert.FromBase64String( textData );

        return new AesCipherText(
            dataBytes.Take( AesGcm.NonceByteSizes.MaxSize ).ToArray(),
            dataBytes[ ^AesGcm.TagByteSizes.MaxSize.. ],
            dataBytes[ AesGcm.NonceByteSizes.MaxSize..^AesGcm.TagByteSizes.MaxSize ]
        );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return Convert.ToBase64String( Nonce.Concat( CipherTextBytes ).Concat( Tag ).ToArray() );
    }
}

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace SharedLib.Database;

/// <summary>
/// A custom value converter that handles conversion between <see cref="Guid"/> and <see cref="byte[]"/>
/// to make MySQL BINARY(16) column compatible with <see cref="Guid"/>
/// </summary>
public class MySqlGuidConverter : ValueConverter<Guid, byte[]>
{
    public MySqlGuidConverter() : base
    (
        guid => GuidToMySqlBinary(guid),
        bytes => MySqlBinaryToGuid(bytes)
    )
    {
    }

    /// <summary>
    /// Converts <see cref="Guid"/> to a MySQL UUID column
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/> to convert</param>
    /// <returns>A 16-byte array representing the GUID in MySQL column</returns>
    public static byte[] GuidToMySqlBinary(Guid guid)
    {
        var bytes = guid.ToByteArray();

        return
        [
            bytes[3], bytes[2], bytes[1], bytes[0],
            bytes[5], bytes[4],
            bytes[7], bytes[6],
            bytes[8], bytes[9],
            bytes[10], bytes[11], bytes[12], bytes[13], bytes[14], bytes[15]
        ];
    }

    /// <summary>
    /// Converts a <see cref="byte[]"/> representing a MySQL GUID back to <see cref="Guid"/>
    /// </summary>
    /// <param name="bytes">A 16-byte array retrieved from MySQL BINARY(16)</param>
    /// <returns>The corresponding <see cref="Guid"/> value</returns>
    public static Guid MySqlBinaryToGuid(byte[] bytes)
    {
        return new Guid(
        [
            bytes[3], bytes[2], bytes[1], bytes[0],
            bytes[5], bytes[4],
            bytes[7], bytes[6],
            bytes[8], bytes[9],
            bytes[10], bytes[11], bytes[12], bytes[13], bytes[14], bytes[15]
        ]);
    }
}
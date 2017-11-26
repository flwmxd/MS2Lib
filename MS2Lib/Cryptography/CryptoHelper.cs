﻿using System;
using System.IO;
using System.Threading.Tasks;

namespace MS2Lib
{
    internal static class CryptoHelper
    {
        public static Task<Stream> DecryptStreamToStreamAsync(MS2CryptoMode decryptionMode, MS2SizeHeader header, bool isCompressed, Stream stream)
        {
            return DecryptStreamToDataAsync(decryptionMode, header, isCompressed, stream).ContinueWith<Stream>(t => new MemoryStream(t.Result));
        }

        public static async Task<byte[]> DecryptStreamToDataAsync(MS2CryptoMode decryptionMode, MS2SizeHeader header, bool isCompressed, Stream stream)
        {
            if (header.EncodedSize == 0 || header.CompressedSize == 0 || header.Size == 0)
            {
                return new byte[0];
            }

            byte[] buffer = new byte[header.EncodedSize];
            int readBytes = await stream.ReadAsync(buffer, 0, (int)header.EncodedSize).ConfigureAwait(false);
            if ((uint)readBytes != header.EncodedSize)
            {
                throw new Exception("Data length mismatch when reading data.");
            }

            if (isCompressed)
            {
                return await Task.Run(() => Decrypt(decryptionMode, buffer, header.CompressedSize, header.Size)).ConfigureAwait(false);
            }
            else
            {
                return await Task.Run(() => Decrypt(decryptionMode, buffer, header.Size)).ConfigureAwait(false);
            }
        }

        private static byte[] Decrypt(MS2CryptoMode decryptionMode, byte[] src, uint compressedSize, uint size)
        {
            MultiArrayFile key;
            MultiArrayFile iv;

            switch (decryptionMode)
            {
                case MS2CryptoMode.MS2F:
                    key = Cryptography.MS2F.Key;
                    iv = Cryptography.MS2F.IV;
                    break;
                case MS2CryptoMode.NS2F:
                    key = Cryptography.NS2F.Key;
                    iv = Cryptography.NS2F.IV;
                    break;
                default:
                case MS2CryptoMode.OS2F:
                case MS2CryptoMode.PS2F:
                    throw new NotImplementedException();
            }

            return Decryption.Decrypt(src, size, key[compressedSize], iv[compressedSize]);
        }

        private static byte[] Decrypt(MS2CryptoMode decryptionMode, byte[] src, uint size)
        {
            MultiArrayFile key;
            MultiArrayFile iv;

            switch (decryptionMode)
            {
                case MS2CryptoMode.MS2F:
                    key = Cryptography.MS2F.Key;
                    iv = Cryptography.MS2F.IV;
                    break;
                case MS2CryptoMode.NS2F:
                    key = Cryptography.NS2F.Key;
                    iv = Cryptography.NS2F.IV;
                    break;
                default:
                case MS2CryptoMode.OS2F:
                case MS2CryptoMode.PS2F:
                    throw new NotImplementedException();
            }

            return Decryption.DecryptNoDecompress(src, size, key[size], iv[size]);
        }
    }
}
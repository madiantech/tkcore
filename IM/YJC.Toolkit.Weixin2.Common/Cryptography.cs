using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace YJC.Toolkit.Weixin
{
    internal static class Cryptography
    {
        public static int HostToNetworkOrder(int inval)
        {
            int outval = 0;
            for (int i = 0; i < 4; i++)
                outval = (outval << 8) + ((inval >> (i * 8)) & 255);
            return outval;
        }

        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="input">密文</param>
        /// <param name="encodingAESKey"></param>
        /// <returns></returns>
        public static string AesDecrypt(string input, string encodingAESKey, out string appId)
        {
            byte[] key = Convert.FromBase64String(encodingAESKey + "=");
            byte[] iv = new byte[16];
            Array.Copy(key, iv, 16);
            byte[] btmpMsg = AesDecrypt(input, iv, key);

            int len = BitConverter.ToInt32(btmpMsg, 16);
            len = IPAddress.NetworkToHostOrder(len);

            byte[] bMsg = new byte[len];
            byte[] bCorpid = new byte[btmpMsg.Length - 20 - len];
            Array.Copy(btmpMsg, 20, bMsg, 0, len);
            Array.Copy(btmpMsg, 20 + len, bCorpid, 0, btmpMsg.Length - 20 - len);
            string oriMsg = Encoding.UTF8.GetString(bMsg);
            appId = Encoding.UTF8.GetString(bCorpid);

            return oriMsg;
            //byte[] bMsg = new byte[btmpMsg.Length - 20];
            //Array.Copy(btmpMsg, 20, bMsg, 0, btmpMsg.Length - 20);
            //string tmpmsg = Encoding.UTF8.GetString(bMsg);
            //string msg = tmpmsg.Substring(0, len);
            //return msg;
        }

        public static string AesEncrypt(string input, string encodingAESKey, string corpid)
        {
            byte[] key = Convert.FromBase64String(encodingAESKey + "=");
            byte[] iv = new byte[16];
            Array.Copy(key, iv, 16);

            string Randcode = CreateRandCode(16);
            byte[] bRand = Encoding.UTF8.GetBytes(Randcode);
            byte[] bCorpid = Encoding.UTF8.GetBytes(corpid);
            byte[] btmpMsg = Encoding.UTF8.GetBytes(input);
            byte[] bMsgLen = BitConverter.GetBytes(HostToNetworkOrder(btmpMsg.Length));
            byte[] bMsg = new byte[bRand.Length + bMsgLen.Length + bCorpid.Length + btmpMsg.Length];

            Array.Copy(bRand, bMsg, bRand.Length);
            Array.Copy(bMsgLen, 0, bMsg, bRand.Length, bMsgLen.Length);
            Array.Copy(btmpMsg, 0, bMsg, bRand.Length + bMsgLen.Length, btmpMsg.Length);
            Array.Copy(bCorpid, 0, bMsg, bRand.Length + bMsgLen.Length + btmpMsg.Length, bCorpid.Length);

            return AesEncrypt(bMsg, iv, key);
        }

        internal static string CreateRandCode(int codeLen)
        {
            const string codeSerial =
                "2,3,4,5,6,7,a,c,d,e,f,h,i,j,k,m,n,p,r,s,t,A,C,D,E,F,G,H,J,K,M,N,P,Q,R,S,U,V,W,X,Y,Z";
            if (codeLen == 0)
                codeLen = 16;

            string[] arr = codeSerial.Split(',');
            string code = "";
            int randValue = -1;
            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < codeLen; i++)
            {
                randValue = rand.Next(0, arr.Length - 1);
                code += arr[randValue];
            }

            return code;
        }

        private static string AesEncrypt(byte[] Input, byte[] Iv, byte[] Key)
        {
            var aes = new RijndaelManaged
            {
                /*秘钥的大小，以位为单位*/
                KeySize = 256,
                /*支持的块大小*/
                BlockSize = 128,
                /*填充模式*/
                /*aes.Padding = PaddingMode.PKCS7;*/
                Padding = PaddingMode.None,
                Mode = CipherMode.CBC,
                Key = Key,
                IV = Iv
            };
            var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] xBuff = null;

            #region 自己进行PKCS7补位，用系统自己带的不行

            byte[] msg = new byte[Input.Length + 32 - Input.Length % 32];
            Array.Copy(Input, msg, Input.Length);
            byte[] pad = KCS7Encoder(Input.Length);
            Array.Copy(pad, 0, msg, Input.Length, pad.Length);

            #endregion 自己进行PKCS7补位，用系统自己带的不行

            #region 注释的也是一种方法，效果一样

            //ICryptoTransform transform = aes.CreateEncryptor();
            //byte[] xBuff = transform.TransformFinalBlock(msg, 0, msg.Length);

            #endregion 注释的也是一种方法，效果一样

            var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
            {
                cs.Write(msg, 0, msg.Length);
                xBuff = ms.ToArray();
            }

            String output = Convert.ToBase64String(xBuff);
            return output;
        }

        private static byte[] KCS7Encoder(int text_length)
        {
            int block_size = 32;
            // 计算需要填充的位数
            int amount_to_pad = block_size - (text_length % block_size);
            if (amount_to_pad == 0)
            {
                amount_to_pad = block_size;
            }

            // 获得补位所用的字符
            char pad_chr = Chr(amount_to_pad);
            string tmp = string.Empty;
            for (int index = 0; index < amount_to_pad; index++)
            {
                tmp += pad_chr;
            }

            return Encoding.UTF8.GetBytes(tmp);
        }

        /**
         * 将数字转化成ASCII码对应的字符，用于对明文进行补码
         *
         * @param a 需要转化的数字
         * @return 转化得到的字符
         */

        private static char Chr(int a)
        {
            byte target = (byte)(a & 0xFF);
            return (char)target;
        }

        private static byte[] AesDecrypt(string Input, byte[] Iv, byte[] Key)
        {
            RijndaelManaged aes = new RijndaelManaged
            {
                KeySize = 256,
                BlockSize = 128,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.None,
                Key = Key,
                IV = Iv
            };
            var decrypt = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] xBuff = null;
            var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
            {
                byte[] xXml = Convert.FromBase64String(Input);
                byte[] msg = new byte[xXml.Length + 32 - xXml.Length % 32];
                Array.Copy(xXml, msg, xXml.Length);
                cs.Write(xXml, 0, xXml.Length);
                xBuff = Decode2(ms.ToArray());
            }

            return xBuff;
        }

        private static byte[] Decode2(byte[] decrypted)
        {
            int pad = (int)decrypted[decrypted.Length - 1];
            if (pad < 1 || pad > 32)
            {
                pad = 0;
            }

            byte[] res = new byte[decrypted.Length - pad];
            Array.Copy(decrypted, 0, res, 0, decrypted.Length - pad);
            return res;
        }
    }
}
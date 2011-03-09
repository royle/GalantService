using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GLTWarter.Data
{
    class Validators
    {
        public static string IsValidBarcode(string barcode)
        {
            if (barcode.IndexOfAny(new char[] { '%', '_', ' ', '\n', '\t' }) >= 0) return Resource.validationBarcodeForSearchInvalid;
            if (barcode.Length < 1 || barcode.Length > 30) return Resource.validationBarcodeForSearchInvalid;
            return string.Empty;
        }
        
        public static string IsValidPassword(string password, string passwordConfirm)
        {
            if (string.IsNullOrEmpty(password)) return Resource.validationEmptyPassword;
            if (password.Length < 4) return Resource.validationPasswordTooShort;
            if (password != passwordConfirm && passwordConfirm != null) return Resource.validationPasswordConfirmMismatch;
            return string.Empty;
        }
        /// <summary>
        /// 别名不能含有特殊符号或空格,最短为4个字符
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        public static string IsValidAlias(string alias)
        {
            //消除前后的空白，并不对空格进行判断
            if (alias.IndexOfAny(new char[] { '%', '_', '\n', '\t' }) >= 0) return Resource.validationShipmentAlias;
            return string.Empty;
        }
    }
}

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CertificateHandler.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Nako.Client
{
    #region Using Directives

    using System.Linq;
    using System.Net;
    using System.Net.Security;
    using System.Security.Cryptography.X509Certificates;

    using Nako.Extensions;

    #endregion

    /// <summary>
    /// The certificate handler.
    /// </summary>
    public class CertificateHandler
    {
        #region Public Methods and Operators

        public static bool ValidateCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return IsValidCryptoRequest(sender) ? IsValidCryptoCertificate(certificate) : sslPolicyErrors == SslPolicyErrors.None;
        }

        #endregion

        #region Methods

        private static bool IsValidCryptoCertificate(X509Certificate certificate)
        {
            return true;
            //var certin = certificate as X509Certificate2;
            //if (certin != null)
            //{
            //    X509Certificate2 certOut = null;
            //    if (CertUtil.TryResolveCertificate(StoreName.CertificateAuthority, StoreLocation.LocalMachine, X509FindType.FindByThumbprint, certin.Thumbprint, out certOut))
            //    {
            //        return true;
            //    }

            //    // TODO: this is a temporary fix to allow fiddler certificates to pass validation.
            //    if (CertUtil.TryResolveCertificate(StoreName.My, StoreLocation.CurrentUser, X509FindType.FindByThumbprint, certin.Thumbprint, out certOut))
            //    {
            //        return true;
            //    }
            //}

            //return false;
        }

        private static bool IsValidCryptoRequest(object sender)
        {
            var req = sender as HttpWebRequest;
            return req != null && req.Headers.AllKeys.Any(key => key == "x-crypto");
        }

        #endregion
    }
}

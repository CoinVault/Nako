////// --------------------------------------------------------------------------------------------------------------------
////// <copyright file="CertUtil.cs" company="SoftChains">
//////   Copyright 2016 Dan Gershony
//////   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//////   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//////   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//////   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
////// </copyright>
////// --------------------------------------------------------------------------------------------------------------------

////namespace Nako.Extensions
////{
////    #region Using Directives

////    using System;
////    using System.Security.Cryptography;
////    using System.Security.Cryptography.X509Certificates;

////    #endregion

////    /// <summary>
////    /// This class supplies support for reading certificates from store.
////    /// this class was taken from WIF (which is internal).
////    /// </summary>
////    public static class CertUtil
////    {
////        #region Public Methods and Operators

////        /// <summary>
////        /// This method returns the private key.
////        /// </summary>
////        /// <param name="certificate">
////        /// The certificate.
////        /// </param>
////        /// <returns>
////        /// Success if the certificate was found.
////        /// </returns>
////        /// <exception cref="ArgumentException">
////        /// Thrown when either the cert doesn't have a valid private key or the private key cannot be cast into an RSA one
////        /// </exception>
////        public static RSA EnsureAndGetPrivateRsaKey(X509Certificate2 certificate)
////        {
////            if (!certificate.HasPrivateKey)
////            {
////                throw new ArgumentException(string.Format("X509Util exception, certificate {0} has no private key", certificate.Thumbprint));
////            }

////            AsymmetricAlgorithm privateKey;

////            try
////            {
////                privateKey = certificate.PrivateKey;
////            }
////            catch (CryptographicException ex)
////            {
////                throw new ArgumentException(string.Format("X509Util exception, certificate {0}", certificate.Thumbprint), ex);
////            }

////            var rsa = privateKey as RSA;
////            if (rsa == null)
////            {
////                throw new ArgumentNullException(string.Format("X509Util exception, certificate {0} private key is not RSA", certificate.Thumbprint));
////            }

////            return rsa;
////        }

////        /// <summary>
////        /// This method returns the certificate id.
////        /// </summary>
////        /// <param name="certificate">
////        /// The certificate.
////        /// </param>
////        /// <returns>
////        /// the certificate id.
////        /// </returns>
////        public static string GetCertificateId(X509Certificate2 certificate)
////        {
////            if (certificate == null)
////            {
////                throw new ArgumentNullException("certificate");
////            }

////            var str = certificate.SubjectName.Name;
////            if (string.IsNullOrEmpty(str))
////            {
////                str = certificate.Thumbprint;
////            }

////            return str;
////        }

////        /// <summary>
////        /// This method resets all the certificates.
////        /// </summary>
////        /// <param name="certificates">
////        /// The certificate.
////        /// </param>
////        public static void ResetAllCertificates(X509Certificate2Collection certificates)
////        {
////            if (certificates == null)
////            {
////                return;
////            }

////            foreach (var t in certificates)
////            {
////                t.Reset();
////            }
////        }

////        /// <summary>
////        /// This methods resolves a certificate by search parameters.
////        /// </summary>
////        /// <param name="storeName">
////        /// The store name to look in.
////        /// </param>
////        /// <param name="storeLocation">
////        /// The certificate location to look in.
////        /// </param>
////        /// <param name="findType">
////        /// The type of certificate.
////        /// </param>
////        /// <param name="findValue">
////        /// The value to look on.
////        /// </param>
////        /// <returns>
////        /// Success if the certificate was found.
////        /// </returns>
////        /// <exception cref="InvalidOperationException">
////        /// Thrown when the certificate cannot be resolved
////        /// </exception>
////        public static X509Certificate2 ResolveCertificate(StoreName storeName, StoreLocation storeLocation, X509FindType findType, object findValue)
////        {
////            X509Certificate2 certificate;
////            if (TryResolveCertificate(storeName, storeLocation, findType, findValue, out certificate))
////            {
////                return certificate;
////            }

////            throw new InvalidOperationException(string.Format("X509Util exception, storeName = {0}, storeLocation = {1}, findType = {2}, findValue = {3}", storeName, storeLocation, findType, findValue));
////        }

////        /// <summary>
////        /// This methods resolves a certificate by search parameters.
////        /// </summary>
////        /// <param name="storeName">
////        /// The store name to look in.
////        /// </param>
////        /// <param name="storeLocation">
////        /// The certificate location to look in.
////        /// </param>
////        /// <param name="findType">
////        /// The type of certificate.
////        /// </param>
////        /// <param name="findValue">
////        /// The value to look on.
////        /// </param>
////        /// <param name="certificate">
////        /// The certificate.
////        /// </param>
////        /// <returns>
////        /// Success if the certificate was found.
////        /// </returns>
////        public static bool TryResolveCertificate(StoreName storeName, StoreLocation storeLocation, X509FindType findType, object findValue, out X509Certificate2 certificate)
////        {
////            var store = new X509Store(storeName, storeLocation);
////            store.Open(OpenFlags.ReadOnly);
////            certificate = null;
////            X509Certificate2Collection certificates1 = null;
////            X509Certificate2Collection certificates2 = null;
////            try
////            {
////                certificates1 = store.Certificates;
////                certificates2 = certificates1.Find(findType, findValue, false);
////                if (certificates2.Count == 1)
////                {
////                    certificate = new X509Certificate2(certificates2[0]);
////                    return true;
////                }
////            }
////            finally
////            {
////                ResetAllCertificates(certificates2);
////                ResetAllCertificates(certificates1);
////                store.Close();
////            }

////            return false;
////        }

////        #endregion
////    }
////}
﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidateAddressResult.cs" company="SoftChains">
//   Copyright 2016 Dan Gershony
//   //  Licensed under the MIT license. See LICENSE file in the project root for full license information.
//   //  THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
//   //  EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
//   //  OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Nako.Client.Types
{
    public class ValidateAddressResult
    {
        #region Public Properties

        public string Account { get; set; }

        public string Address { get; set; }

        public bool IsMine { get; set; }

        public bool IsScript { get; set; }

        public bool IsValid { get; set; }


        public string Pubkey { get; set; }

        #endregion
    }
}

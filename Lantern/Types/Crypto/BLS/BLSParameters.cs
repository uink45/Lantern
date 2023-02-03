// SPDX-FileCopyrightText: 2022 Demerzel Solutions Limited
// SPDX-License-Identifier: LGPL-3.0-only

namespace Lantern.Types.Crypto.BLS
{
    public struct BLSParameters
    {
        public byte[] InputKeyMaterial;
        public byte[] PrivateKey;
        public byte[] PublicKey;
        public BlsScheme Scheme;
        public BlsVariant Variant;
    }
}

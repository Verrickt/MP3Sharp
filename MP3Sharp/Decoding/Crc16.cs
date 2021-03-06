// /***************************************************************************
//  * Crc16.cs
//  * Copyright (c) 2015 the authors.
//  * 
//  * All rights reserved. This program and the accompanying materials
//  * are made available under the terms of the GNU Lesser General Public License
//  * (LGPL) version 3 which accompanies this distribution, and is available at
//  * https://www.gnu.org/licenses/lgpl-3.0.en.html
//  *
//  * This library is distributed in the hope that it will be useful,
//  * but WITHOUT ANY WARRANTY; without even the implied warranty of
//  * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//  * Lesser General Public License for more details.
//  *
//  ***************************************************************************/

using MP3Sharp.Support;

namespace MP3Sharp.Decoding
{
    /// <summary>
    ///     16-Bit CRC checksum
    /// </summary>
    internal sealed class Crc16
    {
        private static readonly short Polynomial;
        private short m_Crc;

        static Crc16()
        {
            Polynomial = (short) SupportClass.Identity(0x8005);
        }

        /// <summary>
        ///     Dummy Constructor
        /// </summary>
        public Crc16()
        {
            m_Crc = (short) SupportClass.Identity(0xFFFF);
        }

        /// <summary>
        ///     Feed a bitstring to the crc calculation (length between 0 and 32, not inclusive).
        /// </summary>
        public void add_bits(int bitstring, int length)
        {
            int bitmask = 1 << (length - 1);
            do
                if (((m_Crc & 0x8000) == 0) ^ ((bitstring & bitmask) == 0))
                {
                    m_Crc <<= 1;
                    m_Crc ^= Polynomial;
                }
                else
                    m_Crc <<= 1; while ((bitmask = SupportClass.URShift(bitmask, 1)) != 0);
        }

        /// <summary>
        ///     Return the calculated checksum.
        ///     Erase it for next calls to add_bits().
        /// </summary>
        public short Checksum()
        {
            short sum = m_Crc;
            m_Crc = (short) SupportClass.Identity(0xFFFF);
            return sum;
        }
    }
}
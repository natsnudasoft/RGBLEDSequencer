﻿// <copyright file="LedGrayscale.cs" company="natsnudasoft">
// Copyright (c) Adrian John Dunstan. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

namespace Natsnudasoft.RgbLedSequencerLibrary
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using Natsnudasoft.NatsnudaLibrary;

    /// <summary>
    /// Represents grayscale (PWM brightness control) values for an RGB LED.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public struct LedGrayscale : IEquatable<LedGrayscale>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LedGrayscale"/> struct.
        /// </summary>
        /// <param name="sequencerConfig">The <see cref="IRgbLedSequencerConfiguration"/> that
        /// describes the configuration of the RGB LED Sequencer.</param>
        /// <param name="red">The grayscale (PWM brightness control) value for the red part of an
        /// RGB LED. This value can not be larger than the maximum grayscale value defined by the
        /// specified configuration.</param>
        /// <param name="green">The grayscale (PWM brightness control) value for the green part of
        /// an RGB LED. This value can not be larger than the maximum grayscale value defined by the
        /// specified configuration.</param>
        /// <param name="blue">The grayscale (PWM brightness control) value for the blue part of an
        /// RGB LED. This value can not be larger than the maximum grayscale value defined by the
        /// specified configuration.</param>
        /// <exception cref="ArgumentNullException"><paramref name="sequencerConfig"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="red"/>,
        /// <paramref name="green"/>, or <paramref name="blue"/> is larger than the maximum
        /// grayscale defined in the specified configuration.</exception>
        public LedGrayscale(
            IRgbLedSequencerConfiguration sequencerConfig,
            byte red,
            byte green,
            byte blue)
        {
            ParameterValidation.IsNotNull(sequencerConfig, nameof(sequencerConfig));
            ParameterValidation.IsLessThanOrEqualTo(
                red,
                sequencerConfig.MaxGrayscale,
                nameof(red));
            ParameterValidation.IsLessThanOrEqualTo(
                green,
                sequencerConfig.MaxGrayscale,
                nameof(green));
            ParameterValidation.IsLessThanOrEqualTo(
                blue,
                sequencerConfig.MaxGrayscale,
                nameof(blue));

            this.Red = red;
            this.Green = green;
            this.Blue = blue;
        }

        /// <summary>
        /// Gets the grayscale (PWM brightness control) value for the red part of an RGB LED.
        /// </summary>
        public byte Red { get; }

        /// <summary>
        /// Gets the grayscale (PWM brightness control) value for the green part of an RGB LED.
        /// </summary>
        public byte Green { get; }

        /// <summary>
        /// Gets the grayscale (PWM brightness control) value for the blue part of an RGB LED.
        /// </summary>
        public byte Blue { get; }

        /// <summary>
        /// Gets the debugger display string.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string DebuggerDisplay => "#" +
            this.Red.ToString("X2", CultureInfo.CurrentCulture) +
            this.Green.ToString("X2", CultureInfo.CurrentCulture) +
            this.Blue.ToString("X2", CultureInfo.CurrentCulture);

        /// <summary>
        /// Determines if the specified operands are equal.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is equal to <paramref name="right"/>;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(LedGrayscale left, LedGrayscale right)
        {
            return left.Red == right.Red &&
                left.Green == right.Green &&
                left.Blue == right.Blue;
        }

        /// <summary>
        /// Determines if the specified operands are not equal.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left"/> is not equal to
        /// <paramref name="right"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(LedGrayscale left, LedGrayscale right)
        {
            return !(left == right);
        }

        /// <inheritdoc/>
        public bool Equals(LedGrayscale other)
        {
            return other == this;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is LedGrayscale && (LedGrayscale)obj == this;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            const int InitPrime = 17;
            const int MultPrime = 23;
            var hash = InitPrime;
            unchecked
            {
                hash = (hash * MultPrime) + this.Red.GetHashCode();
                hash = (hash * MultPrime) + this.Green.GetHashCode();
                hash = (hash * MultPrime) + this.Blue.GetHashCode();
            }

            return hash;
        }
    }
}
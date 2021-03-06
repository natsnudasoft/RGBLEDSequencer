﻿// <copyright file="GrayscaleData.cs" company="natsnudasoft">
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
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using Natsnudasoft.NatsnudaLibrary;

    /// <summary>
    /// Represents grayscale (PWM brightness control) values for a number of RGB LEDs part of
    /// an RGB LED Sequencer.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Microsoft.Naming",
        "CA1710:IdentifiersShouldHaveCorrectSuffix",
        Justification = "We don't follow this convention.")]
    public sealed class GrayscaleData : IReadOnlyList<LedGrayscale>, IEquatable<GrayscaleData>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private readonly LedGrayscale[] ledGrayscales;

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayscaleData"/> class.
        /// </summary>
        /// <param name="sequencerConfig">The <see cref="IRgbLedSequencerConfiguration"/> that
        /// describes the configuration of the RGB LED Sequencer.</param>
        /// <param name="ledGrayscales">The <see cref="LedGrayscale"/> collection that represents
        /// the grayscale value of a number of RGB LEDs.</param>
        /// <exception cref="ArgumentNullException"><paramref name="sequencerConfig"/>, or
        /// <paramref name="ledGrayscales"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">The length of the <paramref name="ledGrayscales"/>
        /// array does not match the number of RGB LEDs.</exception>
        public GrayscaleData(
            IRgbLedSequencerConfiguration sequencerConfig,
            ICollection<LedGrayscale> ledGrayscales)
        {
            ParameterValidation.IsNotNull(sequencerConfig, nameof(sequencerConfig));
            ParameterValidation.IsNotNull(ledGrayscales, nameof(ledGrayscales));
            if (ledGrayscales.Count != sequencerConfig.RgbLedCount)
            {
                throw new ArgumentException(
                    "Collection size must match the number of RGB LEDs.",
                    nameof(ledGrayscales));
            }

            this.ledGrayscales = new LedGrayscale[ledGrayscales.Count];
            ledGrayscales.CopyTo(this.ledGrayscales, 0);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayscaleData"/> class, using the
        /// specified factory to create the number of <see cref="LedGrayscale"/> instances
        /// defined in the specified <see cref="IRgbLedSequencerConfiguration"/>.
        /// </summary>
        /// <param name="sequencerConfig">The <see cref="IRgbLedSequencerConfiguration"/> that
        /// describes the configuration of the RGB LED Sequencer.</param>
        /// <param name="ledGrayscaleFactory">The factory to use to create instances of
        /// <see cref="LedGrayscale"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="sequencerConfig"/>, or
        /// <paramref name="ledGrayscaleFactory"/> is <see langword="null"/>.</exception>
        public GrayscaleData(
            IRgbLedSequencerConfiguration sequencerConfig,
            Func<LedGrayscale> ledGrayscaleFactory)
        {
            ParameterValidation.IsNotNull(sequencerConfig, nameof(sequencerConfig));
            ParameterValidation.IsNotNull(ledGrayscaleFactory, nameof(ledGrayscaleFactory));

            var rgbLedCount = sequencerConfig.RgbLedCount;
            this.ledGrayscales = new LedGrayscale[rgbLedCount];
            for (int i = 0; i < rgbLedCount; ++i)
            {
#pragma warning disable CC0031 // Check for null before calling a delegate
                this.ledGrayscales[i] = ledGrayscaleFactory();
#pragma warning restore CC0031 // Check for null before calling a delegate
            }
        }

        /// <inheritdoc/>
        int IReadOnlyCollection<LedGrayscale>.Count
        {
            get { return this.ledGrayscales.Length; }
        }

        /// <summary>
        /// Gets the debugger display string.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string DebuggerDisplay => "{" +
            string.Join(" ", this.ledGrayscales.Select(l => l.DebuggerDisplay)) + "}";

        /// <summary>
        /// Gets the <see cref="LedGrayscale"/> of the RGB LED of the specified number.
        /// </summary>
        /// <param name="ledIndex">The 0 based index of the RGB LED to get the
        /// <see cref="LedGrayscale"/> data for.</param>
        /// <returns>The <see cref="LedGrayscale"/> of the specified RGB LED.</returns>
        /// <exception cref="IndexOutOfRangeException"><paramref name="ledIndex"/> is not a valid
        /// value.</exception>
        public LedGrayscale this[int ledIndex]
        {
            get { return this.ledGrayscales[ledIndex]; }
        }

        /// <inheritdoc/>
        LedGrayscale IReadOnlyList<LedGrayscale>.this[int index]
        {
            get { return this[index]; }
        }

        /// <inheritdoc/>
        public IEnumerator<LedGrayscale> GetEnumerator()
        {
            return ((IEnumerable<LedGrayscale>)this.ledGrayscales).GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <inheritdoc/>
        public bool Equals(GrayscaleData other)
        {
            bool result;
            if (ReferenceEquals(other, null))
            {
                result = false;
            }
            else if (ReferenceEquals(other, this))
            {
                result = true;
            }
            else if (this.ledGrayscales.Length == other.ledGrayscales.Length)
            {
                result = true;
                for (int i = 0; i < this.ledGrayscales.Length; ++i)
                {
                    if (other.ledGrayscales[i] != this.ledGrayscales[i])
                    {
                        result = false;
                        break;
                    }
                }
            }
            else
            {
                result = false;
            }

            return result;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as GrayscaleData);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            const int InitPrime = 17;
            const int MultPrime = 23;
            var hash = InitPrime;
            unchecked
            {
                foreach (var ledGrayscale in this.ledGrayscales)
                {
                    hash = (hash * MultPrime) + ledGrayscale.GetHashCode();
                }
            }

            return hash;
        }
    }
}
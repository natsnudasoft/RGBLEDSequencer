﻿// <copyright file="LedDotCorrectionCustomization.cs" company="natsnudasoft">
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

namespace Natsnudasoft.RgbLedSequencerLibraryTests.Helper
{
    using AutoFixture;
    using AutoFixture.Kernel;
    using Moq;
    using Natsnudasoft.NatsnudaLibrary.TestExtensions;
    using Natsnudasoft.RgbLedSequencerLibrary;

    public class LedDotCorrectionCustomization : SequencerConfigurationCustomization
    {
        public LedDotCorrectionCustomization(
            Mock<IRgbLedSequencerConfiguration> sequencerConfigMock)
            : base(sequencerConfigMock)
        {
        }

        public int MaxDotCorrection { get; set; }

        public override void Customize(IFixture fixture)
        {
            FavorDotCorrectionsConstructor(fixture);
            this.SequencerConfigMock.Setup(c => c.MaxDotCorrection).Returns(this.MaxDotCorrection);
        }

        private static void FavorDotCorrectionsConstructor(IFixture fixture)
        {
            fixture.Customize<LedDotCorrection>(c => c.FromFactory(
                new MethodInvoker(new ParameterTypeFavoringConstructorQuery(
                    typeof(IRgbLedSequencerConfiguration),
                    typeof(byte),
                    typeof(byte),
                    typeof(byte)))));
        }
    }
}
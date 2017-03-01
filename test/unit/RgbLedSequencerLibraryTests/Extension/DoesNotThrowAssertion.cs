﻿// <copyright file="DoesNotThrowAssertion.cs" company="natsnudasoft">
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

namespace RgbLedSequencerLibraryTests.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Idioms;
    using Ploeh.AutoFixture.Kernel;
    using RgbLedSequencerLibrary;

    /// <summary>
    /// Encapsulates a unit test that verifies a method or constructor does not throw any
    /// exceptions.
    /// </summary>
    /// <seealso cref="IdiomaticAssertion" />
    public sealed class DoesNotThrowAssertion : IdiomaticAssertion
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DoesNotThrowAssertion"/> class.
        /// </summary>
        /// <param name="fixture">The anonymous object creation service to use to create new
        /// specimens.</param>
        public DoesNotThrowAssertion(IFixture fixture)
        {
            ParameterValidation.IsNotNull(fixture, nameof(fixture));

            this.Fixture = fixture;
        }

        /// <summary>
        /// Gets the anonymous object creation service.
        /// </summary>
        public IFixture Fixture { get; }

        /// <inheritdoc/>
        public override void Verify(ConstructorInfo constructorInfo)
        {
            ParameterValidation.IsNotNull(constructorInfo, nameof(constructorInfo));

            constructorInfo.Invoke(this.GetParameters(constructorInfo).ToArray());
        }

        /// <inheritdoc/>
        public override void Verify(MethodInfo methodInfo)
        {
            ParameterValidation.IsNotNull(methodInfo, nameof(methodInfo));

            var parameters = this.GetParameters(methodInfo);
            if (methodInfo.IsStatic)
            {
                methodInfo.Invoke(null, parameters.ToArray());
            }
            else
            {
                var owner = this.CreateOwner(methodInfo.ReflectedType);
                methodInfo.Invoke(owner, parameters.ToArray());
            }
        }

        /// <inheritdoc/>
        public override void Verify(PropertyInfo propertyInfo)
        {
            ParameterValidation.IsNotNull(propertyInfo, nameof(propertyInfo));

            var getMethod = propertyInfo.GetGetMethod();
            var setMethod = propertyInfo.GetSetMethod();

            if (getMethod == null && setMethod == null)
            {
                throw new ArgumentException(
                    "Could not find a public get or set method on property.",
                    nameof(propertyInfo));
            }

            if (getMethod != null)
            {
                this.Verify(getMethod);
            }

            if (setMethod != null)
            {
                this.Verify(setMethod);
            }
        }

        private IEnumerable<object> GetParameters(MethodBase method)
        {
            var context = new SpecimenContext(this.Fixture);
            return method.GetParameters().Select(p => context.Resolve(p));
        }

        private object CreateOwner(Type type)
        {
            var context = new SpecimenContext(this.Fixture);
            return context.Resolve(type);
        }
    }
}

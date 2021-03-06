﻿/*
 JustMock Lite
 Copyright © 2010-2014 Telerik AD

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

   http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using System;
using System.Linq.Expressions;
using Telerik.JustMock.Core;
using Telerik.JustMock.Core.Context;
using Telerik.JustMock.Expectations;

namespace Telerik.JustMock
{
	public partial class Mock
	{
		/// <summary>
		/// Setups the target call to act in a specific way.
		/// </summary>
		/// <typeparam name="T">Target type</typeparam>
		/// <typeparam name="TResult">
		/// Return type for the target setup.
		/// </typeparam>
		/// <param name="func">
		/// Expression delegate to the target call
		/// </param>
		/// <returns>
		/// Reference to <see cref="FuncExpectation{TResult}"/> to setup the mock.
		/// </returns>
		public static FuncExpectation<TResult> Arrange<T, TResult>(Func<TResult> func)
		{
			return ProfilerInterceptor.GuardInternal(() =>
			{
				var repo = MockingContext.CurrentRepository;
				repo.EnableInterception(typeof(T));
				return repo.Arrange(() => func(), () => new FuncExpectation<TResult>());
			});
		}

		/// <summary>
		/// Setups the target mock call with user expectation.
		/// </summary>
		/// <typeparam name="TResult">
		/// Return type for the target setup.
		/// </typeparam>
		/// <param name="expression">
		/// Provide the target method call
		/// </param>
		/// <returns>
		/// Reference to <see cref="FuncExpectation{TResult}"/> to setup the mock.
		/// </returns>
		public static FuncExpectation<TResult> Arrange<TResult>(Expression<Func<TResult>> expression)
		{
			return ProfilerInterceptor.GuardInternal(() =>
			{
				return MockingContext.CurrentRepository.Arrange(expression, () => new FuncExpectation<TResult>());
			});
		}

		/// <summary>
		/// Setups the target mock call with user expectation.
		/// </summary>
		/// <typeparam name="T">Target type</typeparam>
		/// <typeparam name="TResult">
		/// Return type for the target setup.
		/// </typeparam>
		/// <param name="obj">
		/// Target instance.
		/// </param>
		/// <param name="func">
		/// Expression delegate to the target call
		/// </param>
		/// <returns>
		/// Reference to <see cref="FuncExpectation{TResult}"/> to setup the mock.
		/// </returns>
		public static FuncExpectation<TResult> Arrange<T, TResult>(T obj, Func<T, TResult> func)
		{
			return ProfilerInterceptor.GuardInternal(() =>
			{
				var repo = MockingContext.CurrentRepository;
				repo.EnableInterception(typeof(T));
				return repo.Arrange(() => func(obj), () => new FuncExpectation<TResult>());
			});
		}

#if !VISUALBASIC
		/// <summary>
		/// Setups the target call to act in a specific way.
		/// </summary>
		/// <param name="expression">
		/// Target expression
		/// </param>
		/// <returns>
		/// Reference to <see cref="ActionExpectation"/> to setup the mock.
		/// </returns>
		public static ActionExpectation Arrange(Expression<Action> expression)
		{
			return ProfilerInterceptor.GuardInternal(() =>
			{
				return MockingContext.CurrentRepository.Arrange(expression, () => new ActionExpectation());
			});
		}
#endif

		/// <summary>
		/// Setups target property set operation to act in a specific way.  
		/// <example>
		/// <code>
		/// Mock.ArrangeSet(() => foo.MyValue = 10).Throws(new InvalidOperationException());
		/// </code>
		/// This will throw InvalidOperationException for when foo.MyValue is set with 10.
		/// </example>
		/// </summary>
		/// <param name="action">
		/// Target action
		/// </param>
		/// <returns>
		/// Reference to <see cref="ActionExpectation"/> to setup the mock.
		/// </returns>
		public static ActionExpectation ArrangeSet(Action action)
		{
			return ProfilerInterceptor.GuardInternal(() =>
			{
				return MockingContext.CurrentRepository.Arrange(action, () => new ActionExpectation());
			});
		}

		/// <summary>
		/// Arranges the return values of properties and methods according to the given functional specification.
		/// </summary>
		/// <typeparam name="T">Mock type.</typeparam>
		/// <param name="mock">The mock on which to make the arrangements. If 'null' then the specification will be applied to all instances.</param>
		/// <param name="functionalSpecification">The functional specification to apply to this mock.</param>
		/// <remarks>
		/// See article "Create Mocks By Example" for further information on how to write functional specifications.
		/// </remarks>
		public static void ArrangeLike<T>(T mock, Expression<Func<T, bool>> functionalSpecification)
		{
			ProfilerInterceptor.GuardInternal(() => FunctionalSpecParser.ApplyFunctionalSpec(mock, functionalSpecification, ReturnArranger.Instance));
		}
	}
}
﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using Analyzer.Utilities.PooledObjects;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow.CopyAnalysis;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow.PointsToAnalysis;

#pragma warning disable CA1067 // Override Object.Equals(object) when implementing IEquatable<T>

namespace Microsoft.CodeAnalysis.FlowAnalysis.DataFlow.ValueContentAnalysis
{
    using InterproceduralValueContentAnalysisData = InterproceduralAnalysisData<ValueContentAnalysisData, ValueContentAnalysisContext, ValueContentAbstractValue>;
    using ValueContentAnalysisResult = DataFlowAnalysisResult<ValueContentBlockAnalysisResult, ValueContentAbstractValue>;
    using CopyAnalysisResult = DataFlowAnalysisResult<CopyBlockAnalysisResult, CopyAbstractValue>;

    /// <summary>
    /// Analysis context for execution of <see cref="ValueContentAnalysis"/> on a control flow graph.
    /// </summary>
    public sealed class ValueContentAnalysisContext : AbstractDataFlowAnalysisContext<ValueContentAnalysisData, ValueContentAnalysisContext, ValueContentAnalysisResult, ValueContentAbstractValue>
    {
        private ValueContentAnalysisContext(
            AbstractValueDomain<ValueContentAbstractValue> valueDomain,
            WellKnownTypeProvider wellKnownTypeProvider,
            ControlFlowGraph controlFlowGraph,
            ISymbol owningSymbol,
            InterproceduralAnalysisConfiguration interproceduralAnalysisConfig,
            bool pessimisticAnalysis,
            CopyAnalysisResult copyAnalysisResultOpt,
            PointsToAnalysisResult pointsToAnalysisResultOpt,
            Func<ValueContentAnalysisContext, ValueContentAnalysisResult> getOrComputeAnalysisResult,
            ControlFlowGraph parentControlFlowGraphOpt,
            InterproceduralValueContentAnalysisData interproceduralAnalysisDataOpt,
            InterproceduralAnalysisPredicate interproceduralAnalysisPredicateOpt)
            : base(valueDomain, wellKnownTypeProvider, controlFlowGraph, owningSymbol, interproceduralAnalysisConfig,
                  pessimisticAnalysis, predicateAnalysis: true, exceptionPathsAnalysis: false, copyAnalysisResultOpt,
                  pointsToAnalysisResultOpt, getOrComputeAnalysisResult, parentControlFlowGraphOpt,
                  interproceduralAnalysisDataOpt, interproceduralAnalysisPredicateOpt)
        {
        }

        internal static ValueContentAnalysisContext Create(
            AbstractValueDomain<ValueContentAbstractValue> valueDomain,
            WellKnownTypeProvider wellKnownTypeProvider,
            ControlFlowGraph controlFlowGraph,
            ISymbol owningSymbol,
            InterproceduralAnalysisConfiguration interproceduralAnalysisConfig,
            bool pessimisticAnalysis,
            CopyAnalysisResult copyAnalysisResultOpt,
            PointsToAnalysisResult pointsToAnalysisResultOpt,
            Func<ValueContentAnalysisContext, ValueContentAnalysisResult> getOrComputeAnalysisResult,
            InterproceduralAnalysisPredicate interproceduralAnalysisPredicateOpt)
        {
            return new ValueContentAnalysisContext(
                valueDomain, wellKnownTypeProvider, controlFlowGraph, owningSymbol,
                interproceduralAnalysisConfig, pessimisticAnalysis, copyAnalysisResultOpt, pointsToAnalysisResultOpt,
                getOrComputeAnalysisResult, parentControlFlowGraphOpt: null, interproceduralAnalysisDataOpt: null, interproceduralAnalysisPredicateOpt);
        }

        public override ValueContentAnalysisContext ForkForInterproceduralAnalysis(
            IMethodSymbol invokedMethod,
            ControlFlowGraph invokedControlFlowGraph,
            IOperation operation,
            PointsToAnalysisResult pointsToAnalysisResultOpt,
            CopyAnalysisResult copyAnalysisResultOpt,
            InterproceduralValueContentAnalysisData interproceduralAnalysisData)
        {
            return new ValueContentAnalysisContext(ValueDomain, WellKnownTypeProvider, invokedControlFlowGraph, invokedMethod, InterproceduralAnalysisConfiguration,
                PessimisticAnalysis, copyAnalysisResultOpt, pointsToAnalysisResultOpt, GetOrComputeAnalysisResult, ControlFlowGraph, interproceduralAnalysisData,
                InterproceduralAnalysisPredicateOpt);
        }

        protected override void ComputeHashCodePartsSpecific(ArrayBuilder<int> builder)
        {
        }
    }
}

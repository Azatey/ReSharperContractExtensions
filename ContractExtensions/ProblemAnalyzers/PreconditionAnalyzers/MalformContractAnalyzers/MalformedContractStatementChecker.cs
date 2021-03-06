﻿using JetBrains.ReSharper.Daemon.Stages;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using ReSharper.ContractExtensions.ContractsEx.Assertions;
using ReSharper.ContractExtensions.ContractsEx.Assertions.Statements;

namespace ReSharper.ContractExtensions.ProblemAnalyzers.PreconditionAnalyzers.MalformContractAnalyzers
{

    /// <summary>
    /// Warns if method contract is malformed:
    /// - Contract statements are not the first statements in the method
    /// - Ensures statement is after precondition check
    /// </summary>
    [ElementProblemAnalyzer(new[] { typeof(ICSharpStatement) },
    HighlightingTypes = new[] { typeof(CodeContractErrorHighlighting), typeof(CodeContractWarningHighlighting) })]
    public sealed class MalformedContractStatementChecker : ElementProblemAnalyzer<ICSharpStatement>
    {
        protected override void Run(ICSharpStatement element, ElementProblemAnalyzerData data, IHighlightingConsumer consumer)
        {
            var contractStatement = CodeContractStatement.TryCreate(element);
            // We're interested only in Code Contract Statements for know!
            if (contractStatement == null || contractStatement.CodeContractExpression == null)
                return;

            var validationResult = ContractStatementValidator.ValidateStatement(contractStatement);
            if (validationResult.ErrorType == ErrorType.NoError)
                return;

            consumer.AddHighlighting(
                    new MalformedContractErrorHighlighting(element, contractStatement, validationResult),
                    validationResult.Statement.GetDocumentRange(), element.GetContainingFile());
        }
    }
}
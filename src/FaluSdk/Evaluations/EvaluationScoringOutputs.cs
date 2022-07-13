﻿using Falu.Core;

namespace Falu.Evaluations;

/// <summary>
/// Outputs of scoring done in an evaluation.
/// </summary>
public class EvaluationScoringOutputs
{
    /// <summary>Provider used for scoring document.</summary>
    public string? StatementProvider { get; set; }

    /// <summary>Name found in the document.</summary>
    /// <example>JOHN ***</example>
    public string? Name { get; set; }

    /// <summary>Email found in the document.</summary>
    public string? Email { get; set; }

    /// <summary>Phone number found in the document.</summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Time at which the document was generated by the provider.
    /// </summary>
    public DateTimeOffset? Generated { get; set; }

    /// <summary>
    /// Period for which the document was generated.
    /// </summary>
    public Period? Period { get; set; }

    /// <summary>
    /// Risk probability. The higher the value, the higher the risk
    /// </summary>
    /// <example>0.54687</example>
    public float? Risk { get; set; }

    /// <summary>
    /// Limit advised for lending in the smallest currency unit.
    /// </summary>
    /// <example>1500000</example>
    public long? Limit { get; set; }

    /// <summary>
    /// Time till when the score is deemed valid.
    /// </summary>
    public DateTimeOffset? Expires { get; set; }
}

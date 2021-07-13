﻿using Falu.Core;
using System;

namespace Falu.PaymentReversals
{
    /// <summary>
    /// Represents a reversal of a Payment.
    /// </summary>
    public class PaymentReversal : PaymentReversalPatchModel, IHasId, IHasCurrency, IHasCreated, IHasUpdated, IHasWorkspaceId, IHasLive, IHasEtag
    {
        /// <inheritdoc/>
        public string? Id { get; set; }

        /// <summary>
        /// Identifier of the Payment reversed.
        /// </summary>
        public string? PaymentId { get; set; }

        /// <inheritdoc/>
        public string? Currency { get; set; }

        /// <summary>
        /// Amount reversed in smallest currency unit.
        /// This is pulled from the Payment.
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// Reason for the reversal.
        /// </summary>
        public PaymentReversalReason Reason { get; set; }

        /// <summary>
        /// Status of the reversal. 
        /// </summary>
        public PaymentReversalStatus Status { get; set; }

        /// <inheritdoc/>
        public DateTimeOffset Created { get; set; }

        /// <inheritdoc/>
        public DateTimeOffset Updated { get; set; }

        /// <summary>
        /// Time at which the reversal succeeded. Only populated when successful.
        /// </summary>
        public DateTimeOffset? Succeeded { get; set; }

        /// <summary>
        /// Details of the reversal if done via MPESA.
        /// Only populated if the payment being reversed use an MPESA intrument.
        /// </summary>
        public PaymentReversalMpesaDetails? Mpesa { get; set; }

        /// <summary>
        /// Details about failure if the reversal is in failed state.
        /// </summary>
        public PaymentReversalFailureDetails? Failure { get; set; }

        /// <inheritdoc/>
        public string? WorkspaceId { get; set; }

        /// <inheritdoc/>
        public bool Live { get; set; }

        /// <inheritdoc/>
        public string? Etag { get; set; }

    }
}

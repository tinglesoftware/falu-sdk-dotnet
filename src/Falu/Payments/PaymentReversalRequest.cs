﻿using Falu.Payments.Reversals;

namespace Falu.Payments
{
    /// <summary>
    /// Information for initiating a reversal for a payment.
    /// </summary>
    public class PaymentReversalRequest : ReversalPatchModel
    {
        /// <summary>
        /// Identifier of the Payment to reverse.
        /// </summary>
        public string PaymentId { get; set; }

        /// <summary>
        /// Reason for the reversal.
        /// </summary>
        public ReversalReason Reason { get; set; }
    }
}

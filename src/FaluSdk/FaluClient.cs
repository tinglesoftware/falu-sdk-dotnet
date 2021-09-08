﻿using Falu.Evaluations;
using Falu.Events;
using Falu.FileUploadLinks;
using Falu.FileUploads;
using Falu.Identity;
using Falu.Messages;
using Falu.MessageStreams;
using Falu.MessageTemplates;
using Falu.PaymentAuthorizations;
using Falu.PaymentRefunds;
using Falu.Payments;
using Falu.TransferReversals;
using Falu.Transfers;
using Falu.Webhooks;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;

namespace Falu
{
    /// <summary>
    /// Official client for Falu API
    /// </summary>
    public class FaluClient<TOptions> where TOptions : FaluClientOptions
    {
        /// <summary>
        /// Creates an instance of <see cref="FaluClient{TOptions}"/>
        /// </summary>
        /// <param name="backChannel"></param>
        /// <param name="optionsAccessor"></param>
        public FaluClient(HttpClient backChannel, IOptions<TOptions> optionsAccessor)
        {
            BackChannel = backChannel ?? throw new ArgumentNullException(nameof(backChannel));
            Options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));

            Evaluations = new EvaluationsServiceClient(BackChannel, Options);
            Events = new EventsServiceClient(BackChannel, Options);
            FileUploads = new FileUploadsServiceClient(BackChannel, Options);
            FileUploadLinks = new FileUploadLinksServiceClient(BackChannel, Options);
            Identity = new IdentityServiceClient(BackChannel, Options);
            Messages = new MessagesServiceClient(BackChannel, Options);
            MessageStreams = new MessageStreamsServiceClient(BackChannel, Options);
            MessageTemplates = new MessageTemplatesServiceClient(BackChannel, Options);
            MoneyBalances = new MoneyBalancesServiceClient(BackChannel, Options);
            Payments = new PaymentsServiceClient(BackChannel, Options);
            PaymentAuthorizations = new PaymentAuthorizationsServiceClient(BackChannel, Options);
            PaymentRefunds = new PaymentRefundsServiceClient(BackChannel, Options);
            Transfers = new TransfersServiceClient(BackChannel, Options);
            TransferReversals = new TransferReversalsServiceClient(BackChannel, Options);
            Webhooks = new WebhooksServiceClient(BackChannel, Options);
        }

        ///
        protected HttpClient BackChannel { get; }

        ///
        protected TOptions Options { get; }

        #region Services

        ///
        public virtual EvaluationsServiceClient Evaluations { get; protected set; }

        ///
        public virtual EventsServiceClient Events { get; protected set; }

        ///
        public virtual FileUploadsServiceClient FileUploads { get; protected set; }

        ///
        public virtual FileUploadLinksServiceClient FileUploadLinks { get; protected set; }

        ///
        public virtual IdentityServiceClient Identity { get; protected set; }

        ///
        public virtual MessagesServiceClient Messages { get; protected set; }

        ///
        public virtual MessageStreamsServiceClient MessageStreams { get; protected set; }

        ///
        public virtual MessageTemplatesServiceClient MessageTemplates { get; protected set; }

        ///
        public virtual MoneyBalancesServiceClient MoneyBalances { get; protected set; }

        ///
        public virtual PaymentsServiceClient Payments { get; protected set; }

        ///
        public virtual PaymentAuthorizationsServiceClient PaymentAuthorizations { get; protected set; }

        ///
        public virtual PaymentRefundsServiceClient PaymentRefunds { get; protected set; }

        ///
        public virtual TransfersServiceClient Transfers { get; protected set; }

        ///
        public virtual TransferReversalsServiceClient TransferReversals { get; protected set; }

        ///
        public virtual WebhooksServiceClient Webhooks { get; protected set; }

        #endregion
    }

    /// <summary>
    /// Official client for Falu API
    /// </summary>
    public class FaluClient : FaluClient<FaluClientOptions>
    {
        /// <summary>
        /// Creates an instance of <see cref="FaluClient"/>
        /// </summary>
        /// <param name="backChannel"></param>
        /// <param name="optionsAccessor"></param>
        public FaluClient(HttpClient backChannel, IOptions<FaluClientOptions> optionsAccessor) : base(backChannel, optionsAccessor) { }
    }
}

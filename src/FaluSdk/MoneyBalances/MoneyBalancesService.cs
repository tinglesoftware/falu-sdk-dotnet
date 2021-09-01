﻿using Falu.Core;
using Falu.Infrastructure;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Falu.Payments
{
    ///
    public class MoneyBalancesService : BaseService<MoneyBalances>
    {
        ///
        public MoneyBalancesService(HttpClient backChannel, FaluClientOptions options) : base(backChannel, options) { }

        /// <inheritdoc/>
        protected override string BasePath => "/v1/money_balances";

        /// <summary>
        /// Retrieve balance.
        /// </summary>
        /// <param name="options">Options to use for the request.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<ResourceResponse<MoneyBalances>> GetAsync(RequestOptions? options = null,
                                                                      CancellationToken cancellationToken = default)
        {
            var uri = MakePath();
            return RequestAsync<MoneyBalances>(uri, HttpMethod.Get, null, options, cancellationToken);
        }

        /// <summary>
        /// Force a balance refresh.
        /// </summary>
        /// <param name="options">Options to use for the request.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<ResourceResponse<object>> RefreshAsync(RequestOptions? options = null,
                                                                   CancellationToken cancellationToken = default)
        {
            var uri = MakePath("/refresh");
            return RequestAsync<object>(uri, HttpMethod.Post, new { }, options, cancellationToken);
        }
    }
}

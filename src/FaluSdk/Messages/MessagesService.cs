﻿using Falu.Core;
using Falu.Infrastructure;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Tingle.Extensions.JsonPatch;

namespace Falu.Messages
{
    ///
    public class MessagesService : BaseService
    {
        ///
        public MessagesService(HttpClient backChannel, FaluClientOptions options) : base(backChannel, options) { }

        /// <summary>
        /// List messages.
        /// </summary>
        /// <param name="options">Options for filtering and pagination.</param>
        /// <param name="requestOptions">Options to use for the request.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<ResourceResponse<List<Message>>> ListAsync(MessagesListOptions? options = null,
                                                                             RequestOptions? requestOptions = null,
                                                                             CancellationToken cancellationToken = default)
        {
            var args = new Dictionary<string, string>();
            options?.PopulateQueryValues(args);

            var query = QueryHelper.MakeQueryString(args);
            var uri = new Uri(BaseAddress, $"/v1/messages{query}");
            return await GetAsync<List<Message>>(uri, requestOptions, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieve a message.
        /// </summary>
        /// <param name="id">Unique identifier for the message</param>
        /// <param name="options">Options to use for the request.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<ResourceResponse<Message>> GetAsync(string id,
                                                                      RequestOptions? options = null,
                                                                      CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException($"'{nameof(id)}' cannot be null or whitespace.", nameof(id));

            var uri = new Uri(BaseAddress, $"/v1/messages/{id}");
            return await GetAsync<Message>(uri, options, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Send a message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="options">Options to use for the request.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<ResourceResponse<Message>> CreateAsync(MessageCreateRequest message,
                                                                         RequestOptions? options = null,
                                                                         CancellationToken cancellationToken = default)
        {
            if (message is null) throw new ArgumentNullException(nameof(message));
            message.Template?.Model?.GetType().EnsureAllowedForMessageTemplateModel();

            var uri = new Uri(BaseAddress, "/v1/messages");
            return await PostAsync<Message>(uri, message, options, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Update a message.
        /// </summary>
        /// <param name="id">Unique identifier for the message</param>
        /// <param name="patch"></param>
        /// <param name="options">Options to use for the request.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<ResourceResponse<Message>> UpdateAsync(string id,
                                                                         JsonPatchDocument<MessagePatchModel> patch,
                                                                         RequestOptions? options = null,
                                                                         CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException($"'{nameof(id)}' cannot be null or whitespace.", nameof(id));
            if (patch is null) throw new ArgumentNullException(nameof(patch));

            var uri = new Uri(BaseAddress, $"/v1/messages/{id}");
            return await PatchAsync<Message>(uri, patch, options, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Send a batch of messages.
        /// </summary>
        /// <param name="messages"></param>
        /// <param name="options">Options to use for the request.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<ResourceResponse<List<Message>>> CreateBatchAsync(IList<MessageCreateRequest> messages,
                                                                                    RequestOptions? options = null,
                                                                                    CancellationToken cancellationToken = default)
        {
            if (messages is null) throw new ArgumentNullException(nameof(messages));

            if (messages.Count > 10_000)
            {
                throw new ArgumentOutOfRangeException(paramName: nameof(messages),
                                                      message: "The service does not support more than 10,000 (10k) messages");
            }

            foreach(var m in messages)
            {
                m.Template?.Model?.GetType().EnsureAllowedForMessageTemplateModel();
            }

            var uri = new Uri(BaseAddress, "/v1/messages/bulk");
            return await PostAsync<List<Message>>(uri, messages, options, cancellationToken).ConfigureAwait(false);
        }
    }
}

﻿using Falu.Core;
using System.Collections;
using System.Net;
using System.Net.Http.Headers;

namespace Falu
{
    /// <summary>
    /// Model of a HTTP response to an API with typed Error and Resource
    /// </summary>
    /// <typeparam name="TResource">the type of resource</typeparam>
    /// <remarks>
    /// There is no need to implement <see cref="IDisposable"/> because there are no unmanaged resources in use
    /// and there are no resources that the Garbage Collector does not know how to release.
    /// The instance of <see cref="HttpResponseMessage"/> referenced by <see cref="Response"/> is automatically disposed
    /// once an instance of <see cref="ResourceResponse{TResource}"/> is no longer in use.
    /// </remarks>
    public class ResourceResponse<TResource>
    {
        /// <summary>
        /// Create an instance of <see cref="ResourceResponse{TResource}"/>
        /// </summary>
        /// <param name="response"></param>
        /// <param name="resource"></param>
        /// <param name="error"></param>
        public ResourceResponse(HttpResponseMessage response,
                                TResource? resource = default,
                                FaluError? error = default)
        {
            Response = response ?? throw new ArgumentNullException(nameof(response));
            Resource = resource;
            Error = error;

            RequestId = GetHeader(response.Headers, HeadersNames.XRequestId);
            TraceId = GetHeader(response.Headers, HeadersNames.XTraceId) ?? error?.TraceId;
            ContinuationToken = GetHeader(response.Headers, HeadersNames.XContinuationToken);
            CachedResponse = GetHeader<bool?>(response.Headers, HeadersNames.XCachedResponse);
        }

        /// <summary>Gets the ID of the request, as returned by Falu.</summary>
        public string? RequestId { get; }

        /// <summary>
        /// Gets an identifier to correlate the request between the client and the server, as returned by Falu.
        /// </summary>
        public string? TraceId { get; }

        /// <summary>Gets the token to use to fetch more data, as returned by Falu.</summary>
        public string? ContinuationToken { get; }

        /// <summary>
        /// Gets value indicating if the response was returned from cache.
        /// This is true for repeat requests using the same idempotency key.
        /// When <see langword="null" />, the header was not present in the response.
        /// </summary>
        public bool? CachedResponse { get; }

        /// <summary>
        /// The original HTTP response
        /// </summary>
        public HttpResponseMessage Response { get; }

        /// <summary>
        /// The response status code gotten from <see cref="Response"/>
        /// </summary>
        public HttpStatusCode StatusCode => Response.StatusCode;

        /// <summary>
        /// Determines if the request was successful. Value is true if the response code is in the 200 to 299 range
        /// </summary>
        public bool IsSuccessful => (int)Response.StatusCode >= 200 && (int)Response.StatusCode <= 299;

        /// <summary>
        /// The resource extracted from the response body
        /// </summary>
        public TResource? Resource { get; }

        /// <summary>
        /// The error extracted from the response body
        /// </summary>
        public FaluError? Error { get; }

        /// <summary>
        /// Helper method to ensure the response was successful
        /// </summary>
        public void EnsureSuccess()
        {
            // do not bother with successful requests
            if (IsSuccessful) return;

            var lines = new List<string>
            {
                Error?.Detail ?? Error?.Title ?? $"Request failed - {StatusCode} ({(int)StatusCode})",
                $"StatusCode: {(int)StatusCode} ({StatusCode})"
            };
            AddIf(lines, RequestId, "RequestId: {0}", RequestId);
            AddIf(lines, TraceId, "TraceId: {0}", TraceId);
            AddIf(lines, Error?.Title, "Error: {0}", Error?.Title);
            AddIf(lines, Error?.Detail, "Message: {0}", Error?.Detail);
            var message = string.Join("\r\n", lines);

            throw new FaluException(statusCode: StatusCode, message: message)
            {
                Response = Response,
                RequestId = RequestId,
                TraceId = TraceId,
                Error = Error,
            };
        }

        private static void AddIf(IList<string> collection, string? value, string format, params string?[] args)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                collection.Add(string.Format(format, args));
            }
        }

        /// <summary>
        /// Checks if there are more results to retrieve.
        /// The result is null when <typeparamref name="TResource"/> is not assignable from <see cref="IEnumerable"/>.
        /// Otherwise, true when <see cref="ContinuationToken"/> has a valueor false when it doesnt have a value.
        /// </summary>
        public bool? HasMoreResults => typeof(IEnumerable).IsAssignableFrom(typeof(TResource)) ? ContinuationToken != null : null;

        internal static string? GetHeader(HttpResponseHeaders headers, string name)
        {
            if (headers.TryGetValues(name, out var values))
            {
                return values.SingleOrDefault();
            }

            return default;
        }

        private static T? GetHeader<T>(HttpResponseHeaders headers, string name)
        {
            var value = GetHeader(headers, name);
            if (string.IsNullOrWhiteSpace(value)) return default;

            // Handle nullable differently
            var t = typeof(T);
            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null) return default;
                t = Nullable.GetUnderlyingType(t);
            }

            return (T?)Convert.ChangeType(value, t!);
        }
    }
}

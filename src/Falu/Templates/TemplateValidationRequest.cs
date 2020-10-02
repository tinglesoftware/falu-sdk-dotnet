﻿using System.Collections.Generic;

namespace Falu.Templates
{
    /// <summary>
    /// Model for requesting template validation
    /// </summary>
    public class TemplateValidationRequest
    {
        /// <summary>
        /// The content to use when this template is used to send messages.
        /// See our template language documentation for more information on the syntax for this field.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// The template model to be used when rendering test content.
        /// </summary>
        public IDictionary<string, object> TestRenderModel { get; set; }
    }
}

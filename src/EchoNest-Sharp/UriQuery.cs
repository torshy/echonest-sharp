#region Header

////===================================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation and Silverlight
// ===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ===================================================================================
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
////===================================================================================

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace EchoNest
{
    /// <summary>
    /// Represents a query in a Uri.
    /// </summary>
    /// <remarks>
    /// This class can be used to parse a query string to access
    /// </remarks>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "OK")]
    public class UriQuery : IEnumerable<KeyValuePair<string, string>>
    {
        #region Fields

        private readonly string _baseUrl;
        private readonly List<KeyValuePair<string, string>> _entries = new List<KeyValuePair<string, string>>();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UriQuery"/> class.
        /// </summary>
        public UriQuery()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UriQuery"/> class with a query string.
        /// </summary>
        /// <param name="baseUrl"></param>
        public UriQuery(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        #endregion Constructors

        #region Indexers

        /// <summary>
        /// Gets the <see cref="System.String"/> with the specified key.
        /// </summary>
        /// <returns>The value for the specified key, or <see langword="null"/> if the query does not contain such a key.</returns>
        public string this[string key]
        {
            get
            {
                foreach (var kvp in _entries)
                {
                    if (string.Compare(kvp.Key, key, StringComparison.Ordinal) == 0)
                    {
                        return kvp.Value;
                    }
                }

                return null;
            }
        }

        #endregion Indexers

        #region Methods

        /// <summary>
        /// Adds the specified key and value.
        /// </summary>
        /// <param name="key">The name.</param>
        /// <param name="value">The value.</param>
        public UriQuery Add(string key, object value)
        {
            _entries.Add(new KeyValuePair<string, string>(key, value.ToString()));
            return this;
        }

        /// <summary>
        /// Adds the specified key and value.
        /// </summary>
        /// <param name="key">The name.</param>
        /// <param name="value">The value.</param>
        public UriQuery Add(string key, double value)
        {
            _entries.Add(new KeyValuePair<string, string>(key, value.ToString(CultureInfo.InvariantCulture)));
            return this;
        }

        /// <summary>
        /// Adds the specified key and value.
        /// </summary>
        /// <param name="key">The name.</param>
        /// <param name="value">The value.</param>
        public UriQuery Add(string key, bool value)
        {
            _entries.Add(new KeyValuePair<string, string>(key, value.ToString().ToLower()));
            return this;
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _entries.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance as a query string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var queryBuilder = new StringBuilder();

            if (!string.IsNullOrEmpty(_baseUrl))
            {
                queryBuilder.Append(_baseUrl);
            }

            if (_entries.Count > 0)
            {
                if (!string.IsNullOrEmpty(_baseUrl))
                {
                    queryBuilder.Append('?');
                }

                var first = true;

                foreach (var kvp in _entries)
                {
                    if (!first)
                    {
                        queryBuilder.Append('&');
                    }
                    else
                    {
                        first = false;
                    }

                    queryBuilder.Append(Uri.EscapeDataString(kvp.Key));
                    queryBuilder.Append('=');
                    queryBuilder.Append(Uri.EscapeDataString(kvp.Value));
                }
            }

            return queryBuilder.ToString();
        }

        #endregion Methods
    }
}

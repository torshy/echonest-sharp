using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace System.Net.Http.Headers
{
    // This type is used for headers supporting a list of values. It essentially just forwards calls to
    // the actual header-store in HttpHeaders.
    //
    // This type can deal with a so called "special value": The RFC defines some headers which are collection of 
    // values, but the RFC only defines 1 value, e.g. Transfer-Encoding: chunked, Connection: close, 
    // Expect: 100-continue.
    // We expose strongly typed properties for these special values: TransferEncodingChunked, ConnectionClose, 
    // ExpectContinue.
    // So we have 2 properties for each of these headers ('Transfer-Encoding' => TransferEncoding, 
    // TransferEncodingChunked; 'Connection' => Connection, ConnectionClose; 'Expect' => Expect, ExpectContinue)
    //
    // The following solution was chosen:
    // - Keep HttpHeaders clean: HttpHeaders is unaware of these "special values"; it just stores the collection of 
    //   headers. 
    // - It is the responsibility of "higher level" components (HttpHeaderValueCollection, HttpRequestHeaders,
    //   HttpResponseHeaders) to deal with special values. 
    // - HttpHeaderValueCollection can be configured with an IEqualityComparer and a "special value".
    // 
    // Example: Server sends header "Transfer-Encoding: gzip, custom, chunked" to the client.
    // - HttpHeaders: HttpHeaders will have an entry in the header store for "Transfer-Encoding" with values
    //   "gzip", "custom", "chunked"
    // - HttpGeneralHeaders:
    //   - Property TransferEncoding: has two values "gzip" and "custom"
    //   - Property TransferEncodingChunked: is set to "true".
    internal sealed class HttpHeaderValueCollection<T> : ICollection<T> where T : class
    {
        private string headerName;
        private HttpHeaders store;
        private T specialValue;
        private IEqualityComparer specialValueComparer;
        private Action<HttpHeaderValueCollection<T>, T> validator;

        public int Count
        {
            get { return GetCount(); }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        internal bool IsSpecialValueSet
        {
            get
            {
                // If this collection instance has a "special value", then check whether that value was already set.
                if (specialValue == null)
                {
                    return false;
                }
                return store.ContainsParsedValue(headerName, specialValue);
            }
        }

        internal HttpHeaderValueCollection(string headerName, HttpHeaders store)
            : this(headerName, store, null, null, null)
        {
        }

        internal HttpHeaderValueCollection(string headerName, HttpHeaders store,
            Action<HttpHeaderValueCollection<T>, T> validator)
            : this(headerName, store, null, null, validator)
        {
        }

        internal HttpHeaderValueCollection(string headerName, HttpHeaders store, T specialValue,
            IEqualityComparer specialValueComparer)
            : this(headerName, store, specialValue, specialValueComparer, null)
        {
        }

        internal HttpHeaderValueCollection(string headerName, HttpHeaders store, T specialValue,
            IEqualityComparer specialValueComparer, Action<HttpHeaderValueCollection<T>, T> validator)
        {
            Contract.Requires(headerName != null);
            Contract.Requires(store != null);
            Contract.Requires(((specialValue == null) && (specialValueComparer == null)) || (specialValue != null),
                "'specialValueComparer' must only be assigned if we have a 'specialValue'.");

            this.store = store;
            this.headerName = headerName;
            this.specialValue = specialValue;
            this.specialValueComparer = specialValueComparer;
            this.validator = validator;
        }

        public void Add(T item)
        {
            CheckValue(item);
            store.AddParsedValue(headerName, item);
        }

        public void Clear()
        {
            // If the special value was set, then remove everything but the special value.
            if (IsSpecialValueSet)
            {
                store.SetParsedValue(headerName, specialValue);
            }
            else
            {
                store.Remove(headerName);
            }
        }

        public bool Contains(T item)
        {
            CheckValue(item);
            return store.ContainsParsedValue(headerName, item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if ((arrayIndex < 0) || (arrayIndex >= array.Length))
            {
                throw new ArgumentOutOfRangeException("arrayIndex");
            }

            object storeValue = store.GetParsedValues(headerName);

            if (storeValue == null)
            {
                return;
            }

            List<object> storeValues = storeValue as List<object>;

            if (storeValues == null)
            {
                // We only have 1 value: If it is the "special value" just return, otherwise add the value to the
                // array and return.
                Contract.Assert(storeValue is T);
                if (!IsSpecialValue(storeValue))
                {
                    array[arrayIndex] = storeValue as T;
                }
            }
            else
            {
                // We don't know how many values we have. The "special value" may be in the list multiple times. We
                // just know that we'll have at most 'storeValues.Count' values.
                T[] tempArray = new T[storeValues.Count];

                // We have multiple values. Iterate through the values and add them to the array. Skip the 
                // "special value" (if present).
                int addedCount = 0;
                foreach (object item in storeValues)
                {
                    if (!IsSpecialValue(item))
                    {
                        // Validate that the destination array has enough space for this value.
                        if (addedCount + arrayIndex >= array.Length)
                        {
                            throw new ArgumentException("The number of elements is greater than the available space from arrayIndex to the end of the destination array.");
                        }

                        Contract.Assert(item is T);
                        tempArray[addedCount] = item as T;
                        addedCount++;
                    }
                }

                // We successfully copied all values to the temp-array and verified that the destination array has
                // enough space to hold the result. Now copy values from the temp array to the destination array.
                if (addedCount > 0)
                {
                    Array.Copy(tempArray, 0, array, arrayIndex, addedCount);
                }
            }
        }

        private bool IsSpecialValue(object item)
        {
            Contract.Requires(item != null);

            if (specialValue == null)
            {
                return false;
            }

            // In cases where we don't have a dedicated type for a header value, we need to use a comparer to make 
            // sure we compare values correctly (e.g. case-insensitive comparison for strings).
            // If we don't have a comparer, just use Equals() since the header type will do the comparison.
            if (specialValueComparer != null)
            {
                return specialValueComparer.Equals(item, specialValue);
            }

            // We don't have a comparer, so use the Equals() method.
            return item.Equals(specialValue);
        }

        public bool Remove(T item)
        {
            CheckValue(item);
            return store.RemoveParsedValue(headerName, item);
        }

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            object storeValue = store.GetParsedValues(headerName);

            if (storeValue == null)
            {
                yield break;
            }

            List<object> storeValues = storeValue as List<object>;

            if (storeValues == null)
            {
                // We only have 1 value: If it is the "special value" just return, otherwise return the value.
                if (!IsSpecialValue(storeValue))
                {
                    Contract.Assert(storeValue is T);
                    yield return storeValue as T;
                }
            }
            else
            {
                // We have multiple values. Iterate through the values and return them. Skip the 
                // "special value" (if present).
                foreach (object item in storeValues)
                {
                    if (!IsSpecialValue(item))
                    {
                        Contract.Assert(item is T);
                        yield return item as T;
                    }
                }
            }
        }

        #endregion

        #region IEnumerable Members

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        internal void SetSpecialValue()
        {
            Contract.Assert(specialValue != null,
                "This method can only be used if the collection has a 'special value' set.");

            if (!store.ContainsParsedValue(headerName, specialValue))
            {
                store.AddParsedValue(headerName, specialValue);
            }
        }

        internal void RemoveSpecialValue()
        {
            Contract.Assert(specialValue != null,
                "This method can only be used if the collection has a 'special value' set.");

            // We're not interested in the return value. It's OK if the "special value" wasn't in the store
            // before calling RemoveParsedValue().
            store.RemoveParsedValue(headerName, specialValue);
        }

        private void CheckValue(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            // If this instance has a custom validator for validating arguments, call it now.
            if (validator != null)
            {
                validator(this, item);
            }

            // If we have a "special value" (like "close" in the "Connection" header case), throw if the user
            // tries to add the "special value" using the collection: Users should use the appropriate property
            // instead (e.g. ConnectionClose).
            if (IsSpecialValue(item))
            {
                throw new ArgumentException(string.Format("Cannot add the value '{0}' to the collection. Use the corresponding property instead.", specialValue));
            }
        }

        private int GetCount()
        {
            // This is an O(n) operation.

            // If we have a dedicated property for a special value, we only return the number of 
            // "non-special" values, since the "special value" is not part of the HttpHeaderValueCollection.
            object storeValue = store.GetParsedValues(headerName);

            if (storeValue == null)
            {
                return 0;
            }

            List<object> storeValues = storeValue as List<object>;

            if (storeValues == null)
            {
                // We only have 1 value: If it is the "special value" just return, otherwise add the value to the
                // array and return.
                Contract.Assert(storeValue is T);
                if (IsSpecialValue(storeValue))
                {
                    return 0;
                }
                return 1;
            }
            else
            {
                // We have multiple values. Iterate through the values and count only non-special values
                int count = 0;
                foreach (object item in storeValues)
                {
                    if (!IsSpecialValue(item))
                    {
                        count++;
                    }
                }
                return count;
            }
        }
    }
}

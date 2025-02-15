/*

This file is part of the iText (R) project.
Copyright (c) 1998-2021 iText Group NV
Authors: Bruno Lowagie, Paulo Soares, et al.

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License version 3
as published by the Free Software Foundation with the addition of the
following permission added to Section 15 as permitted in Section 7(a):
FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY
ITEXT GROUP. ITEXT GROUP DISCLAIMS THE WARRANTY OF NON INFRINGEMENT
OF THIRD PARTY RIGHTS

This program is distributed in the hope that it will be useful, but
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
or FITNESS FOR A PARTICULAR PURPOSE.
See the GNU Affero General Public License for more details.
You should have received a copy of the GNU Affero General Public License
along with this program; if not, see http://www.gnu.org/licenses or write to
the Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor,
Boston, MA, 02110-1301 USA, or download the license from the following URL:
http://itextpdf.com/terms-of-use/

The interactive user interfaces in modified source and object code versions
of this program must display Appropriate Legal Notices, as required under
Section 5 of the GNU Affero General Public License.

In accordance with Section 7(b) of the GNU Affero General Public License,
a covered work must retain the producer line in every PDF that is created
or manipulated using iText.

You can be released from the requirements of the license by purchasing
a commercial license. Buying such a license is mandatory as soon as you
develop commercial activities involving the iText software without
disclosing the source code of your own applications.
These activities include: offering paid services to customers as an ASP,
serving PDFs on the fly in a web application, shipping iText with a closed
source product.

For more information, please contact iText Software Corp. at this
address: sales@itextpdf.com
*/
using System;

namespace iText.IO.Source {
    /// <summary>
    /// A RandomAccessSource that wraps another RandomAccessSource and provides a window of it at a specific offset and over
    /// a specific length.
    /// </summary>
    /// <remarks>
    /// A RandomAccessSource that wraps another RandomAccessSource and provides a window of it at a specific offset and over
    /// a specific length.  Position 0 becomes the offset position in the underlying source.
    /// </remarks>
    public class WindowRandomAccessSource : IRandomAccessSource {
        /// <summary>The source</summary>
        private readonly IRandomAccessSource source;

        /// <summary>The amount to offset the source by</summary>
        private readonly long offset;

        /// <summary>The length</summary>
        private readonly long length;

        /// <summary>Constructs a new OffsetRandomAccessSource that extends to the end of the underlying source</summary>
        /// <param name="source">the source</param>
        /// <param name="offset">the amount of the offset to use</param>
        public WindowRandomAccessSource(IRandomAccessSource source, long offset)
            : this(source, offset, source.Length() - offset) {
        }

        /// <summary>Constructs a new OffsetRandomAccessSource with an explicit length</summary>
        /// <param name="source">the source</param>
        /// <param name="offset">the amount of the offset to use</param>
        /// <param name="length">the number of bytes to be included in this RAS</param>
        public WindowRandomAccessSource(IRandomAccessSource source, long offset, long length) {
            this.source = source;
            this.offset = offset;
            this.length = length;
        }

        /// <summary>
        /// <inheritDoc/>
        /// Note that the position will be adjusted to read from the corrected location in the underlying source
        /// </summary>
        public virtual int Get(long position) {
            if (position >= length) {
                return -1;
            }
            return source.Get(offset + position);
        }

        /// <summary>
        /// <inheritDoc/>
        /// Note that the position will be adjusted to read from the corrected location in the underlying source
        /// </summary>
        public virtual int Get(long position, byte[] bytes, int off, int len) {
            if (position >= length) {
                return -1;
            }
            long toRead = Math.Min(len, length - position);
            return source.Get(offset + position, bytes, off, (int)toRead);
        }

        /// <summary>
        /// <inheritDoc/>
        /// Note that the length will be adjusted to read from the corrected location in the underlying source
        /// </summary>
        public virtual long Length() {
            return length;
        }

        /// <summary><inheritDoc/></summary>
        public virtual void Close() {
            source.Close();
        }
    }
}

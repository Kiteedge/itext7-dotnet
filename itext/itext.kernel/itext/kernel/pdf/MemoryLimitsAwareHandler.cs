/*
This file is part of the iText (R) project.
Copyright (c) 1998-2021 iText Group NV
Authors: iText Software.

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
using System.Collections.Generic;
using iText.Kernel.Exceptions;

namespace iText.Kernel.Pdf {
    /// <summary>
    /// A
    /// <see cref="MemoryLimitsAwareHandler"/>
    /// handles memory allocation and prevents decompressed
    /// pdf streams from occupation of more space than allowed.
    /// </summary>
    /// <remarks>
    /// A
    /// <see cref="MemoryLimitsAwareHandler"/>
    /// handles memory allocation and prevents decompressed
    /// pdf streams from occupation of more space than allowed.
    /// <para />A configured MemoryLimitsAwareHandler can be set as a property of
    /// <see cref="ReaderProperties"/>
    /// instance which is passed to
    /// <see cref="PdfReader"/>.
    /// </remarks>
    /// <seealso cref="ReaderProperties.SetMemoryLimitsAwareHandler(MemoryLimitsAwareHandler)"/>
    public class MemoryLimitsAwareHandler {
        private const int SINGLE_SCALE_COEFFICIENT = 100;

        private const int SUM_SCALE_COEFFICIENT = 500;

        private const int SINGLE_DECOMPRESSED_PDF_STREAM_MIN_SIZE = int.MaxValue / 100;

        private const long SUM_OF_DECOMPRESSED_PDF_STREAMW_MIN_SIZE = int.MaxValue / 20;

        private int maxSizeOfSingleDecompressedPdfStream;

        private long maxSizeOfDecompressedPdfStreamsSum;

        private long allMemoryUsedForDecompression = 0;

        private long memoryUsedForCurrentPdfStreamDecompression = 0;

        internal bool considerCurrentPdfStream = false;

        /// <summary>
        /// Creates a
        /// <see cref="MemoryLimitsAwareHandler"/>
        /// which will be used to handle decompression of pdf streams.
        /// </summary>
        /// <remarks>
        /// Creates a
        /// <see cref="MemoryLimitsAwareHandler"/>
        /// which will be used to handle decompression of pdf streams.
        /// The max allowed memory limits will be generated by default.
        /// </remarks>
        public MemoryLimitsAwareHandler() {
            maxSizeOfSingleDecompressedPdfStream = SINGLE_DECOMPRESSED_PDF_STREAM_MIN_SIZE;
            maxSizeOfDecompressedPdfStreamsSum = SUM_OF_DECOMPRESSED_PDF_STREAMW_MIN_SIZE;
        }

        /// <summary>
        /// Creates a
        /// <see cref="MemoryLimitsAwareHandler"/>
        /// which will be used to handle decompression of pdf streams.
        /// </summary>
        /// <remarks>
        /// Creates a
        /// <see cref="MemoryLimitsAwareHandler"/>
        /// which will be used to handle decompression of pdf streams.
        /// The max allowed memory limits will be generated by default, based on the size of the document.
        /// </remarks>
        /// <param name="documentSize">the size of the document, which is going to be handled by iText.</param>
        public MemoryLimitsAwareHandler(long documentSize) {
            maxSizeOfSingleDecompressedPdfStream = (int)CalculateDefaultParameter(documentSize, SINGLE_SCALE_COEFFICIENT
                , SINGLE_DECOMPRESSED_PDF_STREAM_MIN_SIZE);
            maxSizeOfDecompressedPdfStreamsSum = CalculateDefaultParameter(documentSize, SUM_SCALE_COEFFICIENT, SUM_OF_DECOMPRESSED_PDF_STREAMW_MIN_SIZE
                );
        }

        /// <summary>Gets the maximum allowed size which can be occupied by a single decompressed pdf stream.</summary>
        /// <returns>the maximum allowed size which can be occupied by a single decompressed pdf stream.</returns>
        public virtual int GetMaxSizeOfSingleDecompressedPdfStream() {
            return maxSizeOfSingleDecompressedPdfStream;
        }

        /// <summary>Sets the maximum allowed size which can be occupied by a single decompressed pdf stream.</summary>
        /// <remarks>
        /// Sets the maximum allowed size which can be occupied by a single decompressed pdf stream.
        /// This value correlates with maximum heap size. This value should not exceed limit of the heap size.
        /// <para />iText will throw an exception if during decompression a pdf stream which was identified as
        /// requiring memory limits awareness occupies more memory than allowed.
        /// </remarks>
        /// <param name="maxSizeOfSingleDecompressedPdfStream">
        /// the maximum allowed size which can be occupied by a single
        /// decompressed pdf stream.
        /// </param>
        /// <returns>
        /// this
        /// <see cref="MemoryLimitsAwareHandler"/>
        /// instance.
        /// </returns>
        /// <seealso cref="IsMemoryLimitsAwarenessRequiredOnDecompression(PdfArray)"/>
        public virtual iText.Kernel.Pdf.MemoryLimitsAwareHandler SetMaxSizeOfSingleDecompressedPdfStream(int maxSizeOfSingleDecompressedPdfStream
            ) {
            this.maxSizeOfSingleDecompressedPdfStream = maxSizeOfSingleDecompressedPdfStream;
            return this;
        }

        /// <summary>Gets the maximum allowed size which can be occupied by all decompressed pdf streams.</summary>
        /// <returns>the maximum allowed size value which streams may occupy</returns>
        public virtual long GetMaxSizeOfDecompressedPdfStreamsSum() {
            return maxSizeOfDecompressedPdfStreamsSum;
        }

        /// <summary>Sets the maximum allowed size which can be occupied by all decompressed pdf streams.</summary>
        /// <remarks>
        /// Sets the maximum allowed size which can be occupied by all decompressed pdf streams.
        /// This value can be limited by the maximum expected PDF file size when it's completely decompressed.
        /// Setting this value correlates with the maximum processing time spent on document reading
        /// <para />iText will throw an exception if during decompression pdf streams which were identified as
        /// requiring memory limits awareness occupy more memory than allowed.
        /// </remarks>
        /// <param name="maxSizeOfDecompressedPdfStreamsSum">
        /// he maximum allowed size which can be occupied by all decompressed pdf
        /// streams.
        /// </param>
        /// <returns>
        /// this
        /// <see cref="MemoryLimitsAwareHandler"/>
        /// instance.
        /// </returns>
        /// <seealso cref="IsMemoryLimitsAwarenessRequiredOnDecompression(PdfArray)"/>
        public virtual iText.Kernel.Pdf.MemoryLimitsAwareHandler SetMaxSizeOfDecompressedPdfStreamsSum(long maxSizeOfDecompressedPdfStreamsSum
            ) {
            this.maxSizeOfDecompressedPdfStreamsSum = maxSizeOfDecompressedPdfStreamsSum;
            return this;
        }

        /// <summary>
        /// Performs a check if the
        /// <see cref="PdfStream"/>
        /// with provided setup of the filters requires
        /// memory limits awareness during decompression.
        /// </summary>
        /// <param name="filters">
        /// is an
        /// <see cref="PdfArray"/>
        /// of names of filters
        /// </param>
        /// <returns>true if PDF stream is suspicious and false otherwise</returns>
        public virtual bool IsMemoryLimitsAwarenessRequiredOnDecompression(PdfArray filters) {
            HashSet<PdfName> filterSet = new HashSet<PdfName>();
            for (int index = 0; index < filters.Size(); index++) {
                PdfName filterName = filters.GetAsName(index);
                if (!filterSet.Add(filterName)) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>Considers the number of bytes which are occupied by the decompressed pdf stream.</summary>
        /// <remarks>
        /// Considers the number of bytes which are occupied by the decompressed pdf stream.
        /// If memory limits have not been faced, throws an exception.
        /// </remarks>
        /// <param name="numOfOccupiedBytes">the number of bytes which are occupied by the decompressed pdf stream.</param>
        /// <returns>
        /// this
        /// <see cref="MemoryLimitsAwareHandler"/>
        /// instance.
        /// </returns>
        /// <seealso cref="MemoryLimitsAwareException"/>
        internal virtual iText.Kernel.Pdf.MemoryLimitsAwareHandler ConsiderBytesOccupiedByDecompressedPdfStream(long
             numOfOccupiedBytes) {
            if (considerCurrentPdfStream && memoryUsedForCurrentPdfStreamDecompression < numOfOccupiedBytes) {
                memoryUsedForCurrentPdfStreamDecompression = numOfOccupiedBytes;
                if (memoryUsedForCurrentPdfStreamDecompression > maxSizeOfSingleDecompressedPdfStream) {
                    throw new MemoryLimitsAwareException(KernelExceptionMessageConstant.DURING_DECOMPRESSION_SINGLE_STREAM_OCCUPIED_MORE_MEMORY_THAN_ALLOWED
                        );
                }
            }
            return this;
        }

        /// <summary>Begins handling of current pdf stream decompression.</summary>
        /// <returns>
        /// this
        /// <see cref="MemoryLimitsAwareHandler"/>
        /// instance.
        /// </returns>
        internal virtual iText.Kernel.Pdf.MemoryLimitsAwareHandler BeginDecompressedPdfStreamProcessing() {
            EnsureCurrentStreamIsReset();
            considerCurrentPdfStream = true;
            return this;
        }

        /// <summary>Ends handling of current pdf stream decompression.</summary>
        /// <remarks>
        /// Ends handling of current pdf stream decompression.
        /// If memory limits have not been faced, throws an exception.
        /// </remarks>
        /// <returns>
        /// this
        /// <see cref="MemoryLimitsAwareHandler"/>
        /// instance.
        /// </returns>
        /// <seealso cref="MemoryLimitsAwareException"/>
        internal virtual iText.Kernel.Pdf.MemoryLimitsAwareHandler EndDecompressedPdfStreamProcessing() {
            allMemoryUsedForDecompression += memoryUsedForCurrentPdfStreamDecompression;
            if (allMemoryUsedForDecompression > maxSizeOfDecompressedPdfStreamsSum) {
                throw new MemoryLimitsAwareException(KernelExceptionMessageConstant.DURING_DECOMPRESSION_MULTIPLE_STREAMS_IN_SUM_OCCUPIED_MORE_MEMORY_THAN_ALLOWED
                    );
            }
            EnsureCurrentStreamIsReset();
            considerCurrentPdfStream = false;
            return this;
        }

        internal virtual long GetAllMemoryUsedForDecompression() {
            return allMemoryUsedForDecompression;
        }

        private static long CalculateDefaultParameter(long documentSize, int scale, long min) {
            long result = documentSize * scale;
            if (result < min) {
                result = min;
            }
            if (result > min * scale) {
                result = min * scale;
            }
            return result;
        }

        private void EnsureCurrentStreamIsReset() {
            memoryUsedForCurrentPdfStreamDecompression = 0;
        }
    }
}

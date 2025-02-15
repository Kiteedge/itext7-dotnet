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
using System.Collections.Generic;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Data;

namespace iText.Kernel.Pdf.Canvas.Parser.Listener {
    /// <summary>
    /// This class expands each
    /// <see cref="iText.Kernel.Pdf.Canvas.Parser.Data.TextRenderInfo"/>
    /// for
    /// <see cref="iText.Kernel.Pdf.Canvas.Parser.EventType.RENDER_TEXT"/>
    /// event types into
    /// multiple
    /// <see cref="iText.Kernel.Pdf.Canvas.Parser.Data.TextRenderInfo"/>
    /// instances for each glyph occurred.
    /// </summary>
    public class GlyphEventListener : IEventListener {
        protected internal readonly IEventListener delegate_;

        /// <summary>
        /// Constructs a
        /// <see cref="GlyphEventListener"/>
        /// instance by a delegate to which the expanded text events for each
        /// glyph occurred will be passed on.
        /// </summary>
        /// <param name="delegate_">delegate to pass the expanded glyph render events to.</param>
        public GlyphEventListener(IEventListener delegate_) {
            this.delegate_ = delegate_;
        }

        public virtual void EventOccurred(IEventData data, EventType type) {
            if (type.Equals(EventType.RENDER_TEXT)) {
                TextRenderInfo textRenderInfo = (TextRenderInfo)data;
                foreach (TextRenderInfo glyphRenderInfo in textRenderInfo.GetCharacterRenderInfos()) {
                    delegate_.EventOccurred(glyphRenderInfo, type);
                }
            }
            else {
                delegate_.EventOccurred(data, type);
            }
        }

        public virtual ICollection<EventType> GetSupportedEvents() {
            return delegate_.GetSupportedEvents();
        }
    }
}

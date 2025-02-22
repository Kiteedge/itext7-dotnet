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
using System;
using iText.IO.Util;
using iText.Kernel.Geom;
using iText.Svg.Renderers;

namespace iText.Svg.Renderers.Impl {
    /// <summary>
    /// <see cref="iText.Svg.Renderers.ISvgNodeRenderer"/>
    /// implementation for the &lt;polygon&gt; tag.
    /// </summary>
    public class PolygonSvgNodeRenderer : PolylineSvgNodeRenderer, IMarkerCapable {
        /// <summary>
        /// Calls setPoints(String) to set
        /// <see cref="PolylineSvgNodeRenderer.points"/>
        /// Then calls
        /// <see cref="ConnectPoints()"/>
        /// to create a path between the first and last point if it doesn't already exist
        /// </summary>
        protected internal override void SetPoints(String pointsAttribute) {
            base.SetPoints(pointsAttribute);
            ConnectPoints();
        }

        /// <summary>
        /// Appends the starting point to the end of
        /// <see cref="PolylineSvgNodeRenderer.points"/>
        /// if it is not already there.
        /// </summary>
        private void ConnectPoints() {
            if (points.Count < 2) {
                return;
            }
            Point start = points[0];
            Point end = points[points.Count - 1];
            if (JavaUtil.DoubleCompare(start.x, end.x) != 0 || JavaUtil.DoubleCompare(start.y, end.y) != 0) {
                points.Add(new Point(start.x, start.y));
            }
        }

        public override ISvgNodeRenderer CreateDeepCopy() {
            PolygonSvgNodeRenderer copy = new PolygonSvgNodeRenderer();
            DeepCopyAttributesAndStyles(copy);
            return copy;
        }
    }
}

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
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Colorspace;
using iText.Test;

namespace iText.Pdfa.Checker {
    public class PdfACheckerTest : ExtendedITextTest {
        private PdfAChecker pdfAChecker;

        [NUnit.Framework.SetUp]
        public virtual void Before() {
            pdfAChecker = new PdfACheckerTest.EmptyPdfAChecker();
            pdfAChecker.SetFullCheckMode(true);
        }

        [NUnit.Framework.Test]
        public virtual void CheckAppearanceStreamsWithCycle() {
            using (MemoryStream bos = new MemoryStream()) {
                using (PdfWriter writer = new PdfWriter(bos)) {
                    using (PdfDocument document = new PdfDocument(writer)) {
                        PdfDictionary normalAppearance = new PdfDictionary();
                        normalAppearance.Put(PdfName.ON, normalAppearance);
                        normalAppearance.MakeIndirect(document);
                        PdfAnnotation annotation = new PdfPopupAnnotation(new Rectangle(0f, 0f));
                        annotation.SetAppearance(PdfName.N, normalAppearance);
                        PdfPage pageToCheck = document.AddNewPage();
                        pageToCheck.AddAnnotation(annotation);
                        // no assertions as we want to check that no exceptions would be thrown
                        pdfAChecker.CheckResourcesOfAppearanceStreams(annotation.GetAppearanceDictionary());
                    }
                }
            }
        }

        private class EmptyPdfAChecker : PdfAChecker {
            protected internal EmptyPdfAChecker()
                : base(null) {
            }

            public override void CheckCanvasStack(char stackOperation) {
            }

            public override void CheckInlineImage(PdfStream inlineImage, PdfDictionary currentColorSpaces) {
            }

            public override void CheckColor(Color color, PdfDictionary currentColorSpaces, bool? fill, PdfStream contentStream
                ) {
            }

            public override void CheckColorSpace(PdfColorSpace colorSpace, PdfDictionary currentColorSpaces, bool checkAlternate
                , bool? fill) {
            }

            public override void CheckRenderingIntent(PdfName intent) {
            }

            public override void CheckFontGlyphs(PdfFont font, PdfStream contentStream) {
            }

            public override void CheckExtGState(CanvasGraphicsState extGState, PdfStream contentStream) {
            }

            public override void CheckFont(PdfFont pdfFont) {
            }

            public override void CheckXrefTable(PdfXrefTable xrefTable) {
            }

            protected internal override void CheckContentStream(PdfStream contentStream) {
            }

            protected internal override void CheckContentStreamObject(PdfObject @object) {
            }

            protected internal override long GetMaxNumberOfIndirectObjects() {
                return 0;
            }

            protected internal override ICollection<PdfName> GetForbiddenActions() {
                return null;
            }

            protected internal override ICollection<PdfName> GetAllowedNamedActions() {
                return null;
            }

            protected internal override void CheckAction(PdfDictionary action) {
            }

            protected internal override void CheckAnnotation(PdfDictionary annotDic) {
            }

            protected internal override void CheckCatalogValidEntries(PdfDictionary catalogDict) {
            }

            protected internal override void CheckColorsUsages() {
            }

            protected internal override void CheckImage(PdfStream image, PdfDictionary currentColorSpaces) {
            }

            protected internal override void CheckFileSpec(PdfDictionary fileSpec) {
            }

            protected internal override void CheckForm(PdfDictionary form) {
            }

            protected internal override void CheckFormXObject(PdfStream form) {
            }

            protected internal override void CheckLogicalStructure(PdfDictionary catalog) {
            }

            protected internal override void CheckMetaData(PdfDictionary catalog) {
            }

            protected internal override void CheckNonSymbolicTrueTypeFont(PdfTrueTypeFont trueTypeFont) {
            }

            protected internal override void CheckOutputIntents(PdfDictionary catalog) {
            }

            protected internal override void CheckPageObject(PdfDictionary page, PdfDictionary pageResources) {
            }

            protected internal override void CheckPageSize(PdfDictionary page) {
            }

            protected internal override void CheckPdfArray(PdfArray array) {
            }

            protected internal override void CheckPdfDictionary(PdfDictionary dictionary) {
            }

            protected internal override void CheckPdfName(PdfName name) {
            }

            protected internal override void CheckPdfNumber(PdfNumber number) {
            }

            protected internal override void CheckPdfStream(PdfStream stream) {
            }

            protected internal override void CheckPdfString(PdfString @string) {
            }

            protected internal override void CheckSymbolicTrueTypeFont(PdfTrueTypeFont trueTypeFont) {
            }

            protected internal override void CheckTrailer(PdfDictionary trailer) {
            }

            protected internal override void CheckPageTransparency(PdfDictionary pageDict, PdfDictionary pageResources
                ) {
            }
        }
    }
}

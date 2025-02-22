/*
This file is part of the iText (R) project.
Copyright (c) 1998-2021 iText Group NV
Authors: iText Software.

This program is offered under a commercial and under the AGPL license.
For commercial licensing, contact us at https://itextpdf.com/sales.  For AGPL licensing, see below.

AGPL licensing:
This program is free software: you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/
using iText.IO.Source;
using iText.Kernel;
using iText.Kernel.Exceptions;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;
using iText.Test;

namespace iText.Kernel.Pdf.Tagging {
    public class PdfStructElemUnitTest : ExtendedITextTest {
        [NUnit.Framework.Test]
        public virtual void NoParentObjectTest() {
            NUnit.Framework.Assert.That(() =>  {
                PdfDictionary parent = new PdfDictionary();
                PdfArray kid = new PdfArray();
                PdfStructElem.AddKidObject(parent, 1, kid);
            }
            , NUnit.Framework.Throws.InstanceOf<PdfException>().With.Message.EqualTo(KernelExceptionMessageConstant.STRUCTURE_ELEMENT_SHALL_CONTAIN_PARENT_OBJECT))
;
        }

        [NUnit.Framework.Test]
        public virtual void AnnotationHasNoReferenceToPageTest() {
            NUnit.Framework.Assert.That(() =>  {
                PdfDocument pdfDoc = new PdfDocument(new PdfWriter(new ByteArrayOutputStream()));
                PdfName pdfName = new PdfName("test");
                PdfAnnotation annotation = new Pdf3DAnnotation(new Rectangle(100, 100), pdfName);
                PdfStructElem structElem = new PdfStructElem(pdfDoc, pdfName, annotation);
            }
            , NUnit.Framework.Throws.InstanceOf<PdfException>().With.Message.EqualTo(KernelExceptionMessageConstant.ANNOTATION_SHALL_HAVE_REFERENCE_TO_PAGE))
;
        }
    }
}

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
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Filespec;
using iText.Test;

namespace iText.Pdfa {
    public class PdfA1EmbeddedFilesCheckTest : ExtendedITextTest {
        public static readonly String sourceFolder = iText.Test.TestUtil.GetParentProjectDirectory(NUnit.Framework.TestContext
            .CurrentContext.TestDirectory) + "/resources/itext/pdfa/";

        [NUnit.Framework.Test]
        public virtual void FileSpecCheckTest01() {
            PdfWriter writer = new PdfWriter(new MemoryStream());
            Stream @is = new FileStream(sourceFolder + "sRGB Color Space Profile.icm", FileMode.Open, FileAccess.Read);
            PdfOutputIntent outputIntent = new PdfOutputIntent("Custom", "", "http://www.color.org", "sRGB IEC61966-2.1"
                , @is);
            PdfADocument pdfDocument = new PdfADocument(writer, PdfAConformanceLevel.PDF_A_1B, outputIntent);
            PdfDictionary fileNames = new PdfDictionary();
            pdfDocument.GetCatalog().Put(PdfName.Names, fileNames);
            PdfDictionary embeddedFiles = new PdfDictionary();
            fileNames.Put(PdfName.EmbeddedFiles, embeddedFiles);
            PdfArray names = new PdfArray();
            fileNames.Put(PdfName.Names, names);
            names.Add(new PdfString("some/file/path"));
            PdfFileSpec spec = PdfFileSpec.CreateEmbeddedFileSpec(pdfDocument, sourceFolder + "sample.wav", "sample.wav"
                , "sample", null, null);
            names.Add(spec.GetPdfObject());
            pdfDocument.AddNewPage();
            Exception e = NUnit.Framework.Assert.Catch(typeof(PdfAConformanceException), () => pdfDocument.Close());
            NUnit.Framework.Assert.AreEqual(PdfAConformanceException.A_NAME_DICTIONARY_SHALL_NOT_CONTAIN_THE_EMBEDDED_FILES_KEY
                , e.Message);
        }

        [NUnit.Framework.Test]
        public virtual void FileSpecCheckTest02() {
            PdfWriter writer = new PdfWriter(new MemoryStream());
            Stream @is = new FileStream(sourceFolder + "sRGB Color Space Profile.icm", FileMode.Open, FileAccess.Read);
            PdfOutputIntent outputIntent = new PdfOutputIntent("Custom", "", "http://www.color.org", "sRGB IEC61966-2.1"
                , @is);
            PdfADocument pdfDocument = new PdfADocument(writer, PdfAConformanceLevel.PDF_A_1B, outputIntent);
            PdfStream stream = new PdfStream();
            pdfDocument.GetCatalog().Put(new PdfName("testStream"), stream);
            PdfFileSpec spec = PdfFileSpec.CreateEmbeddedFileSpec(pdfDocument, sourceFolder + "sample.wav", "sample.wav"
                , "sample", null, null);
            stream.Put(PdfName.F, spec.GetPdfObject());
            pdfDocument.AddNewPage();
            Exception e = NUnit.Framework.Assert.Catch(typeof(PdfAConformanceException), () => pdfDocument.Close());
            NUnit.Framework.Assert.AreEqual(PdfAConformanceException.STREAM_OBJECT_DICTIONARY_SHALL_NOT_CONTAIN_THE_F_FFILTER_OR_FDECODEPARAMS_KEYS
                , e.Message);
        }

        [NUnit.Framework.Test]
        public virtual void FileSpecCheckTest03() {
            PdfWriter writer = new PdfWriter(new MemoryStream());
            Stream @is = new FileStream(sourceFolder + "sRGB Color Space Profile.icm", FileMode.Open, FileAccess.Read);
            PdfOutputIntent outputIntent = new PdfOutputIntent("Custom", "", "http://www.color.org", "sRGB IEC61966-2.1"
                , @is);
            PdfADocument pdfDocument = new PdfADocument(writer, PdfAConformanceLevel.PDF_A_1B, outputIntent);
            PdfStream stream = new PdfStream();
            pdfDocument.GetCatalog().Put(new PdfName("testStream"), stream);
            PdfFileSpec spec = PdfFileSpec.CreateEmbeddedFileSpec(pdfDocument, sourceFolder + "sample.wav", "sample.wav"
                , "sample", null, null);
            stream.Put(new PdfName("fileData"), spec.GetPdfObject());
            pdfDocument.AddNewPage();
            Exception e = NUnit.Framework.Assert.Catch(typeof(PdfAConformanceException), () => pdfDocument.Close());
            NUnit.Framework.Assert.AreEqual(PdfAConformanceException.FILE_SPECIFICATION_DICTIONARY_SHALL_NOT_CONTAIN_THE_EF_KEY
                , e.Message);
        }
    }
}

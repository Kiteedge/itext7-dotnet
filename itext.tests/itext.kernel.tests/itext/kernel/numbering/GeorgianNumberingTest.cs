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
using iText.Test;

namespace iText.Kernel.Numbering {
    public class GeorgianNumberingTest : ExtendedITextTest {
        [NUnit.Framework.Test]
        public virtual void NegativeToGeorgianTest() {
            NUnit.Framework.Assert.AreEqual("", GeorgianNumbering.ToGeorgian(-10));
        }

        [NUnit.Framework.Test]
        public virtual void ZeroToGeorgianTest() {
            NUnit.Framework.Assert.AreEqual("", GeorgianNumbering.ToGeorgian(0));
        }

        [NUnit.Framework.Test]
        public virtual void ToGeorgianTest() {
            NUnit.Framework.Assert.AreEqual("\u10F5", GeorgianNumbering.ToGeorgian(10000));
            NUnit.Framework.Assert.AreEqual("\u10F4\u10E8\u10F2\u10D6", GeorgianNumbering.ToGeorgian(7967));
        }

        [NUnit.Framework.Test]
        public virtual void NumberGreaterThan10000toGeorgianTest() {
            NUnit.Framework.Assert.AreEqual("\u10F5\u10F5\u10F5\u10F5\u10F5\u10F5\u10D2", GeorgianNumbering.ToGeorgian
                (60003));
        }
    }
}

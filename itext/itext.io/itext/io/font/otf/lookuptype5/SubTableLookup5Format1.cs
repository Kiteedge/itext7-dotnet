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
using iText.IO.Font.Otf;
using iText.IO.Util;

namespace iText.IO.Font.Otf.Lookuptype5 {
    /// <summary>Contextual Substitution Subtable: Simple context glyph substitution</summary>
    public class SubTableLookup5Format1 : ContextualTable<ContextualSubstRule> {
        private IDictionary<int, IList<ContextualSubstRule>> substMap;

        public SubTableLookup5Format1(OpenTypeFontTableReader openReader, int lookupFlag, IDictionary<int, IList<ContextualSubstRule
            >> substMap)
            : base(openReader, lookupFlag) {
            this.substMap = substMap;
        }

        protected internal override IList<ContextualSubstRule> GetSetOfRulesForStartGlyph(int startGlyphId) {
            if (substMap.ContainsKey(startGlyphId) && !openReader.IsSkip(startGlyphId, lookupFlag)) {
                return substMap.Get(startGlyphId);
            }
            return JavaCollectionsUtil.EmptyList<ContextualSubstRule>();
        }

        public class SubstRuleFormat1 : ContextualSubstRule {
            // inputGlyphIds array omits the first glyph in the sequence,
            // the first glyph is defined by corresponding coverage glyph
            private int[] inputGlyphIds;

            private SubstLookupRecord[] substLookupRecords;

            public SubstRuleFormat1(int[] inputGlyphIds, SubstLookupRecord[] substLookupRecords) {
                this.inputGlyphIds = inputGlyphIds;
                this.substLookupRecords = substLookupRecords;
            }

            public override int GetContextLength() {
                return inputGlyphIds.Length + 1;
            }

            public override SubstLookupRecord[] GetSubstLookupRecords() {
                return substLookupRecords;
            }

            public override bool IsGlyphMatchesInput(int glyphId, int atIdx) {
                return glyphId == inputGlyphIds[atIdx - 1];
            }
        }
    }
}

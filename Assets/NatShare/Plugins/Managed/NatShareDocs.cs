/* 
*   NatShare
*   Copyright (c) 2018 Yusuf Olokoba
*/

//#define DOC_GEN // Internal. Do not use

namespace NatShareU.Docs {

    using System;
    #if DOC_GEN
    using System.Linq;
    using Calligraphy;
    #endif

    public sealed class DocAttribute : 
    #if DOC_GEN
    CADescriptionAttribute {
        public DocAttribute (string descriptionKey) : base(Docs.docs[descriptionKey]) {}
        public DocAttribute (string summaryKey, string descriptionKey) : base(Docs.docs[descriptionKey], Docs.docs[summaryKey]) {}
    #else
    Attribute {
        public DocAttribute (string descriptionKey) {}
        public DocAttribute (string summaryKey, string descriptionKey) {}
    #endif
    }

    public sealed class CodeAttribute :
    #if DOC_GEN
    CACodeExampleAttribute {
        public CodeAttribute (string key) : base(Docs.examples[key]) {}
    #else
    Attribute {
        public CodeAttribute (string key) {}
    #endif
    }

    public sealed class RefAttribute :
    #if DOC_GEN
    CASeeAlsoAttribute {
        public RefAttribute (params string[] keys) : base(keys.Select(k => Docs.references[k]).ToArray()) {}
    #else
    Attribute {
        public RefAttribute (params string[] keys) {}
    #endif
    }
}
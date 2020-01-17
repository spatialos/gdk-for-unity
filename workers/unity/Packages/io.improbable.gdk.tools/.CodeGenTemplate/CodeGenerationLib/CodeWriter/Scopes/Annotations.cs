using System;
using System.Collections.Generic;
using Improbable.Gdk.CodeGeneration.CodeWriter.Scopes;

namespace CodeGenerationLib.CodeWriter.Scopes
{
    public class AnnotationOutsideType
    {
        protected readonly ScopeBlock ParentBlock;
        protected readonly string Annotation;

        internal AnnotationOutsideType(ScopeBlock parentBlock, string annotation)
        {
            ParentBlock = parentBlock;
            Annotation = annotation;
        }

        public void Type(string declaration, Action<TypeBlock> populate)
        {
            ParentBlock.Add(new TypeBlock(declaration, populate, Annotation));
        }

        public void Type(TypeBlock typeBlock)
        {
            ParentBlock.Add(typeBlock);
        }

        public void Enum(string declaration, Action<EnumBlock> populate)
        {
            ParentBlock.Add(new EnumBlock(declaration, populate, Annotation));
        }

        public void Enum(EnumBlock enumBlock)
        {
            ParentBlock.Add(enumBlock);
        }
    }

    public class AnnotationInsideType : AnnotationOutsideType
    {
        internal AnnotationInsideType(ScopeBlock parentBlock, string annotation) : base(parentBlock, annotation)
        {
        }

        public void Method(string declaration, Action<MethodBlock> populate)
        {
            ParentBlock.Add(new MethodBlock(declaration, populate, Annotation));
        }

        public void Method(string declaration, Func<IEnumerable<string>> populate)
        {
            ParentBlock.Add(new MethodBlock(declaration, populate, Annotation));
        }

        public void Method(MethodBlock methodBlock)
        {
            ParentBlock.Add(methodBlock);
        }
    }
}
